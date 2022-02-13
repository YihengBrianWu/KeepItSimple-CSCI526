using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCount : MonoBehaviour
{
    public static int HitCount;
    public Text score;

    void Start()
    {
        HitCount = 0;    
    }

    void Update()
    {
        score.text = HitCount.ToString();
    }
}
