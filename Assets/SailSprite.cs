using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SailSprite : MonoBehaviour
{
    public Sprite full;
    public Sprite medium;
    public Sprite low;
    public Sprite tied;

    public SpriteRenderer sprRenderer;
    // Start is called before the first frame update
    void Start()
    {
        sprRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeSprite(Sprite spr)
    {
        if (sprRenderer.sprite != spr)
        {
            sprRenderer.sprite = spr;
        }
    }

    public void SpriteFlip(bool flipped)
    {
        sprRenderer.flipY = flipped;
    }
}
