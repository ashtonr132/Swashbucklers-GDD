using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    void Start()
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
            GameObject.Find("SellAll").GetComponent<Button>().onClick.AddListener(SellAll);
        }
        /*if (SceneManager.GetActiveScene().name == "Naval Combat")
        {

        }*/
        if (SceneManager.GetActiveScene().name == "Shipyard")
        {
            GameObject.Find("Canvas/Save and Quit").GetComponent<Button>().onClick.AddListener(delegate { LoadManageScene("Main Menu"); });
            GameObject.Find("Canvas/Market").GetComponent<Button>().onClick.AddListener(delegate { LoadManageScene("Market"); });
            GameObject.Find("Canvas/Start Combat").GetComponent<Button>().onClick.AddListener(delegate { LoadManageScene("Naval Combat"); });
        }
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Market" || SceneManager.GetActiveScene().name == "Shipyard")
        {
            GameObject.Find("current gold").GetComponent<Text>().text = "Current Gold : " + PlayerControls.gold.ToString();
        }
    }
    void LoadManageScene(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }
    void ExitGame()
    {
        Application.Quit();
    }
    void SellAll()
    {
        foreach (string cargo in PlayerControls.storage)
        {
            if (cargo.Contains("Grain"))
            {
                PlayerControls.gold += 1;
            }
            else if (cargo.Contains("Fish"))
            {
                PlayerControls.gold += 2;
            }
            else if(cargo.Contains("Oil"))
            {
                PlayerControls.gold += 3;
            }
            else if(cargo.Contains("Wood"))
            {
                PlayerControls.gold += 5;
            }
            else if(cargo.Contains("Brick"))
            {
                PlayerControls.gold += 8;
            }
            else if(cargo.Contains("Iron"))
            {
                PlayerControls.gold += 10;
            }
            else if (cargo.Contains("Rum"))
            {
                PlayerControls.gold += 15;
            }
            else if (cargo.Contains("Silk"))
            {
                PlayerControls.gold += 20;
            }
            else if (cargo.Contains("Silverware"))
            {
                PlayerControls.gold += 30;
            }
            else if (cargo.Contains("Emerald"))
            {
                PlayerControls.gold += 50;
            }
        }
        PlayerControls.storage.Clear();
    }
}
