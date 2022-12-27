using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ObjectS.TroopCageS
{
    public class TroopCageShootProjectileUpdate : UpdateBehavior<TroopCageShootProjectile>
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

        public override void Update()
        {
            base.Update();
            if (this.data != null)
            {
                switch (this.data.state.v.getType())
                {
                    case TroopCageShootProjectile.State.Type.Move:
                        {
                            TroopCageShootProjectile.State.Move move = this.data.state.v as TroopCageShootProjectile.State.Move;                            
                            if (move.time.v < move.duration.v)
                            {
                                move.time.v += Time.deltaTime;
                            }
                            else
                            {
                                // change to hit
                                TroopCageShootProjectile.State.Hit hit = this.data.state.newOrOld<TroopCageShootProjectile.State.Hit>();
                                {

                                }
                                this.data.state.v = hit;
                            }
                        }
                        break;
                    case TroopCageShootProjectile.State.Type.Hit:
                        {
                            // reduce hit point
                            {
                                Live live = this.data.findDataInParent<Live>();
                                if (live != null)
                                {
                                    live.hitpoint.v--;
                                }
                                else
                                {
                                    Logger.LogError("live null");
                                }
                            }
                            // finish
                            {
                                TroopCageShootProjectile.State.Finish finish = this.data.state.newOrOld<TroopCageShootProjectile.State.Finish>();
                                {

                                }
                                this.data.state.v = finish;
                            }
                        }
                        break;
                    case TroopCageShootProjectile.State.Type.Finish:
                        {
                            Live live = this.data.findDataInParent<Live>();
                            if (live != null)
                            {
                                live.projectiles.remove(this.data);
                            }
                            else
                            {
                                Logger.LogError("live null");
                            }
                        }
                        break;
                    default:
                        Logger.LogError("unknown state: " + this.data.state.v.getType());
                        break;
                }                
            }
            else
            {
                Logger.LogError("data null");
            }
        }

        public override bool isShouldDisableUpdate()
        {
            return false;
        }

        #endregion

        #region implement callBacks

        public override void onAddCallBack<T>(T data)
        {
            if(data is TroopCageShootProjectile)
            {
                TroopCageShootProjectile troopCageShootProjectile = data as TroopCageShootProjectile;
                // Child
                {
                    troopCageShootProjectile.state.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            if(data is TroopCageShootProjectile.State)
            {
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if (data is TroopCageShootProjectile)
            {
                TroopCageShootProjectile troopCageShootProjectile = data as TroopCageShootProjectile;
                // Child
                {
                    troopCageShootProjectile.state.allRemoveCallBack(this);
                }
                this.setDataNull(troopCageShootProjectile);
                return;
            }
            // Child
            if (data is TroopCageShootProjectile.State)
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
            if (wrapProperty.p is TroopCageShootProjectile)
            {
                switch ((TroopCageShootProjectile.Property)wrapProperty.n)
                {
                    case TroopCageShootProjectile.Property.state:
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
            if(wrapProperty.p is TroopCageShootProjectile.State)
            {
                TroopCageShootProjectile.State state = wrapProperty.p as TroopCageShootProjectile.State;
                switch (state.getType())
                {
                    case TroopCageShootProjectile.State.Type.Move:
                        {
                            switch ((TroopCageShootProjectile.State.Move.Property)wrapProperty.n)
                            {
                                case TroopCageShootProjectile.State.Move.Property.time:
                                    dirty = true;
                                    break;
                                case TroopCageShootProjectile.State.Move.Property.duration:
                                    dirty = true;
                                    break;
                                default:                                  
                                    break;
                            }
                        }
                        break;
                    case TroopCageShootProjectile.State.Type.Hit:
                        break;
                    case TroopCageShootProjectile.State.Type.Finish:
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