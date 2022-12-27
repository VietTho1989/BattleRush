using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Images
{
    public class MakeImageCacheUpdate : UpdateBehavior<ImageLoader>
    {

        #region Update

        public override void update()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    HashSet<string> urlHashSet = new HashSet<string>();
                    {
                        foreach (ImageSet imageSet in this.data.imageSets.vs)
                        {
                            urlHashSet.Add(imageSet.url.v);
                        }
                    }
                    // make image cache
                    {
                        foreach(string url in urlHashSet)
                        {
                            if (this.data.findImageCache(url) == null)
                            {
                                ImageCache imageCache = new ImageCache();
                                {
                                    imageCache.uid = this.data.imageCaches.makeId();
                                    imageCache.url.v = url;
                                }
                                this.data.imageCaches.add(imageCache);
                            }
                        }
                    }
                    // remove fail
                    for (int i = this.data.imageCaches.vs.Count - 1; i >= 0; i--)
                    {
                        ImageCache imageCache = this.data.imageCaches.vs[i];
                        if (!urlHashSet.Contains(imageCache.url.v))
                        {
                            if (imageCache.state.v != null && imageCache.state.v.getType() == ImageCache.State.Type.Fail)
                            {
                                Logger.LogError("remove imageCache: " + imageCache);
                                this.data.imageCaches.remove(imageCache);
                            }
                        }
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

        public override void onAddCallBack<T>(T data)
        {
            if(data is ImageLoader)
            {
                ImageLoader imageLoader = data as ImageLoader;
                // Child
                {
                    imageLoader.imageSets.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            if(data is ImageSet)
            {
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if (data is ImageLoader)
            {
                ImageLoader imageLoader = data as ImageLoader;
                // Child
                {
                    imageLoader.imageSets.allRemoveCallBack(this);
                }
                this.setDataNull(imageLoader);
                return;
            }
            // Child
            if (data is ImageSet)
            {
                return;
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
            if (wrapProperty.p is ImageSet)
            {
                switch ((ImageSet.Property)wrapProperty.n)
                {
                    case ImageSet.Property.image:
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
            Logger.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

    }
}