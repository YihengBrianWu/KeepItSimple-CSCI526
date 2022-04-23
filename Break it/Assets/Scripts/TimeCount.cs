using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCount : MonoBehaviour
{
    public static TimeCount Instance { get; private set; }
    
    private float timeDuration;
    private bool needTime;
    public Text time;
    private bool timeIsRunning;
    
    void Start()
    {
        needTime = GameController.Instance.timeCount;
        if (needTime)
        {
            timeDuration = GameController.Instance.timeDuration;
            timeIsRunning = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!needTime)
        {
            time.text = "∞";
        }
        else
        {
            if (timeIsRunning)
            {
                if (timeDuration > 0)
                {
                    timeDuration -= Time.deltaTime;
                    time.text = (Mathf.FloorToInt(timeDuration % 60) + 1).ToString();
                }
                else
                {
                    timeDuration = 0;
                    timeIsRunning = false;
                }
            }
        }
    }

    public void Reset()
    {
        timeDuration = GameController.Instance.timeDuration;
        timeIsRunning = true;
    }
    
}
