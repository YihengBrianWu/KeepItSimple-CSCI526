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
        int levelReachedW = PlayerPrefs.GetInt("levelReachedW", 0)-2;
        //print(levelReachedW);
        for (int i = 1; i < whites.Length; i++)
        {
            if(i > levelReachedW)
            {
                whites[i].interactable = false;
            }
        }

        int levelReachedB = PlayerPrefs.GetInt("levelReachedB", 0) - 12;
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
        int levelReachedW = PlayerPrefs.GetInt("levelReachedW", 0) - 2;
        for (int i = 1; i < whites.Length; i++)
        {
            if(i > levelReachedW)
            {
                whites[i].interactable = false;
            }
        }

        int levelReachedB = PlayerPrefs.GetInt("levelReachedB", 0) - 12;
        for (int i = 1; i < blacks.Length; i++)
        {
            if(i > levelReachedB)
            {
                blacks[i].interactable = false;
            }
        }
    }

    public void SelectW(int levelName)
    {
        const int numberOfPreScene = 3;
        //print(levelName);
        //print(numberOfPreScene);
        SceneManager.LoadScene(levelName + numberOfPreScene);
    }

    public void SelectB(int levelName)
    {
        const int numberOfPreScene = 13;
        SceneManager.LoadScene(levelName + numberOfPreScene);
    }

    public void GotEnd()
    {
        SceneManager.LoadScene(0);
    }

}