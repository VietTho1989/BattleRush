using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Images;

[RequireComponent(typeof(Image))]
public class UrlImage : UrlUI
{

    private Image Image;

    public Image image
    {
        get
        {
            if (Image == null)
            {
                Image = this.GetComponent<Image>();
            }
            return Image;
        }
    }

    public override void setSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }

}