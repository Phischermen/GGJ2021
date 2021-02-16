using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue_ZFF : MonoBehaviour
{

    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    private int index;
    public float typingSpeed;

    private void Start()
    {
        StartCoroutine(Type());
    }

    IEnumerator Type()
    {

        foreach(char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

    }

}
