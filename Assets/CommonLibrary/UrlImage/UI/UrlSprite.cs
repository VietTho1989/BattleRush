using System.Collections;
using System.Collections.Generic;
using Images;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Sprite))]
public class UrlSprite : UrlUI
{

    private SpriteRenderer SpriteRenderer;

    public SpriteRenderer spriteRenderer
    {
        get
        {
            if (SpriteRenderer == null)
            {
                SpriteRenderer = this.GetComponent<SpriteRenderer>();
            }
            return SpriteRenderer;
        }
    }

    public override void setSprite(Sprite sprite)
    {
        this.spriteRenderer.sprite = sprite;
    }

}