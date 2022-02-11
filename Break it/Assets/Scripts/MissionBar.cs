using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionBar : MonoBehaviour
{

    public static GameObject txt;
    private float showTimer = 1;

    // Start is called before the first frame update
    void Start()
    {
        txt = GameObject.Find("MissionBar");
        txt.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        showTimer -= Time.deltaTime;
        if (showTimer < 0)
        {
            txt.SetActive(false);
        }
    }
}
