using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(GameUI))]
public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    
    // 游戏难度
    [Header("Difficulty")] 
    public int difficulty;
    
    // 数量
    [Header("Knife Amount")] 
    [SerializeField] 
    private int knifeCount;
    
    [SerializeField] 
    private int knifeHitLogToWin;
    
    // 位置
    [Header("Knife Spawning")] 
    [SerializeField]
    private Vector2 knifeSpawnPosition;

    [SerializeField]
    private bool isInfinity = false;
    
    // knife的prefab对象
    [SerializeField] 
    private GameObject normalKnife;
    [SerializeField] 
    private GameObject smallKnife;

    private GameObject knifeObject;

    // gameUI对象
    public GameUI GameUI { get; private set; }

    private int knifeAmount;
    public bool win = false;
    public int currentScene = 0;
    
    // analytics 用
    public int knifeCollisionHappens = 0;
    public int knifeObstacleHappens = 0;
    public int knifeHitWrongSection = 0;

    //sound
    public AudioSource music;
    public AudioClip levelUp;

    private void Awake()
    {
        Instance = this;
        GameUI = GetComponent<GameUI>();
        knifeObject = normalKnife;

        knifeAmount = knifeCount;

        music = gameObject.AddComponent<AudioSource>();
        music.playOnAwake = false;
        levelUp = Resources.Load<AudioClip>("sound/levelUp");
    }

    private void Start()
    {
        if (isInfinity)
        {
            currentScene = 1;
        }
        else
        {
            currentScene = difficulty + 2;
        }
        GameUI.SetInitialDisplayedKnifeCount(knifeCount);
        if(PlayerPrefs.GetInt("item1",0) == 1)
        {
             knifeObject = smallKnife;
        }
        SpawnKnife();
    }
    
    // 统计有多少射到log上
    private int hitOnLog = 0;
    private int failHit = 0;

    public void hitOnLogInc()
    {
        hitOnLog++;
    }

    public void failitInc()
    {
        failHit++;
    }

    IEnumerator WaitThreeS()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void OnSuccessfulKnifeHit()
    {
        
        // Debug.Log("knifeCollisionHappens: " + knifeCollisionHappens);
        // Debug.Log("knifeObstacleHappens: " + knifeObstacleHappens);
        // Debug.Log("knifeHitWrongSection: " + knifeHitWrongSection);
        // Debug.Log("failHit: " + failHit);
        
        if (hitOnLog >= knifeHitLogToWin)
        {
            win = true;
            // 埋点 after win 之后的统计数据
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"Level", difficulty},
                {"KnifeRemaining", knifeCount},
                {"State", "win"},
                {"Knife Used", knifeAmount - knifeCount},
                {"Knife Collision", knifeCollisionHappens},
                {"Obstacle Collision", knifeObstacleHappens},
                {"Wrong Section", knifeHitWrongSection}
            };
            AnalyticsResult result = Analytics.CustomEvent("Stats After Win", parameters);
            Analytics.FlushEvents();
            Debug.Log(parameters.Select(kvp => kvp.ToString()).Aggregate((a, b) => a + ", " + b));
            Debug.Log(result);

            GameUI.showLevelUp();
            StartCoroutine("WaitThreeS");

            music.clip = levelUp;
            music.Play();

            return;
        }

        if (failHit > (knifeAmount - knifeHitLogToWin))
        {
            // Debug.Log("knifeCollisionHappens" + knifeCollisionHappens);
            // Debug.Log("knifeObstacleHappens" + knifeObstacleHappens);
            // Debug.Log("knifeHitWrongSection" + knifeHitWrongSection);
            // 埋点 after lose 之后的统计数据
        if (isInfinity)
        {
            SceneManager.LoadScene(10);
            return;
        }
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"Level", difficulty},
                {"KnifeRemaining", knifeCount},
                {"State", "lose"},
                {"Knife Used", knifeAmount - knifeCount},
                {"Knife Collision", knifeCollisionHappens},
                {"Obstacle Collision", knifeObstacleHappens},
                {"Wrong Section", knifeHitWrongSection}
            };
            AnalyticsResult result = Analytics.CustomEvent("Stats After Lose", parameters);
            Analytics.FlushEvents();
            Debug.Log(parameters.Select(kvp => kvp.ToString()).Aggregate((a, b) => a + ", " + b));
            Debug.Log(result);
            SceneManager.LoadScene(10);
            return;
        }
        
        if (knifeCount > 0)
        {
            SpawnKnife();
        }
        
    }

    public void OnFailKnifeHit()
    {
        // Debug.Log("knifeCollisionHappens: " + knifeCollisionHappens);
        // Debug.Log("knifeObstacleHappens: " + knifeObstacleHappens);
        // Debug.Log("knifeHitWrongSection: " + knifeHitWrongSection);
        // Debug.Log("failHit: " + failHit);

        if (isInfinity)
        {
            SceneManager.LoadScene(10);
            return;
        }
        if (failHit > (knifeAmount - knifeHitLogToWin))
        {
            if (isInfinity)
            {
                SceneManager.LoadScene(10);
                return;
            }
            // 埋点 after lose 之后的统计数据
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"Level", difficulty},
                {"KnifeRemaining", knifeCount},
                {"State", "lose"},
                {"Knife Used", knifeAmount - knifeCount},
                {"Knife Collision", knifeCollisionHappens},
                {"Obstacle Collision", knifeObstacleHappens},
                {"Wrong Section", knifeHitWrongSection}
            };
            AnalyticsResult result = Analytics.CustomEvent("Stats After Lose", parameters);
            Analytics.FlushEvents();
            Debug.Log(parameters.Select(kvp => kvp.ToString()).Aggregate((a, b) => a + ", " + b));
            Debug.Log(result);
            SceneManager.LoadScene(10);
            return;
        }
        if (knifeCount > 0)
        {
            SpawnKnife();
        }
    }

    private void SpawnKnife()
    {
        knifeCount--;
        Instantiate(knifeObject, knifeSpawnPosition, Quaternion.identity);
    }
    
    
    
}
