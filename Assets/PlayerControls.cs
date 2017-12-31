using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    private Rigidbody2D rb;
    internal float MinSpeed = -3, MaxSpeed = 3;
    // Use this for initialization
    void Start()
    {
       rb = transform.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics2D.OverlapPoint(transform.position).transform.name.Contains("classic"))
        {
#if UNITY_EDITOR

            if (Input.GetKey(KeyCode.A))
            {
                rb.AddForceAtPosition(-transform.right / 3, (Vector2)transform.position + (Vector2)transform.up - (Vector2)transform.right);
            }
            if (Input.GetKey(KeyCode.D))
            {
                rb.AddForceAtPosition(transform.right / 3, (Vector2)transform.position + (Vector2)transform.up + (Vector2)transform.right);
            }
            if (Input.GetKey(KeyCode.W))
            {
                rb.velocity += (Vector2)transform.up;
            }
            if (Input.GetKey(KeyCode.S))
            {
                rb.velocity += (Vector2)(-transform.up/2);
            }
#endif
#if Unity_Andriod
        //mobile control code
#endif
        }
        else if (Physics2D.OverlapPoint(transform.position).transform.name.Contains("dark water"))
        {
            rb.velocity += (Vector2.zero - (Vector2)transform.position);
        }
        else if (Physics2D.OverlapPoint(transform.position).transform.name.Contains("Dock"))
        {
            rb.velocity += ((Vector2)transform.position + (Vector2)transform.up);
        }
        rb.velocity = rb.velocity.normalized * Mathf.Clamp(rb.velocity.magnitude, MinSpeed, MaxSpeed);
    }
}
