using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AdvancedCoroutines;
using UnityEngine.Networking;

namespace Images.Cache
{
    public class GetUpdate : UpdateBehavior<Get>
    {

        #region Update

        public override void update()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    startRoutine(ref this.getRountine, TaskGetUrlTextures());
                }
                else
                {
                    Logger.LogError("data null");
                }
            }
        }

        public override bool isShouldDisableUpdate()
        {
            return false;
        }

        #endregion

        public static Texture2D ToTexture2D(Texture texture)
        {
            return Texture2D.CreateExternalTexture(
                texture.width,
                texture.height,
                TextureFormat.RGB24,
                false, false,
                texture.GetNativeTexturePtr());
        }

        #region Task

        private Routine getRountine;

        public override List<Routine> getRoutineList()
        {
            List<Routine> ret = new List<Routine>();
            {
                ret.Add(getRountine);
            }
            return ret;
        }

        /*IEnumerator GetImageFromUrl(string linkIcon, CrossAds adsItem)
        {

            Texture2D tex = new Texture2D(0, 0, TextureFormat.ARGB32, false);
            string url = BaseLinkIcon + linkIcon;

            WWW www = new WWW(url);
            do
            {
                yield return www;
            } while (www.error != null);

            www.LoadImageIntoTexture(tex);
            Sprite mySprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100);
            adsItem.icon = mySprite;
            crossAds.Add(adsItem);

        }*/

        public IEnumerator TaskGetUrlTextures()
        {
            if (this.data != null)
            {
                ImageCache imageCache = this.data.findDataInParent<ImageCache>();
                if (imageCache != null)
                {
                    bool haveNetworkConnection = true;
                    {
                        int count = 0;
                        while (Application.internetReachability == NetworkReachability.NotReachable)
                        {
                            count++;
                            if (count >= 60)
                            {
                                haveNetworkConnection = false;
                                break;
                            }
                            yield return new Wait(1);
                        }
                    }
                    if (haveNetworkConnection)
                    {
                        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageCache.url.v);
                        yield return www.SendWebRequest();

                        if (www.isNetworkError || www.isHttpError)
                        {
                            Logger.Log(www.error);
                            // change to fail
                            Fail fail = new Fail();
                            {
                                fail.uid = imageCache.state.makeId();
                                // TODO Can them action
                            }
                            imageCache.state.v = fail;
                        }
                        else
                        {
                            // change to success
                            Success success = new Success();
                            {
                                success.uid = imageCache.state.makeId();
                                success.texture.v = ((DownloadHandlerTexture)www.downloadHandler).texture;
                            }
                            imageCache.state.v = success;
                        }
                    }
                    else
                    {
                        Logger.LogError("Don't have network connection");
                        Fail fail = new Fail();
                        {
                            fail.uid = imageCache.state.makeId();
                            // TODO Can them action
                        }
                        imageCache.state.v = fail;
                    }
                }
                else
                {
                    Logger.LogError("imageCache null");
                }
            }
            else
            {
                Logger.LogError("data null: " + this);
            }
        }

        #endregion

        #region implement callBacks

        public override void onAddCallBack<T>(T data)
        {
            if(data is Get)
            {
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if(data is Get)
            {
                Get get = data as Get;
                this.setDataNull(get);
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
            if(wrapProperty.p is Get)
            {
                switch ((Get.Property)wrapProperty.n)
                {
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