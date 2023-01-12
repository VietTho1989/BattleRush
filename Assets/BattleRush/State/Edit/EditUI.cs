using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.StateS
{
    public class EditUI : UIBehavior<EditUI.UIData>
    {

        #region UIData

        public class UIData : BattleRushUI.UIData.StateUI
        {

            public VO<ReferenceData<Edit>> edit;

            #region Constructor

            public enum Property
            {
                edit
            }

            public UIData() : base()
            {
                this.edit = new VO<ReferenceData<Edit>>(this, (byte)Property.edit, new ReferenceData<Edit>((null)));
            }

            #endregion

            public override BattleRush.State.Type getType()
            {
                return BattleRush.State.Type.Edit;
            }

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
                    Edit edit = this.data.edit.v.data;
                    if (edit != null)
                    {

                    }
                    else
                    {
                        Logger.LogError("edit null");
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
            if(data is UIData)
            {
                UIData uiData = data as UIData;
                // Child
                {
                    uiData.edit.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            if(data is Edit)
            {
                dirty = true;
                return;
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
                    uiData.edit.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // Child
            if (data is Edit)
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
            if (wrapProperty.p is UIData)
            {
                switch ((UIData.Property)wrapProperty.n)
                {
                    case UIData.Property.edit:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    default:
                        break;
                }
                return;
            }
            // Child
            if (wrapProperty.p is Edit)
            {
                switch ((Edit.Property)wrapProperty.n)
                {
                    default:
                        break;
                }
                return;
            }
            Logger.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

        public void onClickBtnReset()
        {
            if (this.data != null)
            {
                Edit edit = this.data.edit.v.data;
                if (edit != null)
                {
                    BattleRush battleRush = edit.findDataInParent<BattleRush>();
                    if (battleRush != null)
                    {
                        battleRush.reset();
                    }
                    else
                    {
                        Logger.LogError("battleRush null");
                    }
                }
                else
                {
                    Logger.LogError("edit null");
                }                
            }
            else
            {
                Logger.LogError("data null");
            }
        }

        public void onClickBtnPlay()
        {
            if (this.data != null)
            {
                Edit edit = this.data.edit.v.data;
                if (edit != null)
                {
                    BattleRush battleRush = edit.findDataInParent<BattleRush>();
                    if (battleRush != null)
                    {
                        Start start = battleRush.state.newOrOld<Start>();
                        {

                        }
                        battleRush.state.v = start;
                    }
                    else
                    {
                        Logger.LogError("battleRush null");
                    }
                }
                else
                {
                    Logger.LogError("edit null");
                }
            }
            else
            {
                Logger.LogError("data null");
            }
        }

    }
}