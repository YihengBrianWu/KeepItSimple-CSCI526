using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ShopManagerScript : MonoBehaviour
{

    public int[,] shopItems = new int[4,5];

    public AudioSource music;
    public AudioClip buy;
    public AudioClip changeKnife;

    void Start()
    {
        //ID
        shopItems[1, 1] = 1;
        shopItems[1, 2] = 2;
        shopItems[1, 3] = 3;
        shopItems[1, 4] = 4;

        //price
        shopItems[2, 1] = 10;
        shopItems[2, 2] = 13;
        shopItems[2, 3] = 15;
        shopItems[2, 4] = 5;

        //Quantity
        shopItems[3, 1] = PlayerPrefs.GetInt("item1",0);
        shopItems[3, 2] = PlayerPrefs.GetInt("item2",0);
        shopItems[3, 3] = PlayerPrefs.GetInt("item3",0);
        shopItems[3, 4] = PlayerPrefs.GetInt("item4",0);

        music = gameObject.GetComponent<AudioSource>();
        buy = Resources.Load<AudioClip>("sound/buy");
        changeKnife = Resources.Load<AudioClip>("sound/changeKnife");

    }

    public void Buy()
    {   
        GameObject buttonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        int id = buttonRef.GetComponent<Goods>().ItemID;
        int coin = PlayerPrefs.GetInt("total");
        // int coin = 99;
        int price = shopItems[2, id];
        if (coin >= price && shopItems[3, id] == 0) {
            PlayerPrefs.SetInt("total", coin-price);
            shopItems[3, id] = 1;
            PlayerPrefs.SetInt("item"+id.ToString(), 1);
            buttonRef.GetComponent<Goods>().quantity.text = "1";
            if(id <= 3 && id > 0)
            {
                PlayerPrefs.SetInt("itemSelected", id);
            }
            else if (id == 4)
            {
                PlayerPrefs.SetInt("extraKnife", id);
            }

            music.clip = buy;
            music.Play();
        }
        else if (shopItems[3, id] != 0)
        {
            if (id <= 3 && id > 0)
            {
                PlayerPrefs.SetInt("itemSelected", id);
                
                music.clip = changeKnife;
                music.Play();
            }
            else if (id == 4)
            {
                PlayerPrefs.SetInt("extraKnife", id);
            }
        }
    }

    public void Back() {
        SceneManager.LoadScene(GameController.Instance.currentScene);
    }
}
