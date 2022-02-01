using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeScript : MonoBehaviour
{
    [SerializeField]
    private Vector2 throwForce;

    private bool isActive = true;

    private Rigidbody2D rb;
    private BoxCollider2D knifeCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        knifeCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && isActive)
        {
            rb.AddForce(throwForce, ForceMode2D.Impulse);
            rb.gravityScale = 1;
            // TODO if it's not active
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!isActive)
        {
            return;
        }

        isActive = false;

        if (col.collider.tag == "Log")
        {
            rb.velocity = new Vector2(0, 0);
            rb.bodyType = RigidbodyType2D.Kinematic;
            this.transform.SetParent(col.collider.transform);

            knifeCollider.offset = new Vector2(knifeCollider.offset.x, -0.18f);
            knifeCollider.size = new Vector2(knifeCollider.size.x, 0.5f);
            
            GameController.Instance.OnSuccessfulKnifeHit();
        }
        else if (col.collider.tag == "Knife")
        {
            rb.velocity = new Vector2(rb.velocity.x, -2);
        }
    }
    
    // TODO game winning
}
