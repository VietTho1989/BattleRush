using System.Collections;
using System.Collections.Generic;
using BattleRushS.ArenaS.TroopS;
using BattleRushS.HeroS;
using UnityEngine;

namespace BattleRushS.ArenaS
{
    public class PreBattleUpdate : UpdateBehavior<PreBattle>
    {

        #region Update

        private const float HeroFormationZ = 10.0f;
        private const float DistanceBetweenTroop = 1.5f;

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
                                                troop.troopType.v = hero.heroInformation.v;
                                                // Live
                                                {
                                                    Live live = troop.state.newOrOld<Live>();
                                                    {
                                                        live.hitpoint.v = 1.0f;
                                                    }
                                                    troop.state.v = live;
                                                }
                                                // position
                                                {
                                                    // start position
                                                    {
                                                        HeroUI heroUI = hero.findCallBack<HeroUI>();
                                                        if (heroUI != null)
                                                        {
                                                            troop.startPosition.v = heroUI.transform.position;
                                                            Logger.Log("PreBattleUpdate: startPosition: " + troop.startPosition.v);
                                                        }
                                                        else
                                                        {
                                                            Logger.LogError("PreBattleUpdate troopFollowUI null");
                                                        }
                                                    }
                                                    // formation position
                                                    {
                                                        ArenaUI arenaUI = arena.findCallBack<ArenaUI>();
                                                        if (arenaUI != null)
                                                        {
                                                            if (arenaUI.center != null)
                                                            {
                                                                // find x, z
                                                                float x = 0; // Random.Range(-10, 10);
                                                                float z = troop.teamId.v == 0 ? -HeroFormationZ : HeroFormationZ;// Random.Range(-15, -5) : Random.Range(5, 15);
                                                                troop.formationPosition.v = new Vector3(arenaUI.center.position.x + x, arenaUI.center.position.y, arenaUI.center.position.z + z);
                                                            }
                                                            else
                                                            {
                                                                Logger.LogError("center null");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Logger.LogError("arenaUI null");
                                                        }
                                                    }
                                                }
                                            }
                                            arena.troops.add(troop);
                                        }
                                    }
                                    // troop
                                    {
                                        List<Troop> team0Troops = new List<Troop>();
                                        foreach (TroopFollow troopFollow in battleRush.hero.v.troopFollows.vs)
                                        {
                                            Troop troop = new Troop();
                                            {
                                                troop.uid = arena.troops.makeId();
                                                troop.teamId.v = 0;
                                                troop.troopType.v = troopFollow.troopType.v;
                                                troop.level.v = troopFollow.level.v;
                                                // Live
                                                {
                                                    Live live = troop.state.newOrOld<Live>();
                                                    {
                                                        live.hitpoint.v = troopFollow.hitPoint.v;
                                                    }
                                                    troop.state.v = live;
                                                }
                                                // position
                                                {
                                                    // start position
                                                    {
                                                        TroopFollowUI troopFollowUI = troopFollow.findCallBack<TroopFollowUI>();
                                                        if (troopFollowUI != null)
                                                        {                                                            
                                                            troop.startPosition.v = troopFollowUI.transform.position;
                                                            Logger.Log("PreBattleUpdate: startPosition: " + troop.startPosition.v);
                                                        }
                                                        else
                                                        {
                                                            Logger.LogError("PreBattleUpdate troopFollowUI null");
                                                        }
                                                    }
                                                    // formation position
                                                    {
                                                        // xu ly phia duoi
                                                    }
                                                }
                                            }
                                            arena.troops.add(troop);
                                            team0Troops.Add(troop);
                                        }
                                        battleRush.hero.v.troopFollows.clear();
                                        // formation position
                                        {
                                            ArenaUI arenaUI = arena.findCallBack<ArenaUI>();
                                            if (arenaUI != null)
                                            {
                                                if (arenaUI.center != null)
                                                {
                                                    // get count
                                                    int rangeCount = 0;
                                                    int meleeCount = 0;
                                                    {
                                                        foreach (Troop troop in team0Troops)
                                                        {
                                                            switch (troop.getAttackType())
                                                            {
                                                                case Troop.AttackType.Range:
                                                                    rangeCount++;
                                                                    break;
                                                                case Troop.AttackType.Melee:
                                                                    meleeCount++;
                                                                    break;
                                                                default:
                                                                    Logger.LogError("unknown attack type: " + troop.getAttackType());
                                                                    break;
                                                            }
                                                        }
                                                    }
                                                    // set position
                                                    {
                                                        int rangeIndex = 0;
                                                        int meleeIndex = 0;
                                                        foreach (Troop troop in team0Troops)
                                                        {
                                                            // row
                                                            int row = 0;
                                                            {
                                                                switch (troop.getAttackType())
                                                                {
                                                                    case Troop.AttackType.Range:
                                                                        row = 0;
                                                                        break;
                                                                    case Troop.AttackType.Melee:
                                                                        row = 1;
                                                                        break;
                                                                    default:
                                                                        Logger.LogError("unknown attack type: " + troop.getAttackType());
                                                                        break;
                                                                }
                                                            }
                                                            // col
                                                            int col = 0;
                                                            {
                                                                switch (troop.getAttackType())
                                                                {
                                                                    case Troop.AttackType.Range:
                                                                        col = rangeIndex;
                                                                        break;
                                                                    case Troop.AttackType.Melee:
                                                                        col = meleeIndex;
                                                                        break;
                                                                    default:
                                                                        Logger.LogError("unknown attack type: " + troop.getAttackType());
                                                                        break;
                                                                }
                                                            }
                                                            // rowNumber
                                                            int rowNumber = 0;
                                                            {
                                                                switch (troop.getAttackType())
                                                                {
                                                                    case Troop.AttackType.Range:
                                                                        rowNumber = rangeCount;
                                                                        break;
                                                                    case Troop.AttackType.Melee:
                                                                        rowNumber = meleeCount;
                                                                        break;
                                                                    default:
                                                                        Logger.LogError("unknown attack type: " + troop.getAttackType());
                                                                        break;
                                                                }
                                                            }
                                                            // set position
                                                            {
                                                                // find x, z
                                                                float x = (col - rowNumber / 2 + 0.5f) * DistanceBetweenTroop;//  Random.Range(-10, 10);
                                                                float z = (-HeroFormationZ + DistanceBetweenTroop) + row * DistanceBetweenTroop;
                                                                troop.formationPosition.v = new Vector3(arenaUI.center.position.x + x, arenaUI.center.position.y, arenaUI.center.position.z + z);
                                                            }

                                                            // new index
                                                            {
                                                                switch (troop.getAttackType())
                                                                {
                                                                    case Troop.AttackType.Range:
                                                                        rangeIndex++;
                                                                        break;
                                                                    case Troop.AttackType.Melee:
                                                                        meleeIndex++;
                                                                        break;
                                                                    default:
                                                                        Logger.LogError("unknown attack type: " + troop.getAttackType());
                                                                        break;
                                                                }
                                                            }
                                                        }
                                                    }                                           
                                                }
                                                else
                                                {
                                                    Logger.LogError("center null");
                                                }
                                            }
                                            else
                                            {
                                                Logger.LogError("arenaUI null");
                                            }
                                        }
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
                                List<Troop> team1Troops = new List<Troop>();
                                for (int i = 0; i < Random.Range(5, 12); i++)
                                {
                                    Troop troop = new Troop();
                                    {
                                        troop.uid = arena.troops.makeId();
                                        troop.teamId.v = 1;
                                        // level
                                        {
                                            troop.level.v = UnityEngine.Random.Range(1, 3);
                                        }
                                        // troopType
                                        {
                                            BattleRushUI battleRushUI = battleRush.findCallBack<BattleRushUI>();
                                            if (battleRushUI != null)
                                            {
                                                troop.troopType.v = battleRushUI.troopInformations[Random.Range(0, battleRushUI.troopInformations.Count)];
                                            }
                                            else
                                            {
                                                Logger.LogError("battleRushUI null");
                                            }
                                        }
                                        // Live
                                        {
                                            Live live = troop.state.newOrOld<Live>();
                                            {
                                                live.hitpoint.v = 1.0f;
                                            }
                                            troop.state.v = live;
                                        }
                                        // position
                                        {
                                            // xu ly phia duoi
                                        }
                                    }
                                    arena.troops.add(troop);
                                    team1Troops.Add(troop);
                                }
                                // formation position
                                {
                                    ArenaUI arenaUI = arena.findCallBack<ArenaUI>();
                                    if (arenaUI != null)
                                    {
                                        if (arenaUI.center != null)
                                        {
                                            // get count
                                            int rangeCount = 0;
                                            int meleeCount = 0;
                                            {
                                                foreach (Troop troop in team1Troops)
                                                {
                                                    switch (troop.getAttackType())
                                                    {
                                                        case Troop.AttackType.Range:
                                                            rangeCount++;
                                                            break;
                                                        case Troop.AttackType.Melee:
                                                            meleeCount++;
                                                            break;
                                                        default:
                                                            Logger.LogError("unknown type: " + troop.getAttackType());
                                                            break;
                                                    }
                                                }
                                            }
                                            // set position
                                            {
                                                int rangeIndex = 0;
                                                int meleeIndex = 0;
                                                foreach (Troop troop in team1Troops)
                                                {
                                                    // row
                                                    int row = 0;
                                                    {
                                                        switch (troop.getAttackType())
                                                        {
                                                            case Troop.AttackType.Range:
                                                                row = 0;
                                                                break;
                                                            case Troop.AttackType.Melee:
                                                                row = 1;
                                                                break;
                                                            default:
                                                                Logger.LogError("unknown attack type: " + troop.getAttackType());
                                                                break;
                                                        }
                                                    }
                                                    // col
                                                    int col = 0;
                                                    {
                                                        switch (troop.getAttackType())
                                                        {
                                                            case Troop.AttackType.Range:
                                                                col = rangeIndex;
                                                                break;
                                                            case Troop.AttackType.Melee:
                                                                col = meleeIndex;
                                                                break;
                                                            default:
                                                                Logger.LogError("unknown attack type: " + troop.getAttackType());
                                                                break;
                                                        }
                                                    }
                                                    // rowNumber
                                                    int rowNumber = 0;
                                                    {
                                                        switch (troop.getAttackType())
                                                        {
                                                            case Troop.AttackType.Range:
                                                                rowNumber = rangeCount;
                                                                break;
                                                            case Troop.AttackType.Melee:
                                                                rowNumber = meleeCount;
                                                                break;
                                                            default:
                                                                Logger.LogError("unknown attack type: " + troop.getAttackType());
                                                                break;
                                                        }
                                                    }
                                                    // set position
                                                    {
                                                        // find x, z
                                                        float x = (col - rowNumber / 2 + 0.5f) * DistanceBetweenTroop;//  Random.Range(-10, 10);
                                                        float z = -((-HeroFormationZ + DistanceBetweenTroop) + row * DistanceBetweenTroop);
                                                        troop.formationPosition.v = new Vector3(arenaUI.center.position.x + x, arenaUI.center.position.y, arenaUI.center.position.z + z);
                                                        troop.startPosition.v = new Vector3(troop.formationPosition.v.x, troop.formationPosition.v.y, troop.formationPosition.v.z + 3 * DistanceBetweenTroop);
                                                    }

                                                    // new index
                                                    {
                                                        switch (troop.getAttackType())
                                                        {
                                                            case Troop.AttackType.Range:
                                                                rangeIndex++;
                                                                break;
                                                            case Troop.AttackType.Melee:
                                                                meleeIndex++;
                                                                break;
                                                            default:
                                                                Logger.LogError("unknown attack type: " + troop.getAttackType());
                                                                break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Logger.LogError("center null");
                                        }
                                    }
                                    else
                                    {
                                        Logger.LogError("arenaUI null");
                                    }
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