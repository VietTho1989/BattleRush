using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BattleRushS.ArenaS.TroopS.TroopMoveS
{
    public class IdleUpdate : UpdateBehavior<Idle>
    {

        #region Update

        public override void update()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    // find NavMeshAgent
                    NavMeshAgent agent = null;
                    {
                        Troop troop = this.data.findDataInParent<Troop>();
                        if (troop != null)
                        {
                            TroopUI troopUI = troop.findCallBack<TroopUI>();
                            if (troopUI != null)
                            {
                                agent = troopUI.GetComponent<NavMeshAgent>();
                            }
                            else
                            {
                                Logger.LogError("troopUI null");
                            }
                        }
                        else
                        {
                            Logger.LogError("troop null");
                        }
                    }
                    // process
                    if (agent != null)
                    {
                        agent.enabled = true;
                        agent.isStopped = true;
                    }
                    else
                    {
                        Logger.LogError("agent null");
                        dirty = true;
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
            if(data is Idle)
            {
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if (data is Idle)
            {
                Idle idle = data as Idle;
                this.setDataNull(idle);
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
            if(wrapProperty.p is Idle)
            {
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + syncs + "; " + this);
        }

        #endregion

    }
}