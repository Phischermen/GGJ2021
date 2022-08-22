using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Caramelldansen : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Color[] colors;

    public float freq;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        var waitForSeconds = new WaitForSeconds(freq);
        while (true)
        {
            spriteRenderer.color = colors[Random.Range(0, colors.Length - 1)];
            yield return waitForSeconds;
        }
    }
}
