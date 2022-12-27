using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalSizeCheckChange : Data, ValueChangeCallBack
{

    public VO<int> change;

    private void notifyChange()
    {
        this.change.v = this.change.v + 1;
    }

    #region Constructor

    public enum Property
    {
        change
    }

    public GlobalSizeCheckChange() : base()
    {
        this.change = new VO<int>(this, (byte)Property.change, 0);
    }

    #endregion

    public void setData()
    {
        Global.get().addCallBack(this);
    }

    public void removeData()
    {
        Global.get().removeCallBack(this);
    }

    #region implement callBacks

    public void onAddCallBack<T>(T data) where T : Data
    {
        if (data is Global)
        {
            this.notifyChange();
            return;
        }
        Logger.LogError("onAddCallBack: " + data + "; " + this);
    }

    public void onRemoveCallBack<T>(T data, bool isHide) where T : Data
    {
        if (data is Global)
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
        if (wrapProperty.p is Global)
        {
            switch ((Global.Property)wrapProperty.n)
            {
                /*case Global.Property.networkReachability:
                    break;*/
                case Global.Property.deviceOrientation:
                    this.notifyChange();
                    break;
                case Global.Property.screenOrientation:
                    this.notifyChange();
                    break;
                case Global.Property.width:
                    this.notifyChange();
                    break;
                case Global.Property.height:
                    this.notifyChange();
                    break;
                case Global.Property.screenWidth:
                    this.notifyChange();
                    break;
                case Global.Property.screenHeight:
                    this.notifyChange();
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