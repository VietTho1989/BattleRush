using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ArenaS
{
    public class ArenaUpdate : UpdateBehavior<Arena>
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
            if(data is Arena)
            {
                Arena arena = data as Arena;
                // Child
                {
                    arena.stage.allAddCallBack(this);
                    arena.troops.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            {
                if (data is Arena.Stage)
                {
                    Arena.Stage state = data as Arena.Stage;
                    // Update
                    {
                        switch (state.getType())
                        {
                            case Arena.Stage.Type.PreBattle:
                                {
                                    PreBattle preBattle = state as PreBattle;
                                    UpdateUtils.makeUpdate<PreBattleUpdate, PreBattle>(preBattle, this.transform);
                                }
                                break;
                            case Arena.Stage.Type.MoveTroopToFormation:
                                {
                                    MoveTroopToFormation moveTroopToFormation = state as MoveTroopToFormation;
                                    UpdateUtils.makeUpdate<MoveTroopToFormationUpdate, MoveTroopToFormation>(moveTroopToFormation, this.transform);
                                }
                                break;
                            case Arena.Stage.Type.AutoFight:
                                {
                                    AutoFight autoFight = state as AutoFight;
                                    UpdateUtils.makeUpdate<AutoFightUpdate, AutoFight>(autoFight, this.transform);
                                }
                                break;
                            case Arena.Stage.Type.FightEnd:
                                {
                                    FightEnd fightEnd = state as FightEnd;
                                    UpdateUtils.makeUpdate<FightEndUpdate, FightEnd>(fightEnd, this.transform);
                                }
                                break;
                        }
                    }
                    dirty = true;
                    return;
                }
                if(data is Troop)
                {
                    Troop troop = data as Troop;
                    // Update
                    {
                        UpdateUtils.makeUpdate<TroopUpdate, Troop>(troop, this.transform);
                    }
                    dirty = true;
                    return;
                }
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if (data is Arena)
            {
                Arena arena = data as Arena;
                // Child
                {
                    arena.stage.allRemoveCallBack(this);
                    arena.troops.allRemoveCallBack(this);
                }
                return;
            }
            // Child
            {
                if (data is Arena.Stage)
                {
                    Arena.Stage state = data as Arena.Stage;
                    // Update
                    {
                        switch (state.getType())
                        {
                            case Arena.Stage.Type.PreBattle:
                                {
                                    PreBattle preBattle = state as PreBattle;
                                    preBattle.removeCallBackAndDestroy(typeof(PreBattleUpdate));
                                }
                                break;
                            case Arena.Stage.Type.MoveTroopToFormation:
                                {
                                    MoveTroopToFormation moveTroopToFormation = state as MoveTroopToFormation;
                                    moveTroopToFormation.removeCallBackAndDestroy(typeof(MoveTroopToFormationUpdate));
                                }
                                break;
                            case Arena.Stage.Type.AutoFight:
                                {
                                    AutoFight autoFight = state as AutoFight;
                                    autoFight.removeCallBackAndDestroy(typeof(AutoFightUpdate));
                                }
                                break;
                            case Arena.Stage.Type.FightEnd:
                                {
                                    FightEnd fightEnd = state as FightEnd;
                                    fightEnd.removeCallBackAndDestroy(typeof(FightEndUpdate));
                                }
                                break;
                        }
                    }
                    dirty = true;
                    return;
                }
                if (data is Troop)
                {
                    Troop troop = data as Troop;
                    // Update
                    {
                        troop.removeCallBackAndDestroy(typeof(TroopUpdate));
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
            if (wrapProperty.p is Arena)
            {
                switch ((Arena.Property)wrapProperty.n)
                {
                    case Arena.Property.stage:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
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
            {
                if (wrapProperty.p is Arena.Stage)
                {
                    return;
                }
                if(wrapProperty.p is Troop)
                {
                    return;
                }
            }
            Logger.LogError("Don't process: " + data + "; " + syncs + "; " + this);
        }

        #endregion

    }
}