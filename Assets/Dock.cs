using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dock : MonoBehaviour {
    GameObject player;
	// Use this for initialization
	void Start ()
    {
        player = GameObject.Find("Player_Ship");
	}
    private void Update()
    {
        if (player != null && Vector2.Distance(player.transform.position, transform.position) >= 2f)
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }
        if (player != null && Vector2.Distance(player.transform.position, transform.position) <= 2f && GetComponent<BoxCollider2D>().isTrigger)
        {
            SceneManager.LoadScene("Shipyard");
        }
    }
}
