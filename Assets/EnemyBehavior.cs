using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    internal float MinSpeed = 0.1f, MaxSpeed = 3, HullHp = 100, SailHp = 100, cannonballdamage = 1, turnrate = 4, acceleration = 1.1f, firecooldownL, firecooldownR, firerate = 3;
    internal GameObject floataround, player;
    internal AudioClip shootsound;
    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        firecooldownL = firerate;
        firecooldownR = firerate;
        floataround = Physics2D.OverlapPoint(transform.position).gameObject;
        player = GameObject.Find("Player_Ship");
        shootsound = (AudioClip)Resources.Load("gun");
    }
    void Update()
    {
        if (firecooldownL >= 0)
        {
            firecooldownL -= Time.deltaTime;
        }
        if (firecooldownR >= 0)
        {
            firecooldownR -= Time.deltaTime;
        }
        if (Vector2.Distance(transform.position, player.transform.position) >= 8)
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
        else
        {
            Vector3 diff = (player.transform.position - transform.position).normalized;
            if (Quaternion.Angle(player.transform.rotation, Quaternion.Euler(0f, 0f, (Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg))) < Quaternion.Angle(player.transform.rotation, Quaternion.Euler(0f, 0f, (Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg) - 180)))
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, (Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg)), Time.deltaTime / turnrate);
                if (firecooldownL <= 0)
                {
                    firecooldownL = firerate;
                    GameObject CannonBall = Instantiate(Resources.Load("CannonBallPrefab") as GameObject);
                    CannonBall.transform.position = transform.position;
                    CannonBall.GetComponent<Rigidbody2D>().velocity = (((Vector2)player.transform.position + Random.insideUnitCircle) - (Vector2)transform.position);
                    AudioSource.PlayClipAtPoint(shootsound, GameObject.Find("Music").transform.position, 1);
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
                    CannonBall.transform.position = transform.position;
                    CannonBall.GetComponent<Rigidbody2D>().velocity = (((Vector2)player.transform.position + Random.insideUnitCircle) - (Vector2)transform.position);
                    AudioSource.PlayClipAtPoint(shootsound, GameObject.Find("Music").transform.position, 1);
                    StartCoroutine(PlayerControls.AnimCount(new GameObject[] { CannonBall}));
                }
            }
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