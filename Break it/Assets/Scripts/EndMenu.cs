using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void rePlay()
    {
        const int firstLevel = 1;
        SceneManager.LoadScene(firstLevel);
    }

    public void exit()
    {
        Debug.Log("The game is quited");
        Application.Quit();
    }

}
