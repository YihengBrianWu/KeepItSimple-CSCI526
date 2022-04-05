using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManagerScript : MonoBehaviour
{
    public Button[] levels;

    private void Start()
    {
        for(int i = 0; i < levels.Length; i++)
        {
            levels[i].interactable = false;

        }
    }
    public void Select(string levelName)
    {

    }

}
