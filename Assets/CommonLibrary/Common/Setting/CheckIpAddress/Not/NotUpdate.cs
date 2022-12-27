using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CheckIpAddressS.NotS;

namespace CheckIpAddressS
{
    public class NotUpdate : UpdateBehavior<Not>
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
            if(data is Not)
            {
                Not not = data as Not;
                // Child
                {
                    not.sub.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            if(data is Not.Sub)
            {
                Not.Sub sub = data as Not.Sub;
                // Update
                {
                    switch (sub.getType())
                    {
                        case Not.Sub.Type.Request:
                            {
                                Request request = sub as Request;
                                UpdateUtils.makeUpdate<RequestUpdate, Request>(request, this.transform);
                            }
                            break;
                        case Not.Sub.Type.Retry:
                            {
                                Retry retry = sub as Retry;
                                UpdateUtils.makeUpdate<RetryUpdate, Retry>(retry, this.transform);
                            }
                            break;
                        default:
                            Logger.LogError("unknown type: " + sub.getType());
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
            if (data is Not)
            {
                Not not = data as Not;
                // Child
                {
                    not.sub.allRemoveCallBack(this);
                }
                this.setDataNull(not);
                return;
            }
            // Child
            if (data is Not.Sub)
            {
                Not.Sub sub = data as Not.Sub;
                // Update
                {
                    switch (sub.getType())
                    {
                        case Not.Sub.Type.Request:
                            {
                                Request request = sub as Request;
                                request.removeCallBackAndDestroy(typeof(RequestUpdate));
                            }
                            break;
                        case Not.Sub.Type.Retry:
                            {
                                Retry retry = sub as Retry;
                                retry.removeCallBackAndDestroy(typeof(RetryUpdate));
                            }
                            break;
                        default:
                            Logger.LogError("unknown type: " + sub.getType());
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
            if (wrapProperty.p is Not)
            {
                switch ((Not.Property)wrapProperty.n)
                {
                    case Not.Property.sub:
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
            if (wrapProperty.p is Not.Sub)
            {
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + syncs + "; " + this);
        }

        #endregion

    }
}