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
