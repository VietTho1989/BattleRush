using System.Collections;
using System.Collections.Generic;
using BattleRushS.ArenaS.ProjectileS;
using UnityEngine;

namespace BattleRushS.ArenaS
{
    public class ProjectileUpdate : UpdateBehavior<Projectile>
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
            if(data is Projectile)
            {
                Projectile projectile = data as Projectile;
                // Child
                {
                    projectile.state.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            if(data is Projectile.State)
            {
                Projectile.State state = data as Projectile.State;
                // Update
                {
                    switch (state.getType())
                    {
                        case Projectile.State.Type.Move:
                            {
                                Move move = state as Move;
                                UpdateUtils.makeUpdate<MoveUpdate, Move>(move, this.transform);
                            }
                            break;
                        case Projectile.State.Type.DealDamage:
                            {
                                DealDamage dealDamage = state as DealDamage;
                                UpdateUtils.makeUpdate<DealDamageUpdate, DealDamage>(dealDamage, this.transform);
                            }
                            break;
                        default:
                            Logger.LogError("unknown type: " + state.getType());
                            break;
                    }
                }
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if(data is Projectile)
            {
                Projectile projectile = data as Projectile;
                // Child
                {
                    projectile.state.allRemoveCallBack(this);
                }
                this.setDataNull(projectile);
                return;
            }
            // Child
            if (data is Projectile.State)
            {
                Projectile.State state = data as Projectile.State;
                // Update
                {
                    switch (state.getType())
                    {
                        case Projectile.State.Type.Move:
                            {
                                Move move = state as Move;
                                move.removeCallBackAndDestroy(typeof(MoveUpdate));
                            }
                            break;
                        case Projectile.State.Type.DealDamage:
                            {
                                DealDamage dealDamage = state as DealDamage;
                                dealDamage.removeCallBackAndDestroy(typeof(DealDamageUpdate));
                            }
                            break;
                        default:
                            Logger.LogError("unknown type: " + state.getType());
                            break;
                    }
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
            if(wrapProperty.p is Projectile)
            {
                switch ((Projectile.Property)wrapProperty.n)
                {
                    case Projectile.Property.state:
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
            if (data is Projectile)
            {
                Projectile projectile = data as Projectile;
                // Child
                {
                    projectile.state.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            if (wrapProperty.p is Projectile.State)
            {
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + syncs + "; " + this);
        }

        #endregion

    }
}