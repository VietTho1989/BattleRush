using System.Collections;
using System.Collections.Generic;
using BattleRushS.ArenaS.TroopS.TroopMoveS;
using UnityEngine;

namespace BattleRushS.ArenaS.TroopS
{
    public class TroopMoveUpdate : UpdateBehavior<TroopMove>
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
            if(data is TroopMove)
            {
                TroopMove troopMove = data as TroopMove;
                // Child
                {
                    troopMove.sub.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            if(data is TroopMove.Sub)
            {
                TroopMove.Sub sub = data as TroopMove.Sub;
                // Update
                {
                    switch (sub.getType())
                    {
                        case TroopMove.Sub.Type.Idle:
                            {
                                Idle idle = sub as Idle;
                                UpdateUtils.makeUpdate<IdleUpdate, Idle>(idle, this.transform);
                            }
                            break;
                        case TroopMove.Sub.Type.MoveToDest:
                            {
                                MoveToDest moveToDest = sub as MoveToDest;
                                UpdateUtils.makeUpdate<MoveToDestUpdate, MoveToDest>(moveToDest, this.transform);
                            }
                            break;
                        default:
                            Logger.LogError("unknown type: " + sub.getType());
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
            if (data is TroopMove)
            {
                TroopMove troopMove = data as TroopMove;
                // Child
                {
                    troopMove.sub.allRemoveCallBack(this);
                }
                this.setDataNull(troopMove);
                return;
            }
            // Child
            if (data is TroopMove.Sub)
            {
                TroopMove.Sub sub = data as TroopMove.Sub;
                // Update
                {
                    switch (sub.getType())
                    {
                        case TroopMove.Sub.Type.Idle:
                            {
                                Idle idle = sub as Idle;
                                idle.removeCallBackAndDestroy(typeof(IdleUpdate));
                            }
                            break;
                        case TroopMove.Sub.Type.MoveToDest:
                            {
                                MoveToDest moveToDest = sub as MoveToDest;
                                moveToDest.removeCallBackAndDestroy(typeof(MoveToDestUpdate));
                            }
                            break;
                        default:
                            Logger.LogError("unknown type: " + sub.getType());
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
            if(wrapProperty.p is TroopMove)
            {
                switch ((TroopMove.Property)wrapProperty.n)
                {
                    case TroopMove.Property.sub:
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
            if (wrapProperty.p is TroopMove.Sub)
            {
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + syncs + "; " + this);
        }

        #endregion

    }
}