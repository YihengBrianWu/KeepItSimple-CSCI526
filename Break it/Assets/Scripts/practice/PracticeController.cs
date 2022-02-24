using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeController : MonoBehaviour
{
    
    public static PracticeController Instance { get; private set; }
    
    // knife的prefab对象
    [SerializeField] 
    private GameObject knifeObject;
    
    [Header("Knife Spawning")] 
    [SerializeField]
    private Vector2 knifeSpawnPosition;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Instantiate(knifeObject, knifeSpawnPosition, Quaternion.identity);
    }
    
}
