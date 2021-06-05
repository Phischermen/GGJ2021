using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelLoader : MonoBehaviour
{

    public Animator transition;

    public float transitionTime = 1f;

    public AudioSource transitionSFX;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel(string levelName)
    {
        StartCoroutine(LoadLevelTransition(levelName));
    }

    IEnumerator LoadLevelTransition(string levelName)
    {
        transition.SetTrigger("start");
        transitionSFX.Play();
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelName);
    }

}
