using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private int degree;
    private bool isOpen = false;
    private void Awake()
    {
        StartCoroutine("waitRotate");
    }
 
    IEnumerator RotateMe(Vector3 byAngles, float inTime)
    {
        var fromAngle = transform.rotation;
        var toAngle = Quaternion.Euler(transform.eulerAngles + byAngles);
        for (var t = 0f; t < 1; t += Time.deltaTime / inTime)
        {
            transform.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
            yield return null;
        }
        degree *= -1;
        StartCoroutine("waitRotate");
    }

    IEnumerator waitRotate()
    {
        if(isOpen)
        {
            yield return new WaitForSeconds(2.0f);
            isOpen = false;
        }
        else{
            yield return new WaitForSeconds(1.0f);
            isOpen = true;
        }
        StartCoroutine(RotateMe(Vector3.back * degree, 0.4f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
