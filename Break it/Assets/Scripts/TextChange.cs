using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextChange : MonoBehaviour
{
    
    
    [SerializeField]
    private GameObject text;

    public void Start()
    {
        ChangeText(GameController.Instance.win);
    }

    public void ChangeText(bool win)
    {
        if (win)
        {
            text.GetComponent<TMP_Text>().text = "You Win";
        }
        else
        {
            text.GetComponent<TMP_Text>().text = "You Lose";
        }
    }
}
