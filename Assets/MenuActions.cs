using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuActions : MonoBehaviour
{
    public static bool HasPlayed = false;
    public void Play()
    {
        SceneManager.LoadScene(HasPlayed ? 1 : 8);
        HasPlayed = true;
    }

    public void Credits()
    {
        SceneManager.LoadScene(6);
    }

    public void Return()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
