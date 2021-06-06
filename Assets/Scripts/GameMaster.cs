using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    //TODO set these variables to the ACTUAL scene id of these scenes.
    public enum endScene
    {
        goodEnding = 2, // Murphey and Captain alive
        neutralEnding = 3, // Murphey alive
        badEnding = 4, // Murphey falls off boat or boat sinks
        badEnding2 = 5
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndGame(bool win, endScene sceneID)
    {
        MenuActions.HasWon = true;
        PlayerPrefs.SetInt("HasWon", 1);
        PlayerPrefs.Save();
        string str = win ? "You win" : "You lose";
        Debug.Log(str);
        StartCoroutine(LoadLevel((int)sceneID));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        Debug.Log("Loading scene# " + levelIndex);
        SceneManager.LoadScene(levelIndex);
        return null;
    }
}
