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
        // Dictionary<string, object> parameters = new Dictionary<string, object>()
        // {
        //     {"Level", GameController.Instance.difficulty}
        // };
        // Analytics.CustomEvent("ClickRestart", parameters);
        
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
        Analytics.CustomEvent("ClickQuit", parameters);
        Debug.Log("The game is quited");
        Application.Quit();
    }

    public void backToMain(){
        SceneManager.LoadScene(0);
    }

    public void infinity()
    {
        SceneManager.LoadScene(32);
    }

}
