using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }


    [Header("Knife Spawning")] 
    [SerializeField]
    private Vector2 knifeSpawnPosition;
    [SerializeField] 
    private GameObject knifeObject;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SpawnKnife();
    }

    public void OnSuccessfulKnifeHit()
    {
        SpawnKnife();
    }

    private void SpawnKnife()
    {
        Instantiate(knifeObject, knifeSpawnPosition, Quaternion.identity);
    }
}
