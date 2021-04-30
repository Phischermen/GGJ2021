using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CursorGuider : MonoBehaviour
{
    public Button button;
    public CursorGuider nextGuider;
    public bool first = false;

    RectTransform rt;
    Vector2 startPos;
    public float timeScale;
    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(buttonClicked);
        rt = GetComponent<RectTransform>();
        startPos = button.transform.position;
        if (first == false) gameObject.SetActive(false);
    }

    private void Update()
    {
        var offset = Mathf.Abs(Mathf.Sin(Time.time * timeScale)) * 10f;
        rt.position = startPos + new Vector2(offset, -offset);
    }

    private void buttonClicked()
    {
        Destroy(gameObject);
        if (nextGuider != null)
            nextGuider.gameObject.SetActive(true);
    }
}
