using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    internal float MinSpeed = 0.1f, MaxSpeed = 3, HullHp = 100, cannonballdamage = 1, turnrate = 4, acceleration = 1.1f, firecooldownL, firecooldownR, firerate = 3, quality, gold;
    internal int Cargo;
    internal GameObject floataround, player;
    internal AudioClip shootsound;
    void Start()
    {
        if (transform.name == "Sloop")
        {
            gold = Random.Range(2, 8);
            Cargo = Random.Range(0,2);
            HullHp = 25;
            cannonballdamage = 15;
            turnrate *= 1;
            quality = 0.8f;
        }
        else if (transform.name == "Caravel")
        {
            gold = Random.Range(4, 15);
            Cargo = Random.Range(1, 4);
            HullHp = 50;
            cannonballdamage = 30;
            turnrate *= 1.2f;
            quality = 0.75f;
        }
        else if (transform.name == "Brigantine")
        {
            gold = Random.Range(8, 25);
            Cargo = Random.Range(3, 8);
            HullHp = 90;
            cannonballdamage = 50;
            turnrate *= 1.5f;
            quality = 0.7f;
        }
        rb = transform.GetComponent<Rigidbody2D>();
        firecooldownL = firerate;
        firecooldownR = firerate;
        player = GameObject.Find("Player_Ship");
        shootsound = (AudioClip)Resources.Load("gun");
    }
    void Update()
    {
        if (HullHp <= 0)
        {
            if (transform.name == "Sloop")
            {
                for (int i = 0; i <= Cargo; i++)
                {
                    int loot = Random.Range(1, 100);
                    if (loot <= 25)
                    {
                        CreateCargo("Grain");
                    }
                    else if (loot <= 25 + 22)
                    {
                        CreateCargo("Fish");
                    }
                    else if (loot <= 25 + 22 + 18)
                    {
                        CreateCargo("Oil");
                    }
                    else if (loot <= 25 + 22 + 18 + 13)
                    {
                        CreateCargo("Wood");
                    }
                    else if (loot <= 25 + 22 + 18 + 13 + 10)
                    {
                        CreateCargo("Brick");
                    }
                    else if (loot <= 25 + 22 + 18 + 13 + 10 + 8)
                    {
                        CreateCargo("Iron");
                    }
                    else if (loot <= 25 + 22 + 18 + 13 + 10 + 8 + 4)
                    {
                        CreateCargo("Rum");
                    }
                }
            }
            if (transform.name == "Caravel")
            {
                for (int i = 0; i <= Cargo; i++)
                {
                    int loot = Random.Range(1, 100);
                    if (loot <= 22)
                    {
                        CreateCargo("Grain");
                    }
                    else if (loot <= 22 + 19)
                    {
                        CreateCargo("Fish");
                    }
                    else if (loot <= 22 + 19 + 15)
                    {
                        CreateCargo("Oil");
                    }
                    else if (loot <= 22 + 19 + 15 + 12)
                    {
                        CreateCargo("Wood");
                    }
                    else if (loot <= 22 + 19 + 15 + 12 + 11)
                    {
                        CreateCargo("Brick");
                    }
                    else if (loot <= 22 + 19 + 15 + 12 + 11 + 9)
                    {
                        CreateCargo("Iron");
                    }
                    else if (loot <= 22 + 19 + 15 + 12 + 11 + 9 + 6)
                    {
                        CreateCargo("Rum");
                    }
                    else if (loot <= 22 + 19 + 15 + 12 + 11 + 9 + 6 + 4)
                    {
                        CreateCargo("Silk");
                    }
                    else if (loot <= 22 + 19 + 15 + 12 + 11 + 9 + 6 + 4 + 2)
                    {
                        CreateCargo("Silverware");
                    }
                }
            }
            if (transform.name == "Brigantine")
            {
                int loot = Random.Range(1, 100);
                if (loot <= 18)
                {
                    CreateCargo("Grain");
                }
                else if (loot <= 18 + 16)
                {
                    CreateCargo("Fish");
                }
                else if (loot <= 18 + 16 + 13)
                {
                    CreateCargo("Oil");
                }
                else if (loot <= 18 + 16 + 13 + 12)
                {
                    CreateCargo("Wood");
                }
                else if (loot <= 18 + 16 + 13 + 12 + 11)
                {
                    CreateCargo("Brick");
                }
                else if (loot <= 18 + 16 + 13 + 12 + 11 + 10)
                {
                    CreateCargo("Iron");
                }
                else if (loot <= 18 + 16 + 13 + 12 + 11 + 10 + 8)
                {
                    CreateCargo("Rum");
                }
                else if (loot <= 18 + 16 + 13 + 12 + 11 + 10 + 8 + 5)
                {
                    CreateCargo("Silk");
                }
                else if (loot <= 18 + 16 + 13 + 12 + 11 + 10 + 8 + 5 + 4)
                {
                    CreateCargo("Silverware");
                }
                else if (loot <= 18 + 16 + 13 + 12 + 11 + 10 + 8 + 5 + 4 + 3)
                {
                    CreateCargo("Emerald");
                }
            }
            AudioSource.PlayClipAtPoint((AudioClip)Resources.Load("sink"), GameObject.Find("Music").transform.position, 0.2f);
            PlayerControls.gold += gold;
            foreach (Transform item in transform)
            {
                Destroy(item.gameObject);
            }
            GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("brokenship")[0];
            transform.rotation = Quaternion.Euler(0,0,180);
            transform.transform.localScale /= 8;
            Destroy(gameObject, 1);
            Destroy(GetComponent<EnemyBehavior>());
        }
        if (firecooldownL >= 0)
        {
            firecooldownL -= Time.deltaTime;
        }
        if (firecooldownR >= 0)
        {
            firecooldownR -= Time.deltaTime;
        }
        if (player != null && Vector2.Distance(transform.position, player.transform.position) >= 12)
        {
            if (Physics2D.OverlapPoint(transform.position).gameObject == floataround)
            {
                floataround = GameObject.Find("Sea").transform.GetChild(Random.Range(0, GameObject.Find("Sea").transform.childCount)).GetChild(Random.Range(0, 8)).gameObject;
                Vector3 diff = (floataround.transform.position - transform.position).normalized;
                transform.rotation = Quaternion.Euler(0f, 0f, (Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg) - 90);
            }
            rb.velocity = (rb.velocity / 10 * 9) + (Vector2)transform.up / 10;
            rb.velocity = rb.velocity.normalized * Mathf.Clamp(Mathf.Abs(rb.velocity.magnitude), MinSpeed, MaxSpeed);
        }
        else if (player != null && Vector2.Distance(transform.position, player.transform.position) <= 12 && Vector2.Distance(transform.position, player.transform.position) >= 8)
        {
            Vector3 diff = (player.transform.position - transform.position).normalized;
            transform.rotation = Quaternion.Euler(0f, 0f, (Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg) - 90);
            rb.velocity = (rb.velocity / 10 * 9) + (Vector2)transform.up / 10;
            rb.velocity = rb.velocity.normalized * Mathf.Clamp(Mathf.Abs(rb.velocity.magnitude), MinSpeed, MaxSpeed);
        }
        else if(player != null)
        {
            Vector3 diff = (player.transform.position - transform.position).normalized;
            if (Quaternion.Angle(player.transform.rotation, Quaternion.Euler(0f, 0f, (Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg))) < Quaternion.Angle(player.transform.rotation, Quaternion.Euler(0f, 0f, (Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg) - 180)))
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, (Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg)), Time.deltaTime / turnrate);
                if (firecooldownL <= 0)
                {
                    firecooldownL = firerate;
                    GameObject CannonBall = Instantiate(Resources.Load("CannonBallPrefab") as GameObject);
                    CannonBall.name = "CannonBall Enemy";
                    CannonBall.transform.position = transform.position;
                    CannonBall.GetComponent<Rigidbody2D>().velocity = ((Vector2)(transform.right * 10) + GetComponent<Rigidbody2D>().velocity) / 3;
                    AudioSource.PlayClipAtPoint(shootsound, GameObject.Find("Music").transform.position, 1);
                    CannonBall.transform.SetParent(transform);
                    StartCoroutine(PlayerControls.AnimCount(new GameObject[] { CannonBall}));
                }
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, (Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg) - 180), Time.deltaTime / turnrate);
                if (firecooldownR <= 0)
                {
                    firecooldownR = firerate;
                    GameObject CannonBall = Instantiate(Resources.Load("CannonBallPrefab") as GameObject);
                    CannonBall.name = "CannonBall " + transform.name;
                    CannonBall.transform.position = transform.position;
                    CannonBall.name = "CannonBall Enemy";
                    CannonBall.GetComponent<Rigidbody2D>().velocity = ((Vector2)(-transform.right * 10) + GetComponent<Rigidbody2D>().velocity) / 3;
                    AudioSource.PlayClipAtPoint(shootsound, GameObject.Find("Music").transform.position, 1);
                    CannonBall.transform.SetParent(transform);
                    StartCoroutine(PlayerControls.AnimCount(new GameObject[]{CannonBall}));
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("CannonBall") && collision.gameObject.name.Contains("Player"))
        {
           HullHp -= (PlayerControls.cannonballdamage + Random.Range((PlayerControls.cannonballdamage*-30/100) , (PlayerControls.cannonballdamage * 30 / 100)) * quality);
           AudioSource.PlayClipAtPoint((AudioClip)Resources.Load("cannonballhit"), GameObject.Find("Music").transform.position, 0.2f);
           Destroy(collision.gameObject);
        }
    }
    internal void CreateCargo(string cargotype)
    {
        GameObject cargo = new GameObject("Cargo" + cargotype, typeof(SpriteRenderer), typeof(Rigidbody2D));
        cargo.transform.position = transform.position + Random.onUnitSphere;
        cargo.GetComponent<SpriteRenderer>().sortingOrder = 1;
        cargo.transform.localScale /= 15;
        cargo.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Cargo/crate");
        Destroy(cargo, 20);
    }
}