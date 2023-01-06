using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BattleRushS.ArenaS.TroopS.IntentionS
{
    public class MoveToDestUpdate : UpdateBehavior<MoveToDest>
    {

        #region update

        public override void update()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    Live live = this.data.findDataInParent<Live>();
                    if (live != null)
                    {
                        TroopMove troopMove = live.troopMove.v;
                        if (troopMove != null)
                        {
                            TroopMoveS.MoveToDest moveToDest = troopMove.sub.newOrOld<TroopMoveS.MoveToDest>();
                            {
                                moveToDest.dest.v = this.data.dest.v;
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
                else
                {
                    Logger.LogError("data null");
                }
            }
        }

        public override bool isShouldDisableUpdate()
        {
            return false;
        }

        #endregion

        #region check when agent stop

        private NavMeshAgent agent;

        public override void Update()
        {
            base.Update();
            if (this.data != null)
            {
                // get agent
                if (agent == null)
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
                    if (!agent.pathPending)
                    {
                        if (agent.remainingDistance <= agent.stoppingDistance)
                        {
                            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                            {
                                // Done, change to rest
                                TroopIntention troopIntention = this.data.findDataInParent<TroopIntention>();
                                if (troopIntention != null)
                                {
                                    Rest rest = troopIntention.intention.newOrOld<Rest>();
                                    {

                                    }
                                    troopIntention.intention.v = rest;
                                }
                                else
                                {
                                    Logger.LogError("troopIntention null");
                                }
                            }
                        }
                    }
                }
                else
                {
                    Logger.LogError("agent null");
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