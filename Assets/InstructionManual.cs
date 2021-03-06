using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InstructionManual : MonoBehaviour
{
    // Initialized via inspector
    public Button next;
    public Button prev;

    public Image image;
    public AudioSource pageTurn;

    public Sprite[] manualPages;

    private int _page;
    private int maxPage;
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
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        manualPages = Resources.LoadAll<Sprite>("Sprites/InstructionManual/Manual");
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
}
