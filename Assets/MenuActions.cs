using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuActions : MonoBehaviour
{
    public levelLoader levelLoader;
    public static bool HasPlayed = false;
    public static bool HasWon = false;
    public static bool HardModeEnabled = false;
    /* Since the title card was built such that every button in the menu has 
     * MenuActions attached to it, this flag is the most convenient way of 
     * identifying the hard mode button! */
    public bool IsHardModeButton = false;
    public bool IsHardModeUnlockedText = false;

    private void Awake()
    {
        HardModeEnabled = false;
        HasWon = PlayerPrefs.GetInt("HasWon", 0) != 0;
        if (IsHardModeButton) GetComponent<Button>().interactable = HasWon;
        if (IsHardModeUnlockedText && !HasWon) gameObject.SetActive(false);
    }
    public void Play()
    {
        levelLoader.LoadLevel(HasPlayed ? "GameplayScene" : "PreGame1");
        HasPlayed = true;
    }

    public void HardMode()
    {
        HardModeEnabled = true;
        Play();
    }

    public void Credits()
    {
        SceneManager.LoadScene(6);
    }

    public void Return()
    {
        levelLoader.LoadLevel("TitleCard");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
