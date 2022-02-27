using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardCount : MonoBehaviour
{
    public static int CircleCount;
    public Text score;

    void Start()
    {
        CircleCount = PlayerPrefs.GetInt("total", 0);    
    }

    void Update()
    {
        score.text = CircleCount.ToString();
    }
}