using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class PlayerControls : MonoBehaviour
{
    private Rigidbody2D rb;
    internal float MinSpeed = 1f, MaxSpeed = 3, firecooldownL, firecooldownR, firerate = 3, SailHp = 100, currenthp, hitcd = 10;
    internal static float gold = 0, quality = 0.95f, acceleration = 1.1f, HullHp = 100, cannonballdamage = 20, storagecapacity = 10, sailqual = 1.1f;
    internal static List<string> storage = new List<string>(), enemies = new List<string>();
    internal AudioClip shootsound;
    private GameObject deathmessage, ReloadText, HealthText, HealthBar, SailBar, SailText, CurrGold, Overlay, Storage, LevelObjective;
    public Sprite[] ripple;
    private Vector2 OverlayPos, healthbarscale;
    // Use this for initialization
    void Start()
    {
        LevelObjective = GameObject.Find("LevelObjective");
        HealthBar = GameObject.Find("Green");
        HealthText = GameObject.Find("Health");
        SailBar = GameObject.Find("Green1");
        SailText = GameObject.Find("SailHealth");
        CurrGold = GameObject.Find("Gold");
        Overlay = GameObject.Find("Overlay");
        Storage = GameObject.Find("Storage");
        OverlayPos = transform.position + Overlay.transform.position;
        healthbarscale = HealthBar.transform.GetComponent<RectTransform>().sizeDelta;
        rb = transform.GetComponent<Rigidbody2D>();
        deathmessage = GameObject.Find("Death Message");
        deathmessage.SetActive(false);
        firecooldownL = firerate;
        firecooldownR = firerate;
        ReloadText = GameObject.Find("Reload Text");
        shootsound = (AudioClip)Resources.Load("gun");
        if (hitcd > 0)
        {
            hitcd -= Time.deltaTime;
        }
        if (enemies.Count == 0)
        {
            for (int i = 0; i < ButtonHandler.level; i++)
            {
                int temp = Random.Range(1, 3);
                if (temp <= 1)
                {
                    enemies.Add("Brigantine");
                }
                else if (temp == 2)
                {
                    enemies.Add("Sloop");
                }
                else
                {
                    enemies.Add("Caravel");
                }
            }
        }
        currenthp = HullHp;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEnemies();
        Storage.GetComponent<Text>().text = StorageText();
        Overlay.transform.position = transform.position + (Vector3)OverlayPos;
        HealthBar.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(healthbarscale.x /HullHp *currenthp, healthbarscale.y);
        SailBar.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(healthbarscale.x / 100 * SailHp, healthbarscale.y);
        CurrGold.GetComponent<Text>().text = "Gold : " + gold.ToString(); 
        if (enemies.Count <= 0 && Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name.Contains("Enemy")).Count() <= 0)
        {
            StartCoroutine("Win");
        }
        foreach (GameObject cargo in Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name.Contains("Cargo")))
        {
            if (Vector2.Distance(cargo.transform.position, transform.position) <= 1 && storage.Count < storagecapacity)
            {
                storage.Add(cargo.name);
                string temp = "coin" + Random.Range(1,10).ToString();
                AudioSource.PlayClipAtPoint((AudioClip)Resources.Load(temp),GameObject.Find("Music").transform.position, 0.2f);
                Destroy(cargo); 
            }
            else if (Vector2.Distance(cargo.transform.position, transform.position) <= 2)
            {
                cargo.GetComponent<Rigidbody2D>().velocity = (transform.position - cargo.transform.position).normalized;
            }
            else
            {
                cargo.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
        ReloadText.transform.position = transform.position;
        ReloadText.transform.rotation = transform.rotation;
        ReloadText.GetComponent<Text>().text = firecooldownL.ToString("F1") + "         " + firecooldownR.ToString("F1");
        if (transform.GetChild(0) != null)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = ripple[(int)(Random.Range(0, 3.99f))];
        }
        if (firecooldownL >= 0)
        {
            firecooldownL -= Time.deltaTime;
        }
        if (firecooldownR >= 0)
        {
            firecooldownR -= Time.deltaTime;
        }
        if (currenthp <= 0)
        {
            StartCoroutine("Death");
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Shoot(-transform.right);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Shoot(transform.right);
        }
        Collider2D[] cols = Physics2D.OverlapPointAll(transform.position);
        bool canmove = false;
        foreach (Collider2D col in cols)
        {
            if (col.name.Contains("Chunk"))
            {
                canmove = true;
            }
        }
        if (canmove)
        {
            if (Input.GetKey(KeyCode.A))
            {
                rb.AddForceAtPosition(-transform.right* (SailHp / 100) * sailqual, (Vector2)transform.position + (Vector2)transform.up - (Vector2)transform.right);
                transform.GetChild(0).rotation = Quaternion.Slerp(transform.GetChild(0).rotation, transform.rotation * Quaternion.Euler(0,0,10), Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                rb.AddForceAtPosition(transform.right * (SailHp / 100) * sailqual, (Vector2)transform.position + (Vector2)transform.up + (Vector2)transform.right);
                transform.GetChild(0).rotation = Quaternion.Slerp(transform.GetChild(0).rotation, transform.rotation * Quaternion.Euler(0, 0, -10), Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.W))
            {
                rb.velocity = rb.velocity * acceleration;
                transform.GetChild(0).rotation = Quaternion.Slerp(transform.GetChild(0).rotation, transform.rotation, Time.deltaTime);
            }
        }
        else if (Physics2D.OverlapPoint(transform.position).transform.name.Contains("Dock") && GameObject.Find("Dock").GetComponent<BoxCollider2D>().isTrigger == false)
        {
            rb.velocity += ((Vector2)transform.position + (Vector2)transform.up);
        }
        rb.velocity = ((rb.velocity / 10 * 9) + (Vector2)transform.up / 10).normalized * Mathf.Clamp(Mathf.Abs(rb.velocity.magnitude), MinSpeed, MaxSpeed *(SailHp/100) * sailqual);
    }

    private IEnumerator Win()
    {
        deathmessage.SetActive(true);
        deathmessage.GetComponent<Text>().text = "Level Complete, Collect your loot!";
        deathmessage.transform.position = transform.position;
        yield return new WaitForSeconds(10);
        ButtonHandler.level++;
        SceneManager.LoadScene("Market");
    }
    private IEnumerator Death()
    {
        AudioSource.PlayClipAtPoint((AudioClip)Resources.Load("sink"), GameObject.Find("Music").transform.position, 0.2f); 
        storage.Clear();
        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }
        GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("brokenship")[0];
        transform.rotation = Quaternion.Euler(0, 0, 180);
        deathmessage.SetActive(true);
        deathmessage.GetComponent<Text>().text = "You Died!";
        deathmessage.transform.position = transform.position;
        gold *= 0.75f;
        AudioSource.PlayClipAtPoint((AudioClip)Resources.Load("sink"), GameObject.Find("Music").transform.position, 0.2f);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Market");
    }
    internal void Shoot(Vector2 Direction)
    {
        if (Direction == (Vector2)(-transform.right) && firecooldownL <= 0)
        {
            firecooldownL = firerate;
        }
        else if(Direction == (Vector2)transform.right && firecooldownR <= 0)
        {
            firecooldownR = firerate;
        }
        else
        {
            return;
        }
        GameObject CannonBallt = Instantiate(Resources.Load("CannonBallPrefab") as GameObject);
        GameObject CannonBallc = Instantiate(Resources.Load("CannonBallPrefab") as GameObject);
        GameObject CannonBallb = Instantiate(Resources.Load("CannonBallPrefab") as GameObject);
        CannonBallt.transform.position = transform.position;
        CannonBallc.transform.position = transform.position;
        CannonBallb.transform.position = transform.position;
        CannonBallt.name = "CannonBall " + transform.name;
        CannonBallc.name = "CannonBall " + transform.name;
        CannonBallb.name = "CannonBall " + transform.name;
        CannonBallt.transform.parent = transform;
        CannonBallc.transform.parent = transform;
        CannonBallb.transform.parent = transform;
        CannonBallt.GetComponent<Rigidbody2D>().velocity = (new Vector2(((Direction * 10) + GetComponent<Rigidbody2D>().velocity).x * Mathf.Cos(45) - ((Direction * 10) + GetComponent<Rigidbody2D>().velocity).y * Mathf.Sin(45), ((Direction * 10) + GetComponent<Rigidbody2D>().velocity).x * Mathf.Sin(45) + ((Direction * 10) + GetComponent<Rigidbody2D>().velocity).y * Mathf.Cos(45)))/3;
        CannonBallc.GetComponent<Rigidbody2D>().velocity = ((Direction * 10) + GetComponent<Rigidbody2D>().velocity)/3;
        CannonBallb.GetComponent<Rigidbody2D>().velocity = (new Vector2(((Direction * 10) + GetComponent<Rigidbody2D>().velocity).x * Mathf.Cos(-45) - ((Direction * 10) + GetComponent<Rigidbody2D>().velocity).y * Mathf.Sin(-45), ((Direction * 10) + GetComponent<Rigidbody2D>().velocity).x * Mathf.Sin(-45) + ((Direction * 10) + GetComponent<Rigidbody2D>().velocity).y * Mathf.Cos(-45)))/3;
        AudioSource.PlayClipAtPoint(shootsound, GameObject.Find("Music").transform.position, 1);
        StartCoroutine(AnimCount(new GameObject[]{CannonBallt, CannonBallc, CannonBallb}));
    }
    internal static IEnumerator AnimCount(GameObject[] array)
    {
        yield return new WaitForSeconds(1.75f);
        Sprite[] sprites = Resources.LoadAll<Sprite>("Splash");
        AudioSource.PlayClipAtPoint((AudioClip)Resources.Load("Splash"), GameObject.Find("Music").transform.position, 0.2f);
        foreach (GameObject CannonBall in array)
        {
            if (CannonBall != null)
            {
                CannonBall.transform.parent = null;
                CannonBall.name = "Splash";
                CannonBall.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                CannonBall.GetComponent<SpriteRenderer>().sprite = sprites[0];
                CannonBall.transform.localScale *= 25;
                Destroy(CannonBall.GetComponent<CircleCollider2D>());
            }
        }
        yield return new WaitForSeconds(0.25f);
        foreach (GameObject CannonBall in array)
        {
            if (CannonBall != null)
            {
                CannonBall.GetComponent<SpriteRenderer>().sprite = sprites[1];
            }
        }
        yield return new WaitForSeconds(0.25f);
        foreach (GameObject CannonBall in array)
        {
            if (CannonBall != null)
            {
                CannonBall.GetComponent<SpriteRenderer>().sprite = sprites[2];
            }
        }
        yield return new WaitForSeconds(0.25f);
        foreach (GameObject CannonBall in array)
        {
            if (CannonBall != null)
            {
                CannonBall.GetComponent<SpriteRenderer>().sprite = sprites[3];
            }
        }
        yield return new WaitForSeconds(0.25f);
        foreach (GameObject CannonBall in array)
        {
            if (CannonBall != null)
            {
                Destroy(CannonBall);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("CannonBall") && collision.gameObject.name.Contains("Enemy"))
        {
            if (Random.Range(1, 100) >= 20)
            {
                currenthp -= (collision.transform.parent.GetComponent<EnemyBehavior>().cannonballdamage + Random.Range((collision.transform.parent.GetComponent<EnemyBehavior>().cannonballdamage * -30 / 100), (collision.transform.parent.GetComponent<EnemyBehavior>().cannonballdamage * 30 / 100))) * quality;
            }
            else
            {
                SailHp -= (collision.transform.parent.GetComponent<EnemyBehavior>().cannonballdamage + Random.Range(-30, 30)) * quality;
            }
            AudioSource.PlayClipAtPoint((AudioClip)Resources.Load("cannonballhit"), GameObject.Find("Music").transform.position, 0.2f);
            Destroy(collision.gameObject);
        }
        else if (!collision.gameObject.name.Contains("CannonBall") && collision.gameObject.name.Contains("Enemy"))
        {
            if (hitcd <= 0)
            {
                currenthp -= HullHp / 2;
                collision.gameObject.GetComponent<EnemyBehavior>().HullHp /= 2;
                AudioSource.PlayClipAtPoint((AudioClip)Resources.Load("sink"), GameObject.Find("Music").transform.position, 0.2f);
            }
        }
    }
    internal string StorageText()
    {
        string temp = "Storage " + storage.Count.ToString() + "/" +  storagecapacity.ToString() + ":\n";
        int temp1 = 1, temp2 = 1, temp3 = 1, temp4 = 1, temp5 = 1, temp6 = 1, temp7 = 1, temp8 = 1, temp9 = 1, temp10 = 1;
        foreach (string cargo in storage)
        {
            if (cargo.Contains("Grain"))
            {
                if (temp.Contains("Grain"))
                {
                    temp1++;
                    temp = temp.Replace("Grain", "Grain x" + temp1.ToString());
                }
                else
                {
                    temp += "Grain\n";
                }
            }
            else if (cargo.Contains("Fish"))
            {
                if (temp.Contains("Fish"))
                {
                    temp2++;
                    temp = temp.Replace("Fish", "Fish x" + temp2.ToString());
                }
                else
                {
                    temp += "Fish\n";
                }
            }
            else if (cargo.Contains("Oil"))
            {
                if (temp.Contains("Oil"))
                {
                    temp3++;
                    temp = temp.Replace("Oil", "Oil x" + temp3.ToString());
                }
                else
                {
                    temp += "Oil\n";
                }
            }
            else if (cargo.Contains("Wood"))
            {
                if (temp.Contains("Wood"))
                {
                    temp4++;
                    temp = temp.Replace("Wood", "Wood x" + temp4.ToString());
                }
                else
                {
                    temp += "Wood\n";
                }
            }
            else if (cargo.Contains("Brick"))
            {
                if (temp.Contains("Brick"))
                {
                    temp5++;
                    temp = temp.Replace("Brick", "Brick x" + temp5.ToString());
                }
                else
                {
                    temp += "Brick\n";
                }
            }
            else if (cargo.Contains("Iron"))
            {
                if (temp.Contains("Iron"))
                {
                    temp6++;
                    temp = temp.Replace("Iron", "Iron x" + temp6.ToString());
                }
                else
                {
                    temp += "Iron\n";
                }
            }
            else if (cargo.Contains("Rum"))
            {
                if (temp.Contains("Rum"))
                {
                    temp7++;
                    temp = temp.Replace("Rum", "Rum x" + temp7.ToString());
                }
                else
                {
                    temp += "Rum\n";
                }
            }
            else if (cargo.Contains("Silk"))
            {
                if (temp.Contains("Silk"))
                {
                    temp8++;
                    temp = temp.Replace("Silk", "Silk x" + temp8.ToString());
                }
                else
                {
                    temp += "Silk\n";
                }
            }
            else if (cargo.Contains("Silverware"))
            {
                if (temp.Contains("Silverware"))
                {
                    temp9++;
                    temp = temp.Replace("Silverware", "Silverware x" + temp9.ToString());
                }
                else
                {
                    temp += "Silverware\n";
                }
            }
            else if (cargo.Contains("Emerald"))
            {
                if (temp.Contains("Emerald"))
                {
                    temp10++;
                    temp = temp.Replace("Emerald", "Emerald x" + temp10.ToString());
                }
                else
                {
                    temp += "Emerald\n";
                }
            }
        }
        return temp;
    }
    internal void UpdateEnemies()
    {
        string temp3 = LevelObjective.GetComponent<Text>().text;
        temp3 = "Level " + ButtonHandler.level.ToString() + "\n";
        foreach (var item in Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name.Contains("Enemy")))
        {
            enemies.Add(item.name);
        }        
        foreach (string item in enemies)
        {
            int temp = 1, temp1 = 1, temp2 = 1;
            if (temp3.Contains("Brigantine"))
            {
                temp++;
                string[] tempo = temp3.Split('\n');
                for (int i = 0; i < temp3.Split('\n').Length; i++)
                {
                    if (temp3.Split('\n')[i].Contains("Brigantine"))
                    {
                        tempo[i] = "Brigantine x" + temp.ToString();
                    }
                }
                temp3 = string.Join("\n", tempo);
            }
            else if(item.Contains("Brigantine"))
            {
                temp3 += "Brigantine\n";
            }
            if (temp3.Contains("Sloop"))
            {
                temp1++;
                string[] tempo = temp3.Split('\n');
                for (int i = 0; i < temp3.Split('\n').Length; i++)
                {
                    if (temp3.Split('\n')[i].Contains("Sloop"))
                    {
                        tempo[i] = "Sloop x" + temp1.ToString();
                    }
                }
                temp3 = string.Join("\n", tempo);
            }
            else if(item.Contains("Sloop"))
            {
                temp3 += "Sloop\n";
            }
            if (temp3.Contains("Caravel"))
            {
                temp2++;
                string[] tempo = temp3.Split('\n');
                for (int i = 0; i < temp3.Split('\n').Length; i++)
                {
                    if (temp3.Split('\n')[i].Contains("Caravel"))
                    {
                        tempo[i] = "Caravel x" + temp1.ToString();
                    }
                }
                temp3 = string.Join("\n", tempo);
            }
            else if(item.Contains("Caravel"))
            {
                temp3 += "Caravel\n";
            }
        }
        foreach (var item in Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name.Contains("Enemy")))
        {
            enemies.Remove(item.name);
        }
        LevelObjective.GetComponent<Text>().text = temp3;
    }
}