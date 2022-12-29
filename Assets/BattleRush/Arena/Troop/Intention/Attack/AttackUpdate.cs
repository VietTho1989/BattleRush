using System.Collections;
using System.Collections.Generic;
using BattleRushS.ArenaS.TroopS.TroopAttackS;
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
                                            // TODO can hoan thien
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
                                        Logger.Log("in range to attack: " + troop.uid);
                                        // stop move
                                        {
                                            NavMeshAgent navMeshAgent = troopUI.GetComponent<NavMeshAgent>();
                                            if (navMeshAgent != null)
                                            {
                                                navMeshAgent.isStopped = true;
                                            }
                                            else
                                            {
                                                Logger.LogError("navMeshAgent null");
                                            }
                                        }
                                        // make troop attack
                                        switch (troop.troopAttack.v.state.v.getType())
                                        {
                                            case TroopAttack.State.Type.Normal:
                                                {
                                                    Normal normal = troop.troopAttack.v.state.v as Normal;
                                                    if (normal.coolDown.v <= 0)
                                                    {
                                                        Anim anim = troop.troopAttack.v.state.newOrOld<Anim>();
                                                        {
                                                            anim.target.v = targetTroop.uid;
                                                        }
                                                        troop.troopAttack.v.state.v = anim;
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
                                                Logger.LogError("unknown type: " + troop.troopAttack.v.state.v.getType());
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        // move to range
                                        NavMeshAgent navMeshAgent = troopUI.GetComponent<NavMeshAgent>();
                                        if (navMeshAgent != null)
                                        {
                                            navMeshAgent.destination = targetTroopUI.transform.position;
                                        }
                                        else
                                        {
                                            Logger.LogError("navMeshAgent null");
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