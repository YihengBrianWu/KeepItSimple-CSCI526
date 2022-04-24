using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleOnLog : MonoBehaviour
{

    // [SerializeField]
    // private int TotalCircle;
    [SerializeField] 
    private GameObject circlePrefab;
    public List<RewardLevel>  RewardLevels;
    // private int levelParam;

    [SerializeField] 
    public bool needAdjust = false;
    void Start()
    {
        //levelParam = GameObject.FindGameObjectWithTag("LevelControl").GetComponent<GameController>().difficulty;
        SpawnCircle();
        //if (NeedKnife)
        //SpawnKnifeOnWheel();
    }

    private void SpawnCircle()
    {

        // int tempCount = TotalCircle; //- PlayerPrefs.GetInt("level"+levelParam, 0);
        foreach (float circleA in RewardLevels[0].circleAngle)
        {
            // if (tempCount == 0)
            //     break;
            GameObject tempCir = Instantiate(circlePrefab);
            tempCir.transform.SetParent(transform);

            SetRotation(transform, tempCir.transform, circleA, 0.2f, 0f);
            tempCir.transform.localScale= new Vector3(0.5f, 0.5f, 1f);
            // tempCount--;
        }
    }
    

    public void SetRotation(Transform log, Transform objectOnLog, float angle, float spaceFromLog, float objectRotation)
    {
        float r;
        if(needAdjust)
        {
            r = 0.4f;
        }
        else{
            r = log.GetComponent<CircleCollider2D>().radius;
        }
        Vector2 OffSet = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad)) * (r + spaceFromLog);
        objectOnLog.localPosition = (Vector2) log.localPosition + OffSet;
        objectOnLog.localRotation = Quaternion.Euler(0, 0, -angle + objectRotation);
    }


}

[System.Serializable] 
public class RewardLevel
{
    // [Range(0,1)] [SerializeField] private float rewardChance;

    public List<float> circleAngle = new List<float>();
}
