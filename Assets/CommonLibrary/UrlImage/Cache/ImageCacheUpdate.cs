using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Images.Cache;

namespace Images
{
    public class ImageCacheUpdate : UpdateBehavior<ImageCache>
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

        #region implement callBacks

        public override void onAddCallBack<T>(T data)
        {
            if(data is ImageCache)
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
                ImageCache.State state = data as ImageCache.State;
                // Update
                {
                    switch (state.getType())
                    {
                        case ImageCache.State.Type.Get:
                            {
                                Get get = state as Get;
                                UpdateUtils.makeUpdate<GetUpdate, Get>(get, this.transform);
                            }
                            break;
                        case ImageCache.State.Type.Success:
                            {
                                Success success = state as Success;
                                UpdateUtils.makeUpdate<SuccessUpdate, Success>(success, this.transform);
                            }
                            break;
                        case ImageCache.State.Type.Fail:
                            {
                                Fail fail = state as Fail;
                                UpdateUtils.makeUpdate<FailUpdate, Fail>(fail, this.transform);
                            }
                            break;
                        default:
                            Logger.LogError("unknown type: " + state.getType());
                            break;
                    }
                }
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if (data is ImageCache)
            {
                ImageCache imageCache = data as ImageCache;
                // Child
                {
                    imageCache.state.allRemoveCallBack(this);
                }
                this.setDataNull(imageCache);
                return;
            }
            // Child
            if (data is ImageCache.State)
            {
                ImageCache.State state = data as ImageCache.State;
                // Update
                {
                    switch (state.getType())
                    {
                        case ImageCache.State.Type.Get:
                            {
                                Get get = state as Get;
                                get.removeCallBackAndDestroy(typeof(GetUpdate));
                            }
                            break;
                        case ImageCache.State.Type.Success:
                            {
                                Success success = state as Success;
                                success.removeCallBackAndDestroy(typeof(SuccessUpdate));
                            }
                            break;
                        case ImageCache.State.Type.Fail:
                            {
                                Fail fail = state as Fail;
                                fail.removeCallBackAndDestroy(typeof(FailUpdate));
                            }
                            break;
                        default:
                            Logger.LogError("unknown type: " + state.getType());
                            break;
                    }
                }
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
            if (wrapProperty.p is ImageCache)
            {
                switch ((ImageCache.Property)wrapProperty.n)
                {
                    case ImageCache.Property.url:
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
                return;
            }
            Logger.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

    }
}