using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class DeviceOrientationCheckUI : MonoBehaviour, ValueChangeCallBack
{

    private bool Dirty = true;

    public bool dirty
    {
        get
        {
            return Dirty;
        }
        set
        {
            // Debug.LogError ("setDirty: " + value + "; " + this);
            Dirty = value;
            this.enabled = Dirty;
        }
    }

    void Awake()
    {
        Global.get().addCallBack(this);
    }

    void OnDestroy()
    {
        Global.get().removeCallBack(this);
    }

    #region behavior

    void FixedUpdate()
    {

        refresh();
    }

    void LateUpdate()
    {
        refresh();
    }

    void OnGUI()
    {
        // updateTransformData();
        refresh();
    }

    void OnPreCull()
    {
        refresh();
    }

    #endregion

    #region Refresh

    public CanvasScaler canvasScaler;

    public void refresh()
    {
        if (dirty)
        {
            dirty = false;
            // Debug.LogError("deviceOrientation: " + Global.get().deviceOrientation.v 
            //    + ", " + Global.get().width.v + ", " + Global.get().height.v);
            /*if (canvasScaler != null)
            {
                bool isPortrait = true;
                {
                    if (Global.get().height.v < Global.get().width.v)
                    {
                        isPortrait = false;
                    }
                }
                // get scale mode
                canvasScaler.matchWidthOrHeight = isPortrait ? 0 : 1;
                // TODO can set width, height theo rotation
            }
            else
            {
                Debug.LogError("canvasScaler null");
            }*/
        }
    }

    #endregion

    #region implement callBacks

    public void onAddCallBack<T>(T data) where T : Data
    {
        if(data is Global)
        {
            dirty = true;
            return;
        }
        Logger.LogError("onAddCallBack: " + data + "; " + this);
    }

    public void onRemoveCallBack<T>(T data, bool isHide) where T : Data
    {
        if(data is Global)
        {
            return;
        }
        Logger.LogError("onRemoveCallBack: " + data + "; " + this);
    }

    public void onUpdateSync<T>(WrapProperty wrapProperty, List<Sync<T>> syncs)
    {
        if (WrapProperty.checkError(wrapProperty))
        {
            return;
        }
        if(wrapProperty.p is Global)
        {
            switch ((Global.Property)wrapProperty.n)
            {
                /*case Global.Property.networkReachability:
                    break;*/
                case Global.Property.deviceOrientation:
                    dirty = true;
                    break;
                case Global.Property.screenOrientation:
                    dirty = true;
                    break;
                case Global.Property.width:
                    dirty = true;
                    break;
                case Global.Property.height:
                    dirty = true;
                    break;
                case Global.Property.screenWidth:
                    dirty = true;
                    break;
                case Global.Property.screenHeight:
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