using System.Collections;
using System.Collections.Generic;
using BattleRushS.ArenaS.ProjectileS;
using UnityEngine;

namespace BattleRushS.ArenaS.TroopS.TroopAttackS
{
    public class AnimUpdate : UpdateBehavior<Anim>
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

        public void FixedUpdate()
        {
            if (this.data != null)
            {
                if (this.data.time.v < this.data.duration.v)
                {
                    this.data.time.v += Time.fixedDeltaTime;
                }
                else
                {
                    // attack: make projectile
                    {
                        Arena arena = this.data.findDataInParent<Arena>();
                        if (arena != null)
                        {
                            Projectile projectile = new Projectile();
                            {
                                projectile.uid = arena.projectiles.makeId();
                                // originId, move
                                {
                                    Troop troop = this.data.findDataInParent<Troop>();
                                    if (troop != null)
                                    {
                                        projectile.originId.v = troop.uid;
                                        // move
                                        {
                                            TroopUI troopUI = troop.findCallBack<TroopUI>();
                                            if (troopUI != null)
                                            {
                                                Move move = projectile.state.newOrOld<Move>();
                                                {
                                                    move.position.v = troopUI.transform.position;
                                                }
                                                projectile.state.v = move;
                                            }
                                            else
                                            {
                                                Logger.LogError("troopUI null");
                                            }                                            
                                        }
                                    }
                                    else
                                    {
                                        Logger.LogError("troop null");
                                    }
                                }
                                projectile.targetId.v = this.data.target.v;
                            }
                            arena.projectiles.add(projectile);
                        }
                        else
                        {
                            Logger.LogError("arena null");
                        }
                    }
                    // change to normal
                    {
                        TroopAttack troopAttack = this.data.findDataInParent<TroopAttack>();
                        if (troopAttack != null)
                        {
                            Normal normal = troopAttack.state.newOrOld<Normal>();
                            {
                                normal.coolDown.v = Normal.CoolDown;
                            }
                            troopAttack.state.v = normal;
                        }
                        else
                        {
                            Logger.LogError("troopAttack null");
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
            if(data is Anim)
            {
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if(data is Anim)
            {
                Anim animation = data as Anim;
                this.setDataNull(animation);
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
            if(wrapProperty.p is Anim)
            {

                return;
            }
            Logger.LogError("Don't process: " + data + "; " + syncs + "; " + this);
        }

        #endregion

    }
}