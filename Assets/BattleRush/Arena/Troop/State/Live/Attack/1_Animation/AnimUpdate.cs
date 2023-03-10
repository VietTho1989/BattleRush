using System.Collections;
using System.Collections.Generic;
using BattleRushS.ArenaS.ProjectileS;
using BattleRushS.HeroS;
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
                    // attack
                    {
                        Troop troop = this.data.findDataInParent<Troop>();
                        if (troop != null)
                        {
                            // make damage
                            Damage damage = new Damage();
                            {
                                // damage
                                {
                                    float damageNumber = 14.0f;
                                    {
                                        if (troop.troopType.v != null)
                                        {
                                            switch (troop.troopType.v.getType())
                                            {
                                                case TroopType.Type.Hero:
                                                    {
                                                        damageNumber = 28.0f;
                                                    }
                                                    break;
                                                case TroopType.Type.Normal:
                                                    {
                                                        TroopInformation troopInformation = troop.troopType.v as TroopInformation;
                                                        // find
                                                        TroopInformation.Level level = troopInformation.levels.Find(check => check.level == troop.level.v);
                                                        // set
                                                        if (level != null)
                                                        {
                                                            damageNumber = level.attack;
                                                        }
                                                        else
                                                        {
                                                            Logger.LogError("level null");
                                                        }
                                                    }
                                                    break;
                                                case TroopType.Type.Monster:
                                                    break;
                                                default:
                                                    Logger.LogError("unknown type: "+troop.troopType.v);
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            Logger.LogError("troop type null");
                                        }
                                    }
                                    damage.damage.v = damageNumber;
                                }
                            }
                            // range attackk
                            if (troop.getAttackType()== Troop.AttackType.Range)
                            {
                                Arena arena = this.data.findDataInParent<Arena>();
                                if (arena != null)
                                {
                                    Projectile projectile = new Projectile();
                                    {
                                        projectile.uid = arena.projectiles.makeId();
                                        // originId, move
                                        {
                                            projectile.originId.v = troop.uid;
                                            // damage
                                            {
                                                damage.uid = projectile.damage.makeId();
                                                projectile.damage.v = damage;
                                            }
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
                                        projectile.targetId.v = this.data.target.v;
                                    }
                                    arena.projectiles.add(projectile);
                                }
                                else
                                {
                                    Logger.LogError("arena null");
                                }
                            }
                            // melee attack, make damage immediate
                            else
                            {
                                // find target troop
                                Troop targetTroop = null;
                                {
                                    Arena arena = this.data.findDataInParent<Arena>();
                                    if (arena != null)
                                    {
                                        targetTroop = arena.troops.vs.Find(check => check.uid == this.data.target.v);
                                    }
                                    else
                                    {
                                        Logger.LogError("arena null");
                                    }
                                }
                                // process
                                if (targetTroop != null)
                                {
                                    switch (targetTroop.state.v.getType())
                                    {
                                        case Troop.State.Type.Live:
                                            {
                                                Live live = targetTroop.state.v as Live;
                                                // target take damage
                                                {
                                                    damage.uid = live.takeDamage.v.damages.makeId();
                                                    live.takeDamage.v.damages.add(damage);
                                                }
                                            }
                                            break;
                                        case Troop.State.Type.Die:
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
                        }
                        else
                        {
                            Logger.LogError("troop null");
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