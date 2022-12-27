using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ArenaS
{
    public class TroopUI : UIBehavior<TroopUI.UIData>
    {

        #region UIData

        public class UIData : Data
        {

            public VO<ReferenceData<Troop>> troop;

            #region Constructor

            public enum Property
            {
                troop
            }

            public UIData() : base()
            {
                this.troop = new VO<ReferenceData<Troop>>(this, (byte)Property.troop, new ReferenceData<Troop>(null));
            }

            #endregion

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
                    Troop troop = this.data.troop.v.data;
                    if (troop != null)
                    {

                    }
                    else
                    {
                        Logger.LogError("troop null");
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
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onUpdateSync<T>(WrapProperty wrapProperty, List<Sync<T>> syncs)
        {
            if (WrapProperty.checkError(wrapProperty))
            {
                return;
            }
            Logger.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

    }
}