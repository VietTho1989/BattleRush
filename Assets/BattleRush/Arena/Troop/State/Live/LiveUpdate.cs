using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ArenaS.TroopS
{
    public class LiveUpdate : UpdateBehavior<Live>
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

        public override bool isShouldDisableUpdate()
        {
            return true;
        }

        #endregion

        #region implement callBacks

        public override void onAddCallBack<T>(T data)
        {
            if(data is Live)
            {
                Live live = data as Live;
                // Child
                {
                    live.takeDamage.allAddCallBack(this);
                    live.intention.allAddCallBack(this);
                    live.troopAttack.allAddCallBack(this);
                    live.troopMove.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            {
                if(data is TakeDamage)
                {
                    TakeDamage takeDamage = data as TakeDamage;
                    // Update
                    {
                        UpdateUtils.makeUpdate<TakeDamageUpdate, TakeDamage>(takeDamage, this.transform);
                    }
                    dirty = true;
                    return;
                }
                if(data is TroopIntention)
                {
                    TroopIntention troopIntention = data as TroopIntention;
                    // Update
                    {
                        UpdateUtils.makeUpdate<TroopIntentionUpdate, TroopIntention>(troopIntention, this.transform);
                    }
                    dirty = true;
                    return;
                }
                if(data is TroopAttack)
                {
                    TroopAttack troopAttack = data as TroopAttack;
                    // Update
                    {
                        UpdateUtils.makeUpdate<TroopAttackUpdate, TroopAttack>(troopAttack, this.transform);
                    }
                    dirty = true;
                    return;
                }
                if(data is TroopMove)
                {
                    TroopMove troopMove = data as TroopMove;
                    // Update
                    {
                        UpdateUtils.makeUpdate<TroopMoveUpdate, TroopMove>(troopMove, this.transform);
                    }
                    dirty = true;
                    return;
                }
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if (data is Live)
            {
                Live live = data as Live;
                // Child
                {
                    live.takeDamage.allRemoveCallBack(this);
                    live.intention.allRemoveCallBack(this);
                    live.troopAttack.allRemoveCallBack(this);
                    live.troopMove.allRemoveCallBack(this);
                }
                this.setDataNull(live);
                return;
            }
            // Child
            {
                if (data is TakeDamage)
                {
                    TakeDamage takeDamage = data as TakeDamage;
                    // Update
                    {
                        takeDamage.removeCallBackAndDestroy(typeof(TakeDamageUpdate));
                    }
                    return;
                }
                if (data is TroopIntention)
                {
                    TroopIntention troopIntention = data as TroopIntention;
                    // Update
                    {
                        troopIntention.removeCallBackAndDestroy(typeof(TroopIntentionUpdate));
                    }
                    return;
                }
                if (data is TroopAttack)
                {
                    TroopAttack troopAttack = data as TroopAttack;
                    // Update
                    {
                        troopAttack.removeCallBackAndDestroy(typeof(TroopAttackUpdate));
                    }
                    return;
                }
                if (data is TroopMove)
                {
                    TroopMove troopMove = data as TroopMove;
                    // Update
                    {
                        troopMove.removeCallBackAndDestroy(typeof(TroopMoveUpdate));
                    }
                    return;
                }
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onUpdateSync<T>(WrapProperty wrapProperty, List<Sync<T>> syncs)
        {
            if (WrapProperty.checkError(wrapProperty))
            {
                return;
            }
            if (wrapProperty.p is Live)
            {
                switch ((Live.Property)wrapProperty.n)
                {
                    case Live.Property.takeDamage:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case Live.Property.intention:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case Live.Property.troopAttack:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case Live.Property.troopMove:
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
            // Child
            {
                if (wrapProperty.p is TakeDamage)
                {
                    return;
                }
                if (wrapProperty.p is TroopIntention)
                {
                    return;
                }
                if (wrapProperty.p is TroopAttack)
                {
                    return;
                }
                if(wrapProperty.p is TroopMove)
                {
                    return;
                }
            }
            Logger.LogError("Don't process: " + data + "; " + syncs + "; " + this);
        }

        #endregion

    }
}