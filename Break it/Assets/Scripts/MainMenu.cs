using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //start the game
    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void HardMenu()
    {
        SceneManager.LoadScene(2);
    }

    //quit the game
    public void Quit()
    {
        Debug.Log("The game is quited");
        Application.Quit();
    }

    public void infinity()
    {
        SceneManager.LoadScene(32);
    }
}
