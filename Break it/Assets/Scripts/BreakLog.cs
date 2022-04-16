using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakLog : MonoBehaviour
{
    [SerializeField]
    int direction;

    Rigidbody2D rb2d;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        switch(direction)
        {
            case 1:
            rb2d.AddForce(this.transform.up * 40, ForceMode2D.Impulse);
            break;
            case 2:
            rb2d.AddForce(this.transform.up * 40, ForceMode2D.Impulse);
            break;
            case 3:
            rb2d.AddForce(-this.transform.right * 90, ForceMode2D.Impulse);
            break;
            case 4:
            rb2d.AddForce(this.transform.right * 90, ForceMode2D.Impulse);
            break;
        }
        // rb2d.AddTorque(torque);
        rb2d.gravityScale = 4;
    }

}
