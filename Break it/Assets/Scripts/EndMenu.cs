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
        // 统计埋点 统计 restart button 点击次数
        Dictionary<string, object> parameters = new Dictionary<string, object>()
        {
            {"Level", GameController.Instance.difficulty}
        };
        AnalyticsResult result = Analytics.CustomEvent("Click Restart", parameters);
        Analytics.FlushEvents();
        Debug.Log(result);
        // const int firstLevel = 1;
        // SceneManager.LoadScene(firstLevel);
        // load the level that the play just failed to reduce frustration
        SceneManager.LoadScene(GameController.Instance.currentScene);
    }

    public void exit()
    {
        // 统计埋点 统计 quit button 点击次数
        Dictionary<string, object> parameters = new Dictionary<string, object>()
        {
            {"Level", GameController.Instance.difficulty}
        };
        AnalyticsResult result = Analytics.CustomEvent("Click Quit", parameters);
        Analytics.FlushEvents();
        Debug.Log(result);
        Debug.Log("The game is quited");
        Application.Quit();
    }

    public void backToLevelManual()
    {
        SceneManager.LoadScene(1);
    }

}
