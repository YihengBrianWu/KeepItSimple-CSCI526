using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    
    // 展示knife使用情况的panel
    [Header("Knife Count Display")] [SerializeField]
    private GameObject panelKnives;
    
    // knife的prefab对象
    [SerializeField] 
    private GameObject iconKnife;
    
    // 使用过的knife的颜色
    [SerializeField] 
    private Color usedKnifeIconColor;
    
    // Level up panel
    [Header("LevelPanel")] [SerializeField] 
    private GameObject LevelUpPanel;

    // Log 
    [Header("LogObj")] [SerializeField] 
    private GameObject LogObj;

    [Header("ScoreObj1")] [SerializeField] 
    private GameObject ScoreObj1;

    [Header("ScoreObj2")] [SerializeField] 
    private GameObject ScoreObj2;

    [Header("KnifeIconObj")] [SerializeField] 
    private GameObject KnifeIconObj;

    public void showLevelUp()
    {
        LogObj.SetActive(false);
        LevelUpPanel.SetActive(true);
        ScoreObj1.SetActive(false);
        ScoreObj2.SetActive(false);
        KnifeIconObj.SetActive(false);
    }

    // 生成图像
    public void SetInitialDisplayedKnifeCount(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(iconKnife, panelKnives.transform);
        }
    }
    
    // track使用过的knife的下标
    private int knifeIconIndexToChange = 0;
    
    // 改变颜色
    public void DecrementDisplayedKnifeCount()
    {
        panelKnives.transform.GetChild(knifeIconIndexToChange++).GetComponent<Image>().color = usedKnifeIconColor;
    }

}
