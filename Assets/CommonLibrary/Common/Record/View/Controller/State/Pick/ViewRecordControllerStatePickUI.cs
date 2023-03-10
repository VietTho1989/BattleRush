using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Record
{
    /*public class ViewRecordControllerStatePickUI : UIBehavior<ViewRecordControllerStatePickUI.UIData>
    {

        #region UIData

        public class UIData : ViewRecordControllerUI.UIData.StateUI
        {

            public VP<ReferenceData<ViewRecordControllerStatePick>> pick;

            #region Constructor

            public enum Property
            {
                pick
            }

            public UIData() : base()
            {
                this.pick = new VP<ReferenceData<ViewRecordControllerStatePick>>(this, (byte)Property.pick, new ReferenceData<ViewRecordControllerStatePick>(null));
            }

            #endregion

            public override ViewRecordControllerUI.UIData.State.Type getType()
            {
                return ViewRecordControllerUI.UIData.State.Type.Pick;
            }

        }

        #endregion

        #region txt

        public Text tvMessage;
        private static readonly TxtLanguage txtMessage = new TxtLanguage("Buffering");

        static ViewRecordControllerStatePickUI()
        {
            txtMessage.add(Language.Type.vi, "Buffering");
        }

        #endregion

        #region Refresh

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    ViewRecordControllerStatePick pick = this.data.pick.v.data;
                    if (pick != null)
                    {

                    }
                    else
                    {
                        Debug.LogError("pick null: " + this);
                    }
                    // txt
                    {
                        if (tvMessage != null)
                        {
                            tvMessage.text = txtMessage.get();
                        }
                        else
                        {
                            Debug.LogError("tvMessage null: " + this);
                        }
                    }
                }
                else
                {
                    Debug.LogError("data null: " + this);
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
                Setting.get().addCallBack(this);
                // Child
                {
                    uiData.pick.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Setting
            if (data is Setting)
            {
                dirty = true;
                return;
            }
            // Child
            if (data is ViewRecordControllerStatePick)
            {
                dirty = true;
                return;
            }
            Debug.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if (data is UIData)
            {
                UIData uiData = data as UIData;
                // Setting
                Setting.get().removeCallBack(this);
                // Child
                {
                    uiData.pick.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // Setting
            if (data is Setting)
            {
                return;
            }
            // Child
            if (data is ViewRecordControllerStatePick)
            {
                return;
            }
            Debug.LogError("Don't process: " + data + "; " + this);
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
                    case UIData.Property.pick:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    default:
                        Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                        break;
                }
                return;
            }
            // Setting
            if (wrapProperty.p is Setting)
            {
                switch ((Setting.Property)wrapProperty.n)
                {
                    case Setting.Property.language:
                        dirty = true;
                        break;
                    case Setting.Property.showLastMove:
                        break;
                    case Setting.Property.viewUrlImage:
                        break;
                    case Setting.Property.animationSetting:
                        break;
                    case Setting.Property.maxThinkCount:
                        break;
                    default:
                        Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                        break;
                }
                return;
            }
            // Child
            if (wrapProperty.p is ViewRecordControllerStatePick)
            {
                switch ((ViewRecordControllerStatePick.Property)wrapProperty.n)
                {
                    case ViewRecordControllerStatePick.Property.startTime:
                        break;
                    case ViewRecordControllerStatePick.Property.pickTime:
                        break;
                    case ViewRecordControllerStatePick.Property.playState:
                        break;
                    default:
                        Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                        break;
                }
                return;
            }
            Debug.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

    }*/
}