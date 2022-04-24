using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelHardManager : MonoBehaviour
{
    public Button[] whites;
    public Button[] blacks;

    private void Start()
    {
        int levelReachedW = PlayerPrefs.GetInt("levelReachedWH", 0);
        for (int i = 1; i < whites.Length; i++)
        {
            if(i > levelReachedW)
            {
                whites[i].interactable = false;
            }
        }

        int levelReachedB = PlayerPrefs.GetInt("levelReachedBH", 0);
        for (int i = 1; i < blacks.Length; i++)
        {
            if(i > levelReachedB)
            {
                blacks[i].interactable = false;
            }
        }
    }

    private void Awake()
    {
        int levelReachedW = PlayerPrefs.GetInt("levelReachedWH", 0);
        for (int i = 0; i < whites.Length; i++)
        {
            if(i > levelReachedW)
            {
                whites[1].interactable = false;
            }
        }

        int levelReachedB = PlayerPrefs.GetInt("levelReachedBH", 0);
        for (int i = 0; i < blacks.Length; i++)
        {
            if(i > levelReachedB)
            {
                blacks[1].interactable = false;
            }
        }
    }

    public void SelectWH(int levelName)
    {
        const int numberOfPreScene = 19;
        SceneManager.LoadScene(levelName + numberOfPreScene);
    }

    public void SelectBH(int levelName)
    {
        const int numberOfPreScene = 24;
        SceneManager.LoadScene(levelName + numberOfPreScene);
    }

    public void GotEnd()
    {
        SceneManager.LoadScene(0);
    }

}