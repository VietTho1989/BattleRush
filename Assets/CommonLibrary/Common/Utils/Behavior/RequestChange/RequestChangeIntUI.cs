using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RequestChangeIntUI : UIBehavior<RequestChangeIntUI.UIData>
{

    #region UIData

    public class UIData : RequestChange.UIData<int>
    {

        public VD<RequestChangeIntUpdate.UpdateData> updateData;

        public VD<IntLimit> limit;

        #region compare

        public VO<bool> showDifferent;

        public VO<int> compare;

        #endregion

        #region Constructor

        public enum Property
        {
            updateData,
            limit,
            // comare
            showDifferent,
            compare
        }

        public UIData() : base()
        {
            this.updateData = new VD<RequestChangeIntUpdate.UpdateData>(this, (byte)Property.updateData, new RequestChangeIntUpdate.UpdateData());
            this.limit = new VD<IntLimit>(this, (byte)Property.limit, new IntLimit.No());
            // compare
            {
                this.showDifferent = new VO<bool>(this, (byte)Property.showDifferent, false);
                this.compare = new VO<int>(this, (byte)Property.compare, 0);
            }
        }

        #endregion

        #region implement base

        public override RequestChangeUpdate<int>.UpdateData getUpdate()
        {
            return this.updateData.v;
        }

        public override void setShowDifferent(bool showDifferent)
        {
            this.showDifferent.v = showDifferent;
        }

        public override void setCompare(int compare)
        {
            this.compare.v = compare;
        }

        #endregion

    }

    #endregion

    #region Refresh

    public InputField edtValue;
    public Slider sliderValue;

    public override void Awake()
    {
        base.Awake();
        if (edtValue != null)
        {
            edtValue.onEndEdit.AddListener(delegate
            {
                if (edtValue.gameObject.activeInHierarchy)
                {
                    if (this.data != null)
                    {
                        int newValue = this.data.updateData.v.current.v;
                        if (System.Int32.TryParse(edtValue.text, out newValue))
                        {
                            this.data.updateData.v.current.v = newValue;
                        }
                    }
                    else
                    {
                        Logger.LogError("data null: " + this);
                    }
                }
                else
                {
                    Logger.LogError("edtValue not active: " + this);
                }
            });
        }
        else
        {
            Logger.LogError("edtValue null: " + this);
        }
        if (sliderValue != null)
        {
            sliderValue.onValueChanged.AddListener(delegate
            {
                // Debug.LogError ("sliderValue: onValueChanged: " + sliderValue.value);
                if (sliderValue.gameObject.activeInHierarchy)
                {
                    if (this.data != null)
                    {
                        this.data.updateData.v.current.v = (int)sliderValue.value;
                    }
                    else
                    {
                        Logger.LogError("data null: " + this);
                    }
                }
                else
                {
                    // Debug.LogError("slider not active: "+this);
                }
            });
        }
        else
        {
            Logger.LogError("sliderValue null: " + this);
        }
    }

    public static readonly Color DifferentColor = Color.red;
    public static readonly Color NormalColor = new Color(50 / 255f, 50 / 255f, 50 / 255f);
    public static readonly Color RequestColor = new Color(52 / 255f, 152 / 255f, 13 / 255f);
    public Text tvValue;

    public Text tvPlaceHolder;

    public Image imgBackground;

    public override void refresh()
    {
        if (dirty)
        {
            dirty = false;
            if (this.data != null)
            {
                // slider
                {
                    if (sliderValue != null)
                    {
                        IntLimit intLimit = this.data.limit.v;
                        if (intLimit != null)
                        {
                            switch (intLimit.getType())
                            {
                                case IntLimit.Type.No:
                                    {
                                        // slider
                                        sliderValue.gameObject.SetActive(false);
                                        // edtValue
                                        if (edtValue != null)
                                        {
                                            UIConstants.RequestChangeFullRect.set((RectTransform)edtValue.transform);
                                        }
                                        else
                                        {
                                            Logger.LogError("edtValue null");
                                        }
                                    }
                                    break;
                                case IntLimit.Type.Have:
                                    {
                                        // slider
                                        IntLimit.Have have = intLimit as IntLimit.Have;
                                        if (have.min.v < have.max.v)
                                        {
                                            sliderValue.gameObject.SetActive(true);
                                            sliderValue.minValue = have.min.v;
                                            sliderValue.maxValue = have.max.v;
                                        }
                                        else
                                        {
                                            Logger.LogError("why min >= max: " + have.min.v + "; " + have.max.v);
                                            sliderValue.gameObject.SetActive(false);
                                        }
                                        // edtValue
                                        if (edtValue != null)
                                        {
                                            if (sliderValue.gameObject.activeSelf)
                                            {
                                                UIConstants.RequestChangePartRect.set((RectTransform)edtValue.transform);
                                            }
                                            else
                                            {
                                                UIConstants.RequestChangeFullRect.set((RectTransform)edtValue.transform);
                                            }
                                        }
                                        else
                                        {
                                            Logger.LogError("edtValue null");
                                        }
                                    }
                                    break;
                                default:
                                    Logger.LogError("unknown type: " + intLimit.getType() + "; " + this);
                                    break;
                            }
                        }
                        else
                        {
                            Logger.LogError("limit null: " + this);
                        }
                    }
                    else
                    {
                        Logger.LogError("sliderValue null: " + this);
                    }
                }
                // Update UI
                {
                    // Process
                    if (this.data.updateData.v.canRequestChange.v)
                    {
                        // make interactable
                        if (edtValue != null)
                        {
                            edtValue.interactable = true;
                        }
                        else
                        {
                            Logger.LogError("edtValue null: " + this);
                        }
                        if (sliderValue != null)
                        {
                            sliderValue.interactable = true;
                        }
                        else
                        {
                            Logger.LogError("sliderValue null: " + this);
                        }
                        // background
                        if (imgBackground != null)
                        {
                            imgBackground.enabled = true;
                        }
                        else
                        {
                            Logger.LogError("imgBackground null");
                        }
                    }
                    else
                    {
                        // disable interactable
                        if (edtValue != null)
                        {
                            edtValue.interactable = false;
                        }
                        else
                        {
                            Logger.LogError("edtValue null: " + this);
                        }
                        if (sliderValue != null)
                        {
                            sliderValue.interactable = false;
                        }
                        else
                        {
                            Logger.LogError("sliderValue null: " + this);
                        }
                        // background
                        if (imgBackground != null)
                        {
                            imgBackground.enabled = false;
                        }
                        else
                        {
                            Logger.LogError("imgBackground null");
                        }
                    }
                    // set value
                    {
                        // edtValue
                        if (edtValue != null)
                        {
                            edtValue.text = "" + this.data.updateData.v.current.v;
                        }
                        else
                        {
                            Logger.LogError("edtValue null: " + this);
                        }
                        // sliderValue
                        if (sliderValue != null)
                        {
                            sliderValue.value = this.data.updateData.v.current.v;
                        }
                        else
                        {
                            Logger.LogError("sliderValue null: " + this);
                        }
                        // tvValue
                        if (tvValue != null)
                        {
                            // color
                            {
                                switch (this.data.updateData.v.changeState.v)
                                {
                                    case Data.ChangeState.None:
                                        {
                                            // check different
                                            bool isDifferent = false;
                                            {
                                                if (this.data.showDifferent.v)
                                                {
                                                    RequestChangeIntUpdate.UpdateData updateData = this.data.updateData.v;
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
                                                tvValue.color = DifferentColor;
                                            }
                                            else
                                            {
                                                tvValue.color = NormalColor;
                                            }
                                        }
                                        break;
                                    case Data.ChangeState.Request:
                                    case Data.ChangeState.Requesting:
                                        {
                                            tvValue.color = RequestColor;
                                        }
                                        break;
                                    default:
                                        Logger.LogError("unknown state: " + this.data.updateData.v.changeState.v + "; " + this);
                                        break;
                                }
                            }
                            // textSize
                            {
                                // tvValue.fontSize = Mathf.Clamp(Setting.get().contentTextSize.v, Setting.MinContentTextSize, Setting.MaxContentTextSize);
                            }
                        }
                        else
                        {
                            Logger.LogError("tvValue null");
                        }
                        // tvPlaceHolder
                        if (tvPlaceHolder != null)
                        {
                            // tvPlaceHolder.fontSize = Mathf.Clamp(Setting.get().contentTextSize.v, Setting.MinContentTextSize, Setting.MaxContentTextSize);
                        }
                        else
                        {
                            Logger.LogError("tvPlaceHolder null");
                        }
                    }
                }
            }
            else
            {
                // Debug.LogError ("data null: " + this);
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
            // Setting
            Settings.get().addCallBack(this);
            // Child
            {
                uiData.updateData.allAddCallBack(this);
                uiData.limit.allAddCallBack(this);
            }
            dirty = true;
            return;
        }
        // Setting
        if(data is Settings)
        {
            dirty = true;
            return;
        }
        // Child
        {
            if (data is RequestChangeIntUpdate.UpdateData)
            {
                RequestChangeIntUpdate.UpdateData updateData = data as RequestChangeIntUpdate.UpdateData;
                // Update
                {
                    UpdateUtils.makeUpdate<RequestChangeIntUpdate, RequestChangeIntUpdate.UpdateData>(updateData, this.transform);
                }
                dirty = true;
                return;
            }
            if (data is IntLimit)
            {
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
            // Setting
            Settings.get().removeCallBack(this);
            // Child
            {
                uiData.updateData.allRemoveCallBack(this);
                uiData.limit.allRemoveCallBack(this);
            }
            this.setDataNull(uiData);
            return;
        }
        // Setting
        if(data is Settings)
        {
            return;
        }
        // Child
        {
            if (data is RequestChangeIntUpdate.UpdateData)
            {
                RequestChangeIntUpdate.UpdateData updateData = data as RequestChangeIntUpdate.UpdateData;
                // Update
                {
                    updateData.removeCallBackAndDestroy(typeof(RequestChangeIntUpdate));
                }
                return;
            }
            if (data is IntLimit)
            {
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
                case UIData.Property.limit:
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
        // Setting
        if(wrapProperty.p is Settings)
        {
            switch ((Settings.Property)wrapProperty.n)
            {
                case Settings.Property.language:
                    break;
                /*case Setting.Property.style:
                    break;
                case Setting.Property.contentTextSize:
                    dirty = true;
                    break;
                case Setting.Property.titleTextSize:
                    dirty = true;
                    break;
                case Setting.Property.labelTextSize:
                    dirty = true;
                    break;
                case Setting.Property.buttonSize:
                    dirty = true;
                    break;
                case Setting.Property.itemSize:
                    break;
                case Setting.Property.confirmQuit:
                    break;
                case Setting.Property.showLastMove:
                    break;
                case Setting.Property.viewUrlImage:
                    break;
                case Setting.Property.animationSetting:
                    break;
                case Setting.Property.maxThinkCount:
                    break;
                case Setting.Property.defaultChosenGame:
                    break;
                case Setting.Property.defaultRoomName:
                    break;
                case Setting.Property.defaultChatRoomStyle:
                    break;*/
                default:
                    Logger.LogError("Don't process: " + wrapProperty + "; " + this);
                    break;
            }
            return;
        }
        // Child
        {
            if (wrapProperty.p is RequestChangeIntUpdate.UpdateData)
            {
                switch ((RequestChangeIntUpdate.UpdateData.Property)wrapProperty.n)
                {
                    case RequestChangeIntUpdate.UpdateData.Property.origin:
                        break;
                    case RequestChangeIntUpdate.UpdateData.Property.current:
                        dirty = true;
                        break;
                    case RequestChangeIntUpdate.UpdateData.Property.canRequestChange:
                        dirty = true;
                        break;
                    case RequestChangeIntUpdate.UpdateData.Property.changeState:
                        dirty = true;
                        break;
                    case RequestChangeIntUpdate.UpdateData.Property.serverState:
                        break;
                    case RequestChangeIntUpdate.UpdateData.Property.request:
                        break;
                    default:
                        Logger.LogError("Don't process: " + wrapProperty + "; " + this);
                        break;
                }
                return;
            }
            if (wrapProperty.p is IntLimit)
            {
                if (wrapProperty.p is IntLimit.Have)
                {
                    switch ((IntLimit.Have.Property)wrapProperty.n)
                    {
                        case IntLimit.Have.Property.min:
                            dirty = true;
                            break;
                        case IntLimit.Have.Property.max:
                            dirty = true;
                            break;
                        default:
                            Logger.LogError("Don't process: " + wrapProperty + "; " + this);
                            break;
                    }
                    return;
                }
                return;
            }
        }
        Logger.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
    }

    #endregion

}