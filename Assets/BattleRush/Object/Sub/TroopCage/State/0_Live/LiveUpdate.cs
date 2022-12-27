using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ObjectS.TroopCageS
{
    public class LiveUpdate : UpdateBehavior<Live>
    {

        #region Update

        public override void update()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    // change to release troop
                    if(this.data.hitpoint.v==0 && this.data.projectiles.vs.Count == 0)
                    {
                        TroopCage troopCage = this.data.findDataInParent<TroopCage>();
                        if (troopCage != null)
                        {
                            ReleaseTroop releaseTroop = troopCage.state.newOrOld<ReleaseTroop>();
                            {

                            }
                            troopCage.state.v = releaseTroop;
                        }
                        else
                        {
                            Logger.LogError("troopCage null");
                        }
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
            if(data is Live)
            {
                Live live = data as Live;
                // Child
                {
                    live.projectiles.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            if(data is TroopCageShootProjectile)
            {
                TroopCageShootProjectile troopCageShootProjectile = data as TroopCageShootProjectile;
                // Update
                {
                    UpdateUtils.makeUpdate<TroopCageShootProjectileUpdate, TroopCageShootProjectile>(troopCageShootProjectile, this.transform);
                }
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if(data is Live)
            {
                Live live = data as Live;
                // Child
                {
                    live.projectiles.allRemoveCallBack(this);
                }
                this.setDataNull(live);
                return;
            }
            // Child
            if (data is TroopCageShootProjectile)
            {
                TroopCageShootProjectile troopCageShootProjectile = data as TroopCageShootProjectile;
                // Update
                {
                    troopCageShootProjectile.removeCallBackAndDestroy(typeof(TroopCageShootProjectileUpdate));
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
            if(wrapProperty.p is Live)
            {
                switch ((Live.Property)wrapProperty.n)
                {
                    case Live.Property.hitpoint:
                        dirty = true;
                        break;
                    case Live.Property.projectiles:
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
            if (wrapProperty.p is TroopCageShootProjectile)
            {
                switch ((TroopCageShootProjectile.Property)wrapProperty.n)
                {
                    case TroopCageShootProjectile.Property.startPosition:
                        break;
                    case TroopCageShootProjectile.Property.state:
                        dirty = true;
                        break;
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
