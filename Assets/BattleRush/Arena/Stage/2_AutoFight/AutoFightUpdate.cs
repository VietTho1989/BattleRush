using System.Collections;
using System.Collections.Generic;
using BattleRushS.ArenaS.StateS.AutoFightS;
using UnityEngine;

namespace BattleRushS.ArenaS
{
    public class AutoFightUpdate : UpdateBehavior<AutoFight>
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
            if(data is AutoFight)
            {

                AutoFight autoFight = data as AutoFight;
                // Update
                {
                    UpdateUtils.makeUpdate<CheckFightEndUpdate, AutoFight>(autoFight, this.transform);
                }
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if(data is AutoFight)
            {
                AutoFight autoFight = data as AutoFight;
                // Update
                {
                    autoFight.removeCallBackAndDestroy(typeof(CheckFightEndUpdate));
                }
                this.setDataNull(autoFight);
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
            if(wrapProperty.p is AutoFight)
            {
                switch ((AutoFight.Property)wrapProperty.n)
                {
                    default:
                        break;
                }
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + syncs + "; " + this);
        }

        #endregion

    }
}