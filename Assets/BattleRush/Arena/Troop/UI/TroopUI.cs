using System.Collections;
using System.Collections.Generic;
using BattleRushS.ArenaS.TroopS;
using BattleRushS.HeroS;
using UnityEngine;

namespace BattleRushS.ArenaS
{
    public class TroopUI : UIBehavior<TroopUI.UIData>
    {

        #region UIData

        public class UIData : Data
        {

            public VO<ReferenceData<Troop>> troop;

            public VD<HealthBarUI.UIData> healthBar;

            public VD<TroopAnimationUI.UIData> animation;

            #region Constructor

            public enum Property
            {
                troop,
                healthBar,
                animation
            }

            public UIData() : base()
            {
                this.troop = new VO<ReferenceData<Troop>>(this, (byte)Property.troop, new ReferenceData<Troop>(null));
                this.healthBar = new VD<HealthBarUI.UIData>(this, (byte)Property.healthBar, null);
                this.animation = new VD<TroopAnimationUI.UIData>(this, (byte)Property.animation, new TroopAnimationUI.UIData());
            }

            #endregion

        }

        #endregion

        #region Refresh

        private TroopInformation currentTroopTypeModel;

        public TroopInformation getCurrentTroopTypeModel()
        {
            return currentTroopTypeModel;
        }

