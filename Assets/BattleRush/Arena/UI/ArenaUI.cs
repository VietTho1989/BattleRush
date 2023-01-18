
using System.Collections;
using System.Collections.Generic;
using BattleRushS.ArenaS.StateS;
using UnityEngine;
using UnityEngine.AI;

namespace BattleRushS.ArenaS
{
    public class ArenaUI : UIBehavior<ArenaUI.UIData>
    {

        #region UIData

        public class UIData : Data
        {

            public VO<ReferenceData<Arena>> arena;

            public LD<TroopUI.UIData> troops;

            public LD<ProjectileUI.UIData> projectiles;

            #region stageUI

            public abstract class StageUI : Data
            {

                public abstract Arena.Stage.Type getType();


            }

            public VD<StageUI> stage;

            #endregion

            #region Constructor

            public enum Property
            {
                arena,
                troops,
                projectiles,
                stage
            }

            public UIData() : base()
            {
                this.arena = new VO<ReferenceData<Arena>>(this, (byte)Property.arena, new ReferenceData<Arena>(null));
                this.troops = new LD<TroopUI.UIData>(this, (byte)Property.troops);
                this.projectiles = new LD<ProjectileUI.UIData>(this, (byte)Property.projectiles);
                this.stage = new VD<StageUI>(this, (byte)Property.stage, null);
            }

            #endregion

        }

        #endregion

        public NavMeshSurface navMeshSurface;

