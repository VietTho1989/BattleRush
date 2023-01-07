using System.Collections;
using System.Collections.Generic;
using BattleRushS.ArenaS.TroopS;
using BattleRushS.ArenaS.TroopS.IntentionS;
using UnityEngine;

namespace BattleRushS.ArenaS
{
    public class MoveTroopToFormationUpdate : UpdateBehavior<MoveTroopToFormation>
    {

        #region update

        public override void update()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    switch (this.data.state.v)
                    {
                        case MoveTroopToFormation.State.Move:
                            {
                                if (this.data.time.v >= this.data.duration.v)
                                {
                                    this.data.state.v = MoveTroopToFormation.State.Came;
                                }
                            }
                            break;
                        case MoveTroopToFormation.State.Came:
                            {
                                if (this.data.time.v >= this.data.duration.v + 1.0f)
                                {
                                    this.data.state.v = MoveTroopToFormation.State.Ready;
                                }
                            }
                            break;
                        case MoveTroopToFormation.State.Ready:
                            {
                                if (this.data.time.v >= this.data.duration.v + 4.0f)
                                {
                                    // change to auto fight
                                    Arena arena = this.data.findDataInParent<Arena>();
                                    if (arena != null)
                                    {
                                        AutoFight autoFight = arena.stage.newOrOld<AutoFight>();
                                        {

                                        }
                                        arena.stage.v = autoFight;
                                    }
                                    else
                                    {
                                        Logger.LogError("arena null");
                                    }
                                }
                            }
                            break;
                        default:
                            Logger.LogError("unknown state: " + this.data.state.v);
                            break;
                    }
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
                // start move to formation position
                {
                    if (this.data.state.v == MoveTroopToFormation.State.Start)
                    {                       
                        Arena arena = this.data.findDataInParent<Arena>();
                        if (arena != null)
                        {
                            foreach(Troop troop in arena.troops.vs)
                            {
                                switch (troop.state.v.getType())
                                {
                                    case Troop.State.Type.Live:
                                        {
                                            Live live = troop.state.v as Live;
                                            // make move to dest
                                            {
                                                TroopIntention troopIntention = live.intention.v;
                                                TroopIntentionUpdate troopIntentionUpdate = troopIntention.findCallBack<TroopIntentionUpdate>();
                                                if (troopIntentionUpdate != null)
                                                {
                                                    troopIntentionUpdate.update();
                                                }
                                                else
                                                {
                                                    Logger.LogError("troopIntentionUpdate null");
                                                }
                                                /*TroopIntention troopIntention = live.intention.v;
                                                MoveToDest moveToDest = troopIntention.intention.newOrOld<MoveToDest>();
                                                {
                                                    // dest
                                                    {
                                                        moveToDest.dest.v = troop.formationPosition.v;
                                                    }
                                                    Logger.Log("MoveTroopToFormationUpdate move to dest: " + troop.uid + ": " + moveToDest.dest.v);
                                                }
                                                troopIntention.intention.v = moveToDest;*/
                                            }
                                        }
                                        break;
                                    case Troop.State.Type.Die:
                                        break;
                                    default:
                                        Logger.LogError("unknown type: " + troop.state.v.getType());
                                        break;
                                }
                            }
                        }
                        else
                        {
                            Logger.LogError("arena null");
                        }
                        this.data.state.v = MoveTroopToFormation.State.Move;
                    }
                }
                this.data.time.v += Time.deltaTime;
                // check finish move to formation
                {
                    switch (this.data.state.v)
                    {
                        case MoveTroopToFormation.State.Move:
                            {
                                // find
                                bool isFinish = true;
                                {
                                    Arena arena = this.data.findDataInParent<Arena>();
                                    if (arena != null)
                                    {
                                        foreach(Troop troop in arena.troops.vs)
                                        {
                                            switch (troop.state.v.getType())
                                            {
                                                case Troop.State.Type.Live:
                                                    {
                                                        Live live = troop.state.v as Live;
                                                        switch (live.intention.v.intention.v.getType())
                                                        {
                                                            case TroopIntention.Intention.Type.Rest:
                                                                {
                                                                    Logger.Log("MoveTroopToFormationUpdate: already finish: " + troop.uid);
                                                                }
                                                                break;
                                                            case TroopIntention.Intention.Type.Attack:
                                                                {
                                                                    Logger.Log("MoveTroopToFormationUpdate: not finish: " + troop.uid);
                                                                    isFinish = false;
                                                                }
                                                                break;
                                                            case TroopIntention.Intention.Type.MoveToDest:
                                                                {
                                                                    Logger.Log("MoveTroopToFormationUpdate: not finish: " + troop.uid);
                                                                    isFinish = false;
                                                                }
                                                                break;
                                                            default:
                                                                Logger.LogError("unknown type: " + live.intention.v.intention.v.getType());
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case Troop.State.Type.Die:
                                                    break;
                                                default:
                                                    Logger.LogError("unknown type: " + troop.state.v.getType());
                                                    break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Logger.LogError("arena null");
                                    }
                                }
                                // process
                                Logger.Log("MoveTroopToFormationUpdate: finish: " + isFinish);
                                if (isFinish)
                                {
                                    this.data.time.v = this.data.duration.v + 1.0f;
                                }
                            }
                            break;
                        case MoveTroopToFormation.State.Came:
                            break;
                        case MoveTroopToFormation.State.Ready:
                            break;
                        default:
                            Logger.LogError("unknown state: " + this.data.state.v);
                            break;
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
            if(data is MoveTroopToFormation)
            {
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if (data is MoveTroopToFormation)
            {
                MoveTroopToFormation moveTroopToFormation = data as MoveTroopToFormation;
                this.setDataNull(moveTroopToFormation);
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
            if(wrapProperty.p is MoveTroopToFormation)
            {
                switch ((MoveTroopToFormation.Property)wrapProperty.n)
                {
                    case MoveTroopToFormation.Property.time:
                        dirty = true;
                        break;
                    case MoveTroopToFormation.Property.duration:
                        dirty = true;
                        break;
                    case MoveTroopToFormation.Property.state:
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