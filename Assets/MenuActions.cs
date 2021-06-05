using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuActions : MonoBehaviour
{
    public levelLoader levelLoader;
    public static bool HasPlayed = false;
    public void Play()
    {
        levelLoader.LoadLevel(HasPlayed ? "GameplayScene" : "PreGame1");
        HasPlayed = true;
    }

    public void HardMode()
    {
        //
    }

    public void Credits()
    {
        SceneManager.LoadScene(6);
    }

    public void Return()
    {
        levelLoader.LoadLevel("TitleCard")
    }

    public void Quit()
    {
        Application.Quit();
    }
}