        public List<TroopInformation> troopPrefabs;
        public TroopInformation defaultTroopPrefab;

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    Troop troop = this.data.troop.v.data;
                    if (troop != null)
                    {
                        this.name = "TroopUI " + troop.uid;
                        // troopTypeModel
                        {
                            // find
                            bool isNeedMakeNew = true;
                            {
                                if (currentTroopTypeModel != null)
                                {
                                    if (currentTroopTypeModel.troopType == troop.troopType.v)
                                    {
                                        isNeedMakeNew = false;
                                    }
                                }
                            }
                            // make new
                            if (isNeedMakeNew)
                            {
                                // find prefab
                                TroopInformation prefab = null;
                                {
                                    foreach (TroopInformation check in troopPrefabs)
                                    {
                                        if (check != null)
                                        {
                                            if (check.troopType == troop.troopType.v)
                                            {
                                                prefab = check;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            Logger.LogError("why prefab check null");
                                        }
                                    }
                                    // prevent null
                                    if (prefab == null)
                                    {
                                        if (currentTroopTypeModel != defaultTroopPrefab)
                                        {
                                            prefab = defaultTroopPrefab;
                                        }
                                        else
                                        {
                                            // already choose default, no need to make new
                                            isNeedMakeNew = false;
                                        }                                                                       
                                    }
                                }
                                // process
                                if (prefab != null)
                                {
                                    currentTroopTypeModel = Instantiate(prefab, this.transform);
                                    currentTroopTypeModel.transform.localPosition = Vector3.zero;
                                }
                                else
                                {
                                    Logger.LogError("prefab null");
                                }
                            }
                        }
                        // position
                        {
                            Arena arena = troop.findDataInParent<Arena>();
                            if (arena != null)
                            {
                                switch (arena.stage.v.getType())
                                {
                                    case Arena.Stage.Type.PreBattle:
                                        break;
                                    case Arena.Stage.Type.MoveTroopToFormation:
                                        {
                                            ArenaUI arenaUI = arena.findCallBack<ArenaUI>();
                                            if (arenaUI != null)
                                            {
                                                if (arenaUI.center != null)
                                                {
                                                    // find x, z
                                                    float x = Random.Range(-10, 10);
                                                    float z = troop.teamId.v == 0 ? Random.Range(-15, 5) : Random.Range(5, 15);
                                                    this.transform.position = new Vector3(arenaUI.center.position.x + x, arenaUI.center.position.y, arenaUI.center.position.z + z);
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
                                        break;
                                    case Arena.Stage.Type.AutoFight:
                                        break;
                                    case Arena.Stage.Type.FightEnd:
                                        break;
                                    default:
                                        Logger.LogError("unknown type: " + arena.stage.v.getType());
                                        break;
                                }
                            }
                            else
                            {
                                Logger.LogError("arena null");
                            }
                        }
                        // healthBar
                        {
                            switch (troop.state.v.getType())
                            {
                                case Troop.State.Type.Live:
                                    {
                                        HealthBarUI.UIData healthBarUIData = this.data.healthBar.newOrOld<HealthBarUI.UIData>();
                                        {

                                        }
                                        this.data.healthBar.v = healthBarUIData;
                                    }
                                    break;
                                case Troop.State.Type.Die:
                                    {
                                        this.data.healthBar.v = null;
                                    }
                                    break;
                                default:
                                    Logger.LogError("unknown type: "+troop.state.v.getType());
                                    break;
                            }
                        }
                    }
                    else
                    {
                        Logger.LogError("troop null");
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
            return false;
        }

        public override void Update()
        {
            base.Update();
            if (this.data != null)
            {
                Troop troop = this.data.troop.v.data;
                if (troop != null)
                {
                    troop.worldPosition.v = this.transform.position;
                }
                else
                {
                    Logger.LogError("troop null");
                }
            }
            else
            {
                Logger.Log("data null");
            }
        }

        #endregion

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (this.data != null)
            {
                // so co luc van con hien health bar
                this.data.healthBar.v = null;
            }
            else
            {
                Logger.LogError("data null");
            }
        }

        #region implement callBacks

        private Arena arena = null;

        public HealthBarUI healthBarPrefab;
        public TroopAnimationUI troopAnimationUI;

        public override void onAddCallBack<T>(T data)
        {
            if(data is UIData)
            {
                UIData uiData = data as UIData;
                // Child
                {
                    uiData.troop.allAddCallBack(this);
                    uiData.healthBar.allAddCallBack(this);
                    uiData.animation.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            {
                // troop
                {
                    if (data is Troop)
                    {
                        Troop troop = data as Troop;
                        // Parent
                        {
                            DataUtils.addParentCallBack(troop, this, ref this.arena);
                        }
                        dirty = true;
                        return;
                    }
                    // Parent
                    if (data is Arena)
                    {
                        dirty = true;
                        return;
                    }
                }    
                if(data is HealthBarUI.UIData)
                {
                    HealthBarUI.UIData healthBarUIData = data as HealthBarUI.UIData;
                    // UI
                    {
                        // find parent transform
                        Transform parentTransform = this.transform;
                        {
                            BattleRushUI.UIData battleRushUIData = healthBarUIData.findDataInParent<BattleRushUI.UIData>();
                            if (battleRushUIData != null)
                            {
                                BattleRushUI battleRushUI = battleRushUIData.findCallBack<BattleRushUI>();
                                if (battleRushUI != null)
                                {
                                    if (battleRushUI.worldCanvas != null)
                                    {
                                        parentTransform = battleRushUI.worldCanvas;
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
                        // process
                        UIUtils.Instantiate(healthBarUIData, healthBarPrefab, parentTransform);
                    }
                    dirty = true;
                    return;
                }
                if(data is TroopAnimationUI.UIData)
                {
                    TroopAnimationUI.UIData troopAnimationUIData = data as TroopAnimationUI.UIData;
                    // UI
                    {
                        if (troopAnimationUI != null)
                        {
                            troopAnimationUI.setData(troopAnimationUIData);
                        }
                        else
                        {
                            Logger.Log("troopAnimationUI null");
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
            if (data is UIData)
            {
                UIData uiData = data as UIData;
                // Child
                {
                    uiData.troop.allRemoveCallBack(this);
                    uiData.healthBar.allRemoveCallBack(this);
                    uiData.animation.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // Child
            {
                // Troop
                {
                    if (data is Troop)
                    {
                        Troop troop = data as Troop;
                        // Parent
                        {
                            DataUtils.removeParentCallBack(troop, this, ref this.arena);
                        }
                        return;
                    }
                    // Parent
                    if (data is Arena)
                    {
                        return;
                    }
                }
                if(data is HealthBarUI.UIData)
                {
                    HealthBarUI.UIData healthBarUIData = data as HealthBarUI.UIData;
                    // UI
                    {
                        healthBarUIData.removeCallBackAndDestroy(typeof(HealthBarUI));
                    }
                    return;
                }
                if (data is TroopAnimationUI.UIData)
                {
                    TroopAnimationUI.UIData troopAnimationUIData = data as TroopAnimationUI.UIData;
                    // UI
                    {
                        if (troopAnimationUI != null)
                        {
                            troopAnimationUI.setDataNull(troopAnimationUIData);
                        }
                        else
                        {
                            Logger.Log("troopAnimationUI null");
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
            if (wrapProperty.p is UIData)
            {
                switch ((UIData.Property)wrapProperty.n)
                {
                    case UIData.Property.troop:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case UIData.Property.healthBar:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case UIData.Property.animation:
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
                // Troop
                {
                    if (wrapProperty.p is Troop)
                    {
                        switch ((Troop.Property)wrapProperty.n)
                        {
                            case Troop.Property.troopType:
                                dirty = true;
                                break;
                            case Troop.Property.state:
                                dirty = true;
                                break;
                            default:
                                break;
                        }
                        return;
                    }
                    // Parent
                    if (wrapProperty.p is Arena)
                    {
                        switch ((Arena.Property)wrapProperty.n)
                        {
                            case Arena.Property.stage:
                                dirty = true;
                                break;
                            case Arena.Property.troops:
                                dirty = true;
                                break;
                            default:
                                break;
                        }
                        return;
                    }
                }
                if(wrapProperty.p is HealthBarUI.UIData)
                {
                    return;
                }
                if(wrapProperty.p is TroopAnimationUI.UIData)
                {
                    return;
                }
            }
            Logger.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

    }
}