using System.Collections;
using System.Collections.Generic;
using BattleRushS.ArenaS.TroopS.TroopMoveS;
using UnityEngine;

namespace BattleRushS.ArenaS.TroopS.IntentionS
{
    public class RestUpdate : UpdateBehavior<Rest>
    {

        #region Update

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

        private Live live = null;

        public override void onAddCallBack<T>(T data)
        {
            if(data is Rest)
            {
                Rest rest = data as Rest;
                // Parent
                {
                    DataUtils.addParentCallBack(rest, this, ref this.live);
                }
                dirty = true;
                return;
            }
            // Parent
            {
                if(data is Live)
                {
                    Live live = data as Live;
                    // Child
                    {
                        live.troopMove.allAddCallBack(this);
                    }
                    dirty = true;
                    return;
                }
                // Child
                if(data is TroopMove)
                {
                    dirty = true;
                    return;
                }
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if(data is Rest)
            {
                Rest rest = data as Rest;
                // Parent
                {
                    DataUtils.removeParentCallBack(rest, this, ref this.live);
                }
                this.setDataNull(rest);
                return;
            }
            // Parent
            {
                if (data is Live)
                {
                    Live live = data as Live;
                    // Child
                    {
                        live.troopMove.allRemoveCallBack(this);
                    }
                    return;
                }
                // Child
                if (data is TroopMove)
                {
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
            if(wrapProperty.p is Rest)
            {
                switch ((Rest.Property)wrapProperty.n)
                {
                    default:
                        break;
                }
                return;
            }
            // Parent
            {
                if (wrapProperty.p is Live)
                {
                    switch ((Live.Property)wrapProperty.n)
                    {
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
                if (wrapProperty.p is TroopMove)
                {
                    switch ((TroopMove.Property)wrapProperty.n)
                    {
                        case TroopMove.Property.sub:
                            dirty = true;
                            break;
                        default:
                            break;
                    }
                    return;
                }
            }
            Logger.LogError("Don't process: " + data + "; " + syncs + "; " + this);
        }

        #endregion

    }
}