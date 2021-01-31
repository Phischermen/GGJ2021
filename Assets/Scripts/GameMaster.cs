﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    //TODO set these variables to the ACTUAL scene id of these scenes.
    public enum endScene
    {
        goodEnding = 0, // Murphey and Captain alive
        badEnding = 1, // Murphey falls off boat or boat sinks
        neutralEnding = 2 // Murphey alive
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
        string str = win ? "You win" : "You lose";
        Debug.Log(str);
        StartCoroutine(LoadLevel((int)sceneID));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        yield return new WaitForSeconds(1);
        Debug.Log("Loading scene# " + levelIndex);
        SceneManager.LoadScene(levelIndex);
    }
}
