using System.Collections;
using System.Collections.Generic;
using BattleRushS.HeroS;
using UnityEngine;

namespace BattleRushS.ArenaS
{
    public class PreBattleUpdate : UpdateBehavior<PreBattle>
    {

        #region Update

        public override void update()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    // find hero already move to arena
                    bool isHeroMovedToArena = false;
                    {
                        BattleRush battleRush = this.data.findDataInParent<BattleRush>();
                        if (battleRush != null)
                        {
                            if (battleRush.hero.v.heroMove.v.sub.v != null)
                            {
                                switch (battleRush.hero.v.heroMove.v.sub.v.getType())
                                {
                                    case HeroMove.Sub.Type.Arena:
                                        {
                                            HeroMoveArena heroMoveArena = battleRush.hero.v.heroMove.v.sub.v as HeroMoveArena;
                                            // check current segment is this arena segment in
                                            Segment currentSegment = battleRush.hero.v.heroMove.v.currentSegment.v;
                                            if (currentSegment != null)
                                            {
                                                Arena arena = this.data.findDataInParent<Arena>();
                                                if (arena != null)
                                                {
                                                    ArenaUI arenaUI = arena.findCallBack<ArenaUI>();
                                                    if (arenaUI != null)
                                                    {
                                                        if (arenaUI.GetComponentInParent<Segment>() == currentSegment)
                                                        {
                                                            isHeroMovedToArena = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Logger.LogError("arenaUI null");
                                                    }
                                                }
                                                else
                                                {
                                                    Logger.LogError("arena null");
                                                }
                                            }
                                        }
                                        break;
                                    case HeroMove.Sub.Type.Run:
                                        break;
                                    default:
                                        Logger.LogError("unknown type: " + battleRush.hero.v.heroMove.v.sub.v.getType());
                                        break;
                                }
                            }
                        }
                        else
                        {
                            Logger.LogError("battleRush null");
                        }
                    }
                    // process
                    if (isHeroMovedToArena)
                    {
                        // make hero's troop
                        {
                            Arena arena = this.data.findDataInParent<Arena>();
                            if (arena != null)
                            {
                                BattleRush battleRush = this.data.findDataInParent<BattleRush>();
                                if (battleRush != null)
                                {
                                    // hero
                                    {
                                        Hero hero = battleRush.hero.v;
                                        // make hero troop
                                        {
                                            Troop troop = new Troop();
                                            {
                                                troop.uid = arena.troops.makeId();
                                                troop.teamId.v = 0;
                                                troop.troopType.v = hero.troopType.v;
                                                troop.hitpoint.v = 1.0f;
                                            }
                                            arena.troops.add(troop);
                                        }
                                    }
                                    // troop
                                    foreach (TroopFollow troopFollow in battleRush.hero.v.troopFollows.vs)
                                    {
                                        Troop troop = new Troop();
                                        {
                                            troop.uid = arena.troops.makeId();
                                            troop.teamId.v = 0;
                                            troop.troopType.v = troopFollow.troopType.v;
                                            troop.hitpoint.v = troopFollow.hitPoint.v;
                                        }
                                        arena.troops.add(troop);
                                    }
                                }
                                else
                                {
                                    Logger.LogError("battleRush null");
                                }
                            }
                            else
                            {
                                Logger.LogError("arena null");
                            }
                        }
                        // make enemy's troop
                        {
                            Arena arena = this.data.findDataInParent<Arena>();
                            if (arena != null)
                            {
                                // get
                                List<TroopFollow.TroopType> enemyTroopTypes = new List<TroopFollow.TroopType>();
                                {
                                    foreach (TroopFollow.TroopType check in System.Enum.GetValues(typeof(TroopFollow.TroopType)))
                                    {
                                        if (!TroopFollow.IsHero(check))
                                        {
                                            enemyTroopTypes.Add(check);
                                        }
                                    }
                                }
                                // make
                                for (int i = 0; i < Random.Range(5, 12); i++)
                                {
                                    Troop troop = new Troop();
                                    {
                                        troop.uid = arena.troops.makeId();
                                        troop.teamId.v = 1;
                                        // troopType
                                        {
                                            if (enemyTroopTypes.Count > 0)
                                            {
                                                troop.troopType.v = enemyTroopTypes[Random.Range(0, enemyTroopTypes.Count)];
                                            }
                                            else
                                            {
                                                Logger.LogError("enemyTroopTypes null");
                                            }
                                        }
                                        troop.hitpoint.v = 1.0f;
                                    }
                                    arena.troops.add(troop);
                                }
                            }
                            else
                            {
                                Logger.LogError("arena null");
                            }
                        }
                        // change to next stage
                        {
                            Arena arena = this.data.findDataInParent<Arena>();
                            if (arena != null)
                            {
                                MoveTroopToFormation moveTroopToFormation = arena.stage.newOrOld<MoveTroopToFormation>();
                                {

                                }
                                arena.stage.v = moveTroopToFormation;
                            }
                            else
                            {
                                Logger.LogError("arena null");
                            }
                        }
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
            if(data is PreBattle)
            {
                PreBattle preBattle = data as PreBattle;
                // Parent
                {
                    DataUtils.addParentCallBack(preBattle, this, ref this.battleRush);
                }
                dirty = true;
                return;
            }
            // Parent
            {
                if(data is BattleRush)
                {
                    BattleRush battleRush = data as BattleRush;
                    // Child
                    {
                        battleRush.hero.allAddCallBack(this);
                    }
                    dirty = true;
                    return;
                }
                // Child
                {
                    if(data is Hero)
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
                    if(data is HeroMove)
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
            if(data is PreBattle)
            {
                PreBattle preBattle = data as PreBattle;
                // Parent
                {
                    DataUtils.removeParentCallBack(preBattle, this, ref this.battleRush);
                }
                this.setDataNull(preBattle);
                return;
            }
            // Parent
            {
                if (data is BattleRush)
                {
                    BattleRush battleRush = data as BattleRush;
                    // Child
                    {
                        battleRush.hero.allRemoveCallBack(this);
                    }
                    return;
                }
                // Child
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
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onUpdateSync<T>(WrapProperty wrapProperty, List<Sync<T>> syncs)
        {
            if (WrapProperty.checkError(wrapProperty))
            {
                return;
            }
            if(wrapProperty.p is PreBattle)
            {
                switch ((PreBattle.Property)wrapProperty.n)
                {
                    default:
                        break;
                }
                return;
            }
            // Parent
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
                        default:
                            break;
                    }
                    return;
                }
                // Child
                {
                    if (wrapProperty.p is Hero)
                    {
                        switch ((Hero.Property)wrapProperty.n)
                        {
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
                            case HeroMove.Property.currentSegment:
                                dirty = true;
                                break;
                            case HeroMove.Property.sub:
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