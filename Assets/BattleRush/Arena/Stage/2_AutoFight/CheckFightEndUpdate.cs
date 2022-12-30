using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ArenaS.StateS.AutoFightS
{
    public class CheckFightEndUpdate : UpdateBehavior<AutoFight>
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
                        // find
                        bool isTeam0Alive = false;
                        bool isTeam1Alive = false;
                        {
                            foreach(Troop troop in arena.troops.vs)
                            {
                                if(troop.state.v.getType() == Troop.State.Type.Live)
                                {
                                    switch (troop.teamId.v)
                                    {
                                        case 0:
                                            isTeam0Alive = true;
                                            break;
                                        case 1:
                                            isTeam1Alive = true;
                                            break;
                                        default:
                                            Logger.LogError("unknown teamId: " + troop.teamId.v);
                                            break;
                                    }
                                }
                            }
                        }
                        // process
                        if(!isTeam0Alive || !isTeam1Alive)
                        {
                            FightEnd fightEnd = arena.stage.newOrOld<FightEnd>();
                            {
                                // team win
                                {
                                    if (isTeam0Alive)
                                    {
                                        fightEnd.teamWin.v = 0;
                                    }
                                    else
                                    {
                                        fightEnd.teamWin.v = 1;
                                    }
                                }
                            }
                            arena.stage.v = fightEnd;
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
            if(data is AutoFight)
            {
                AutoFight autoFight = data as AutoFight;
                // Parent
                {
                    DataUtils.addParentCallBack(autoFight, this, ref this.arena);
                }
                dirty = true;
                return;
            }
            // Child
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
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if (data is AutoFight)
            {
                AutoFight autoFight = data as AutoFight;
                // Parent
                {
                    DataUtils.removeParentCallBack(autoFight, this, ref this.arena);
                }
                this.setDataNull(autoFight);
                return;
            }
            // Child
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
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onUpdateSync<T>(WrapProperty wrapProperty, List<Sync<T>> syncs)
        {
            if (WrapProperty.checkError(wrapProperty))
            {
                return;
            }
            if (wrapProperty.p is AutoFight)
            {
                return;
            }
            // Child
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
                        case Troop.Property.state:
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
