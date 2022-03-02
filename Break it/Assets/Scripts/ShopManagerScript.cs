using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ShopManagerScript : MonoBehaviour
{

    public int[,] shopItems = new int[4,2];
    public Text CoinsTXT;


    // Start is called before the first frame update
    void Start()
    {
        CoinsTXT.text = "Coins: " + PlayerPrefs.GetInt("total");

        //ID
        shopItems[1, 1] = 1;

        //price
        shopItems[2, 1] = 10;

        //Quantity
        shopItems[3, 1] = PlayerPrefs.GetInt("item1",0);
    }

    // Update is called once per frame
    public void Buy()
    {   
        GameObject buttonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        int id = buttonRef.GetComponent<Goods>().ItemID;
        int coin = PlayerPrefs.GetInt("total");
        int price = shopItems[2, id];
        if (coin >= price && shopItems[3, id] == 0) {
            PlayerPrefs.SetInt("total", coin-price);
            shopItems[3, id] = 1;
            CoinsTXT.text = "Coins: " + PlayerPrefs.GetInt("total");
            PlayerPrefs.SetInt("item"+id.ToString(), 1);
            buttonRef.GetComponent<Goods>().quantity.text = "1";
        }
    }

    public void Back() {
        SceneManager.LoadScene(GameController.Instance.currentScene);
    }
}
