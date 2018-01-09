using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaGen : MonoBehaviour {
    GameObject SeaChunk;
    GameObject chunk;
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
            Collider2D[] overlaps = Physics2D.OverlapPointAll(GameObject.Find("Player_Ship").transform.position);
            foreach (Collider2D cols in overlaps)
            {
                if (cols.transform.name.Contains("Chunk"))
                {
                    chunk = cols.gameObject;
                }
            }
            GenerateSea((Vector2)chunk.transform.position + new Vector2(12, 0));
            GenerateSea((Vector2)chunk.transform.position + new Vector2(-12, 0));
            GenerateSea((Vector2)chunk.transform.position + new Vector2(0, 12));
            GenerateSea((Vector2)chunk.transform.position + new Vector2(0, -12));
            GenerateSea((Vector2)chunk.transform.position + new Vector2(12, 12));
            GenerateSea((Vector2)chunk.transform.position + new Vector2(12, -12));
            GenerateSea((Vector2)chunk.transform.position + new Vector2(-12, 12));
            GenerateSea((Vector2)chunk.transform.position + new Vector2(-12, -12));
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
            if (t.name.Contains("Chunk"))
            {
                if ((Vector2)t.position == pos)
                {
                    e = false;
                }
            }
        }
        if (e)
        {
            GameObject SC = Instantiate(SeaChunk, pos, Quaternion.identity, GameObject.Find("Sea").transform);
            SC.name = "Sea Chunk " + transform.childCount.ToString();
        }
        else
        {
            if (PlayerControls.enemies.Count > 0 )
            {
                GameObject enemy = Instantiate((GameObject)Resources.Load(PlayerControls.enemies[0]), pos, Quaternion.identity, null);
                enemy.name = "Enemy " + PlayerControls.enemies[0];
                PlayerControls.enemies.RemoveAt(0);
                enemy.GetComponent<EnemyBehavior>().floataround = chunk;
            }
        }
    }
}
