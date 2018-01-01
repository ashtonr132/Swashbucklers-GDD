using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaGen : MonoBehaviour {
    GameObject SeaChunk;
	// Use this for initialization
	void Start () {
        GameObject.Find("Sea/Sea Chunk"); //chunks are made of 3x3 tiles, 4x4 in size  
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
