using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Images;

public abstract class UrlUI : MonoBehaviour
{

    public void setUrl(string url)
    {
        ImageLoader.get().addImageSet(this, url);
    }

    public Sprite loadingSprite;
    public Sprite failSprite;

    private void OnDestroy()
    {
        ImageLoader.get().removeImageSet(this);
    }

    public abstract void setSprite(Sprite sprite);

}