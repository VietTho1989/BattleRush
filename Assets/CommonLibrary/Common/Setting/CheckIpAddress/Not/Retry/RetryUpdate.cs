using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CheckIpAddressS.NotS
{
    public class RetryUpdate : UpdateBehavior<Retry>
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

        private float time = 0;
        private float WaitTime = Logger.IsEditor() ? 10.0f : 40.0f;

        public override void Update()
        {
            if (this.data != null)
            {
                // Debug.Log("internetReachability: " + Application.internetReachability + ", " + time);
                if (time >= WaitTime)
                {
                    if(Application.internetReachability != NetworkReachability.NotReachable)
                    {
                        Not not = this.data.findDataInParent<Not>();
                        if (not != null)
                        {
                            Request request = not.sub.newOrOld<Request>();
                            {

                            }
                            not.sub.v = request;
                        }
                        else
                        {
                            Logger.LogError("not null");
                        }
                    }
                    else
                    {
                        Logger.Log("Don't have network connection");
                    }
                }
                else
                {
                    time += Time.deltaTime;
                }
            }
            else
            {
                Logger.LogError("data null");
            }
        }

        public override bool isShouldDisableUpdate()
        {
            return false;
        }

        #endregion

        #region implement callBacks

        public override void onAddCallBack<T>(T data)
        {
            if (data is Retry)
            {
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if (data is Retry)
            {
                Retry retry = data as Retry;
                this.setDataNull(retry);
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
            if (wrapProperty.p is Retry)
            {
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + syncs + "; " + this);
        }

        #endregion

    }
}