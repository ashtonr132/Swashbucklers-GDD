using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAntiRotation : MonoBehaviour {
    GameObject player;
	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player_Ship");
    }
	// Update is called once per frame
	void Update ()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y +3, player.transform.position.z -20);
	}
}
