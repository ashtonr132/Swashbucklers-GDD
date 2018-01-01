using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControls : MonoBehaviour
{
    private Rigidbody2D rb;
    internal float MinSpeed = 0.1f, MaxSpeed = 3, HullHp = 100, SailHp = 100, cannonballdamage = 1, turnrate = 2, acceleration = 1.1f, firecooldown, firerate = 10;
    internal int storagesize = 100;
    internal GameObject[] Storage;
    private GameObject deathmessage;
    public Sprite[] ripple;
    // Use this for initialization
    void Start()
    {
       rb = transform.GetComponent<Rigidbody2D>();
       Storage = new GameObject[8];
       deathmessage = GameObject.Find("Death Message");
       deathmessage.SetActive(false);
       firecooldown = firerate;
    }

    // Update is called once per frame
    void Update()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = ripple[(int)(Random.Range(0, 3.99f))];
        firecooldown -= Time.deltaTime;
        if (HullHp <= 0)
        {
            StartCoroutine("Death");
        }
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Shoot(-transform.right);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Shoot(transform.up);
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
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("Shipyard");
    }
    private void Shoot(Vector2 Direction)
    {
        if (firecooldown <= 0)
        {
            firecooldown = firerate;
            GameObject CannonBall = Instantiate(Resources.Load("CannonBallPrefab") as GameObject);
            CannonBall.transform.position = transform.position;
            CannonBall.GetComponent<Rigidbody2D>().velocity = (Direction * 10) + GetComponent<Rigidbody2D>().velocity;
            Destroy(CannonBall, 3);
        }
    }
}
