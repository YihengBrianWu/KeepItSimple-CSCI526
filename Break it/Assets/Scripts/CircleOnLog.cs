using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleOnLog : MonoBehaviour
{

    // [SerializeField]
    // private int TotalCircle;
    [SerializeField]
    private bool NeedKnife;
    [SerializeField] 
    private GameObject circlePrefab;
    [SerializeField] 
    private GameObject knifePrefab;
    public List<RewardLevel>  RewardLevels;
    // private int levelParam;

    void Start()
    {
        //levelParam = GameObject.FindGameObjectWithTag("LevelControl").GetComponent<GameController>().difficulty;
        SpawnCircle();
        if (NeedKnife)
        SpawnKnifeOnWheel();
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
    
    private void SpawnKnifeOnWheel()
    {
        foreach (float knifeA in RewardLevels[0].kinfeAngle)
        {
            GameObject tempKnife = Instantiate(knifePrefab);
            tempKnife.transform.SetParent(transform);
            SetRotation(transform, tempKnife.transform, knifeA, 0.25f, 180f);
        }
    }

    public void SetRotation(Transform log, Transform objectOnLog, float angle, float spaceFromLog, float objectRotation)
    {
        Vector2 OffSet = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad)) * (log.GetComponent<CircleCollider2D>().radius + spaceFromLog);
        objectOnLog.localPosition = (Vector2) log.localPosition + OffSet;
        objectOnLog.localRotation = Quaternion.Euler(0, 0, -angle + objectRotation);
    }


}

[System.Serializable] 
public class RewardLevel
{
    // [Range(0,1)] [SerializeField] private float rewardChance;

    public List<float> circleAngle = new List<float>();
    public List<float> kinfeAngle = new List<float>();
}
