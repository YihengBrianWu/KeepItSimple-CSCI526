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

    
    // 是否需要开启facemouse功能
    [Header("unility")] 
    [SerializeField]
    public bool faceMouse = true;
    
    // 开启knife移动功能
    [SerializeField] 
    public bool loopMove = false;
    
    // 开启计时功能
    [SerializeField] 
    public bool timeCount = false;
    
    // 限时多久
    [SerializeField] 
    public float timeDuration = 0;

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
    private AudioSource music;
    private AudioClip levelUp;
    private AudioClip getKnives;
    private AudioClip vanish;


    // pause menu
    public GameObject pauseMenu;
    public static bool isPaused = false;
    
    // 追踪新生成的knife
    public GameObject newKnife;
    public bool isShort = false;
    public int predict = 3;
    public bool isBlack;

    public GameObject nextKnife;
    [SerializeField]
    private bool isExampleLevel = false;
    [SerializeField]
    private GameObject TipTwo;
    [SerializeField]
    private bool needToShowTipTwo = false;

    private void Awake()
    {
        PlayerPrefs.SetInt("total", 50);
        isPaused = false;
        Instance = this;
        GameUI = GetComponent<GameUI>();
        knifeObject = normalKnife;
        //PlayerPrefs.SetInt("item4", 0);
        // if (PlayerPrefs.GetInt("extraKnife", 0) == 4)
        // {
        //     knifeCount += 2;
        //     knifeAmount = knifeCount;
        // }

        knifeAmount = knifeCount;

        music = gameObject.GetComponent<AudioSource>();
        levelUp = Resources.Load<AudioClip>("sound/levelUp");
        getKnives = Resources.Load<AudioClip>("sound/getKnives");
        vanish = Resources.Load<AudioClip>("sound/vanish");
    }

    private bool obstacleDestoryUsed = false;
    [SerializeField]
    private bool containWall = false;
    public void DestroyRandomObstacle()
    {

        if(obstacleDestoryUsed)
        {
            return;
        }
        
        obstacleDestoryUsed = true;

        bool chooseReflect = true;
        if (!containObstacle && !containWall)
        {
            return;
        }
        else if (!containObstacle && containWall)
        {
            chooseReflect= true;
        }
        else if (containObstacle && !containWall)
        {
            chooseReflect= false;
        }
        else if (containObstacle && containWall)
        {
            if(Random.Range(0, 2) == 1)
            {
                chooseReflect= false;
            }
        }

        if(!chooseReflect)
        {
            GameObject[] obstacles;
            obstacles = GameObject.FindGameObjectsWithTag("MovingObstacle");
            if (obstacles.Length == 1)
            {
                obstacles[0].GetComponent<ObstaclesDestory>().selfDestoryObstacles();
                // 扣分
                if (!isExampleLevel)
                {
                    PlayerPrefs.SetInt("total", PlayerPrefs.GetInt("total") - 7);
                }
            }
            // 随机选择
            else
            {
                // 随机选择下标
                int val = Random.Range(0, obstacles.Length);
                if (val == obstacles.Length)
                {
                    val -= 1;
                }
                
                obstacles[val].GetComponent<ObstaclesDestory>().selfDestoryObstacles();
                // 扣分
                if (!isExampleLevel)
                {
                    PlayerPrefs.SetInt("total", PlayerPrefs.GetInt("total") - 7);
                }
            }
        }
        else
        {
            ///Wall
            GameObject[] walls;
            walls = GameObject.FindGameObjectsWithTag("Wall");
            if (walls.Length == 1)
            {
                walls[0].GetComponent<ObstaclesDestory>().selfDestoryObstacles();
                // 扣分
                if (!isExampleLevel)
                {
                    PlayerPrefs.SetInt("total", PlayerPrefs.GetInt("total") - 7);
                }
            }
            // 随机选择
            else
            {
                // 随机选择下标
                int val = Random.Range(0, walls.Length);
                if (val == walls.Length)
                {
                    val -= 1;
                }
                
                walls[val].GetComponent<ObstaclesDestory>().selfDestoryObstacles();
                // 扣分
                if (!isExampleLevel)
                {
                    PlayerPrefs.SetInt("total", PlayerPrefs.GetInt("total") - 7);
                }
            }
        }

        music.clip = vanish;
        music.Play();

        if (isExampleLevel)
        {
          TipTwo.SetActive(true);
        }

    }

    private void Start()
    {
        Time.timeScale = 1;
        currentScene = SceneManager.GetActiveScene().buildIndex;
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
    public int hitOnLog = 0;
    private int failHit = 0;

    public void hitOnLogInc()
    {
        hitOnLog++;
    }

    public void failitInc()
    {
        failHit++;
    }

    // IEnumerator WaitThreeS()
    // {
    //     yield return new WaitForSeconds(1.0f);
    //     PlayerPrefs.SetInt("levelReached", Math.Max(currentScene - 1, PlayerPrefs.GetInt("levelReached", 0)));
    //     SceneManager.LoadScene(currentScene + 1);
    // }
    [SerializeField]
    private bool containObstacle = false;
    public void destoryLog()
    {
        if (containObstacle)
        {
            GameObject.FindGameObjectWithTag("ObstacleGroup").SetActive(false);
        }
        // GameObject ogLog = GameObject.FindGameObjectWithTag("Log");
        // GameObject logPieces = (GameObject)Instantiate(logBreak);
        // logPieces.transform.position = ogLog.transform.position;
        // ogLog.SetActive(false);
    }

    IEnumerator WaitBreak()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject knifeToShot2 = GameObject.FindGameObjectWithTag("Knife");
        knifeToShot2.SetActive(false);
        destoryLog();
        StartCoroutine("levelUpRoutine");
    }

    IEnumerator levelUpRoutine()
    {
        GameUI.showLevelUp();
        GameObject knifeToShot2 = GameObject.FindGameObjectWithTag("Knife");
        knifeToShot2.SetActive(false);
        music.clip = levelUp;
        music.Play();
        yield return new WaitForSeconds(1.0f);
        PlayerPrefs.SetInt("levelReached", Math.Max(currentScene - 1, PlayerPrefs.GetInt("levelReached", 0)));
        SceneManager.LoadScene(currentScene + 1);
    }
    IEnumerator WaitFail()
    {
        yield return new WaitForSeconds(0.7f);
        if (!isInfinity)
        {
            SceneManager.LoadScene(14);
        }
        else
        {
            SceneManager.LoadScene(15);
        }
    }
    public void OnSuccessfulKnifeHit()
    {
        if (needToShowTipTwo)
        {
            TipTwo.SetActive(true);
            needToShowTipTwo = false;
        }
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

            // GameObject knifeToShot = GameObject.FindGameObjectWithTag("Knife");
            // knifeToShot.SetActive(false);
            StartCoroutine("WaitBreak");

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

    // public void ConvertBlack()
    // {
    //     if(PlayerPrefs.GetInt("itemSelected",0) == 1)
    //     {
    //          knifeObject = smallKnifeB;
    //     }
    //     else if(PlayerPrefs.GetInt("itemSelected",0) == 2)
    //     {
    //         knifeObject = shortKnifeB;
    //     }
    //     else if(PlayerPrefs.GetInt("itemSelected",0) == 3)
    //     {
    //         knifeObject = smallAndShortKnifeB;
    //     }
    //     else{
    //         knifeObject = normalKnifeB;
    //     }
    // }
    //     public void ConvertWhite()
    // {
    //     if(PlayerPrefs.GetInt("itemSelected",0) == 1)
    //     {
    //          knifeObject = smallKnife;
    //     }
    //     else if(PlayerPrefs.GetInt("itemSelected",0) == 2)
    //     {
    //         knifeObject = shortKnife;
    //     }
    //     else if(PlayerPrefs.GetInt("itemSelected",0) == 3)
    //     {
    //         knifeObject = smallAndShortKnife;
    //     }
    //     else{
    //         knifeObject = normalKnife;
    //     }
    // }
    public void SpawnKnife()
    {           
        if (knifeCount > 0) 
        {
            int ran = UnityEngine.Random.Range(0, 2);

            if (difficulty == 2 && predict == 1)
            {
                isBlack = true;
            }
            if (difficulty == 2 && predict == 0)
            {
                isBlack = false;
            }

            if (difficulty == 2 && ran == 1)
            {
                nextKnife.GetComponent<SpriteRenderer>().color = new Color32(0,0,0,255);
                predict = 1;
            }
            if (difficulty == 2 && ran == 0)
            {
                nextKnife.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,255);
                predict = 0;
            }

            if (isExampleLevel && difficulty == 2)
            {
                isBlack = true;
                predict = 0;
                isExampleLevel = false;  
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
        if(PlayerPrefs.GetInt("total") < 5 && !isExampleLevel)
        {
            return;
        }

        // 音效
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
                child.GetComponent<KnifeScript>().selfDestory();
            }
            // 扣分
            if (!isExampleLevel)
            {
                PlayerPrefs.SetInt("total", PlayerPrefs.GetInt("total") - 5);
            }
            
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
                children[val].GetComponent<KnifeScript>().selfDestory();
                usedValues.Add(val);
            }
            // 扣分
            if (!isExampleLevel)
            {
                PlayerPrefs.SetInt("total", PlayerPrefs.GetInt("total") - 5);
            }
        }

        music.clip = vanish;
        music.Play();

        if (isExampleLevel)
        {
          TipTwo.SetActive(true);
        }

    }

    private bool knifeAdded = false;
    public void addKnifes()
    {
        if(PlayerPrefs.GetInt("total") < 5 && !isExampleLevel)
        {
            return;
        }

        if (!knifeAdded)
        {
            knifeAmount += 3;
            knifeCount += 3;
            GameUI.panelAddKnifes();
            knifeAdded = true;
            if (isExampleLevel)
            {
                TipTwo.SetActive(true);
            }
            else
            {
                PlayerPrefs.SetInt("total", PlayerPrefs.GetInt("total") - 5);
            }

            music.clip = getKnives;
            music.Play();
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
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(currentScene);
    }
    
}