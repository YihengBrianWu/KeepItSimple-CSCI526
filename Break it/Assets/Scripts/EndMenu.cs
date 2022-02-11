using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void rePlay()
    {
        // 统计埋点
        Analytics.CustomEvent("Click Restart", new Dictionary<string, object>
        {
            {"Level", GameController.Instance.difficulty}
        });
        const int firstLevel = 1;
        SceneManager.LoadScene(firstLevel);
    }

    public void exit()
    {
        // 统计埋点
        Analytics.CustomEvent("Click Quit", new Dictionary<string, object>
        {
            {"Level", GameController.Instance.difficulty}
        });
        Debug.Log("The game is quited");
        Application.Quit();
    }

}
