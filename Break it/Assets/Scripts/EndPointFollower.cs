using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPointFollower : MonoBehaviour
{
    [SerializeField] private GameObject[] endPoints;
    [SerializeField] private bool NeedStop = false;
    private int currentPointIndex = 0;
    private bool stopMoving = false;
    private bool waitCalled = false;

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

            if (NeedStop && !waitCalled)
            {
                stopMoving = true;
                waitCalled = true;
                StartCoroutine("WaitOneS");
            }
        }
        if(!stopMoving)
            transform.position = Vector2.MoveTowards(transform.position, endPoints[currentPointIndex].transform.position, Time.deltaTime * speed);
    }
    IEnumerator WaitOneS()
    {
        yield return new WaitForSeconds(2.5f);
        stopMoving = false;
        waitCalled = false;
    }
}
