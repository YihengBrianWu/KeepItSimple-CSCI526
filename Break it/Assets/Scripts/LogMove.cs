using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogMove : MonoBehaviour
{
    private Vector3 pointA;
    private Vector3 pointB;
    private bool towardsA = true;

    private void Awake()
    {
        pointA = new Vector3(-4.3f, 2.4f);
        pointB = new Vector3(4.3f, 2.4f);
    }
    void Start()
    {
        
    }

    void Update()
    {

        if (transform.position == pointA)
        {
            towardsA = false;
        }

        if (transform.position == pointB)
        {
            towardsA = true;
        }

        if (towardsA)
        {
            float step = 3 * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, pointA, step);
        }
        else
        {
            float step = 3 * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, pointB, step);
        }
        
    }
}
