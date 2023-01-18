using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ArenaS.StateS
{
    public class AutoFightUI : UIBehavior<AutoFightUI.UIData>
    {

        #region UIData

        public class UIData : ArenaUI.UIData.StageUI
        {

            public VO<ReferenceData<AutoFight>> autoFight;

            #region Constructor

            public enum Property
            {
                autoFight
            }

            public UIData() : base()
            {
                this.autoFight = new VO<ReferenceData<AutoFight>>(this, (byte)Property.autoFight, new ReferenceData<AutoFight>(null));
            }

            #endregion

            public override Arena.Stage.Type getType()
            {
                return Arena.Stage.Type.AutoFight;
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
            if (data is UIData)
            {
                UIData uiData = data as UIData;
                // Child
                {
                    uiData.autoFight.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            if (data is AutoFight)
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
                    uiData.autoFight.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // Child
            if (data is AutoFight)
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
                    case UIData.Property.autoFight:
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
            if (wrapProperty.p is AutoFight)
            {
                switch ((AutoFight.Property)wrapProperty.n)
                {
                    default:
                        break;
                }
                return;
            }
            Logger.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

    }
}