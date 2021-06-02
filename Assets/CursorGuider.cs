using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CursorGuider : MonoBehaviour
{
    public Button button;
    [HideInInspector]
    public RawImage rawImage;
    public CursorGuider nextGuider;
    public bool hideOnPlay = false;

    RectTransform rt;
    Vector2 startPos;
    public float timeScale;
    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(buttonClicked);
        rawImage = GetComponent<RawImage>();
        rt = GetComponent<RectTransform>();
        startPos = button.transform.position;
        if (hideOnPlay) gameObject.SetActive(false);
    }

    private void Update()
    {
        var offset = Mathf.Abs(Mathf.Sin(Time.time * timeScale)) * 10f;
        rt.position = startPos + new Vector2(offset, -offset);
        rawImage.enabled = button.isActiveAndEnabled;
    }

    public void Reveal()
    {
        gameObject.SetActive(true);
    }

    private void buttonClicked()
    {
        if (isActiveAndEnabled == false) return;
        Destroy(gameObject);
        if (nextGuider != null)
            nextGuider.gameObject.SetActive(true);
    }
}
