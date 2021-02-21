using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue_ZFF : MonoBehaviour
{

    public TextMeshProUGUI textDisplay;
    public Message[] sentences;
    private int index;
    public float typingSpeed;
    [System.Serializable]
    public class Message
    {
        public string sentence;
        public Sprite portrait;
    }
    private void Start()
    {
        StartCoroutine(Type());
    }

    IEnumerator Type()
    {

        foreach(char letter in sentences[index].sentence.ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

    }

}
