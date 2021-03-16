using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PreGameSceneController : MonoBehaviour
{
    AsyncOperation loadScene;
    InstructionManual instructionManual;
    bool WantsToStartGame = false;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        instructionManual = GetComponent<InstructionManual>();
        // Wait for instruction manual to initialize
        yield return null;
        instructionManual.preGameVersion = true;
        instructionManual.OnNextPressedOnLastPage += StartGame;
        instructionManual.OpenManual(atLog: true);
        StartCoroutine(nameof(LoadGamePlayScene));
    }
    /*In my testing, if the pre game scene is ran from the inspector,
     * the gameplay scene is loaded and switched to imediately. I do not
     * know why this happens. Thankfully, when the pre game scene is 
     * switched to from another, I have no problems. If the the pre game
     * scene appears to "skip" immediately then I will refactor this to 
     * not use async loading.
     * Thanks Unity (ô_ô) */
    IEnumerator LoadGamePlayScene()
    {
        loadScene = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        loadScene.allowSceneActivation = false;
        while (!loadScene.isDone)
        {
            Debug.Log(loadScene.progress);
            if (WantsToStartGame)
            {
                loadScene.allowSceneActivation = true;
            }
            yield return null;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    void StartGame()
    {
        Debug.Log("(ô_ô)");
        WantsToStartGame = true;
    }
}
