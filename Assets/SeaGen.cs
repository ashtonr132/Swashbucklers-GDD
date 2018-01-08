using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaGen : MonoBehaviour {
    GameObject SeaChunk;
	// Use this for initialization
	void Start ()
    {
       SeaChunk = GameObject.Find("Sea/Sea Chunk"); //chunks are made of 3x3 tiles, 4x4 in size
	}
	
	// Update is called once per frame
	void Update ()
    {
        try
        {
            Vector2 chunk = Physics2D.OverlapPoint(GameObject.Find("Player_Ship").transform.position).transform.parent.position;
            GenerateSea(chunk + new Vector2(12,0));
            GenerateSea(chunk + new Vector2(-12,0));
            GenerateSea(chunk + new Vector2(0,12));
            GenerateSea(chunk + new Vector2(0,-12));
            GenerateSea(chunk + new Vector2(12,12));
            GenerateSea(chunk + new Vector2(12,-12));
            GenerateSea(chunk + new Vector2(-12,12));
            GenerateSea(chunk + new Vector2(-12,-12));
        }
        catch (System.Exception)
        {
            //ignore, this will catch every time the ship isnt one a regular sea tile
        }
	}
    void GenerateSea(Vector2 pos)
    {
        bool e = true;
        foreach (Transform t in GameObject.Find("Sea").transform)
        {
            if ((Vector2)t.position == pos)
            {
                e = false;
            }
        }
        if (e)
        {
            GameObject SC = Instantiate(SeaChunk, pos, Quaternion.identity, GameObject.Find("Sea").transform);
            SC.name = "Sea Chunk " + transform.childCount.ToString();
        }
    }
}
