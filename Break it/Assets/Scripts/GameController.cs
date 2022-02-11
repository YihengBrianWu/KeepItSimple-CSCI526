using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(GameUI))]
public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    
    // 数量
    [Header("Knife Amount")] 
    [SerializeField] 
    private int knifeCount;
    
    [SerializeField] 
    private int knifeHitLogToWin;
    
    // 位置
    [Header("Knife Spawning")] 
    [SerializeField]
    private Vector2 knifeSpawnPosition;
    
    // knife的prefab对象
    [SerializeField] 
    private GameObject knifeObject;

    // gameUI对象
    public GameUI GameUI { get; private set; }

    private int knifeAmount;
    public bool win = false;

    private void Awake()
    {
        Instance = this;
        GameUI = GetComponent<GameUI>();
        knifeAmount = knifeCount;
    }

    private void Start()
    {
        GameUI.SetInitialDisplayedKnifeCount(knifeCount);
        SpawnKnife();
    }
    
    // 统计有多少射到log上有多少射到knife上
    private int hitOnLog = 0;
    private int hitOnKnife = 0;

    public void hitOnLogInc()
    {
        hitOnLog++;
    }

    public void hitOnKnifeInc()
    {
        hitOnKnife++;
    }
    public void OnSuccessfulKnifeHit()
    {
        if (hitOnLog >= knifeHitLogToWin)
        {
            win = true;
            SceneManager.LoadScene(2);
            return;
        }

        if (hitOnKnife > (knifeAmount - knifeHitLogToWin))
        {
            SceneManager.LoadScene(2);
            return;
        }
        
        if (knifeCount > 0)
        {
            SpawnKnife();
        }
        
    }

    private void SpawnKnife()
    {
        knifeCount--;
        Instantiate(knifeObject, knifeSpawnPosition, Quaternion.identity);
    }
    
    
    
}
