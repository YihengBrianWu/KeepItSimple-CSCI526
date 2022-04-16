using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

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

    [SerializeField] 
    private UnityEngine.Object logBreak;
    
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
    [SerializeField] 
    private GameObject shortKnife;
    [SerializeField] 
    private GameObject smallAndShortKnife;

    [SerializeField] 
    private GameObject normalKnifeB;
    [SerializeField] 
    private GameObject smallKnifeB;
    [SerializeField] 
    private GameObject shortKnifeB;
    [SerializeField] 
    private GameObject smallAndShortKnifeB;
    
    // 是否需要开启facemouse功能
    [Header("Face mouse")] 
    [SerializeField]
    public bool faceMouse = true;

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
    public int blackWhiteCollision = 0;

    //sound
    public AudioSource music;
    public AudioClip levelUp;


    // pause menu
    public GameObject pauseMenu;
    public static bool isPaused = false;
    
    // 追踪新生成的knife
    public GameObject newKnife;
    public bool isShort = false;
    public int predict = 0;
    public bool isBlack;

    public GameObject nextKnife;
    public GameObject nextKnifeB;

    private void Awake()
    {
        isPaused = false;
        Instance = this;
        GameUI = GetComponent<GameUI>();
        knifeObject = normalKnife;
        //PlayerPrefs.SetInt("item4", 0);
        if (PlayerPrefs.GetInt("extraKnife", 0) == 4)
        {
            knifeCount += 2;
            knifeAmount = knifeCount;
        }

        knifeAmount = knifeCount;

        music = gameObject.AddComponent<AudioSource>();
        music.playOnAwake = false;
        levelUp = Resources.Load<AudioClip>("sound/levelUp");
    }

    private void Update()
    {
        if (Input.GetKeyDown("b"))
        {
            if (PlayerPrefs.GetInt("total", 0) < 5)
            {
                Debug.Log("don't have enough points.");
                // TODO 提醒玩家分数不够
            }
            else
            {
                DestroyRandomThree();
            }
        }
        
    }

    private void Start()
    {
        Time.timeScale = 1;
        if (isInfinity)
        {
            currentScene = 12;
        }
        else
        {
            currentScene = difficulty + 2;
        }
        GameUI.SetInitialDisplayedKnifeCount(knifeCount);
        if(PlayerPrefs.GetInt("itemSelected",0) == 1)
        {
             knifeObject = smallKnife;
        }
        else if(PlayerPrefs.GetInt("itemSelected",0) == 2)
        {
            isShort =true;
            knifeObject = shortKnife;
        }
        else if(PlayerPrefs.GetInt("itemSelected",0) == 3)
        {
            knifeObject = smallAndShortKnife;
            isShort =true;
        }
        SpawnKnife();
        //PlayerPrefs.SetInt("item4", 0);
        //if (PlayerPrefs.GetInt("extraKnife", 0) == 4)
        //{
        //    Debug.Log("dada");
        //    knifeCount += 2;
        //    knifeAmount = knifeCount;
        //}
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
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("levelReached", Math.Max(currentScene - 1, PlayerPrefs.GetInt("levelReached", 0)));
        SceneManager.LoadScene(currentScene + 1);
    }
    
    public void destoryLog()
    {
        GameObject ogLog = GameObject.FindGameObjectWithTag("Log");
        GameObject logPieces = (GameObject)Instantiate(logBreak);
        logPieces.transform.position = ogLog.transform.position;
        ogLog.SetActive(false);
    }

    IEnumerator WaitBreak()
    {
        yield return new WaitForSeconds(1.0f);
        GameUI.showLevelUp();
        StartCoroutine("WaitThreeS");

        music.clip = levelUp;
        music.Play();
    }
    IEnumerator WaitFail()
    {
        yield return new WaitForSeconds(0.7f);
        SceneManager.LoadScene(10);
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
                {"Wrong Section", knifeHitWrongSection},
                {"Black and white knives collision", blackWhiteCollision}
            };
            AnalyticsResult result = Analytics.CustomEvent("Stats After Win", parameters);
            Analytics.FlushEvents();
            Debug.Log(parameters.Select(kvp => kvp.ToString()).Aggregate((a, b) => a + ", " + b));
            Debug.Log(result);

            GameObject knifeToShot = GameObject.FindGameObjectWithTag("Knife");
            knifeToShot.SetActive(false);
            destoryLog();
            StartCoroutine("WaitBreak");
            GameObject knifeToShot2 = GameObject.FindGameObjectWithTag("Knife");
            knifeToShot2.SetActive(false);

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
            StartCoroutine("WaitFail");
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
                {"Wrong Section", knifeHitWrongSection},
                {"Black and white knives collision", blackWhiteCollision}
            };
            AnalyticsResult result = Analytics.CustomEvent("Stats After Lose", parameters);
            Analytics.FlushEvents();
            Debug.Log(parameters.Select(kvp => kvp.ToString()).Aggregate((a, b) => a + ", " + b));
            Debug.Log(result);
            StartCoroutine("WaitFail");
            return;
        }
        
        // if (knifeCount > 0)
        // {
        //     SpawnKnife();
        // }
        
    }

    public void OnFailKnifeHit()
    {
        // Debug.Log("knifeCollisionHappens: " + knifeCollisionHappens);
        // Debug.Log("knifeObstacleHappens: " + knifeObstacleHappens);
        // Debug.Log("knifeHitWrongSection: " + knifeHitWrongSection);
        // Debug.Log("failHit: " + failHit);

        if (isInfinity)
        {
            int rewardGet = ScoreCount.HitCount / 5;
            PlayerPrefs.SetInt("total", PlayerPrefs.GetInt("total", 0) + rewardGet);
            StartCoroutine("WaitFail");
            return;
        }
        if (failHit > (knifeAmount - knifeHitLogToWin))
        {
            if (isInfinity)
            {
                StartCoroutine("WaitFail");
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
                {"Wrong Section", knifeHitWrongSection},
                {"Black and white knives collision", blackWhiteCollision}
            };
            AnalyticsResult result = Analytics.CustomEvent("Stats After Lose", parameters);
            Analytics.FlushEvents();
            Debug.Log(parameters.Select(kvp => kvp.ToString()).Aggregate((a, b) => a + ", " + b));
            Debug.Log(result);
            StartCoroutine("WaitFail");
            return;
        }
        // if (knifeCount > 0)
        // {
        //     SpawnKnife();
        // }
    }

    public void ConvertBlack()
    {
        if(PlayerPrefs.GetInt("itemSelected",0) == 1)
        {
             knifeObject = smallKnifeB;
        }
        else if(PlayerPrefs.GetInt("itemSelected",0) == 2)
        {
            knifeObject = shortKnifeB;
        }
        else if(PlayerPrefs.GetInt("itemSelected",0) == 3)
        {
            knifeObject = smallAndShortKnifeB;
        }
        else{
            knifeObject = normalKnifeB;
        }
    }
        public void ConvertWhite()
    {
        if(PlayerPrefs.GetInt("itemSelected",0) == 1)
        {
             knifeObject = smallKnife;
        }
        else if(PlayerPrefs.GetInt("itemSelected",0) == 2)
        {
            knifeObject = shortKnife;
        }
        else if(PlayerPrefs.GetInt("itemSelected",0) == 3)
        {
            knifeObject = smallAndShortKnife;
        }
        else{
            knifeObject = normalKnife;
        }
    }
    public void SpawnKnife()
    {           
        if (knifeCount > 0) 
        {
            int ran = UnityEngine.Random.Range(0, 2);

            if (difficulty == 2 && predict == 1)
            {
                ConvertBlack();
                isBlack = true;
            }
            if (difficulty == 2 && predict == 0)
            {
                ConvertWhite();
                isBlack = false;
            }

            if (difficulty == 2 && ran == 1)
            {
                nextKnife.SetActive(false);
                nextKnifeB.SetActive(true);
                predict = 1;
            }
            if (difficulty == 2 && ran == 0)
            {
                nextKnife.SetActive(true);
                nextKnifeB.SetActive(false);
                predict = 0;
            }

            knifeCount--;
            newKnife = Instantiate(knifeObject, knifeSpawnPosition, Quaternion.identity);
        }
        else 
        {
            return;
        }
    }

    public void DestroyRandomThree()
    {
        // 用来存放knife的list
        List<GameObject> children = new List<GameObject>();
        // 首先找到Log gameObject
        GameObject log = GameObject.Find("Log");
        // 得到他的children transform列表
        Transform[] list = log.GetComponentsInChildren<Transform>();
        // 遍历每一个child
        foreach (Transform child in list)
        {   
            // 如果是knife的话就加入list
            if (child.CompareTag("Knife"))
            {
                // Debug.Log(child.gameObject);
                children.Add(child.gameObject);
            }
        }

        // 如果没有刀的话则什么也不干，提醒玩家没有刀，也不扣分
        if (children.Count == 0)
        {
            // TODO 弹窗提醒玩家没有刀
        }
        // 如果小于等于三把刀，全部摧毁
        else if (children.Count <= 3)
        {
            foreach (GameObject child in children)
            {
                Destroy(child);
            }
            // 扣分
            PlayerPrefs.SetInt("total", PlayerPrefs.GetInt("total") - 5);
        }
        // 如果大于三把，则随机选择
        else
        {
            // 随机生成不重复的三个下标，destroy
            List<int> usedValues = new List<int>();
            for (int i = 0; i < 3; i++)
            {
                int val = Random.Range(0, children.Count - 1);
                while (usedValues.Contains(val))
                {
                    val = Random.Range(0, children.Count - 1);
                }
                Destroy(children[val]);
                usedValues.Add(val);
            }
            // 扣分
            PlayerPrefs.SetInt("total", PlayerPrefs.GetInt("total") - 5);
        }

    }


    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }
    
    public void GotEnd()
    {
        const int endScene = 10;
        SceneManager.LoadScene(endScene);
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }
    
}