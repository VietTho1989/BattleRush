using System.Collections;
using System.Collections.Generic;
using BattleRushS.HeroS.RunS;
using UnityEngine;

namespace BattleRushS.HeroS
{
    public class HeroMoveRunUpdate : UpdateBehavior<HeroMoveRun>
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
            if(data is HeroMoveRun)
            {
                HeroMoveRun heroMoveRun = data as HeroMoveRun;
                // Child
                {
                    heroMoveRun.heroRunShoot.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            if(data is HeroRunShoot)
            {
                HeroRunShoot heroRunShoot = data as HeroRunShoot;
                // Update
                {
                    UpdateUtils.makeUpdate<HeroRunShootUpdate, HeroRunShoot>(heroRunShoot, this.transform);
                }
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if(data is HeroMoveRun)
            {
                HeroMoveRun heroMoveRun = data as HeroMoveRun;
                // Child
                {
                    heroMoveRun.heroRunShoot.allRemoveCallBack(this);
                }
                this.setDataNull(heroMoveRun);
                return;
            }
            // Child
            if (data is HeroRunShoot)
            {
                HeroRunShoot heroRunShoot = data as HeroRunShoot;
                // Update
                {
                    heroRunShoot.removeCallBackAndDestroy(typeof(HeroRunShootUpdate));
                }
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
            if(wrapProperty.p is HeroMoveRun)
            {
                switch ((HeroMoveRun.Property)wrapProperty.n)
                {
                    case HeroMoveRun.Property.heroRunShoot:
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
            if (wrapProperty.p is HeroRunShoot)
            {
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + syncs + "; " + this);
        }

        #endregion

    }
}