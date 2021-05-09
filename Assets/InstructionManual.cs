using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;

public class InstructionManual : MonoBehaviour
{
    public enum ManualContents
    {
        log = 0,
        log1 = 0,
        log2 = 1,
        log3 = 2,
        instructions = 3,
        manning = 3,
        repairing = 4,
        helm = 5,
        rudder = 6,
        sail = 7,
        lantern = 8,
        captain = 9,
    }
    // Initialized via inspector
    public Button next;
    public Button prev;
    public Image nextImage;
    public Sprite nextSprite;
    public Sprite playSprite;
    public Sprite closeSprite;

    public Image image;
    public AudioSource pageTurn;
    public AudioSource bookOpenClose;
    public AudioClip bookOpen;
    public AudioClip bookClose;

    [HideInInspector]
    public Sprite[] manualPages;
    public Action OnNextPressedOnLastPage;
    public Action<int> OnPageOpen;
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
                nextImage.sprite = preGameVersion ? playSprite : closeSprite;
            }
            else
            {
                nextImage.sprite = nextSprite;
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
    public bool OnLastPage
    {
        get { return Page == manualPages.Length - 1; }
    }
    public bool open = false;
    public bool preGameVersion = true;
    // Start is called before the first frame update
    void Start()
    {
        // Manual starts closed
        GetComponent<Canvas>().enabled = false;
        manualPages = Resources.LoadAll<Sprite>("Sprites/InstructionManual/Manual01");
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
        if (!OnLastPage)
        {
            pageTurn.Play();
            Page += 1;
            OnPageOpen?.Invoke(Page);
            prev.gameObject.SetActive(true);
        }
        else
        {
            OnNextPressedOnLastPage?.Invoke();
        }
        
    }

    void PrevPressed()
    {
        if (Page != 0)
        {
            pageTurn.Play();
            Page -= 1;
            OnPageOpen?.Invoke(Page);
        }
    }

    public void OpenManual(bool atLog = false)
    {
        if (open) return;
        open = true;
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
    public void CloseManual()
    {
        if (!open) return;
        open = false;
        bookOpenClose.PlayOneShot(bookClose);
        GetComponent<Canvas>().enabled = false;
    }
}
