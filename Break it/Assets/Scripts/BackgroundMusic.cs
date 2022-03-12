using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic BGMPlayer;

    void Awake()
    {
        if (BGMPlayer == null)
        {
            BGMPlayer = this;
        }
        else if (BGMPlayer != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}
