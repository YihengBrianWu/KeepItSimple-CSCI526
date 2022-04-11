using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Introduction : MonoBehaviour
{
    [SerializeField]
    Text KnivesText;
    Text ScoreText;
    Text PauseText;
    Text ShootText;
    Text PanText;
    GameObject btn;

    // Start is called before the first frame update
    void Start()
    {
        btn = GameObject.Find("Canvas/StartButton");   
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void Delete()
    {
        // KnivesText = GameObject.Find("Canvas/KnivesIntro").GetComponent<Text>();
        // KnivesText.text = "";
        // ScoreText = GameObject.Find("Canvas/ScoreIntro").GetComponent<Text>();
        // ScoreText.text = "";
        // PauseText = GameObject.Find("Canvas/PauseIntro").GetComponent<Text>();
        // PauseText.text = "";
        // ShootText = GameObject.Find("Canvas/ShootIntro").GetComponent<Text>();
        // ShootText.text = "";
        // PanText = GameObject.Find("Canvas/PanIntro").GetComponent<Text>();
        // PanText.text = "";
        GameObject.Find("Canvas/IntroSum").SetActive(false);
        btn.SetActive(false);
    }
}
