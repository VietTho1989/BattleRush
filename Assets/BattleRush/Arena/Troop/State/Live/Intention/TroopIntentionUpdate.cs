using System.Collections;
using System.Collections.Generic;
using BattleRushS.ArenaS.TroopS.IntentionS;
using UnityEngine;

namespace BattleRushS.ArenaS.TroopS
{
    public class TroopIntentionUpdate : UpdateBehavior<TroopIntention>
    {

        #region Update

        public override void update()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    Arena arena = this.data.findDataInParent<Arena>();
                    if (arena != null)
                    {
                        switch (arena.stage.v.getType())
                        {
                            case Arena.Stage.Type.PreBattle:
                                {
                                    Rest rest = this.data.intention.newOrOld<Rest>();
                                    {

                                    }
                                    this.data.intention.v = rest;
                                }
                                break;
                            case Arena.Stage.Type.MoveTroopToFormation:
                                {
                                    MoveTroopToFormation moveTroopToFormation = arena.stage.v as MoveTroopToFormation;
                                    switch (moveTroopToFormation.state.v)
                                    {
                                        case MoveTroopToFormation.State.Start:
                                            {
                                                Logger.Log("TroopIntentionUpdate move troop to formation");
                                                MoveToDest moveToDest = this.data.intention.newOrOld<MoveToDest>();
                                                {
                                                    // delay
                                                    {
                                                        // find
                                                        float delay = 0;
                                                        {
                                                            Troop troop = this.data.findDataInParent<Troop>();
                                                            if (troop != null)
                                                            {
                                                                if (troop.troopType.v.getType() == TroopType.Type.Hero)
                                                                {
                                                                    delay = 3.0f;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                Logger.LogError("troop null");
                                                            }
                                                        }
                                                        // process
                                                        moveToDest.delay.v = delay;
                                                    }
                                                    // dest
                                                    {
                                                        Troop troop = this.data.findDataInParent<Troop>();
                                                        if (troop != null)
                                                        {
                                                            // find arena center
                                                            Transform arenaCenter = null;
                                                            {
                                                                ArenaUI arenaUI = arena.findCallBack<ArenaUI>();
                                                                if (arenaUI != null)
                                                                {
                                                                    arenaCenter = arenaUI.transform;
                                                                }
                                                                else
                                                                {
                                                                    Logger.LogError("arenaUI null");
                                                                }
                                                            }
                                                            // process
                                                            if (arenaCenter != null)
                                                            {
                                                                moveToDest.dest.v = arenaCenter.TransformPoint(troop.formationPosition.v);
                                                            }
                                                            else
                                                            {
                                                                Logger.LogError("arenaCenter null");
                                                                dirty = true;
                                                            }                                                            
                                                        }
                                                        else
                                                        {
                                                            Logger.LogError("troop null");
                                                        }
                                                    }
                                                }
                                                this.data.intention.v = moveToDest;
                                            }
                                            break;
                                        case MoveTroopToFormation.State.Move:
                                            break;
                                        case MoveTroopToFormation.State.Came:
                                            break;
                                        case MoveTroopToFormation.State.Ready:
                                            break;
                                        default:
                                            Logger.LogError("unknown state: " + moveTroopToFormation.state.v);
                                            break;
                                    }                                   
                                }
                                break;
                            case Arena.Stage.Type.AutoFight:
                                {
                                    // find
                                    bool needFindTargetToAttack = false;
                                    {
                                        switch (this.data.intention.v.getType())
                                        {
                                            case TroopIntention.Intention.Type.Rest:
                                                needFindTargetToAttack = true;
                                                break;
                                            case TroopIntention.Intention.Type.MoveToDest:
                                                needFindTargetToAttack = true;
                                                break;
                                            case TroopIntention.Intention.Type.Attack:
                                                {
                                                    Attack attack = this.data.intention.v as Attack;
                                                    // check target is dead or not
                                                    Troop targetTroop = arena.troops.vs.Find(check => check.uid == attack.targetId.v);
                                                    if (targetTroop != null)
                                                    {
                                                        // check target troop can be targeted any more
                                                        bool canTarget = true;
                                                        {
                                                            // target already dead
                                                            if (canTarget)
                                                            {
                                                                if (targetTroop.state.v.getType() != Troop.State.Type.Live)
                                                                {
                                                                    canTarget = false;
                                                                }
                                                            }                                                            
                                                        }
                                                        // process
                                                        if (!canTarget)
                                                        {
                                                            needFindTargetToAttack = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        // target troop not exist anymore
                                                        needFindTargetToAttack = true;
                                                    }
                                                }
                                                break;
                                            default:
                                                Logger.LogError("unknown type: " + this.data.intention.v.getType());
                                                break;
                                        }
                                    }
                                    // process
                                    if (needFindTargetToAttack)
                                    {
                                        Logger.Log("need find target to attack: " + needFindTargetToAttack);
                                        // find target to attack
                                        Troop targetToAttack = null;
                                        {
                                            Troop troop = this.data.findDataInParent<Troop>();
                                            if (troop != null)
                                            {
                                                foreach (Troop check in arena.troops.vs)
                                                {
                                                    // find can target
                                                    bool canTarget = true;
                                                    {
                                                        // check team
                                                        if (canTarget)
                                                        {
                                                            if (check.teamId.v == troop.teamId.v)
                                                            {
                                                                canTarget = false;
                                                            }
                                                        }
                                                        // check hitpoint
                                                        if (canTarget)
                                                        {
                                                            if (check.state.v.getType() != Troop.State.Type.Live)
                                                            {
                                                                canTarget = false;
                                                            }
                                                        }                                                        
                                                    }
                                                    if (canTarget)
                                                    {
                                                        // set or not
                                                        if (targetToAttack == null)
                                                        {
                                                            targetToAttack = check;
                                                        }
                                                        else
                                                        {
                                                            // choose target nearest
                                                            if (Vector3.Distance(troop.worldPosition.v, targetToAttack.worldPosition.v) > Vector3.Distance(troop.worldPosition.v, check.worldPosition.v))
                                                            {
                                                                targetToAttack = check;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Logger.LogError("troop null");
                                            }                                           
                                        }
                                        // process
                                        if (targetToAttack != null)
                                        {
                                            Attack attack = this.data.intention.newOrOld<Attack>();
                                            {
                                                attack.targetId.v = targetToAttack.uid;
                                            }
                                            this.data.intention.v = attack;
                                        }
                                        else
                                        {
                                            Logger.LogError("why don't have target to attack");
                                            Rest rest = this.data.intention.newOrOld<Rest>();
                                            {

                                            }
                                            this.data.intention.v = rest;
                                        }
                                    }
                                }
                                break;
                            case Arena.Stage.Type.FightEnd:
                                break;
                            default:
                                Logger.LogError("unknown type: " + arena.stage.v.getType());
                                break;
                        }
                    }
                    else
                    {
                        Logger.LogError("arena null");
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

        private Arena arena = null;

        public override void onAddCallBack<T>(T data)
        {
            if(data is TroopIntention)
            {
                TroopIntention troopIntention = data as TroopIntention;
                // Parent
                {
                    DataUtils.addParentCallBack(troopIntention, this, ref this.arena);
                }
                // Child
                {
                    troopIntention.intention.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Parent
            {
                if(data is Arena)
                {
                    Arena arena = data as Arena;
                    // Child
                    {
                        arena.troops.allAddCallBack(this);
                    }
                    dirty = true;
                    return;
                }
                // Child
                if (data is Troop)
                {
                    dirty = true;
                    return;
                }
            }
            // Child
            if(data is TroopIntention.Intention)
            {
                TroopIntention.Intention intention = data as TroopIntention.Intention;
                // Update
                {
                    switch (intention.getType())
                    {
                        case TroopIntention.Intention.Type.Rest:
                            {
                                Rest rest = intention as Rest;
                                UpdateUtils.makeUpdate<RestUpdate, Rest>(rest, this.transform);
                            }
                            break;
                        case TroopIntention.Intention.Type.Attack:
                            {
                                Attack attack = intention as Attack;
                                UpdateUtils.makeUpdate<AttackUpdate, Attack>(attack, this.transform);
                            }
                            break;
                        case TroopIntention.Intention.Type.MoveToDest:
                            {
                                MoveToDest moveToDest = intention as MoveToDest;
                                UpdateUtils.makeUpdate<MoveToDestUpdate, MoveToDest>(moveToDest, this.transform);
                            }
                            break;
                        default:
                            Logger.LogError("unknown type: "+intention.getType());
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
            if(data is TroopIntention)
            {
                TroopIntention troopIntention = data as TroopIntention;
                // Parent
                {
                    DataUtils.removeParentCallBack(troopIntention, this, ref this.arena);
                }
                // Child
                {
                    troopIntention.intention.allRemoveCallBack(this);
                }
                this.setDataNull(troopIntention);
                return;
            }
            // Parent
            {
                if (data is Arena)
                {
                    Arena arena = data as Arena;
                    // Child
                    {
                        arena.troops.allRemoveCallBack(this);
                    }
                    return;
                }
                // Child
                if (data is Troop)
                {
                    return;
                }
            }
            // Child
            if (data is TroopIntention.Intention)
            {
                TroopIntention.Intention intention = data as TroopIntention.Intention;
                // Update
                {
                    switch (intention.getType())
                    {
                        case TroopIntention.Intention.Type.Rest:
                            {
                                Rest rest = intention as Rest;
                                rest.removeCallBackAndDestroy(typeof(RestUpdate));
                            }
                            break;
                        case TroopIntention.Intention.Type.Attack:
                            {
                                Attack attack = intention as Attack;
                                attack.removeCallBackAndDestroy(typeof(AttackUpdate));
                            }
                            break;
                        case TroopIntention.Intention.Type.MoveToDest:
                            {
                                MoveToDest moveToDest = intention as MoveToDest;
                                moveToDest.removeCallBackAndDestroy(typeof(MoveToDestUpdate));
                            }
                            break;
                        default:
                            Logger.LogError("unknown type: " + intention.getType());
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
            if(wrapProperty.p is TroopIntention)
            {
                switch ((TroopIntention.Property)wrapProperty.n)
                {
                    case TroopIntention.Property.intention:
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
            // Parent
            {
                if (wrapProperty.p is Arena)
                {
                    switch ((Arena.Property)wrapProperty.n)
                    {
                        case Arena.Property.troops:
                            {
                                ValueChangeUtils.replaceCallBack(this, syncs);
                                dirty = true;
                            }
                            break;
                        case Arena.Property.stage:
                            dirty = true;
                            break;
                        default:
                            break;
                    }
                    return;
                }
                // Child
                if (wrapProperty.p is Troop)
                {
                    switch ((Troop.Property)wrapProperty.n)
                    {
                        case Troop.Property.teamId:
                            dirty = true;
                            break;
                        case Troop.Property.troopType:
                            break;
                        case Troop.Property.state:                        
                            break;
                        case Troop.Property.worldPosition:
                            dirty = true;
                            break;
                        default:
                            break;
                    }
                    return;
                }
            }
            // Child
            if (wrapProperty.p is TroopIntention.Intention)
            {
                TroopIntention.Intention intention = wrapProperty.p as TroopIntention.Intention;
                switch (intention.getType())
                {
                    case TroopIntention.Intention.Type.Rest:
                        break;
                    case TroopIntention.Intention.Type.Attack:
                        {
                            switch ((Attack.Property)wrapProperty.n)
                            {
                                case Attack.Property.targetId:
                                    dirty = true;
                                    break;
                                case Attack.Property.time:
                                    dirty = true;
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    default:
                        Logger.LogError("unknown type: " + intention.getType());
                        break;
                }
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + syncs + "; " + this);
        }

        #endregion

    }
}