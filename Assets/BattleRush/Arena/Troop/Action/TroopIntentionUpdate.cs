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
                if(data is Troop)
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
                        case Troop.Property.level:
                            break;
                        case Troop.Property.hitpoint:
                            dirty = true;
                            break;
                        case Troop.Property.attack:
                            break;
                        case Troop.Property.attackRange:
                            break;
                        case Troop.Property.worldPosition:
                            dirty = true;
                            break;
                        case Troop.Property.takeDamage:
                            break;
                        case Troop.Property.intention:
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