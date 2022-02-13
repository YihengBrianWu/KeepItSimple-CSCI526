using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPointFollower : MonoBehaviour
{
    [SerializeField] private GameObject[] endPoints;
    private int currentPointIndex = 0;


    [SerializeField] private float speed = 2f; 
    void Update()
    {
        if (Vector2.Distance(endPoints[currentPointIndex].transform.position, transform.position)  < 0.1f)
        {
            currentPointIndex ++;
            if (currentPointIndex >= endPoints.Length)
            {
                currentPointIndex = 0;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, endPoints[currentPointIndex].transform.position, Time.deltaTime * speed);


    }
}
