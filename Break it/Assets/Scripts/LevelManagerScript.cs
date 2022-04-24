using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManagerScript : MonoBehaviour
{
    public Button[] whites;
    public Button[] blacks;

    private void Start()
    {
        int levelReachedW = PlayerPrefs.GetInt("levelReachedW", 0);
        for (int i = 1; i < whites.Length; i++)
        {
            if(i > levelReachedW)
            {
                whites[i].interactable = false;
            }
        }

        int levelReachedB = PlayerPrefs.GetInt("levelReachedB", 0);
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
        int levelReachedW = PlayerPrefs.GetInt("levelReachedW", 0);
        for (int i = 0; i < whites.Length; i++)
        {
            if(i > levelReachedW)
            {
                whites[1].interactable = false;
            }
        }

        int levelReachedB = PlayerPrefs.GetInt("levelReachedB", 0);
        for (int i = 0; i < blacks.Length; i++)
        {
            if(i > levelReachedB)
            {
                blacks[1].interactable = false;
            }
        }
    }

    public void SelectW(int levelName)
    {
        const int numberOfPreScene = 3;
        SceneManager.LoadScene(levelName + numberOfPreScene);
    }

    public void SelectB(int levelName)
    {
        const int numberOfPreScene = 12;
        SceneManager.LoadScene(levelName + numberOfPreScene);
    }

    public void GotEnd()
    {
        SceneManager.LoadScene(0);
    }

}