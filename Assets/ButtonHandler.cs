using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject.Find("Canvas/Play").GetComponent<Button>().onClick.AddListener(ButtonFunc);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void ButtonFunc()
    {
        SceneManager.LoadScene("Management");
    }
}
