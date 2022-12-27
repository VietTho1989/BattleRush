using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Images.Cache;

namespace Images
{
    public class ImageSetUpdate : UpdateBehavior<ImageSet>
    {

        #region Update

        private ImageCache imageCache = null;

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (this.imageCache != null)
            {
                this.imageCache.removeCallBack(this);
                this.imageCache = null;
            }
        }

        public override void update()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    // check correct imageCache
                    {
                        if (this.imageCache != null)
                        {
                            if (this.imageCache.url.v != this.data.url.v)
                            {
                                this.imageCache.removeCallBack(this);
                                this.imageCache = null;
                            }
                        }
                    }
                    // find image cache
                    {
                        if (this.imageCache == null)
                        {
                            ImageLoader imageLoader = this.data.findDataInParent<ImageLoader>();
                            if (imageLoader != null)
                            {
                                this.imageCache = imageLoader.findImageCache(this.data.url.v);
                                if (this.imageCache != null)
                                {
                                    this.imageCache.addCallBack(this);
                                }
                                else
                                {
                                    // Debug.LogError("imageCache null");
                                }
                            }
                            else
                            {
                                Logger.LogError("imageLoader null");
                            }
                        }
                    }
                    // set
                    if (imageCache != null)
                    {
                        UrlUI urlUI = this.data.image.v;
                        if (urlUI != null)
                        {
                            switch (imageCache.state.v.getType())
                            {
                                case ImageCache.State.Type.Get:
                                    urlUI.setSprite(urlUI.loadingSprite);
                                    break;
                                case ImageCache.State.Type.Success:
                                    {
                                        Success success = imageCache.state.v as Success;
                                        urlUI.setSprite(success.getCommonSprite());// Sprite.Create(success.texture.v, new Rect(0, 0, success.texture.v.width, success.texture.v.height), new Vector2(0.5f, 0.5f));
                                    }
                                    break;
                                case ImageCache.State.Type.Fail:
                                    urlUI.setSprite(urlUI.failSprite);
                                    break;
                                default:
                                    Logger.LogError("unknown state: " + imageCache.state.v);
                                    break;
                            }
                        }
                        else
                        {
                            Logger.LogError("urlImage null");
                        }
                    }
                    else
                    {
                        Logger.LogError("imageCache null");
                        this.data.image.v.setSprite(this.data.image.v.failSprite);
                    }
                }
                else
                {
                    Logger.LogError("data null");
                }
            }
        }

        public override bool isShouldDisableUpdate()
        {
            return true;
        }

        #endregion

        #region implement callBacks

        private ImageLoader imageLoader = null;

        public override void onAddCallBack<T>(T data)
        {
            if(data is ImageSet)
            {
                ImageSet imageSet = data as ImageSet;
                // Parent
                {
                    DataUtils.addParentCallBack(imageSet, this, ref this.imageLoader);
                }
                dirty = true;
                return;
            }
            // Parent
            {
                if (data is ImageLoader)
                {
                    dirty = true;
                    return;
                }
                // Child
                {
                    if (data is ImageCache)
                    {
                        ImageCache imageCache = data as ImageCache;
                        // Child
                        {
                            imageCache.state.allAddCallBack(this);
                        }
                        dirty = true;
                        return;
                    }
                    // Child
                    if(data is ImageCache.State)
                    {
                        dirty = true;
                        return;
                    }
                }
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if (data is ImageSet)
            {
                ImageSet imageSet = data as ImageSet;
                // Parent
                {
                    DataUtils.removeParentCallBack(imageSet, this, ref this.imageLoader);
                }
                this.setDataNull(imageSet);
                return;
            }
            // Parent
            {
                if (data is ImageLoader)
                {
                    return;
                }
                // Child
                {
                    if (data is ImageCache)
                    {
                        ImageCache imageCache = data as ImageCache;
                        // Child
                        {
                            imageCache.state.allRemoveCallBack(this);
                        }
                        return;
                    }
                    // Child
                    if (data is ImageCache.State)
                    {
                        return;
                    }
                }
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onUpdateSync<T>(WrapProperty wrapProperty, List<Sync<T>> syncs)
        {
            if (WrapProperty.checkError(wrapProperty))
            {
                return;
            }
            if (wrapProperty.p is ImageSet)
            {
                switch ((ImageSet.Property)wrapProperty.n)
                {
                    case ImageSet.Property.image:
                        dirty = true;
                        break;
                    case ImageSet.Property.url:
                        dirty = true;
                        break;
                    default:
                        Logger.LogError("Don't process: " + wrapProperty + "; " + this);
                        break;
                }
                return;
            }
            // Parent
            {
                if (wrapProperty.p is ImageLoader)
                {
                    switch ((ImageLoader.Property)wrapProperty.n)
                    {
                        case ImageLoader.Property.imageCaches:
                            dirty = true;
                            break;
                        case ImageLoader.Property.imageSets:
                            break;
                        default:
                            Logger.LogError("Don't process: " + wrapProperty + "; " + this);
                            break;
                    }
                    return;
                }
                // Child
                {
                    if (wrapProperty.p is ImageCache)
                    {
                        switch ((ImageCache.Property)wrapProperty.n)
                        {
                            case ImageCache.Property.url:
                                dirty = true;
                                break;
                            case ImageCache.Property.state:
                                {
                                    ValueChangeUtils.replaceCallBack(this, syncs);
                                    dirty = true;
                                }
                                break;
                            default:
                                Logger.LogError("Don't process: " + wrapProperty + "; " + this);
                                break;
                        }
                        return;
                    }
                    // Child
                    if (wrapProperty.p is ImageCache.State)
                    {
                        ImageCache.State state = wrapProperty.p as ImageCache.State;
                        switch (state.getType())
                        {
                            case ImageCache.State.Type.Get:
                                break;
                            case ImageCache.State.Type.Success:
                                {
                                    switch ((Success.Property)wrapProperty.n)
                                    {
                                        case Success.Property.texture:
                                            dirty = true;
                                            break;
                                        default:
                                            Logger.LogError("Don't process: " + wrapProperty + "; " + this);
                                            break;
                                    }
                                }
                                break;
                            case ImageCache.State.Type.Fail:
                                break;
                            default:
                                Logger.LogError("unknown type: " + state.getType());
                                break;
                        }
                        return;
                    }
                }
            }
            Logger.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

    }
}