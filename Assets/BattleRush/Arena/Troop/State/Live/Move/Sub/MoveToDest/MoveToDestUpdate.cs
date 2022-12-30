using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ArenaS.TroopS.TroopMoveS
{
    public class MoveToDestUpdate : UpdateBehavior<MoveToDest>
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
                    UnityEngine.AI.NavMeshAgent agent = null;
                    {
                        Troop troop = this.data.findDataInParent<Troop>();
                        if (troop != null)
                        {
                            TroopUI troopUI = troop.findCallBack<TroopUI>();
                            if (troopUI != null)
                            {
                                agent = troopUI.GetComponent<UnityEngine.AI.NavMeshAgent>();
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
                        agent.isStopped = false;
                        agent.destination = this.data.dest.v;
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
            if(data is MoveToDest)
            {
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if(data is MoveToDest)
            {
                MoveToDest moveToDest = data as MoveToDest;
                this.setDataNull(moveToDest);
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
            if(wrapProperty.p is MoveToDest)
            {
                switch ((MoveToDest.Property)wrapProperty.n)
                {
                    case MoveToDest.Property.dest:
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