        public override void Awake()
        {
            base.Awake();
            // bake navMesh
            /*{
                if (navMeshSurface != null)
                {
                    navMeshSurface.BuildNavMesh();
                }
                else
                {
                    Logger.LogError("navMeshSurface null");
                }
            }*/
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            var navMesh = UnityEngine.AI.NavMesh.CalculateTriangulation();
            if (this.data != null)
            {
                Arena arena = this.data.arena.v.data;
                if (arena != null)
                {
                    BattleRushUI.UIData battleRushUIData = this.data.findDataInParent<BattleRushUI.UIData>();
                    if (battleRushUIData != null)
                    {
                        // remove data
                        {
                            BattleRush battleRush = battleRushUIData.battleRush.v.data;
                            if (battleRush != null)
                            {
                                battleRush.arenas.remove(arena);
                            }
                            else
                            {
                                Logger.LogError("battleRush null");
                            }
                        }
                        // remove ui data
                        battleRushUIData.arenas.remove(this.data);
                    }
                    else
                    {
                        Logger.LogError("battleRushUIData null");
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

        #region Refresh

        public Transform center;

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    Arena arena = this.data.arena.v.data;
                    if (arena != null)
                    {
                        Logger.Log("ArenaUI: make troops: " + arena.troops.vs.Count);
                        // troops
                        {
                            // get old 
                            List<TroopUI.UIData> olds = new List<TroopUI.UIData>();
                            {
                                olds.AddRange(this.data.troops.vs);
                            }
                            // Update
                            {
                                foreach (Troop troop in arena.troops.vs)
                                {
                                    // get UIData
                                    TroopUI.UIData troopUIData = null;
                                    bool needAdd = false;
                                    {
                                        // get old
                                        foreach (TroopUI.UIData check in olds)
                                        {
                                            if (check.troop.v.data == troop)
                                            {
                                                troopUIData = check;
                                                break;
                                            }
                                        }
                                        // make new
                                        if (troopUIData == null)
                                        {
                                            troopUIData = new TroopUI.UIData();
                                            {
                                                troopUIData.uid = this.data.troops.makeId();
                                            }
                                            needAdd = true;
                                        }
                                        else
                                        {
                                            olds.Remove(troopUIData);
                                        }
                                    }
                                    // update
                                    {
                                        troopUIData.troop.v = new ReferenceData<Troop>(troop);
                                    }
                                    // add
                                    if (needAdd)
                                    {
                                        this.data.troops.add(troopUIData);
                                    }
                                }
                            }
                            // remove old
                            foreach(TroopUI.UIData old in olds)
                            {
                                this.data.troops.remove(old);
                            }
                        }
                        // projectiles
                        {
                            Logger.Log("ArenaUI: projectile number: " + arena.projectiles.vs.Count);
                            // get old 
                            List<ProjectileUI.UIData> olds = new List<ProjectileUI.UIData>();
                            {
                                olds.AddRange(this.data.projectiles.vs);
                            }
                            // Update
                            {
                                foreach (Projectile projectile in arena.projectiles.vs)
                                {
                                    // get UIData
                                    ProjectileUI.UIData projectileUIData = null;
                                    bool needAdd = false;
                                    {
                                        // get old
                                        foreach (ProjectileUI.UIData check in olds)
                                        {
                                            if (check.projectile.v.data == projectile)
                                            {
                                                projectileUIData = check;
                                                break;
                                            }
                                        }
                                        // make new
                                        if (projectileUIData == null)
                                        {
                                            projectileUIData = new ProjectileUI.UIData();
                                            {
                                                projectileUIData.uid = this.data.projectiles.makeId();
                                            }
                                            needAdd = true;
                                        }
                                        else
                                        {
                                            olds.Remove(projectileUIData);
                                        }
                                    }
                                    // update
                                    {
                                        projectileUIData.projectile.v = new ReferenceData<Projectile>(projectile);
                                    }
                                    // add
                                    if (needAdd)
                                    {
                                        Logger.Log("ArenaUI: projectile add");
                                        this.data.projectiles.add(projectileUIData);
                                    }
                                }
                            }
                            // remove old
                            foreach (ProjectileUI.UIData old in olds)
                            {
                                this.data.projectiles.remove(old);
                            }
                        }
                        // stage
                        {
                            // UI
                            {
                                switch (arena.stage.v.getType())
                                {
                                    case Arena.Stage.Type.PreBattle:
                                        {
                                            PreBattle preBattle = arena.stage.v as PreBattle;
                                            // UI
                                            {
                                                PreBattleUI.UIData preBattleUIData = this.data.stage.newOrOld<PreBattleUI.UIData>();
                                                {
                                                    preBattleUIData.preBattle.v = new ReferenceData<PreBattle>(preBattle);
                                                }
                                                this.data.stage.v = preBattleUIData;
                                            }
                                        }
                                        break;
                                    case Arena.Stage.Type.MoveTroopToFormation:
                                        {
                                            MoveTroopToFormation moveTroopToFormation = arena.stage.v as MoveTroopToFormation;
                                            // UI
                                            {
                                                MoveTroopToFormationUI.UIData moveTroopToFormationUIData = this.data.stage.newOrOld<MoveTroopToFormationUI.UIData>();
                                                {
                                                    moveTroopToFormationUIData.moveTroopToFormation.v = new ReferenceData<MoveTroopToFormation>(moveTroopToFormation);
                                                }
                                                this.data.stage.v = moveTroopToFormationUIData;
                                            }
                                        }
                                        break;
                                    case Arena.Stage.Type.AutoFight:
                                        {
                                            AutoFight autoFight = arena.stage.v as AutoFight;
                                            // UI
                                            {
                                                AutoFightUI.UIData autoFightUIData = this.data.stage.newOrOld<AutoFightUI.UIData>();
                                                {
                                                    autoFightUIData.autoFight.v = new ReferenceData<AutoFight>(autoFight);
                                                }
                                                this.data.stage.v = autoFightUIData;
                                            }
                                        }
                                        break;
                                    case Arena.Stage.Type.FightEnd:
                                        {
                                            FightEnd fightEnd = arena.stage.v as FightEnd;
                                            // UI
                                            {
                                                FightEndUI.UIData fightEndUIData = this.data.stage.newOrOld<FightEndUI.UIData>();
                                                {
                                                    fightEndUIData.fightEnd.v = new ReferenceData<FightEnd>(fightEnd);
                                                }
                                                this.data.stage.v = fightEndUIData;
                                            }
                                        }
                                        break;
                                    default:
                                        Logger.LogError("unknown type: " + arena.stage.v.getType());
                                        break;
                                }
                            }
                            // set parent transform
                            if (this.data.stage.v != null)
                            {
                                BattleRushUI.UIData battleRushUIData = this.data.findDataInParent<BattleRushUI.UIData>();
                                if (battleRushUIData != null)
                                {
                                    BattleRushUI battleRushUI = battleRushUIData.findCallBack<BattleRushUI>();
                                    if (battleRushUI != null)
                                    {
                                        if (battleRushUI.arenaCanvas != null)
                                        {
                                            // find
                                            Transform stageTransform = null;
                                            {
                                                switch (this.data.stage.v.getType())
                                                {
                                                    case Arena.Stage.Type.PreBattle:
                                                        {
                                                            PreBattleUI preBattleUI = this.data.stage.v.findCallBack<PreBattleUI>();
                                                            if (preBattleUI != null)
                                                            {
                                                                stageTransform = preBattleUI.transform;
                                                            }
                                                            else
                                                            {
                                                                Logger.LogError("preBattleUI null");
                                                            }
                                                        }
                                                        break;
                                                    case Arena.Stage.Type.MoveTroopToFormation:
                                                        {
                                                           MoveTroopToFormationUI moveTroopToFormationUI = this.data.stage.v.findCallBack<MoveTroopToFormationUI>();
                                                            if (moveTroopToFormationUI != null)
                                                            {
                                                                stageTransform = moveTroopToFormationUI.transform;
                                                            }
                                                            else
                                                            {
                                                                Logger.LogError("moveTroopToFormationUI null");
                                                            }
                                                        }
                                                        break;
                                                    case Arena.Stage.Type.AutoFight:
                                                        {
                                                            AutoFightUI autoFightUI = this.data.stage.v.findCallBack<AutoFightUI>();
                                                            if (autoFightUI != null)
                                                            {
                                                                stageTransform = autoFightUI.transform;
                                                            }
                                                            else
                                                            {
                                                                Logger.LogError("autoFightUI null");
                                                            }
                                                        }
                                                        break;
                                                    case Arena.Stage.Type.FightEnd:
                                                        {
                                                            FightEndUI fightEndUI = this.data.stage.v.findCallBack<FightEndUI>();
                                                            if (fightEndUI != null)
                                                            {
                                                                stageTransform = fightEndUI.transform;
                                                            }
                                                            else
                                                            {
                                                                Logger.LogError("fightEndUI null");
                                                            }
                                                        }
                                                        break;
                                                    default:
                                                        Logger.LogError("unknown type: " + this.data.stage.v.getType());
                                                        break;
                                                }
                                            }
                                            // set
                                            if (stageTransform != null)
                                            {
                                                stageTransform.SetParent(battleRushUI.arenaCanvas, false);
                                            }
                                            else
                                            {
                                                Logger.LogError("stageTransform null");
                                            }
                                        }
                                        else
                                        {
                                            Logger.LogError("worldCanvas null");
                                        }
                                    }
                                    else
                                    {
                                        Logger.LogError("battleRushUI null");
                                    }
                                }
                                else
                                {
                                    Logger.LogError("battleRushUIData null");
                                }
                            }
                            else
                            {
                                Logger.LogError("stage null");
                            }
                        }
                    }
                    else
                    {
                        Logger.LogError("arena null");
                    }
                }
                else
                {
                    Logger.LogError("ArenaUI: data null");
                    BattleRushUI battleRushUI = this.GetComponentInParent<BattleRushUI>();
                    if (battleRushUI != null)
                    {
                        BattleRushUI.UIData battleRushUIData = battleRushUI.data;
                        if (battleRushUIData != null)
                        {
                            BattleRush battleRush = battleRushUIData.battleRush.v.data;
                            if (battleRush != null)
                            {
                                UIData arenaUIData = new UIData();
                                {
                                    arenaUIData.uid = battleRushUIData.arenas.makeId();
                                    // arena
                                    {
                                        Arena arena = new Arena();
                                        {
                                            arena.uid = battleRush.arenas.makeId();
                                            battleRush.arenas.add(arena);
                                        }
                                        arenaUIData.arena.v = new ReferenceData<Arena>(arena);
                                    }
                                }
                                // set
                                {
                                    battleRushUIData.arenas.add(arenaUIData);
                                    this.setData(arenaUIData);
                                }                                
                            }
                            else
                            {
                                Logger.LogError("ArenaUI: battleRush null");
                            }
                        }
                        else
                        {
                            Logger.LogError("ArenaUI: battleRushUIData null");
                        }
                    }
                    else
                    {
                        Logger.LogError("ArenaUI: battleRushUI null");
                        dirty = true;
                    }
                }
            }
        }

        public override bool isShouldDisableUpdate()
        {
            return true;
        }

        #endregion

        #region implement callBacks

        public TroopUI troopPrefab;
        public ProjectileUI projectilePrefab;

        public PreBattleUI preBattlePrefab;
        public MoveTroopToFormationUI moveTroopToFormationPrefab;
        public AutoFightUI autoFightPrefab;
        public FightEndUI fightEndPrefab;

        public override void onAddCallBack<T>(T data)
        {
            if(data is UIData)
            {
                UIData uiData = data as UIData;
                // Child
                {
                    uiData.arena.allAddCallBack(this);
                    uiData.troops.allAddCallBack(this);
                    uiData.projectiles.allAddCallBack(this);
                    uiData.stage.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            {
                if(data is Arena)
                {
                    dirty = true;
                    return;
                }
                if(data is TroopUI.UIData)
                {
                    TroopUI.UIData troopUIData = data as TroopUI.UIData;
                    // UI
                    {
                        UIUtils.Instantiate(troopUIData, troopPrefab, this.transform);
                    }
                    dirty = true;
                    return;
                }
                if(data is ProjectileUI.UIData)
                {
                    ProjectileUI.UIData projectileUIData = data as ProjectileUI.UIData;
                    // UI
                    {
                        UIUtils.Instantiate(projectileUIData, projectilePrefab, this.transform);
                    }
                    dirty = true;
                    return;
                }
                if(data is UIData.StageUI)
                {
                    UIData.StageUI stageUI = data as UIData.StageUI;
                    // UI
                    {
                        switch (stageUI.getType())
                        {
                            case Arena.Stage.Type.PreBattle:
                                {
                                    PreBattleUI.UIData preBattleUIData = stageUI as PreBattleUI.UIData;
                                    UIUtils.Instantiate(preBattleUIData, preBattlePrefab, this.transform);
                                }
                                break;
                            case Arena.Stage.Type.MoveTroopToFormation:
                                {
                                    MoveTroopToFormationUI.UIData moveTroopToFormationUIData = stageUI as MoveTroopToFormationUI.UIData;
                                    UIUtils.Instantiate(moveTroopToFormationUIData, moveTroopToFormationPrefab, this.transform);
                                }
                                break;
                            case Arena.Stage.Type.AutoFight:
                                {
                                    AutoFightUI.UIData autoFightUIData = stageUI as AutoFightUI.UIData;
                                    UIUtils.Instantiate(autoFightUIData, autoFightPrefab, this.transform);
                                }
                                break;
                            case Arena.Stage.Type.FightEnd:
                                {
                                    FightEndUI.UIData fightEndUIData = stageUI as FightEndUI.UIData;
                                    UIUtils.Instantiate(fightEndUIData, fightEndPrefab, this.transform);
                                }
                                break;
                            default:
                                Logger.LogError("unknown type: " + stageUI.getType());
                                break;
                        }
                    }
                    dirty = true;
                    return;
                }
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if(data is UIData)
            {
                UIData uiData = data as UIData;
                // Child
                {
                    uiData.arena.allRemoveCallBack(this);
                    uiData.troops.allRemoveCallBack(this);
                    uiData.projectiles.allRemoveCallBack(this);
                    uiData.stage.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // Child
            {
                if (data is Arena)
                {
                    return;
                }
                if (data is TroopUI.UIData)
                {
                    TroopUI.UIData troopUIData = data as TroopUI.UIData;
                    // UI
                    {
                        troopUIData.removeCallBackAndDestroy(typeof(TroopUI));
                    }
                    return;
                }
                if (data is ProjectileUI.UIData)
                {
                    ProjectileUI.UIData projectileUIData = data as ProjectileUI.UIData;
                    // UI
                    {
                        projectileUIData.removeCallBackAndDestroy(typeof(ProjectileUI));
                    }
                    return;
                }
                if (data is UIData.StageUI)
                {
                    UIData.StageUI stageUI = data as UIData.StageUI;
                    // UI
                    {
                        switch (stageUI.getType())
                        {
                            case Arena.Stage.Type.PreBattle:
                                {
                                    PreBattleUI.UIData preBattleUIData = stageUI as PreBattleUI.UIData;
                                    preBattleUIData.removeCallBackAndDestroy(typeof(PreBattleUI));
                                }
                                break;
                            case Arena.Stage.Type.MoveTroopToFormation:
                                {
                                    MoveTroopToFormationUI.UIData moveTroopToFormationUIData = stageUI as MoveTroopToFormationUI.UIData;
                                    moveTroopToFormationUIData.removeCallBackAndDestroy(typeof(MoveTroopToFormationUI));
                                }
                                break;
                            case Arena.Stage.Type.AutoFight:
                                {
                                    AutoFightUI.UIData autoFightUIData = stageUI as AutoFightUI.UIData;
                                    autoFightUIData.removeCallBackAndDestroy(typeof(AutoFightUI));
                                }
                                break;
                            case Arena.Stage.Type.FightEnd:
                                {
                                    FightEndUI.UIData fightEndUIData = stageUI as FightEndUI.UIData;
                                    fightEndUIData.removeCallBackAndDestroy(typeof(FightEndUI));
                                }
                                break;
                            default:
                                Logger.LogError("unknown type: " + stageUI.getType());
                                break;
                        }
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
            if(wrapProperty.p is UIData)
            {
                switch ((UIData.Property)wrapProperty.n)
                {
                    case UIData.Property.arena:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case UIData.Property.troops:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case UIData.Property.projectiles:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case UIData.Property.stage:
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
                if (wrapProperty.p is Arena)
                {
                    switch ((Arena.Property)wrapProperty.n)
                    {
                        case Arena.Property.troops:
                            dirty = true;
                            break;
                        case Arena.Property.projectiles:
                            dirty = true;
                            break;
                        case Arena.Property.stage:
                            dirty = true;
                            break;
                        default:
                            break;
                    }
                    return;
                }
                if (wrapProperty.p is TroopUI.UIData)
                {
                    return;
                }
                if(wrapProperty.p is ProjectileUI.UIData)
                {
                    return;
                }
                if(wrapProperty.p is UIData.StageUI)
                {
                    return;
                }
            }
            Logger.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

    }
}