using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class RequestChangeBoolUI : UIBehavior<RequestChangeBoolUI.UIData>
{

    #region UIData

    public class UIData : RequestChange.UIData<bool>
    {

        public VD<RequestChangeBoolUpdate.UpdateData> updateData;

        #region compare

        public VO<bool> showDifferent;

        public VO<bool> compare;

        #endregion

        #region Constructor

        public enum Property
        {
            updateData,
            // comare
            showDifferent,
            compare
        }

        public UIData() : base()
        {
            this.updateData = new VD<RequestChangeBoolUpdate.UpdateData>(this, (byte)Property.updateData, new RequestChangeBoolUpdate.UpdateData());
            // compare
            {
                this.showDifferent = new VO<bool>(this, (byte)Property.showDifferent, false);
                this.compare = new VO<bool>(this, (byte)Property.compare, true);
            }
        }

        #endregion

        #region implement base

        public override RequestChangeUpdate<bool>.UpdateData getUpdate()
        {
            return this.updateData.v;
        }

        public override void setShowDifferent(bool showDifferent)
        {
            this.showDifferent.v = showDifferent;
        }

        public override void setCompare(bool compare)
        {
            this.compare.v = compare;
        }

        #endregion

        public bool processEvent(Event e)
        {
            bool isProcess = false;
            {
                // shortKey
                if (!isProcess)
                {
                    /*if (Setting.get().useShortKey.v)
                    {
                        RequestChangeBoolUI requestChangeBoolUI = this.findCallBack<RequestChangeBoolUI>();
                        if (requestChangeBoolUI != null)
                        {
                            isProcess = requestChangeBoolUI.useShortKey(e);
                        }
                        else
                        {
                            Debug.LogError("requestChangeBoolUI null: " + this);
                        }
                    }*/
                }
            }
            return isProcess;
        }

    }

    #endregion

    #region Refresh

    private int tempNotRefresh = 0;

    public Button btnValue;

    public Sprite blackCheckMark;
    public Sprite blueCheckMark;
    public Sprite redCheckMark;

    public Sprite redDot;
    public Sprite blueDot;
    public Image checkMark;

    public override void refresh()
    {
        if (dirty)
        {
            dirty = false;
            if (this.data != null)
            {
                if (tempNotRefresh > 0)
                {
                    tempNotRefresh--;
                    dirty = true;
                    return;
                }
                // interactable
                if (this.data.updateData.v.canRequestChange.v)
                {
                    // make interactable
                    if (btnValue != null)
                    {
                        btnValue.interactable = true;
                    }
                    else
                    {
                        Logger.LogError("btnValue null: " + this);
                    }
                }
                else
                {
                    // disable interactable
                    if (btnValue != null)
                    {
                        btnValue.interactable = false;
                    }
                    else
                    {
                        Logger.LogError("btnValue null: " + this);
                    }
                }
                // checkMark
                if (checkMark != null)
                {
                    bool isTransparent = false;
                    switch (this.data.updateData.v.changeState.v)
                    {
                        case Data.ChangeState.None:
                            {
                                // check different
                                bool isDifferent = false;
                                {
                                    if (this.data.showDifferent.v)
                                    {
                                        RequestChangeBoolUpdate.UpdateData updateData = this.data.updateData.v;
                                        if (updateData != null)
                                        {
                                            if (updateData.current.v != this.data.compare.v)
                                            {
                                                isDifferent = true;
                                            }
                                        }
                                        else
                                        {
                                            Logger.LogError("updateData null: " + this);
                                        }
                                    }
                                }
                                // Process
                                if (isDifferent)
                                {
                                    checkMark.sprite = this.data.updateData.v.current.v ? redCheckMark : redDot;
                                }
                                else
                                {
                                    if (this.data.updateData.v.current.v)
                                    {
                                        checkMark.sprite = blackCheckMark;
                                    }
                                    else
                                    {
                                        isTransparent = true;
                                    }
                                }
                            }
                            break;
                        case Data.ChangeState.Request:
                        case Data.ChangeState.Requesting:
                            {
                                checkMark.sprite = this.data.updateData.v.current.v ? blueCheckMark : blueDot;
                            }
                            break;
                        default:
                            Logger.LogError("unknown state: " + this.data.updateData.v.changeState.v + "; " + this);
                            break;
                    }
                    checkMark.color = isTransparent ? new Color(0, 0, 0, 0) : Color.white;
                }
                else
                {
                    Logger.LogError("checkMark null: " + this);
                }
            }
            else
            {
                // Debug.Log ("data null: " + this);
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
        if (data is UIData)
        {
            UIData uiData = data as UIData;
            // Child
            {
                uiData.updateData.allAddCallBack(this);
            }
            dirty = true;
            return;
        }
        // Child
        {
            if (data is RequestChangeBoolUpdate.UpdateData)
            {
                RequestChangeBoolUpdate.UpdateData updateData = data as RequestChangeBoolUpdate.UpdateData;
                // Update
                {
                    UpdateUtils.makeUpdate<RequestChangeBoolUpdate, RequestChangeBoolUpdate.UpdateData>(updateData, this.transform);
                }
                dirty = true;
                return;
            }
        }
        Logger.LogError("Don't process: " + data + "; " + this);
    }

    public override void onRemoveCallBack<T>(T data, bool isHide)
    {
        if (data is UIData)
        {
            UIData uiData = data as UIData;
            // Child
            {
                uiData.updateData.allRemoveCallBack(this);
            }
            this.setDataNull(uiData);
            return;
        }
        // Child
        {
            if (data is RequestChangeBoolUpdate.UpdateData)
            {
                RequestChangeBoolUpdate.UpdateData updateData = data as RequestChangeBoolUpdate.UpdateData;
                // Update
                {
                    updateData.removeCallBackAndDestroy(typeof(RequestChangeBoolUpdate));
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
        if (wrapProperty.p is UIData)
        {
            switch ((UIData.Property)wrapProperty.n)
            {
                case UIData.Property.updateData:
                    {
                        ValueChangeUtils.replaceCallBack(this, syncs);
                        dirty = true;
                    }
                    break;
                case UIData.Property.showDifferent:
                    dirty = true;
                    break;
                case UIData.Property.compare:
                    dirty = true;
                    break;
                default:
                    Logger.LogError("Don't process: " + wrapProperty + "; " + this);
                    break;
            }
            return;
        }
        // Child
        {
            if (wrapProperty.p is RequestChangeBoolUpdate.UpdateData)
            {
                switch ((RequestChangeBoolUpdate.UpdateData.Property)wrapProperty.n)
                {
                    case RequestChangeBoolUpdate.UpdateData.Property.origin:
                        break;
                    case RequestChangeBoolUpdate.UpdateData.Property.current:
                        dirty = true;
                        break;
                    case RequestChangeBoolUpdate.UpdateData.Property.canRequestChange:
                        dirty = true;
                        break;
                    case RequestChangeBoolUpdate.UpdateData.Property.changeState:
                        dirty = true;
                        break;
                    case RequestChangeBoolUpdate.UpdateData.Property.serverState:
                        break;
                    case RequestChangeBoolUpdate.UpdateData.Property.request:
                        break;
                    default:
                        Logger.LogError("Don't process: " + wrapProperty + "; " + this);
                        break;
                }
                return;
            }
        }
        Logger.LogError("Don't process:  " + wrapProperty + "; " + syncs + "; " + this);
    }

    #endregion

    public override void Awake()
    {
        base.Awake();
        // OnClick
        {
            UIUtils.SetButtonOnClick(btnValue, onClickBtnChange);
        }
    }

    public bool useShortKey(Event e)
    {
        bool isProcess = false;
        {
            if (e.isKey && e.type == EventType.KeyUp)
            {
                switch (e.keyCode)
                {
                    case KeyCode.KeypadEnter:
                        {
                            if (btnValue != null && btnValue.gameObject.activeInHierarchy && btnValue.interactable)
                            {
                                this.onClickBtnChange();
                                isProcess = true;
                            }
                            else
                            {
                                Logger.LogError("cannot click btnValue");
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        return isProcess;
    }

    [UnityEngine.Scripting.Preserve]
    public void onClickBtnChange()
    {
        // Debug.LogError ("onClickBtnChange: " + this);
        if (this.data != null)
        {
            this.data.updateData.v.current.v = !this.data.updateData.v.current.v;
            tempNotRefresh = 8;
        }
        else
        {
            Logger.LogError("data null: " + this);
        }
    }

}