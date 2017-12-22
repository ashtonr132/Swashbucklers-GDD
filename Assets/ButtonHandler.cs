using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            GameObject.Find("Canvas/Play").GetComponent<Button>().onClick.AddListener(delegate { LoadManageScene("Shipyard"); });
            // GameObject.Find("Canvas/Settings").GetComponent<Button>().onClick.AddListener(delegate { LoadManageScene("Settings"); });
            GameObject.Find("Canvas/Exit").GetComponent<Button>().onClick.AddListener(ExitGame);
        }
        if (SceneManager.GetActiveScene().name == "Market")
        {
            GameObject.Find("Canvas/Save and Quit").GetComponent<Button>().onClick.AddListener(delegate { LoadManageScene("Main Menu"); });
            GameObject.Find("Canvas/Docks").GetComponent<Button>().onClick.AddListener(delegate { LoadManageScene("Shipyard"); });
            GameObject.Find("Canvas/Start Combat").GetComponent<Button>().onClick.AddListener(delegate { LoadManageScene("Naval Combat"); });
        }
        if (SceneManager.GetActiveScene().name == "Naval Combat")
        {

        }
        if (SceneManager.GetActiveScene().name == "Shipyard")
        {
            GameObject.Find("Canvas/Save and Quit").GetComponent<Button>().onClick.AddListener(delegate { LoadManageScene("Main Menu"); });
            GameObject.Find("Canvas/Market").GetComponent<Button>().onClick.AddListener(delegate { LoadManageScene("Market"); });
            GameObject.Find("Canvas/Start Combat").GetComponent<Button>().onClick.AddListener(delegate { LoadManageScene("Naval Combat"); });
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void LoadManageScene(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }
    void ExitGame()
    {

        Application.Quit();
    }
}
