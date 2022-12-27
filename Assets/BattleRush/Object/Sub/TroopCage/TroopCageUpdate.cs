using System.Collections;
using System.Collections.Generic;
using BattleRushS.HeroS;
using BattleRushS.ObjectS.TroopCageS;
using UnityEngine;

namespace BattleRushS.ObjectS
{
    public class TroopCageUpdate : UpdateBehavior<TroopCage>
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
            if(data is TroopCage)
            {
                TroopCage troopCage = data as TroopCage;
                // Child
                {
                    troopCage.state.allAddCallBack(this);
                    troopCage.troops.allAddCallBack(this);
                    troopCage.obstruction.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            {
                if (data is TroopCage.State)
                {
                    TroopCage.State state = data as TroopCage.State;
                    // Update
                    {
                        switch (state.getType())
                        {
                            case TroopCage.State.Type.Live:
                                {
                                    Live live = state as Live;
                                    UpdateUtils.makeUpdate<LiveUpdate, Live>(live, this.transform);
                                }
                                break;
                            case TroopCage.State.Type.ReleaseTroop:
                                {
                                    ReleaseTroop releaseTroop = state as ReleaseTroop;
                                    UpdateUtils.makeUpdate<ReleaseTroopUpdate, ReleaseTroop>(releaseTroop, this.transform);
                                }
                                break;
                            case TroopCage.State.Type.Destroyed:
                                {
                                    Destroyed destroyed = state as Destroyed;
                                    UpdateUtils.makeUpdate<DestroyedUpdate, Destroyed>(destroyed, this.transform);
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
                if(data is TroopFollow)
                {
                    dirty = true;
                    return;
                }
                if(data is Obstruction)
                {
                    Obstruction obstruction = data as Obstruction;
                    // Update
                    {
                        UpdateUtils.makeUpdate<ObstructionUpdate, Obstruction>(obstruction, this.transform);
                    }
                    dirty = true;
                    return;
                }
            }            
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if (data is TroopCage)
            {
                TroopCage troopCage = data as TroopCage;
                // Child
                {
                    troopCage.state.allRemoveCallBack(this);
                    troopCage.troops.allRemoveCallBack(this);
                    troopCage.obstruction.allRemoveCallBack(this);
                }
                this.setDataNull(troopCage);
                return;
            }
            // Child
            {
                if (data is TroopCage.State)
                {
                    TroopCage.State state = data as TroopCage.State;
                    // Update
                    {
                        switch (state.getType())
                        {
                            case TroopCage.State.Type.Live:
                                {
                                    Live live = state as Live;
                                    live.removeCallBackAndDestroy(typeof(LiveUpdate));
                                }
                                break;
                            case TroopCage.State.Type.ReleaseTroop:
                                {
                                    ReleaseTroop releaseTroop = state as ReleaseTroop;
                                    releaseTroop.removeCallBackAndDestroy(typeof(ReleaseTroopUpdate));
                                }
                                break;
                            case TroopCage.State.Type.Destroyed:
                                {
                                    Destroyed destroyed = state as Destroyed;
                                    destroyed.removeCallBackAndDestroy(typeof(DestroyedUpdate));
                                }
                                break;
                            default:
                                Logger.LogError("unknown type: " + state.getType());
                                break;
                        }
                    }
                    return;
                }
                if (data is TroopFollow)
                {
                    TroopFollow troopFollow = data as TroopFollow;
                    // Update
                    {
                        UpdateUtils.makeUpdate<TroopFollowUpdate, TroopFollow>(troopFollow, this.transform);
                    }
                    dirty = true;
                    return;
                }
                if (data is Obstruction)
                {
                    Obstruction obstruction = data as Obstruction;
                    // Update
                    {
                        obstruction.removeCallBackAndDestroy(typeof(ObstructionUpdate));
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
            if (wrapProperty.p is TroopCage)
            {
                switch ((TroopCage.Property)wrapProperty.n)
                {
                    case TroopCage.Property.state:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case TroopCage.Property.troops:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case TroopCage.Property.obstruction:
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
                if (wrapProperty.p is TroopCage.State)
                {
                    return;
                }
                if(wrapProperty.p is TroopFollow)
                {
                    return;
                }
                if(wrapProperty.p is Obstruction)
                {
                    return;
                }
            }
            Logger.LogError("Don't process: " + data + "; " + syncs + "; " + this);
        }

        #endregion

    }
}