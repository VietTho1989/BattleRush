using System.Collections;
using System.Collections.Generic;
using BattleRushS.ArenaS;
using BattleRushS.HeroS;
using Dreamteck.Forever;
using UnityEngine;

namespace BattleRushS.StateS
{
    public class CheckGameEndUpdate : UpdateBehavior<Play>
    {

        #region Update

        public override void update()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    BattleRush battleRush = this.data.findDataInParent<BattleRush>();
                    if (battleRush != null)
                    {
                        Hero hero = battleRush.hero.v;
                        if (hero != null)
                        {
                            if (hero.heroMove.v.sub.v != null)
                            {
                                switch (hero.heroMove.v.sub.v.getType())
                                {
                                    case HeroS.HeroMove.Sub.Type.Run:
                                        {
                                            if (hero.hitPoint.v <= 0)
                                            {
                                                // hit point 0, hero die, game change to end
                                                End end = battleRush.state.newOrOld<End>();
                                                {
                                                    end.isWin.v = false;
                                                }
                                                battleRush.state.v = end;
                                            }
                                        }
                                        break;
                                    case HeroS.HeroMove.Sub.Type.Arena:
                                        {
                                            HeroMoveArena heroMoveArena = hero.heroMove.v.sub.v as HeroMoveArena;
                                            // check arena fight end
                                            {
                                                // find arena the fight is on
                                                Arena arenaFightCurrentOn = null;
                                                {
                                                    foreach(Arena check in battleRush.arenas.vs)
                                                    {
                                                        ArenaUI arenaUI = check.findCallBack<ArenaUI>();
                                                        if (arenaUI != null)
                                                        {
                                                            Segment segment = arenaUI.GetComponentInParent<Segment>();
                                                            if (segment != null)
                                                            {
                                                                if (segment == battleRush.hero.v.heroMove.v.currentSegment.v)
                                                                {
                                                                    arenaFightCurrentOn = check;
                                                                    break;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                Logger.LogError("levelSegment null");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Logger.LogError("arenaUI null");
                                                        }
                                                    }
                                                }
                                                // process
                                                if (arenaFightCurrentOn != null)
                                                {
                                                    switch (arenaFightCurrentOn.stage.v.getType())
                                                    {
                                                        case Arena.Stage.Type.PreBattle:
                                                            break;
                                                        case Arena.Stage.Type.MoveTroopToFormation:
                                                            break;
                                                        case Arena.Stage.Type.AutoFight:
                                                            break;
                                                        case Arena.Stage.Type.FightEnd:
                                                            {
                                                                FightEnd fightEnd = arenaFightCurrentOn.stage.v as FightEnd;
                                                                // change state to end
                                                                {
                                                                    End end = battleRush.state.newOrOld<End>();
                                                                    {
                                                                        // isWin
                                                                        {
                                                                            switch (fightEnd.teamWin.v)
                                                                            {
                                                                                case 0:
                                                                                    end.isWin.v = true;
                                                                                    break;
                                                                                default:
                                                                                    end.isWin.v = false;
                                                                                    break;
                                                                            }
                                                                        }                                                                        
                                                                    }
                                                                    battleRush.state.v = end;
                                                                }                                                               
                                                            }
                                                            break;
                                                        default:
                                                            Logger.LogError("unknown type: " + arenaFightCurrentOn.stage.v.getType());
                                                            break;
                                                    }
                                                }
                                                else
                                                {
                                                    Logger.LogError("why don't have arena");
                                                }
                                            }
                                        }
                                        break;
                                    default:
                                        Logger.LogError("unknown type: " + hero.heroMove.v.sub.v.getType());
                                        break;
                                }
                            }
                            else
                            {
                                Logger.LogError("heroMove null");
                            }
                        }
                        else
                        {
                            Logger.LogError("hero null");
                        }
                    }
                    else
                    {
                        Logger.LogError("battleRush null");
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

        private BattleRush battleRush = null;

        public override void onAddCallBack<T>(T data)
        {
            if (data is Play)
            {
                Play play = data as Play;
                // Parent
                {
                    DataUtils.addParentCallBack(play, this, ref this.battleRush);
                }
                dirty = true;
                return;
            }
            //  Parent
            {
                if (data is BattleRush)
                {
                    BattleRush battleRush = data as BattleRush;
                    // Child
                    {
                        battleRush.hero.allAddCallBack(this);
                        battleRush.arenas.allAddCallBack(this);
                    }
                    dirty = true;
                    return;
                }
                // Child
                {
                    // Hero
                    {
                        if (data is Hero)
                        {
                            Hero hero = data as Hero;
                            // Child
                            {
                                hero.heroMove.allAddCallBack(this);
                            }
                            dirty = true;
                            return;
                        }
                        // Child
                        if (data is HeroMove)
                        {
                            dirty = true;
                            return;
                        }
                    }
                    if(data is Arena)
                    {
                        dirty = true;
                        return;
                    }
                }
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if (data is Play)
            {
                Play play = data as Play;
                // Parent
                {
                    DataUtils.removeParentCallBack(play, this, ref this.battleRush);
                }
                this.setDataNull(play);
                return;
            }
            //  Parent
            {
                if (data is BattleRush)
                {
                    BattleRush battleRush = data as BattleRush;
                    // Child
                    {
                        battleRush.hero.allRemoveCallBack(this);
                        battleRush.arenas.allRemoveCallBack(this);
                    }
                    return;
                }
                // Child
                {
                    // Hero
                    {
                        if (data is Hero)
                        {
                            Hero hero = data as Hero;
                            // Child
                            {
                                hero.heroMove.allRemoveCallBack(this);
                            }
                            return;
                        }
                        // Child
                        if (data is HeroMove)
                        {
                            return;
                        }
                    }
                    if(data is Arena)
                    {
                        return;
                    }
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
            if (wrapProperty.p is Play)
            {
                switch ((Play.Property)wrapProperty.n)
                {
                    case Play.Property.state:
                        dirty = true;
                        break;
                    default:
                        Logger.LogError("Don't process: " + wrapProperty + "; " + this);
                        break;
                }
                return;
            }
            //  Parent
            {
                if (wrapProperty.p is BattleRush)
                {
                    switch ((BattleRush.Property)wrapProperty.n)
                    {
                        case BattleRush.Property.hero:
                            {
                                ValueChangeUtils.replaceCallBack(this, syncs);
                                dirty = true;
                            }
                            break;
                        case BattleRush.Property.arenas:
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
                    // Hero
                    {
                        if (wrapProperty.p is Hero)
                        {
                            switch ((Hero.Property)wrapProperty.n)
                            {
                                case Hero.Property.hitPoint:
                                    dirty = true;
                                    break;
                                case Hero.Property.heroMove:
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
                        if (wrapProperty.p is HeroMove)
                        {
                            switch ((HeroMove.Property)wrapProperty.n)
                            {
                                case HeroMove.Property.sub:
                                    dirty = true;
                                    break;
                                default:
                                    break;
                            }
                            return;
                        }
                    }
                    // arena
                    if(wrapProperty.p is Arena)
                    {
                        switch ((Arena.Property)wrapProperty.n)
                        {
                            case Arena.Property.stage:
                                dirty = true;
                                break;
                            default:
                                break;
                        }
                        return;
                    }
                }
            }
            Logger.LogError("Don't process: " + data + "; " + syncs + "; " + this);
        }

        #endregion

    }
}