using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//animator obj

public class KnifeScript : MonoBehaviour
{
    private HitAnim hitAnim;
    [SerializeField]
    private float throwForce;

    private bool isActive = true;

    private Rigidbody2D rb;
    private BoxCollider2D knifeCollider;

    // 在发射之后禁止facemouse功能
    private bool stopFaceMouse = false;
    // 是否还在界面内
    private bool isInView = true;

    private SpriteRenderer sprite;
    private bool isBlack = false;
    private GameController gameController;

    private Vector2 lastVelocity;

    private bool lockRotation = false;
    private float newDirValueDeg;

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("LevelControl").GetComponent<GameController>();
        hitAnim = GameObject.FindGameObjectWithTag("TargetHit").GetComponent<HitAnim>();

        rb = GetComponent<Rigidbody2D>();
        knifeCollider = GetComponent<BoxCollider2D>();

        if (gameController.difficulty != 1 && gameController.difficulty != 0 && UnityEngine.Random.Range(0,2) == 1)
        {
            sprite = GetComponent<SpriteRenderer>();
            sprite.color = new Color (0, 0, 0, 1); 
            isBlack = true;
        }

    }

    private void Update()
    {
        lastVelocity = rb.velocity;
        isInView = IsInView(transform.position);
        // 只有在准备过程中才会facemouse，一旦发射，则禁用功能
        if (isActive && !stopFaceMouse)
        {
            FaceMouse();
        }
        if (Input.GetMouseButtonDown(0) && isActive)
        {
            if (!stopFaceMouse)
                GameController.Instance.GameUI.DecrementDisplayedKnifeCount();
            stopFaceMouse = true;
            rb.AddForce(transform.up * throwForce, ForceMode2D.Impulse);
            rb.gravityScale = 1;
        }
    
        // isActive保证这个if只进入一次
        if (!isInView && isActive)
        {
            isActive = false;
            GameController.Instance.failitInc();
            GameController.Instance.OnFailKnifeHit();
        }

        if (lockRotation)
        {
            transform.rotation = Quaternion.Euler(0, 0, newDirValueDeg);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!isActive)
        {
            return;
        }


        if (col.collider.CompareTag("Log"))
        {
            
            isActive = false;
            print( col.collider.transform.rotation.z);
            if ((isBlack && (col.collider.transform.rotation.z<1 &&col.collider.transform.rotation.z>0.72)||(isBlack && col.collider.transform.rotation.z>-1 &&col.collider.transform.rotation.z<-0.72))
                 || (!isBlack && col.collider.transform.rotation.z>-0.72 && col.collider.transform.rotation.z<0.72) || gameController.difficulty == 1 || gameController.difficulty == 0)
            {
                lockRotation = false;
                //play visual effects
                GetComponent<ParticleSystem>().Play();
                hitAnim.HitShake();

                ScoreCount.HitCount ++;
                rb.velocity = new Vector2(0, 0);
                rb.bodyType = RigidbodyType2D.Kinematic;
                this.transform.SetParent(col.collider.transform);

                knifeCollider.offset = new Vector2(knifeCollider.offset.x, -0.2f);
                knifeCollider.size = new Vector2(knifeCollider.size.x, 0.45f);
                
                GameController.Instance.hitOnLogInc();
                GameController.Instance.OnSuccessfulKnifeHit();
            }
            else
            {
                GameController.Instance.knifeHitWrongSection++;
                hitAnim.MissShake();

                rb.velocity = new Vector2(rb.velocity.x, -2);
                StartCoroutine("WaitNotInView");
            }
        }
        else if (col.collider.CompareTag("Knife"))
        {
            GameController.Instance.knifeCollisionHappens++;
            hitAnim.MissShake();
            isActive = false;

            rb.velocity = new Vector2(rb.velocity.x, -2);
            StartCoroutine("WaitNotInView");
        }
        else if (col.collider.CompareTag("MovingObstacle"))
        {
            GameController.Instance.knifeObstacleHappens++;
            hitAnim.MissShake();
            isActive = false;

            rb.velocity = new Vector2(rb.velocity.x, -2);
            StartCoroutine("WaitNotInView");
        }
        else if (col.collider.CompareTag("Wall") || (col.collider.CompareTag("WhiteWall") && !isBlack) || (col.collider.CompareTag("BlackWall") && isBlack))
        {
            var speed = lastVelocity.magnitude;
            var direction = Vector2.Reflect(lastVelocity.normalized, col.contacts[0].normal);
            rb.velocity = direction * Mathf.Max(speed, 0f);
            
            Vector2 newDir = new Vector3(transform.position.x, transform.position.y, 0);
            float newDirValue = Mathf.Atan2(newDir.y - direction.y, newDir.x - direction.x);
            newDirValueDeg = -(300 / Mathf.PI) * newDirValue;
            transform.rotation = Quaternion.Euler(0, 0, newDirValueDeg);
            lockRotation = true;
            
            StartCoroutine("WaitReflect");
        }
        else
        {
            hitAnim.MissShake();
            isActive = false;

            rb.velocity = new Vector2(rb.velocity.x, -2);
            StartCoroutine("WaitNotInView");
        }
    }

    IEnumerator WaitReflect() {
        
        yield return new WaitForSecondsRealtime(0.8f);
        if (isActive)
            isActive = false;
    }
    IEnumerator WaitNotInView() {
        
        yield return new WaitUntil(() => isInView == false);
        yield return new WaitForSecondsRealtime(1);
        GameController.Instance.failitInc();
        GameController.Instance.OnFailKnifeHit();
        
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
