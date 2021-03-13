using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InstructionManual : MonoBehaviour
{
    enum ManualContents
    {
        log,
        instructions
    }
    // Initialized via inspector
    public Button next;
    public Button prev;
    public Text nextText;

    public Image image;
    public AudioSource pageTurn;
    public AudioSource bookOpenClose;
    public AudioClip bookOpen;
    public AudioClip bookClose;

    [HideInInspector]
    public Sprite[] manualPages;

    private int _page;
    public int Page
    {
        get
        {
            return _page;
        }
        set
        {
            value = (int)Mathf.Clamp(value, 0f, manualPages.Length);
            _page = value;
            image.sprite = manualPages[value];

            if (Page == manualPages.Length - 1)
            {
                nextText.text = "Play";
            }
            else
            {
                nextText.text = "Next";
            }
            if (Page == 0)
            {
                prev.gameObject.SetActive(false);
            }
            else
            {
                prev.gameObject.SetActive(true);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        manualPages = Resources.LoadAll<Sprite>("Sprites/InstructionManual/Manual");
        Page = 0;
        next.onClick.AddListener(NextPressed);
        prev.onClick.AddListener(PrevPressed);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void NextPressed()
    {
        if (Page != manualPages.Length - 1)
        {
            pageTurn.Play();
            Page += 1;
            prev.gameObject.SetActive(true);

        }
    }

    void PrevPressed()
    {
        if (Page != 0)
        {
            pageTurn.Play();
            Page -= 1;
        }
    }

    public void OpenInstructions(bool atLog)
    {
        bookOpenClose.PlayOneShot(bookOpen);
        GetComponent<Canvas>().enabled = true;
        if (atLog)
        {
            Page = (int)ManualContents.log;
        }
        else
        {
            Page = (int)ManualContents.instructions;
        }
    }
    public void CloseInstructions()
    {
        bookOpenClose.PlayOneShot(bookClose);
        GetComponent<Canvas>().enabled = false;
    }
}
