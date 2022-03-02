using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopManagerScript : MonoBehaviour
{

    public int[,] shopItems = new int[4,2];
    public int coins;
    public Text CoinsTXT;


    // Start is called before the first frame update
    void Start()
    {
        CoinsTXT.text = "Coins: " + coins.ToString();

        //ID
        shopItems[1, 1] = 1;

        //price
        shopItems[2, 1] = 10;

        //Quantity
        shopItems[3, 1] = 0;
    }

    // Update is called once per frame
    public void Buy()
    {   
        GameObject buttonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        int id = buttonRef.GetComponent<Goods>().ItemID;
        int price = shopItems[2, id];
        if (coins >= price && shopItems[3, id] == 0) {
            coins -= price;
            shopItems[3, id] = 1;
            CoinsTXT.text = "Coins: " + coins.ToString();
            buttonRef.GetComponent<Goods>().quantity.text = "1";
        }
    }
}
