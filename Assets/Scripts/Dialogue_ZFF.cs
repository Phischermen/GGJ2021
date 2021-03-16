using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue_ZFF : MonoBehaviour
{

    public TextMeshProUGUI textDisplay;
    public Sprite portraitDisplay;
    public Message[] sentences;
    private int index;
    public float typingSpeed;
    public GameObject dismissButton;
    
    [System.Serializable]
    public class Message
    {
        public string sentence;
        public Sprite portrait;
    }

    private void Update()
    {
        if (textDisplay.text == sentences[index].sentence)
        {
            dismissButton.SetActive(true);
        }
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

    private void Dismiss()
    {
        // The dialogue box will *shoop!* downwards when dismissed
        dismissButton.SetActive(false);
    }

}
