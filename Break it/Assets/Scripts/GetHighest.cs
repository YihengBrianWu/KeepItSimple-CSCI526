using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetHighest : MonoBehaviour
{
    public Text score;
    public Text scoreToApple;
    private int highest;
    [SerializeField]
    private bool isInfinityMode = false;
    void Start()
    {
        if (!isInfinityMode)
        {
            highest = PlayerPrefs.GetInt("highest",0);
            score.text = highest.ToString();
        }
        else
        {
            highest = GameController.Instance.hitOnLog;
            score.text = highest.ToString();
            int castInt = highest / 5;
            scoreToApple.text = castInt.ToString();

        }
    }

}
