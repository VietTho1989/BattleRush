using System.Collections;
using System.Collections.Generic;
using BattleRushS.ArenaS.TroopS;
using UnityEngine;

namespace BattleRushS.ArenaS
{
    public class TroopUpdate : UpdateBehavior<Troop>
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
            if(data is Troop)
            {
                Troop troop = data as Troop;
                // Child
                {
                    troop.state.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            if(data is Troop.State)
            {
                Troop.State state = data as Troop.State;
                // Update
                {
                    switch (state.getType())
                    {
                        case Troop.State.Type.Live:
                            {
                                Live live = state as Live;
                                UpdateUtils.makeUpdate<LiveUpdate, Live>(live, this.transform);
                            }
                            break;
                        case Troop.State.Type.Die:
                            {
                                Die die = state as Die;
                                UpdateUtils.makeUpdate<DieUpdate, Die>(die, this.transform);
                            }
                            break;
                        default:
                            Logger.LogError("unknown type: " + state.getType());
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
            if (data is Troop)
            {
                Troop troop = data as Troop;
                // Child
                {
                    troop.state.allRemoveCallBack(this);
                }
                this.setDataNull(troop);
                return;
            }
            // Child
            if (data is Troop.State)
            {
                Troop.State state = data as Troop.State;
                // Update
                {
                    switch (state.getType())
                    {
                        case Troop.State.Type.Live:
                            {
                                Live live = state as Live;
                                live.removeCallBackAndDestroy(typeof(LiveUpdate));
                            }
                            break;
                        case Troop.State.Type.Die:
                            {
                                Die die = state as Die;
                                die.removeCallBackAndDestroy(typeof(DieUpdate));
                            }
                            break;
                        default:
                            Logger.LogError("unknown type: " + state.getType());
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
            if (wrapProperty.p is Troop)
            {
                switch ((Troop.Property)wrapProperty.n)
                {
                    case Troop.Property.state:
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
            if(wrapProperty.p is Troop.State)
            {
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + syncs + "; " + this);
        }

        #endregion

    }
}