using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ShopManagerScript : MonoBehaviour
{

    public int[,] shopItems = new int[4,4];


    void Start()
    {
        //ID
        shopItems[1, 1] = 1;
        shopItems[1, 2] = 2;
        shopItems[1, 3] = 3;

        //price
        shopItems[2, 1] = 10;
        shopItems[2, 2] = 13;
        shopItems[2, 3] = 15;

        //Quantity
        shopItems[3, 1] = PlayerPrefs.GetInt("item1",0);
        shopItems[3, 2] = PlayerPrefs.GetInt("item2",0);
        shopItems[3, 3] = PlayerPrefs.GetInt("item3",0);


    }

    public void Buy()
    {   
        GameObject buttonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        int id = buttonRef.GetComponent<Goods>().ItemID;
        int coin = PlayerPrefs.GetInt("total");
        //int coin = 99;
        int price = shopItems[2, id];
        if (coin >= price && shopItems[3, id] == 0) {
            PlayerPrefs.SetInt("total", coin-price);
            shopItems[3, id] = 1;
            PlayerPrefs.SetInt("item"+id.ToString(), 1);
            buttonRef.GetComponent<Goods>().quantity.text = "1";
        }
    }

    public void Back() {
        SceneManager.LoadScene(GameController.Instance.currentScene);
    }
}
