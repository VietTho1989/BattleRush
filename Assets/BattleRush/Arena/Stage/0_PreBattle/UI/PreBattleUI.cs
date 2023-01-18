using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ArenaS.StateS
{
    public class PreBattleUI : UIBehavior<PreBattleUI.UIData>
    {

        #region UIData

        public class UIData : ArenaUI.UIData.StageUI
        {

            public VO<ReferenceData<PreBattle>> preBattle;

            #region Constructor

            public enum Property
            {
                preBattle
            }

            public UIData() : base()
            {
                this.preBattle = new VO<ReferenceData<PreBattle>>(this, (byte)Property.preBattle, new ReferenceData<PreBattle>(null));
            }

            #endregion

            public override Arena.Stage.Type getType()
            {
                return Arena.Stage.Type.PreBattle;
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
            if(data is UIData)
            {
                UIData uiData = data as UIData;
                // Child
                {
                    uiData.preBattle.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            if(data is PreBattle)
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
                    uiData.preBattle.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // Child
            if (data is PreBattle)
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
                    case UIData.Property.preBattle:
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
            if (wrapProperty.p is PreBattle)
            {
                switch ((PreBattle.Property)wrapProperty.n)
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