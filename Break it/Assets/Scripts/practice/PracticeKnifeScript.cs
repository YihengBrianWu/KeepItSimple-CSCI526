using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeKnifeScript : MonoBehaviour
{
    
    [SerializeField]
    private float throwForce;
    
    private bool isActive = true;
    
    private Rigidbody2D rb;
    private BoxCollider2D knifeCollider;
    
    // 在发射之后禁止facemouse功能
    private bool stopFaceMouse = false;
    // 是否还在界面内
    private bool isInView = true;

    // last frame velocity
    private Vector2 lastVelocity;

    // private Vector2 arrowDir;
    // private Vector2 bowScript;
    
    
    // private void Start()
    // {
    //     arrowDir = bowScript.;
    //     arrowDir = 0;
    //     rb.velocity = arrowDir.normalized * lastVelocity;
    // }

    
    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        knifeCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {

        lastVelocity = rb.velocity;
        
        if (isActive && !stopFaceMouse)
        {
            FaceMouse();
        }
        
        if (Input.GetMouseButtonDown(0) && isActive)
        {
            stopFaceMouse = true;
            Debug.Log("here");
            rb.AddForce(transform.up * throwForce, ForceMode2D.Impulse);
            rb.gravityScale = 1;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!isActive)
        {
            return;
        }

        isActive = false;

        if (col.collider.CompareTag("Wall"))
        {
            // Vector2 inDirection = rb.velocity;
            // Vector2 inNormal = col.contacts[0].normal;
            // Vector2 newVelocity = Vector2.Reflect(inDirection, inNormal);

            var speed = lastVelocity.magnitude;
            var direction = Vector2.Reflect(lastVelocity.normalized, col.contacts[0].normal);
            rb.velocity = direction * Mathf.Max(speed, 0f);
            
            Vector2 newDir = new Vector3(transform.position.x, transform.position.y, 0);
            float newDirValue = Mathf.Atan2(newDir.y - direction.y, newDir.x - direction.x);
            float newDirValueDeg = -(270 / Mathf.PI) * newDirValue;
            transform.rotation = Quaternion.Euler(0, 0, newDirValueDeg);
            // Vector3 newDir = new Vector3(transform.position.x, transform.position.y, 0);
            // float newDirValue = Mathf.Atan2(newDir.y - currDir.y, newDir.x - currDir.x);
            // float newDirValueDeg = (180 / Mathf.PI) * newDirValue;
            // transform.rotation = Quaternion.Euler(0, 0, newDirValueDeg);

            
            // Vector2D inDirection = GetComponent<RigidBody2D>().velocity;
            // Vector2D inNormal = collision.contacts[0].normal;
            // Vector2D newVelocity = Vector2D.Reflect(inDirection, inNormal);
        }
    }
    
    // 让knife面对鼠标位置
    public void FaceMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = new Vector2(
            mousePosition.x - transform.position.x,
            mousePosition.y - transform.position.y
        );
        transform.up = direction;

    }

    // 判断物体是否还在相机范围内
    public bool IsInView(Vector3 worldPos)
    {
        Transform camTransform = Camera.main.transform;
        Vector2 viewPos = Camera.main.WorldToViewportPoint(worldPos);

        //判断物体是否在相机前面  
        Vector3 dir = (worldPos - camTransform.position).normalized;
        float dot = Vector3.Dot(camTransform.forward, dir);

        if (dot > 0 && viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
