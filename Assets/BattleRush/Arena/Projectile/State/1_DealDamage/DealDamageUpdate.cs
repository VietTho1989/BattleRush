using System.Collections;
using System.Collections.Generic;
using BattleRushS.ArenaS.TroopS;
using UnityEngine;

namespace BattleRushS.ArenaS.ProjectileS
{
    public class DealDamageUpdate : UpdateBehavior<DealDamage>
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

        private void FixedUpdate()
        {
            if (this.data != null)
            {
                // first: make target take damage
                {
                    if (this.data.time.v == 0)
                    {
                        Arena arena = this.data.findDataInParent<Arena>();
                        if (arena != null)
                        {
                            Projectile projectile = this.data.findDataInParent<Projectile>();
                            if (projectile != null)
                            {
                                Troop targetTroop = arena.troops.vs.Find(check => check.uid == projectile.targetId.v);
                                if (targetTroop != null)
                                {
                                    switch (targetTroop.state.v.getType())
                                    {
                                        case Troop.State.Type.Live:
                                            {
                                                Live live = targetTroop.state.v as Live;
                                                // add damage
                                                {
                                                    Damage newDamage = DataUtils.cloneData(projectile.damage.v) as Damage;
                                                    {
                                                        newDamage.uid = live.takeDamage.v.damages.makeId();
                                                    }
                                                    live.takeDamage.v.damages.add(newDamage);
                                                }
                                            }
                                            break;
                                        case Troop.State.Type.Die:
                                            {
                                                // already die, not add damage anymore
                                            }
                                            break;
                                        default:
                                            Logger.LogError("unknown type: " + targetTroop.state.v.getType());
                                            break;
                                    }                                   
                                }
                                else
                                {
                                    Logger.LogError("targetTroop null");
                                }
                            }
                            else
                            {
                                Logger.LogError("projectile null");
                            }                            
                        }
                        else
                        {
                            Logger.LogError("arena null");
                        }
                    }
                }
                // time
                this.data.time.v += Time.fixedDeltaTime;
                // remove projectile
                {
                    if (this.data.time.v > this.data.duration.v)
                    {
                        Arena arena = this.data.findDataInParent<Arena>();
                        if (arena != null)
                        {
                            Projectile projectile = this.data.findDataInParent<Projectile>();
                            if (projectile != null)
                            {
                                arena.projectiles.remove(projectile);
                            }
                            else
                            {
                                Logger.LogError("projectile null");
                            }                           
                        }
                        else
                        {
                            Logger.LogError("arena null");
                        }
                    }
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
            if(data is DealDamage)
            {
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if (data is DealDamage)
            {
                DealDamage dealDamage = data as DealDamage;
                this.setDataNull(dealDamage);
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
            if(wrapProperty.p is DealDamage)
            {
                switch ((DealDamage.Property)wrapProperty.n)
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