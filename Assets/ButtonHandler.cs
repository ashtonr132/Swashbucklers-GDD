using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    internal static int USSail = 15, USDamage = 25, USHull = 10, USStorage = 5, USQuality = 20, level = 1;
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
            GameObject.Find("Sail").GetComponent<Button>().onClick.AddListener(delegate { UpgradeShip("Sail"); });
            GameObject.Find("Damage").GetComponent<Button>().onClick.AddListener(delegate { UpgradeShip("Damage"); });
            GameObject.Find("Hull").GetComponent<Button>().onClick.AddListener(delegate { UpgradeShip("Hull"); });
            GameObject.Find("Storage").GetComponent<Button>().onClick.AddListener(delegate { UpgradeShip("Storage"); });
            GameObject.Find("Quality").GetComponent<Button>().onClick.AddListener(delegate { UpgradeShip("Quality"); });
        }
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Market" || SceneManager.GetActiveScene().name == "Shipyard")
        {
            GameObject.Find("current gold").GetComponent<Text>().text = "Current Gold : " + PlayerControls.gold.ToString();
            if (SceneManager.GetActiveScene().name == "Shipyard")
            {
                GameObject.Find("Sail").transform.GetChild(0).GetComponent<Text>().text = "Upgrade Sail;\nCost " + USSail.ToString() + ";\nCurrent Value " + PlayerControls.sailqual +"/2.0" + ";\nAffects;\nManeuverability – Turn Speed\n";
                GameObject.Find("Damage").transform.GetChild(0).GetComponent<Text>().text = "Upgrade Damage;\nCost " + USDamage.ToString() + ";\nCurrent Value " + PlayerControls.cannonballdamage + "/65" + ";\nAffects;\nCannonBall Damage";
                GameObject.Find("Hull").transform.GetChild(0).GetComponent<Text>().text = "Upgrade Hull;\nCost " + USHull.ToString() + ";\nCurrent Value " + PlayerControls.HullHp + "/150" + ";\nAffects;\nShip Health";
                GameObject.Find("Storage").transform.GetChild(0).GetComponent<Text>().text = "Upgrade Storage;\nCost " + USStorage.ToString() + ";\nCurrent Value " + PlayerControls.storagecapacity + "/60" + ";\nAffects; \nCargo Capacity";
                GameObject.Find("Quality").transform.GetChild(0).GetComponent<Text>().text = "Upgrade Ship Quality; \nCost " + USQuality.ToString() + ";\nCurrent Value " + PlayerControls.quality + "/0.5" + ";\nAffects;\nShip Armour";
            }
        }
    }
    void LoadManageScene(string scenename)
    {
        AudioSource.PlayClipAtPoint((AudioClip)Resources.Load("click"), GameObject.Find("Music").transform.position, 0.3f);
        SceneManager.LoadScene(scenename);
    }
    void ExitGame()
    {
        AudioSource.PlayClipAtPoint((AudioClip)Resources.Load("click"), GameObject.Find("Music").transform.position, 0.3f);
        Application.Quit();
    }
    void SellAll()
    {
        AudioSource.PlayClipAtPoint((AudioClip)Resources.Load("click"), GameObject.Find("Music").transform.position, 0.3f);
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
        string temp = "coin" + Random.Range(1, 10).ToString();
        AudioSource.PlayClipAtPoint((AudioClip)Resources.Load(temp), GameObject.Find("Music").transform.position, 0.2f);
    }
    void UpgradeShip(string name)
    {
        AudioSource.PlayClipAtPoint((AudioClip)Resources.Load("click"), GameObject.Find("Music").transform.position, 0.3f);
        switch (name)
        {
            case "Sail":
                if (PlayerControls.gold >= USSail && PlayerControls.sailqual < 2)
                {
                    PlayerControls.gold -= USSail;
                    PlayerControls.sailqual += 0.1f;
                    USSail += 5;
                }
                break;
            case "Damage":
                if (PlayerControls.gold >= USDamage && PlayerControls.cannonballdamage < 65)
                {
                    PlayerControls.gold -= USDamage;
                    PlayerControls.cannonballdamage += 5;
                    USDamage += 5;
                }
                break;
            case "Hull":
                if (PlayerControls.gold >= USHull && PlayerControls.HullHp < 150)
                {
                    PlayerControls.gold -= USHull;
                    PlayerControls.HullHp += 10;
                    USHull += 5;
                }
                break;
            case "Storage":
                if (PlayerControls.gold >= USStorage && PlayerControls.storagecapacity < 60)
                {
                    PlayerControls.gold -= USStorage;
                    PlayerControls.storagecapacity += 5;
                    USStorage += 5;
                }
                break;
            case "Quality":
                if (PlayerControls.gold >= USQuality && PlayerControls.quality > 0.5f)
                {
                    PlayerControls.gold -= USQuality;
                    PlayerControls.quality -= 0.05f;
                    USQuality += 5;
                }
                break;
        }
    }
}
