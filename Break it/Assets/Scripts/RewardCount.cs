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
        CircleCount = 0;    
    }

    void Update()
    {
        score.text = CircleCount.ToString();
    }
}