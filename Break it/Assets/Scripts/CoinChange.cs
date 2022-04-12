using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinChange : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI CoinsTXT;

    void Start()
    {
        CoinsTXT.text = "Coins: " + PlayerPrefs.GetInt("total", 0);
    }

    void Update()
    {
        CoinsTXT.text = "Coins: " + PlayerPrefs.GetInt("total", 0);
    }
}
