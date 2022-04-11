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
    public bool isBlack = false;
    private GameController gameController;
    public static int predict;

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
    
    // 多线程检测确保spawnknife
    private bool isDestroy = false;

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("LevelControl").GetComponent<GameController>();
        hitAnim = GameObject.FindGameObjectWithTag("TargetHit").GetComponent<HitAnim>();

        rb = GetComponent<Rigidbody2D>();
        knifeCollider = GetComponent<BoxCollider2D>();

        int ran = UnityEngine.Random.Range(0, 2);
        
        if (gameController.difficulty != 1 && gameController.difficulty != 0 && predict == 1)
        {
            sprite = GetComponent<SpriteRenderer>();
            sprite.color = new Color(0, 0, 0, 1);
            isBlack = true;
        }
        if (gameController.difficulty != 1 && gameController.difficulty != 0 && predict == 2)
        {
            sprite = GetComponent<SpriteRenderer>();
            sprite.color = new Color(1, 1, 1, 1);
            isBlack = false;
        }
        
        if (gameController.difficulty != 1 && gameController.difficulty != 0 && ran == 1)
        {
            SpriteRenderer nextKnife = GameObject.FindGameObjectWithTag("nextKnife").GetComponent<SpriteRenderer>();
            nextKnife.color = new Color(0, 0, 0, 1);
            predict = 1;
        }
        if (gameController.difficulty != 1 && gameController.difficulty != 0 && ran == 0)
        {
            SpriteRenderer nextKnife = GameObject.FindGameObjectWithTag("nextKnife").GetComponent<SpriteRenderer>();
            nextKnife.color = new Color(1, 1, 1, 1);
            predict = 2;
        }

        music = gameObject.AddComponent<AudioSource>();
        music.playOnAwake = false;
        hitLog = Resources.Load<AudioClip>("sound/hitLog");
        hitKnife = Resources.Load<AudioClip>("sound/hitKnife");
        throwSound = Resources.Load<AudioClip>("sound/throw");
        rebound = Resources.Load<AudioClip>("sound/rebound");
        eliminate = Resources.Load<AudioClip>("sound/eliminate");
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
            // print(tempZ);
            // print("isBlack " + isBlack);
            // print("reflected " + reflected);
            // print("isTiltedLeft " + isTiltedLeft);
            if ((!reflected && (isBlack && tempZ<270 &&tempZ>90))
                 || (!reflected && !isBlack && ((tempZ>270 && tempZ<360) || (tempZ>0 && tempZ<90)))
                 || gameController.difficulty == 1 
                 || gameController.difficulty == 0
                 || (!isTiltedLeft && reflected && (isBlack && tempZ<310 &&tempZ>135))
                 || (isTiltedLeft && reflected && (isBlack && tempZ>35 &&tempZ<210))
                 || (!isTiltedLeft && reflected&& !isBlack && ((tempZ<135 && tempZ>0) || (tempZ>330 && tempZ<360)))
                 || (isTiltedLeft && reflected&& !isBlack && ((tempZ>220 && tempZ<360) || (tempZ<40 && tempZ>0)))
                 )
            {
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
                isActive = false;
                GameController.Instance.failitInc();
                GameController.Instance.OnFailKnifeHit();
                
            }

            music.clip = hitLog;
            music.Play();
        }
        else if (col.collider.CompareTag("Knife"))
        {

            Debug.Log(col.collider.GetComponent<KnifeScript>().isBlack);
            // different colors
            if (isBlack && !col.collider.GetComponent<KnifeScript>().isBlack ||
                !isBlack && col.collider.GetComponent<KnifeScript>().isBlack)
            {
                ScoreCount.HitCount++;
                GameController.Instance.blackWhiteCollision++;
                Debug.Log("black and white hit");
                GameController.Instance.hitOnLogInc();
                GameController.Instance.OnSuccessfulKnifeHit();
                Destroy(col.collider.gameObject);
                Destroy(this.gameObject);

                music.clip = eliminate;
                music.Play();
            }
            else
            {
                GameController.Instance.knifeCollisionHappens++;
                hitAnim.MissShake();
                isActive = false;
                
                rb.velocity = new Vector2(rb.velocity.x, -2);
                GameController.Instance.failitInc();
                GameController.Instance.OnFailKnifeHit();

                music.clip = hitKnife;
                music.Play();
            }
        }
        else if (col.collider.CompareTag("MovingObstacle"))
        {
            GameController.Instance.knifeObstacleHappens++;
            hitAnim.MissShake();
            isActive = false;

            rb.velocity = new Vector2(rb.velocity.x, -2);
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
            hitAnim.MissShake();
            isActive = false;

            rb.velocity = new Vector2(rb.velocity.x, -2);
            GameController.Instance.failitInc();
            GameController.Instance.OnFailKnifeHit();
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

    IEnumerator WaitForPointFive()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);
        yield return new WaitForSecondsRealtime(0.15f);
        GameController.Instance.SpawnKnife();
        //After we have waited 0.5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }

    IEnumerator SpawnKnifeBeforeDestroy()
    {
        yield return new WaitUntil(() => isDestroy == true);
        GameController.Instance.SpawnKnife();
    }

}
