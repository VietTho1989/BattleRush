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

            public VD<TroopAuraUI.UIData> aura;

            #region Constructor

            public enum Property
            {
                troop,
                healthBar,
                animation,
                aura
            }

            public UIData() : base()
            {
                this.troop = new VO<ReferenceData<Troop>>(this, (byte)Property.troop, new ReferenceData<Troop>(null));
                this.healthBar = new VD<HealthBarUI.UIData>(this, (byte)Property.healthBar, null);
                this.animation = new VD<TroopAnimationUI.UIData>(this, (byte)Property.animation, new TroopAnimationUI.UIData());
                this.aura = new VD<TroopAuraUI.UIData>(this, (byte)Property.aura, new TroopAuraUI.UIData());
            }

            #endregion

        }

        #endregion

        #region Refresh

        private TroopType currentTroopTypeModel;

        public TroopType getCurrentTroopTypeModel()
        {
            return currentTroopTypeModel;
        }

        private bool isAlreadySetFirstPosition = false;

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
                        // set first position
                        {
                            if (!isAlreadySetFirstPosition)
                            {
                                isAlreadySetFirstPosition = true;
                                this.transform.position = troop.startPosition.v;
                            }
                        }
                        // troopTypeModel
                        {
                            // find
                            bool isNeedMakeNew = true;
                            {
                                if (currentTroopTypeModel != null)
                                {
                                    if (currentTroopTypeModel.getType() == troop.troopType.v.getType())
                                    {
                                        switch (troop.troopType.v.getType())
                                        {
                                            case TroopType.Type.Hero:
                                                {
                                                    HeroInformation currentHero = currentTroopTypeModel as HeroInformation;
                                                    HeroInformation troopType = troop.troopType.v as HeroInformation;
                                                    if (currentHero.id == troopType.id)
                                                    {
                                                        isNeedMakeNew = false;
                                                    }                                                    
                                                }
                                                break;
                                            case TroopType.Type.Normal:
                                                {
                                                    TroopInformation currentTroop = currentTroopTypeModel as TroopInformation;
                                                    TroopInformation troopType = troop.troopType.v as TroopInformation;
                                                    if (currentTroop.modelName == troopType.modelName)
                                                    {
                                                        isNeedMakeNew = false;
                                                    }
                                                }
                                                break;
                                            case TroopType.Type.Monster:
                                                break;
                                            default:
                                                Logger.LogError("unknown type: "+troop.troopType.v.getType());
                                                break;
                                        }                                       
                                    }
                                }
                            }
                            // make new
                            if (isNeedMakeNew)
                            {
                                // destroy old
                                {
                                    if (currentTroopTypeModel != null)
                                    {
                                        Destroy(currentTroopTypeModel.getGameObject());
                                    }
                                    else
                                    {
                                        Logger.LogError("currentTroopTypeModel null");
                                    }
                                }
                                // instantiate new
                                switch (troop.troopType.v.getType())
                                {
                                    case TroopType.Type.Hero:
                                        {
                                            HeroInformation heroInformation = troop.troopType.v as HeroInformation;
                                            currentTroopTypeModel = Instantiate(heroInformation, this.transform);
                                            currentTroopTypeModel.getGameObject().transform.localPosition = Vector3.zero;
                                        }
                                        break;
                                    case TroopType.Type.Normal:
                                        {
                                            TroopInformation troopInformation = troop.troopType.v as TroopInformation;
                                            currentTroopTypeModel = Instantiate(troopInformation, this.transform);
                                            currentTroopTypeModel.getGameObject().transform.localPosition = Vector3.zero;
                                        }
                                        break;
                                    case TroopType.Type.Monster:
                                        {
                                            // TODO can hoan thien
                                        }
                                        break;
                                    default:
                                        Logger.LogError("unknown type: " + troop.troopType.v.getType());
                                        break;
                                }
                            }
                        }
                        // scale
                        {
                            float scale = 1.0f;
                            {
                                switch (troop.troopType.v.getType())
                                {
                                    case TroopType.Type.Hero:
                                        break;
                                    case TroopType.Type.Normal:
                                        {
                                            TroopInformation troopInformation = troop.troopType.v as TroopInformation;
                                            TroopInformation.Level level = troopInformation.levels.Find(check => check.level == troop.level.v);
                                            if (level != null)
                                            {
                                                scale = level.scale;
                                            }
                                            else
                                            {
                                                Logger.LogError("level null");
                                            }
                                        }
                                        break;
                                    case TroopType.Type.Monster:
                                        {
                                            // TODO can hoan thien
                                        }
                                        break;
                                    default:
                                        Logger.LogError("unknown type: " + troop.troopType.v.getType());
                                        break;
                                }
                            }
                            this.transform.localScale = new Vector3(scale, scale, scale);
                        }
                        // team
                        {
                            if (currentTroopTypeModel != null)
                            {
                                currentTroopTypeModel.setUIByTeam(troop.teamId.v);
                            }
                            else
                            {
                                Logger.LogError("currentTroopTypeModel null");
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
                                        {
                                            this.transform.position = troop.startPosition.v;
                                        }
                                        break;
                                    case Arena.Stage.Type.MoveTroopToFormation:
                                        {
                                            // TODO can hoan thien                                           
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
        public TroopAuraUI troopAuraUI;

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
                    uiData.aura.allAddCallBack(this);
                }
                isAlreadySetFirstPosition = false;
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
                if(data is TroopAuraUI.UIData)
                {
                    TroopAuraUI.UIData troopAuraUIData = data as TroopAuraUI.UIData;
                    // UI
                    {
                        if (troopAuraUI != null)
                        {
                            troopAuraUI.setData(troopAuraUIData);
                        }
                        else
                        {
                            Logger.LogError("troopAuraUI null");
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
                    uiData.aura.allRemoveCallBack(this);
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
                if (data is TroopAuraUI.UIData)
                {
                    TroopAuraUI.UIData troopAuraUIData = data as TroopAuraUI.UIData;
                    // UI
                    {
                        if (troopAuraUI != null)
                        {
                            troopAuraUI.setDataNull(troopAuraUIData);
                        }
                        else
                        {
                            Logger.LogError("troopAuraUI null");
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
                    case UIData.Property.aura:
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
                            case Troop.Property.startPosition:
                                dirty = true;
                                break;
                            case Troop.Property.formationPosition:
                                dirty = true;
                                break;
                            case Troop.Property.troopType:
                                dirty = true;
                                break;
                            case Troop.Property.teamId:
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
                if (wrapProperty.p is TroopAuraUI.UIData)
                {
                    return;
                }
            }
            Logger.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

    }
}