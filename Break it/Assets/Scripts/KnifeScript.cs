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

    public bool isActive = true;

    private Rigidbody2D rb;
    private BoxCollider2D knifeCollider;

    // 在发射之后禁止facemouse功能
    private bool stopFaceMouse;
    // 是否还在界面内
    private bool isInView = true;
    public bool isBlack = false;
    private GameController gameController;

    private Vector2 lastVelocity;

    private bool lockRotation = false;
    private float newDirValueDeg;
    private bool isTiltedLeft = false;
    private bool reflected = false;

    //sound
    public AudioSource music;
    public AudioClip hitLog;
    public AudioClip hitKnife;
    public AudioClip throwSound;
    public AudioClip rebound;
    public AudioClip eliminate;

    private bool firstTime = true;
    public bool onTheLog = false;
    private GameObject tempKnife;
    private Material m1;
    private Material m2;
    private bool startDissolve = false;
    float fade = 1.2f;
    [SerializeField]
    private bool isExample = false;
    private bool breakThree = false; 
    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("LevelControl").GetComponent<GameController>();
        hitAnim = GameObject.FindGameObjectWithTag("TargetHit").GetComponent<HitAnim>();

        if(gameController.isBlack)
        {
            isBlack = true;
        }

        rb = GetComponent<Rigidbody2D>();
        knifeCollider = GetComponent<BoxCollider2D>();

        if (isExample)
        {
            stopFaceMouse = true;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.velocity = new Vector2(0, 0);
            GameObject logObj = GameObject.FindGameObjectWithTag("Log");
            this.transform.SetParent(logObj.transform);
        }
        else
        {
            stopFaceMouse = !GameController.Instance.faceMouse;
        }
        

        music = gameObject.GetComponent<AudioSource>();
        hitLog = Resources.Load<AudioClip>("sound/hitLog");
        hitKnife = Resources.Load<AudioClip>("sound/hitKnife");
        throwSound = Resources.Load<AudioClip>("sound/throw");
        rebound = Resources.Load<AudioClip>("sound/rebound");
        eliminate = Resources.Load<AudioClip>("sound/eliminate");
    }

    private void Update()
    {
        if (GameController.isPaused)
        {
            return;
        }
        lastVelocity = rb.velocity;
        isInView = IsInView(transform.position);
        // 只有在准备过程中才会facemouse，一旦发射，则禁用功能
        if (isActive && !stopFaceMouse)
        {
            FaceMouse();
        }
        if (Input.GetMouseButtonDown(0) && isActive && firstTime && Input.mousePosition[0] < 2000)
        {
            firstTime = false;
            // if (!stopFaceMouse)
            GameController.Instance.GameUI.DecrementDisplayedKnifeCount();
            stopFaceMouse = true;
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.AddForce(transform.up * throwForce, ForceMode2D.Impulse);
            rb.gravityScale = 1;

            music.clip = throwSound;
            music.Play();

            StartCoroutine(WaitForPointFive());
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

        if (!isInView && !isActive)
        {
            Destroy(this.gameObject);
        }

        if(startDissolve)
        {
            fade -= Time.deltaTime;
            if (fade <= 0f)
            {
                startDissolve = false;
            }

            m1.SetFloat("_Fade", fade);
            if (!breakThree)
            {
                m2.SetFloat("_Fade",fade);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!isActive)
        {
            return;
        }

        if (rb.bodyType == RigidbodyType2D.Static)
        {
            return;
        }

        lockRotation = false;    

        if (col.collider.CompareTag("Log"))
        {   
            isActive = false;

            float tempZ = col.collider.transform.rotation.eulerAngles.z; 
            //print(tempZ);
            // if ((!reflected && (isBlack && tempZ<270 &&tempZ>90))
            //      || (!reflected && !isBlack && ((tempZ>270 && tempZ<360) || (tempZ>0 && tempZ<90)))
            //      || gameController.difficulty == 1 
            //      || gameController.difficulty == 0
            //      || (!isTiltedLeft && reflected && (isBlack && tempZ<310 &&tempZ>135))
            //      || (isTiltedLeft && reflected && (isBlack && tempZ>35 &&tempZ<210))
            //      || (!isTiltedLeft && reflected&& !isBlack && ((tempZ<135 && tempZ>0) || (tempZ>330 && tempZ<360)))
            //      || (isTiltedLeft && reflected&& !isBlack && ((tempZ>220 && tempZ<360) || (tempZ<40 && tempZ>0)))
            //      )
            if (gameController.difficulty == 1
            || (gameController.difficulty == 2 && isBlack && tempZ<270 &&tempZ>90)
            || (gameController.difficulty == 2 && !isBlack && ((tempZ>=270 && tempZ<=360) || (tempZ>=0 && tempZ<=90)))
            )
            {
                onTheLog = true;
                //play visual effects
                GetComponent<ParticleSystem>().Play();
                hitAnim.HitShake();

                ScoreCount.HitCount++;
                rb.velocity = new Vector2(0, 0);
                rb.bodyType = RigidbodyType2D.Kinematic;
                this.transform.SetParent(col.collider.transform);

                if (gameController.isShort)
                {
                    knifeCollider.offset = new Vector2(knifeCollider.offset.x, -0.18f);
                    knifeCollider.size = new Vector2(knifeCollider.size.x, 0.4f);
                }
                else{
                    knifeCollider.offset = new Vector2(knifeCollider.offset.x, -0.15f);
                    knifeCollider.size = new Vector2(knifeCollider.size.x, 0.45f);
                }

                
                GameController.Instance.hitOnLogInc();
                GameController.Instance.OnSuccessfulKnifeHit();
                music.clip = hitLog;
                music.Play();
            }
            else
            {
                //bounce off log
                GameController.Instance.knifeHitWrongSection++;
                hitAnim.MissShake();

                rb.velocity = new Vector2(rb.velocity.x, -2);
                isActive = false;

                this.GetComponent<BoxCollider2D>().enabled = false;
                GameController.Instance.failitInc();
                GameController.Instance.OnFailKnifeHit();
                music.clip = rebound;
                music.Play();
                
            }

        }
        else if (col.collider.CompareTag("Knife"))
        {

            //Debug.Log(col.collider.GetComponent<KnifeScript>().isBlack);
            // different colors
            if (isBlack && !col.collider.GetComponent<KnifeScript>().isBlack && col.collider.GetComponent<KnifeScript>().onTheLog ||
                !isBlack && col.collider.GetComponent<KnifeScript>().isBlack && col.collider.GetComponent<KnifeScript>().onTheLog)
            {
                isActive = false;
                // hitAnim.MissShake();
                ScoreCount.HitCount++;
                rb.velocity = new Vector2(0, 0);
                rb.bodyType = RigidbodyType2D.Kinematic;
                this.transform.SetParent(col.collider.transform);

                GameController.Instance.blackWhiteCollision++;
                Debug.Log("black and white hit");
                GameController.Instance.hitOnLogInc();
                GameController.Instance.OnSuccessfulKnifeHit();

                tempKnife = col.collider.gameObject;
                m1 = this.GetComponent<SpriteRenderer>().material;
                m2 = tempKnife.GetComponent<SpriteRenderer>().material;
                this.GetComponent<Rigidbody2D>().gravityScale = 0.3f;
                startDissolve = true;
                tempKnife.GetComponent<BoxCollider2D>().enabled = false;  
                this.GetComponent<BoxCollider2D>().enabled = false;                
                StartCoroutine("waitEliminate");
                print("bounce of different knife");////////////////////////////////////
                music.clip = eliminate;
                music.Play();
            }
            else
            {
                //bounce off knife
                GameController.Instance.knifeCollisionHappens++;
                hitAnim.MissShake();
                isActive = false;
                
                rb.velocity = new Vector2(rb.velocity.x, -2);
                this.GetComponent<BoxCollider2D>().enabled = false;
                GameController.Instance.failitInc();
                GameController.Instance.OnFailKnifeHit();

                music.clip = hitKnife;
                music.Play();
            }
        }
        else if (col.collider.CompareTag("MovingObstacle") || (col.collider.CompareTag("WhiteWall") && isBlack) || (col.collider.CompareTag("BlackWall") && !isBlack))
        {
            //bounce off obstacles
            GameController.Instance.knifeObstacleHappens++;
            hitAnim.MissShake();
            isActive = false;

            rb.velocity = new Vector2(rb.velocity.x, -2);
            this.GetComponent<BoxCollider2D>().enabled = false;
            GameController.Instance.failitInc();
            GameController.Instance.OnFailKnifeHit();

            music.clip = hitKnife;
            music.Play();
        }
        else if (col.collider.CompareTag("Wall") || (col.collider.CompareTag("WhiteWall") && !isBlack) || (col.collider.CompareTag("BlackWall") && isBlack))
        {
            reflected = true;
            var speed = lastVelocity.magnitude;
            var direction = Vector2.Reflect(lastVelocity.normalized, col.contacts[0].normal);
            rb.velocity = direction * Mathf.Max(speed, 0f);
            
            Vector2 newDir = new Vector3(transform.position.x, transform.position.y, 0);
            float newDirValue = Mathf.Atan2(newDir.y - direction.y, newDir.x - direction.x);
            if (newDirValue < -0.8)
            {
                newDirValueDeg = -(350 / Mathf.PI) * newDirValue;
                
                isTiltedLeft = true;
            }
            else
            {
                newDirValueDeg = -(300 / Mathf.PI) * newDirValue;
            }
            transform.rotation = Quaternion.Euler(0, 0, newDirValueDeg);
            lockRotation = true;
            
            StartCoroutine("WaitReflect");

            music.clip = rebound;
            music.Play();
        }
        else
        {
            //should not enter
            //print("should not enter");////////////////////////////////////
            hitAnim.MissShake();
            isActive = false;

            rb.velocity = new Vector2(rb.velocity.x, -2);
            GameController.Instance.failitInc();
            GameController.Instance.OnFailKnifeHit();
        }
    }

    public void selfDestory()
    {   
        m1 = this.GetComponent<SpriteRenderer>().material;
        this.GetComponent<Rigidbody2D>().gravityScale = 0.3f;
        this.GetComponent<BoxCollider2D>().enabled = false;  
        startDissolve = true;
        breakThree = true;
        StartCoroutine("waitEliminate");
    }

    IEnumerator WaitReflect() {
        
        yield return new WaitForSecondsRealtime(0.8f);
        if (isActive)
            isActive = false;
    }
    IEnumerator waitEliminate() {
        yield return new WaitForSecondsRealtime(1.2f);
        if (!breakThree)
        {
            Destroy(tempKnife);
        }
        Destroy(this.gameObject);
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

    IEnumerator WaitForPointFive()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);
        yield return new WaitForSecondsRealtime(0.15f);
        GameController.Instance.SpawnKnife();
        //After we have waited 0.15 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }

}
