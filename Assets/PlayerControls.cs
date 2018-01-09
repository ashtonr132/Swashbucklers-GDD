using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class PlayerControls : MonoBehaviour
{
    private Rigidbody2D rb;
    internal float MinSpeed = 0.1f, MaxSpeed = 3, firecooldownL, firecooldownR, firerate = 3;
    internal static float gold = 0, quality = 0.95f, acceleration = 1.1f, turnrate = 2, HullHp = 100, SailHp = 100, cannonballdamage = 20, storagecapacity = 10, level = 1;
    internal static List<string> storage = new List<string>(), enemies = new List<string>();
    internal AudioClip shootsound;
    private GameObject deathmessage, ReloadText;
    public Sprite[] ripple;

    // Use this for initialization
    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        deathmessage = GameObject.Find("Death Message");
        deathmessage.SetActive(false);
        firecooldownL = firerate;
        firecooldownR = firerate;
        ReloadText = GameObject.Find("Reload Text");
        shootsound = (AudioClip)Resources.Load("gun");
        if (enemies.Count == 0) {
            for (int i = 0; i < level; i++)
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
    }

    // Update is called once per frame
    void Update()
    {
        if (enemies.Count <= 0 && Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name.Contains("Enemy")).Count() <= 0)
        {
            StartCoroutine("Win");
        }
        foreach (GameObject cargo in Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name.Contains("Cargo")))
        {
            if (Vector2.Distance(cargo.transform.position, transform.position) <= 2 && storage.Count < storagecapacity)
            {
                storage.Add(cargo.name);
                string temp = "coin" + Random.Range(1,10).ToString();
                AudioSource.PlayClipAtPoint((AudioClip)Resources.Load(temp),GameObject.Find("Music").transform.position, 0.2f);
                Destroy(cargo); 
            }
            else if (Vector2.Distance(cargo.transform.position, transform.position) <= 5)
            {
                cargo.GetComponent<Rigidbody2D>().velocity = (transform.position - cargo.transform.position).normalized;
            }
        }
        ReloadText.transform.position = transform.position;
        ReloadText.transform.rotation = transform.rotation;
        ReloadText.GetComponent<Text>().text = firecooldownL.ToString("F1") + "         " + firecooldownR.ToString("F1");
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = ripple[(int)(Random.Range(0, 3.99f))];
        if (firecooldownL >= 0)
        {
            firecooldownL -= Time.deltaTime;
        }
        if (firecooldownR >= 0)
        {
            firecooldownR -= Time.deltaTime;
        }
        if (HullHp <= 0)
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
                rb.AddForceAtPosition(-transform.right / turnrate * SailHp/100, (Vector2)transform.position + (Vector2)transform.up - (Vector2)transform.right);
                transform.GetChild(0).rotation = Quaternion.Slerp(transform.GetChild(0).rotation, transform.rotation * Quaternion.Euler(0,0,10), Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                rb.AddForceAtPosition(transform.right / turnrate * SailHp / 100, (Vector2)transform.position + (Vector2)transform.up + (Vector2)transform.right);
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
        rb.velocity = ((rb.velocity / 10 * 9) + (Vector2)transform.up / 10).normalized * Mathf.Clamp(Mathf.Abs(rb.velocity.magnitude), MinSpeed, MaxSpeed *SailHp/100);
    }

    private IEnumerator Win()
    {
        deathmessage.SetActive(true);
        deathmessage.GetComponent<Text>().text = "Level Complete";
        deathmessage.transform.position = transform.position;
        yield return new WaitForSeconds(10);
        level++;
        SceneManager.LoadScene("ShipYard");
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
        SceneManager.LoadScene("Shipyard");
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
            if(Random.Range(1,100) >= 20)
            {
                HullHp -= (collision.transform.parent.GetComponent<EnemyBehavior>().cannonballdamage + Random.Range((collision.transform.parent.GetComponent<EnemyBehavior>().cannonballdamage *-30/100), (collision.transform.parent.GetComponent<EnemyBehavior>().cannonballdamage*30/100))) * quality;
            }
            else
            {
                SailHp -= (collision.transform.parent.GetComponent<EnemyBehavior>().cannonballdamage + Random.Range(-30, 30)) * quality;
            }
            AudioSource.PlayClipAtPoint((AudioClip)Resources.Load("cannonballhit"), GameObject.Find("Music").transform.position, 0.2f);
            Destroy(collision.gameObject);  
        }
    }
}