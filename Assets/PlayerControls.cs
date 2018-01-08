using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour
{
    private Rigidbody2D rb;
    internal float MinSpeed = 0.1f, MaxSpeed = 3, HullHp = 100, SailHp = 100, cannonballdamage = 1, turnrate = 2, acceleration = 1.1f, firecooldownL, firecooldownR, firerate = 3;
    internal int storagesize = 100;
    internal AudioClip shootsound;
    internal GameObject[] Storage;
    private GameObject deathmessage, ReloadText;
    public Sprite[] ripple;

    // Use this for initialization
    void Start()
    {
       rb = transform.GetComponent<Rigidbody2D>();
       Storage = new GameObject[8];
       deathmessage = GameObject.Find("Death Message");
       deathmessage.SetActive(false);
       firecooldownL = firerate;
       firecooldownR = firerate;
       ReloadText = GameObject.Find("Reload Text");
       shootsound = (AudioClip)Resources.Load("gun");
    }

    // Update is called once per frame
    void Update()
    {
        print(SailHp);
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
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Shoot(-transform.right);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Shoot(transform.right);
        }
        if (Physics2D.OverlapPoint(transform.position).transform.name.Contains("classic"))
        {
            if (Input.GetKey(KeyCode.A))
            {
                rb.AddForceAtPosition(-transform.right / turnrate, (Vector2)transform.position + (Vector2)transform.up - (Vector2)transform.right);
            }
            if (Input.GetKey(KeyCode.D))
            {
                rb.AddForceAtPosition(transform.right / turnrate, (Vector2)transform.position + (Vector2)transform.up + (Vector2)transform.right);
            }
            if (Input.GetKey(KeyCode.W))
            {
                rb.velocity = rb.velocity * acceleration;
            }
        }
#endif
#if Unity_Andriod //mobile control code
        if (Physics2D.OverlapPoint(transform.position).transform.name.Contains("classic"))
        {
        
        }
#endif
        else if (Physics2D.OverlapPoint(transform.position).transform.name.Contains("Dock") && GameObject.Find("Dock").GetComponent<BoxCollider2D>().isTrigger == false)
        {
            rb.velocity += ((Vector2)transform.position + (Vector2)transform.up);
        }
        rb.velocity = (rb.velocity/10*9) + (Vector2)transform.up/10;
        rb.velocity = rb.velocity.normalized * Mathf.Clamp(Mathf.Abs(rb.velocity.magnitude), MinSpeed, MaxSpeed);
    }
    private IEnumerator Death()
    {
        deathmessage.SetActive(true);
        deathmessage.transform.position = transform.position;
        yield return new WaitForSeconds(4);
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
        foreach (GameObject CannonBall in array)
        {
            CannonBall.name = "Splash";
            CannonBall.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            CannonBall.GetComponent<SpriteRenderer>().sprite = sprites[0];
            CannonBall.transform.localScale *= 25;
            Destroy(CannonBall.GetComponent<CircleCollider2D>());
        }
        yield return new WaitForSeconds(0.25f);
        foreach (GameObject CannonBall in array)
        {
            CannonBall.GetComponent<SpriteRenderer>().sprite = sprites[1];
        }
        yield return new WaitForSeconds(0.25f);
        foreach (GameObject CannonBall in array)
        {
            CannonBall.GetComponent<SpriteRenderer>().sprite = sprites[2];
        }
        yield return new WaitForSeconds(0.25f);
        foreach (GameObject CannonBall in array)
        {
            CannonBall.GetComponent<SpriteRenderer>().sprite = sprites[3];
        }
        yield return new WaitForSeconds(0.25f);
        foreach (GameObject CannonBall in array)
        {
            Destroy(CannonBall);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("CannonBall"))
        {
            SailHp -= 1;
        }
    }
}