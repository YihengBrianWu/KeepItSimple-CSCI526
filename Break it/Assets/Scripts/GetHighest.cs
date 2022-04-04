using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetHighest : MonoBehaviour
{
    public Text score;
    private int highest;
    void Start()
    {
        highest = PlayerPrefs.GetInt("highest",0);
        score.text = highest.ToString();
    }

}
