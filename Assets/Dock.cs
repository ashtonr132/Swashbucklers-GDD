using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dock : MonoBehaviour {
    GameObject player, deathmessage;
	// Use this for initialization
	void Start ()
    {
        player = GameObject.Find("Player_Ship");
        deathmessage = GameObject.Find("Canvas").transform.GetChild(0).gameObject;
	}
    private void Update()
    {
        if (player != null && Vector2.Distance(player.transform.position, transform.position) >= 2f)
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }
        if (player != null && deathmessage.activeSelf == false && Vector2.Distance(player.transform.position, transform.position) <= 2f && GetComponent<BoxCollider2D>().isTrigger)
        {
            SceneManager.LoadScene("Market");
        }
    }
}
