using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goods : MonoBehaviour
{
    public int ItemID;
    public Text priceText;
    public Text quantity;
    public GameObject ShopManager;

    // Update is called once per frame
    void Update()
    {
        priceText.text = ShopManager.GetComponent<ShopManagerScript>().shopItems[2, ItemID].ToString() + " Conins";
        quantity.text = ShopManager.GetComponent<ShopManagerScript>().shopItems[3, ItemID].ToString();
    }
}
