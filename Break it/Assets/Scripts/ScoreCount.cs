using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCount : MonoBehaviour
{
    public static int HitCount;
    public Text score;
    public Text highestText;
    private int highest;
    [SerializeField]
    private bool isInfinity = false;


    void Start()
    {
        HitCount = 0;  
        if (isInfinity)
        {
            highest = PlayerPrefs.GetInt("highest",0);
            highestText.text = highest.ToString();  
        }
    }

    void Update()
    {   
        score.text = HitCount.ToString();
        if(isInfinity && HitCount > highest)
        {
            highest = HitCount;
            PlayerPrefs.SetInt("highest",HitCount);
            highestText.text = highest.ToString();

        }
    }
}
