using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndGame(bool win, int sceneID)
    {
        string str = win ? "You win" : "You lose";
        Debug.Log(str);
        StartCoroutine(LoadLevel(sceneID));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        yield return new WaitForSeconds(1);
        Debug.Log("Loading scene# " + levelIndex);
        SceneManager.LoadScene(levelIndex);
    }
}
