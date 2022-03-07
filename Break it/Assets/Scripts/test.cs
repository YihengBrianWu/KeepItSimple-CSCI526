using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField]
    private bool canPrint = false;
    void Update()
    {
        if (canPrint)
            print( transform.rotation.eulerAngles.z);
    }
}
