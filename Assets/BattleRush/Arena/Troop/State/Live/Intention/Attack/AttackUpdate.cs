using System.Collections;
using System.Collections.Generic;
using BattleRushS.ArenaS.TroopS.TroopAttackS;
using BattleRushS.ArenaS.TroopS.TroopMoveS;
using UnityEngine;
using UnityEngine.AI;

namespace BattleRushS.ArenaS.TroopS.IntentionS
{
    public class AttackUpdate : UpdateBehavior<Attack>
    {

        #region Update

        public override void update()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    Troop troop = this.data.findDataInParent<Troop>();
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
                    Logger.Log("data null");
                }
            }
        }

        public override bool isShouldDisableUpdate()
        {
            return false;
        }

        public override void Update()
        {
            base.Update();
            if (this.data != null)
            {
                // time
                this.data.time.v += Time.deltaTime;
                // move and attack
                {
                    Troop troop = this.data.findDataInParent<Troop>();
                    if (troop != null)
                    {
                        Arena arena = this.data.findDataInParent<Arena>();
                        if (arena != null)
                        {
                            // find target troop
                            Troop targetTroop = arena.troops.vs.Find(check => check.uid == this.data.targetId.v);
                            if (targetTroop != null)
                            {
                                TroopUI troopUI = troop.findCallBack<TroopUI>();
                                TroopUI targetTroopUI = targetTroop.findCallBack<TroopUI>();
                                if(troopUI && targetTroopUI != null)
                                {
                                    // check in range
                                    bool isInRange = false;
                                    {
                                        // find range
                                        float rangeToAttack = 10.0f;
                                        {
                                            if (troop.isRange())
                                            {
                                                rangeToAttack = 10.0f;
                                            }
                                            else
                                            {
                                                rangeToAttack = 1.0f;
                                            }
                                        }
                                        // process
                                        if(Vector3.Distance(troopUI.transform.position, targetTroopUI.transform.position) < rangeToAttack)
                                        {
                                            isInRange = true;
                                        }
                                    }
                                    // process
                                    if (isInRange)
                                    {
                                        // Logger.Log("in range to attack: " + troop.uid);
                                        // stop move
                                        {
                                            Live live = this.data.findDataInParent<Live>();
                                            if (live != null)
                                            {
                                                TroopMove troopMove = live.troopMove.v;
                                                if (troopMove != null)
                                                {
                                                    Idle idle = troopMove.sub.newOrOld<Idle>();
                                                    {

                                                    }
                                                    troopMove.sub.v = idle;
                                                }
                                                else
                                                {
                                                    Logger.LogError("troopMove null");
                                                }
                                            }
                                            else
                                            {
                                                Logger.LogError("live null");
                                            }
                                        }
                                        // make troop attack
                                        {
                                            Live live = this.data.findDataInParent<Live>();
                                            if (live != null)
                                            {
                                                switch (live.troopAttack.v.state.v.getType())
                                                {
                                                    case TroopAttack.State.Type.Normal:
                                                        {
                                                            Normal normal = live.troopAttack.v.state.v as Normal;
                                                            if (normal.coolDown.v <= 0)
                                                            {
                                                                Anim anim = live.troopAttack.v.state.newOrOld<Anim>();
                                                                {
                                                                    anim.target.v = targetTroop.uid;
                                                                }
                                                                live.troopAttack.v.state.v = anim;
                                                            }
                                                            else
                                                            {
                                                                // in cooldown, cannot attack anymore
                                                            }
                                                        }
                                                        break;
                                                    case TroopAttack.State.Type.Animation:
                                                        break;
                                                    default:
                                                        Logger.LogError("unknown type: " + live.troopAttack.v.state.v.getType());
                                                        break;
                                                }
                                            }
                                            else
                                            {
                                                Logger.LogError("live null");
                                            }                                            
                                        }                                       
                                    }
                                    else
                                    {
                                        Live live = this.data.findDataInParent<Live>();
                                        if (live != null)
                                        {
                                            TroopMove troopMove = live.troopMove.v;
                                            if (troopMove != null)
                                            {
                                                MoveToDest moveToDest = troopMove.sub.newOrOld<MoveToDest>();
                                                {
                                                    moveToDest.dest.v = targetTroopUI.transform.position;
                                                }
                                                troopMove.sub.v = moveToDest;
                                            }
                                            else
                                            {
                                                Logger.LogError("troopMove null");
                                            }
                                        }
                                        else
                                        {
                                            Logger.LogError("live null");
                                        }
                                    }
                                }
                                else
                                {
                                    Logger.LogError("troopUI null");
                                }                                
                            }
                            else
                            {
                                Logger.LogError("targetTroop null");
                            }
                        }
                        else
                        {
                            Logger.LogError("arena null");
                        }
                    }
                    else
                    {
                        Logger.LogError("troop null");
                    }                    
                }
            }
            else
            {
                Logger.LogError("data null");
            }
        }

        #endregion

        #region implement callBacks

        public override void onAddCallBack<T>(T data)
        {
            if(data is Attack)
            {
                Attack attack = data as Attack;
                // name
                {
                    Troop troop = attack.findDataInParent<Troop>();
                    if (troop != null)
                    {
                        this.name = "AttackUpdate: " + troop.uid + " attack " + attack.targetId.v;
                    }
                    else
                    {
                        Logger.LogError("troop null");
                    }                   
                }
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if(data is Attack)
            {
                Attack attack = data as Attack;
                this.setDataNull(attack);
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
            if(wrapProperty.p is Attack)
            {
                switch ((Attack.Property)wrapProperty.n)
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