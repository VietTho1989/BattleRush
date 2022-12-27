using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Images
{
    public class ImageLoaderUpdate : UpdateBehavior<ImageLoader>
    {

        #region Update

        public override void update()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {

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

        private void Awake()
        {
            this.setData(ImageLoader.get());
            DontDestroyOnLoad(this);
        }

        #region implement callBacks

        public override void onAddCallBack<T>(T data)
        {
            if(data is ImageLoader)
            {
                ImageLoader imageLoader = data as ImageLoader;
                // Update
                {
                    UpdateUtils.makeUpdate<MakeImageCacheUpdate, ImageLoader>(imageLoader, this.transform);
                }
                // Child
                {
                    imageLoader.imageCaches.allAddCallBack(this);
                    imageLoader.imageSets.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            {
                if (data is ImageCache)
                {
                    ImageCache imageCache = data as ImageCache;
                    // Update
                    {
                        UpdateUtils.makeUpdate<ImageCacheUpdate, ImageCache>(imageCache, this.transform);
                    }
                    dirty = true;
                    return;
                }
                if(data is ImageSet)
                {
                    ImageSet imageSet = data as ImageSet;
                    // Update
                    {
                        UpdateUtils.makeUpdate<ImageSetUpdate, ImageSet>(imageSet, this.transform);
                    }
                    dirty = true;
                    return;
                }
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if (data is ImageLoader)
            {
                ImageLoader imageLoader = data as ImageLoader;
                // Update
                {
                    imageLoader.removeCallBackAndDestroy(typeof(MakeImageCacheUpdate));
                }
                // Child
                {
                    imageLoader.imageCaches.allRemoveCallBack(this);
                    imageLoader.imageSets.allRemoveCallBack(this);
                }
                this.setDataNull(imageLoader);
                return;
            }
            // Child
            {
                if (data is ImageCache)
                {
                    ImageCache imageCache = data as ImageCache;
                    // Update
                    {
                        imageCache.removeCallBackAndDestroy(typeof(ImageCacheUpdate));
                    }
                    return;
                }
                if (data is ImageSet)
                {
                    ImageSet imageSet = data as ImageSet;
                    // Update
                    {
                        imageSet.removeCallBackAndDestroy(typeof(ImageSetUpdate));
                    }
                    return;
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
            if (wrapProperty.p is ImageLoader)
            {
                switch ((ImageLoader.Property)wrapProperty.n)
                {
                    case ImageLoader.Property.imageCaches:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case ImageLoader.Property.imageSets:
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
            {
                if (wrapProperty.p is ImageCache)
                {
                    return;
                }
                if (wrapProperty.p is ImageSet)
                {
                    return;
                }
            }
            Logger.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

    }
}