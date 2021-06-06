using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PreGame1SceneController : MonoBehaviour
{
    AsyncOperation loadScene;
    string message;
    public float speed = 0.09f;
    public float newLinePause = 0.2f;
    bool wantsToStartGame;
    public GameObject PlayButton;
    float timeUntilSkipPossible = 1f;
    bool wantsToSkipLimmerick;
    
    void Start()
    {
        StartCoroutine(nameof(LimmerickDisplay));
    }

    private void Update()
    {
        timeUntilSkipPossible -= Time.deltaTime;
        if (Input.anyKeyDown && timeUntilSkipPossible < 0)
        {
            wantsToSkipLimmerick = true;
        }
    }

    IEnumerator LimmerickDisplay()
    {
        StartCoroutine(nameof(LoadGamePlayScene));
        PlayButton.SetActive(false);
        var textComponent = GetComponentInChildren<Text>();
        var waitCharacter = new WaitForSeconds(speed);
        var waitNewLine = new WaitForSeconds(newLinePause);
        message = textComponent.text;
        textComponent.text = "";
        foreach (var character in message)
        {
            if (character != '\n')
            {
                textComponent.text += character;
                // Pressing any key causes full line to be displayed
                if (!wantsToSkipLimmerick) yield return waitCharacter;
            }
            else
            {
                yield return waitNewLine;
                wantsToSkipLimmerick = false;
                textComponent.text += character;
            }
        }
        textComponent.text = message;
        PlayButton.SetActive(true);
    }

    IEnumerator LoadGamePlayScene()
    {
        loadScene = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        loadScene.allowSceneActivation = false;
        while (!loadScene.isDone)
        {
            //Debug.Log(loadScene.progress);
            if (wantsToStartGame)
            {
                loadScene.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    public void StartGame()
    {
        Debug.Log("(ô_ô)");
        wantsToStartGame = true;
    }
}
