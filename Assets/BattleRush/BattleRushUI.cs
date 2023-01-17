using System.Collections;
using System.Collections.Generic;
using BattleRushS.ArenaS;
using BattleRushS.HeroS;
using BattleRushS.ObjectS;
using BattleRushS.StateS;
using Dreamteck.Forever;
using UnityEngine;

namespace BattleRushS
{
    public class BattleRushUI : UIBehavior<BattleRushUI.UIData>
    {

        public bool isEditorMode = false;
        public List<HeroInformation> heroInformations;
        public List<TroopInformation> troopInformations;

        public List<ItemMap> itemMaps;

        #region Data

        public class UIData : Data
        {

            public VO<ReferenceData<BattleRush>> battleRush;

            public VD<CameraUI.UIData> camera;

            public VD<MainCanvasUI.UIData> mainCanvas;

            #region stateUI 

            public abstract class StateUI : Data
            {
                public abstract BattleRush.State.Type getType();
            }

            public VD<StateUI> stateUI;

            #endregion

            public VD<HeroUI.UIData> hero;

            public VD<PlayerInputUI.UIData> playerInput;

            #region object in path

            public abstract class ObjectInPathUI : Data
            {
                public abstract ObjectInPath.Type getType();

                public abstract ObjectInPath getObjectInPath();
            }

            public interface ObjectInPathUIInterface
            {
                public void setObjectInPathUIData(Data data);

                public GameObject getMyGameObject();

                public ObjectInPath.Type getType();
            }

            public LD<ObjectInPathUI> objectInPaths;

            #endregion

            public LD<ArenaUI.UIData> arenas;

            #region Constructor

            public enum Property
            {
                battleRush,
                camera,
                mainCanvas,
                stateUI,
                hero,
                playerInput,
                objectInPaths,
                arenas
            }

            public UIData() : base()
            {
                this.battleRush = new VO<ReferenceData<BattleRush>>(this, (byte)Property.battleRush, new ReferenceData<BattleRush>(null));
                this.camera = new VD<CameraUI.UIData>(this, (byte)Property.camera, new CameraUI.UIData());
                this.mainCanvas = new VD<MainCanvasUI.UIData>(this, (byte)Property.mainCanvas, new MainCanvasUI.UIData());
                this.stateUI = new VD<StateUI>(this, (byte)Property.stateUI, null);
                this.hero = new VD<HeroUI.UIData>(this, (byte)Property.hero, new HeroUI.UIData());
                this.playerInput = new VD<PlayerInputUI.UIData>(this, (byte)Property.playerInput, new PlayerInputUI.UIData());
                this.objectInPaths = new LD<ObjectInPathUI>(this, (byte)Property.objectInPaths);
                this.arenas = new LD<ArenaUI.UIData>(this, (byte)Property.arenas);
            }

            #endregion

        }

        #endregion

        public LevelGenerator levelGenerator;

        /**
         * TODO de tam level 0
         * */
        public MapAsset level_0;

        public override void Awake()
        {
            base.Awake();
            // prevent uiData null
            {
                if (this.data == null)
                {
                    UIData uiData = new UIData();
                    {
                        BattleRush battleRush = new BattleRush();
                        {
                            battleRush.isInEditMode.v = this.isEditorMode;
                        }
                        uiData.battleRush.v = new ReferenceData<BattleRush>(battleRush);
                    }
                    this.setData(uiData);
                }
            }
            // levelGenerator
            {
                // find
                if (levelGenerator == null)
                {
                    levelGenerator = GetComponent<LevelGenerator>();
                }
                // process
                if (levelGenerator != null)
                {
                    // set my custom sequence
                    {
                        CustomSequence customSequence = levelGenerator.currentLevel.sequenceCollection.sequences[0].customSequence;
                        Logger.Log("myCustomSequence: " + customSequence);
                        if (customSequence!=null && customSequence is MySequence)
                        {
                            MySequence mySequence = customSequence as MySequence;
                            mySequence.setBattleRush(this.data.battleRush.v.data);
                        }                        
                    }
                }
            }
        }

        #region Refresh

        public Transform worldCanvas;

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    BattleRush battleRush = this.data.battleRush.v.data;
                    if (battleRush != null)
                    {
                        // camera
                        {
                            CameraUI.UIData cameraUIData = this.data.camera.v;
                            if (cameraUIData != null)
                            {
                                cameraUIData.battleRush.v = new ReferenceData<BattleRush>(battleRush);
                            }
                            else
                            {
                                Logger.LogError("cameraUIData null");
                            }
                        }
                        // index in MainUICanvas
                        {
                            UIRectTransform.SetSiblingIndex(this.data.playerInput.v, 0);
                            UIRectTransform.SetSiblingIndex(this.data.mainCanvas.v, 1);
                        }
                        // hero
                        {
                            HeroUI.UIData heroUIData = this.data.hero.v;
                            if (heroUIData != null)
                            {
                                heroUIData.hero.v = new ReferenceData<Hero>(battleRush.hero.v);
                            }
                            else
                            {
                                Logger.LogError("heroUIData null");
                            }
                        }
                        // stateUI
                        {
                            switch (battleRush.state.v.getType())
                            {
                                case BattleRush.State.Type.Load:
                                    {
                                        Load load = battleRush.state.v as Load;
                                        // make UI
                                        LoadUI.UIData loadUIData = this.data.stateUI.newOrOld<LoadUI.UIData>();
                                        {
                                            loadUIData.load.v = new ReferenceData<Load>(load);
                                        }
                                        this.data.stateUI.v = loadUIData;
                                    }
                                    break;
                                case BattleRush.State.Type.Start:
                                    {
                                        Start start = battleRush.state.v as Start;
                                        // make UI
                                        StartUI.UIData startUIData = this.data.stateUI.newOrOld<StartUI.UIData>();
                                        {
                                            startUIData.start.v = new ReferenceData<Start>(start);
                                        }
                                        this.data.stateUI.v = startUIData;
                                    }
                                    break;
                                case BattleRush.State.Type.Play:
                                    {
                                        Play play = battleRush.state.v as Play;
                                        // make UI
                                        PlayUI.UIData playUIData = this.data.stateUI.newOrOld<PlayUI.UIData>();
                                        {
                                            playUIData.play.v = new ReferenceData<Play>(play);
                                        }
                                        this.data.stateUI.v = playUIData;
                                    }
                                    break;
                                case BattleRush.State.Type.End:
                                    {
                                        End end = battleRush.state.v as End;
                                        // make UI
                                        EndUI.UIData endUIData = this.data.stateUI.newOrOld<EndUI.UIData>();
                                        {
                                            endUIData.end.v = new ReferenceData<End>(end);
                                        }
                                        this.data.stateUI.v = endUIData;
                                    }
                                    break;
                                case BattleRush.State.Type.Edit:
                                    {
                                        Edit edit = battleRush.state.v as Edit;
                                        // make UI
                                        EditUI.UIData editUIData = this.data.stateUI.newOrOld<EditUI.UIData>();
                                        {
                                            editUIData.edit.v = new ReferenceData<Edit>(edit);
                                        }
                                        this.data.stateUI.v = editUIData;
                                    }
                                    break;
                                default:
                                    Logger.LogError("unknown state: " + battleRush.state.v);
                                    break;
                            }
                        }
                        // playerInput
                        {
                            PlayerInputUI.UIData playerInputUIData = this.data.playerInput.v;
                            if (playerInputUIData != null)
                            {
                                playerInputUIData.game.v = new ReferenceData<BattleRush>(battleRush);
                            }
                            else
                            {
                                Logger.LogError("playerInputUIData null");
                            }
                        }
                        // object in paths
                        /*{
                            //Debug.Log("objectInPaths: " + battleRush.laneObjects.vs.Count);
                            // get old
                            List<UIData.ObjectInPathUI> olds = new List<UIData.ObjectInPathUI>();
                            {
                                olds.AddRange(this.data.objectInPaths.vs);
                            }
                            // update
                            {
                                List<UIData.ObjectInPathUI> newAds = new List<UIData.ObjectInPathUI>();
                                foreach (ObjectInPath objectInPath in battleRush.laneObjects.vs)
                                {
                                    // find UI
                                    UIData.ObjectInPathUI objectInPathUIData = null;
                                    bool needAdd = false;
                                    {
                                        // find old
                                        foreach (UIData.ObjectInPathUI check in olds)
                                        {
                                            if (check.getObjectInPath() == objectInPath)
                                            {
                                                objectInPathUIData = check;
                                                break;
                                            }
                                        }
                                        // make new
                                        if (objectInPathUIData == null)
                                        {
                                            switch (objectInPath.getType())
                                            {
                                                case ObjectInPath.Type.OkgCoin:
                                                    {
                                                        objectInPathUIData = new CoinUI.UIData();
                                                        {
                                                            objectInPathUIData.uid = this.data.objectInPaths.makeId();
                                                        }
                                                        needAdd = true;
                                                    }
                                                    break;
                                                case ObjectInPath.Type.EnergyOrbNormal:
                                                    {
                                                        objectInPathUIData = new EnergyOrbNormalUI.UIData();
                                                        {
                                                            objectInPathUIData.uid = this.data.objectInPaths.makeId();
                                                        }
                                                        needAdd = true;
                                                    }
                                                    break;
                                                case ObjectInPath.Type.TroopCage:
                                                    {
                                                        objectInPathUIData = new TroopCageUI.UIData();
                                                        {
                                                            objectInPathUIData.uid = this.data.objectInPaths.makeId();
                                                        }
                                                        needAdd = true;
                                                    }
                                                    break;
                                                case ObjectInPath.Type.SawBlade:
                                                    {
                                                        objectInPathUIData = new SawBladeUI.UIData();
                                                        {
                                                            objectInPathUIData.uid = this.data.objectInPaths.makeId();
                                                        }
                                                        needAdd = true;
                                                    }
                                                    break;
                                                case ObjectInPath.Type.Blade:
                                                    {
                                                        objectInPathUIData = new BladeUI.UIData();
                                                        {
                                                            objectInPathUIData.uid = this.data.objectInPaths.makeId();
                                                        }
                                                        needAdd = true;
                                                    }
                                                    break;
                                                case ObjectInPath.Type.FireNozzle:
                                                    {
                                                        objectInPathUIData = new FireNozzleUI.UIData();
                                                        {
                                                            objectInPathUIData.uid = this.data.objectInPaths.makeId();
                                                        }
                                                        needAdd = true;
                                                    }
                                                    break;
                                                case ObjectInPath.Type.Pike:
                                                    {
                                                        objectInPathUIData = new PikeUI.UIData();
                                                        {
                                                            objectInPathUIData.uid = this.data.objectInPaths.makeId();
                                                        }
                                                        needAdd = true;
                                                    }
                                                    break;
                                                case ObjectInPath.Type.EnergyOrbUpgrade:
                                                    {
                                                        objectInPathUIData = new EnergyOrbUpgradeUI.UIData();
                                                        {
                                                            objectInPathUIData.uid = this.data.objectInPaths.makeId();
                                                        }
                                                        needAdd = true;
                                                    }
                                                    break;
                                                case ObjectInPath.Type.UpgradeGateFree:
                                                    {
                                                        objectInPathUIData = new UpgradeGateFreeUI.UIData();
                                                        {
                                                            objectInPathUIData.uid = this.data.objectInPaths.makeId();
                                                        }
                                                        needAdd = true;
                                                    }
                                                    break;
                                                case ObjectInPath.Type.UpgradeGateCharge:
                                                    {
                                                        objectInPathUIData = new UpgradeGateChargeUI.UIData();
                                                        {
                                                            objectInPathUIData.uid = this.data.objectInPaths.makeId();
                                                        }
                                                        needAdd = true;
                                                    }
                                                    break;
                                                case ObjectInPath.Type.Hammer:
                                                    {
                                                        objectInPathUIData = new HammerUI.UIData();
                                                        {
                                                            objectInPathUIData.uid = this.data.objectInPaths.makeId();
                                                        }
                                                        needAdd = true;
                                                    }
                                                    break;
                                                case ObjectInPath.Type.Grinder:
                                                    {
                                                        objectInPathUIData = new GrinderUI.UIData();
                                                        {
                                                            objectInPathUIData.uid = this.data.objectInPaths.makeId();
                                                        }
                                                        needAdd = true;
                                                    }
                                                    break;
                                                case ObjectInPath.Type.EnergyOrbPower:
                                                    {
                                                        objectInPathUIData = new EnergyOrbPowerUI.UIData();
                                                        {
                                                            objectInPathUIData.uid = this.data.objectInPaths.makeId();
                                                        }
                                                        needAdd = true;
                                                    }
                                                    break;
                                                case ObjectInPath.Type.CocoonMantah:
                                                    {
                                                        objectInPathUIData = new CocoonMantahUI.UIData();
                                                        {
                                                            objectInPathUIData.uid = this.data.objectInPaths.makeId();
                                                        }
                                                        needAdd = true;
                                                    }
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            olds.Remove(objectInPathUIData);
                                        }
                                    }
                                    // update
                                    {
                                        switch (objectInPath.getType())
                                        {
                                            case ObjectInPath.Type.OkgCoin:
                                                {
                                                    Coin coin = objectInPath as Coin;
                                                    if (objectInPathUIData != null && objectInPathUIData is CoinUI.UIData)
                                                    {
                                                        ((CoinUI.UIData)objectInPathUIData).coin.v = new ReferenceData<Coin>(coin);
                                                    }
                                                }
                                                break;
                                            case ObjectInPath.Type.EnergyOrbNormal:
                                                {
                                                    EnergyOrbNormal energyOrbNormal = objectInPath as EnergyOrbNormal;
                                                    if (objectInPathUIData != null && objectInPathUIData is EnergyOrbNormalUI.UIData)
                                                    {
                                                        ((EnergyOrbNormalUI.UIData)objectInPathUIData).energyOrbNormal.v = new ReferenceData<EnergyOrbNormal>(energyOrbNormal);
                                                    }
                                                }
                                                break;
                                            case ObjectInPath.Type.TroopCage:
                                                {
                                                    TroopCage troopCage = objectInPath as TroopCage;
                                                    if (objectInPathUIData != null && objectInPathUIData is TroopCageUI.UIData)
                                                    {
                                                        ((TroopCageUI.UIData)objectInPathUIData).troopCage.v = new ReferenceData<TroopCage>(troopCage);
                                                    }
                                                }
                                                break;
                                            case ObjectInPath.Type.SawBlade:
                                                {
                                                    SawBlade sawBlade = objectInPath as SawBlade;
                                                    if (objectInPathUIData != null && objectInPathUIData is SawBladeUI.UIData)
                                                    {
                                                        ((SawBladeUI.UIData)objectInPathUIData).sawBlade.v = new ReferenceData<SawBlade>(sawBlade);
                                                    }
                                                }
                                                break;
                                            case ObjectInPath.Type.Blade:
                                                {
                                                    Blade blade = objectInPath as Blade;
                                                    if (objectInPathUIData != null && objectInPathUIData is BladeUI.UIData)
                                                    {
                                                        ((BladeUI.UIData)objectInPathUIData).blade.v = new ReferenceData<Blade>(blade);
                                                    }
                                                }
                                                break;
                                            case ObjectInPath.Type.FireNozzle:
                                                {
                                                    FireNozzle fireNozzle = objectInPath as FireNozzle;
                                                    if (objectInPathUIData != null && objectInPathUIData is FireNozzleUI.UIData)
                                                    {
                                                        ((FireNozzleUI.UIData)objectInPathUIData).fireNozzle.v = new ReferenceData<FireNozzle>(fireNozzle);
                                                    }
                                                }
                                                break;
                                            case ObjectInPath.Type.Pike:
                                                {
                                                    Pike pike = objectInPath as Pike;
                                                    if (objectInPathUIData != null && objectInPathUIData is PikeUI.UIData)
                                                    {
                                                        ((PikeUI.UIData)objectInPathUIData).pike.v = new ReferenceData<Pike>(pike);
                                                    }
                                                }
                                                break;
                                            case ObjectInPath.Type.EnergyOrbUpgrade:
                                                {
                                                    EnergyOrbUpgrade energyOrbUpgrade = objectInPath as EnergyOrbUpgrade;
                                                    if (objectInPathUIData != null && objectInPathUIData is EnergyOrbUpgradeUI.UIData)
                                                    {
                                                        ((EnergyOrbUpgradeUI.UIData)objectInPathUIData).energyOrbUpgrade.v = new ReferenceData<EnergyOrbUpgrade>(energyOrbUpgrade);
                                                    }
                                                }
                                                break;
                                            case ObjectInPath.Type.UpgradeGateFree:
                                                {
                                                    UpgradeGateFree upgradeGateFree = objectInPath as UpgradeGateFree;
                                                    if (objectInPathUIData != null && objectInPathUIData is UpgradeGateFreeUI.UIData)
                                                    {
                                                        ((UpgradeGateFreeUI.UIData)objectInPathUIData).upgradeGateFree.v = new ReferenceData<UpgradeGateFree>(upgradeGateFree);
                                                    }
                                                }
                                                break;
                                            case ObjectInPath.Type.UpgradeGateCharge:
                                                {
                                                    UpgradeGateCharge upgradeGateCharge = objectInPath as UpgradeGateCharge;
                                                    if (objectInPathUIData != null && objectInPathUIData is UpgradeGateChargeUI.UIData)
                                                    {
                                                        ((UpgradeGateChargeUI.UIData)objectInPathUIData).upgradeGateCharge.v = new ReferenceData<UpgradeGateCharge>(upgradeGateCharge);
                                                    }
                                                }
                                                break;
                                            case ObjectInPath.Type.Hammer:
                                                {
                                                    Hammer hammer = objectInPath as Hammer;
                                                    if (objectInPathUIData != null && objectInPathUIData is HammerUI.UIData)
                                                    {
                                                        ((HammerUI.UIData)objectInPathUIData).hammer.v = new ReferenceData<Hammer>(hammer);
                                                    }
                                                }
                                                break;
                                            case ObjectInPath.Type.Grinder:
                                                {
                                                    Grinder grinder = objectInPath as Grinder;
                                                    if (objectInPathUIData != null && objectInPathUIData is GrinderUI.UIData)
                                                    {
                                                        ((GrinderUI.UIData)objectInPathUIData).grinder.v = new ReferenceData<Grinder>(grinder);
                                                    }
                                                }
                                                break;
                                            case ObjectInPath.Type.EnergyOrbPower:
                                                {
                                                    EnergyOrbPower energyOrbPower = objectInPath as EnergyOrbPower;
                                                    if (objectInPathUIData != null && objectInPathUIData is EnergyOrbPowerUI.UIData)
                                                    {
                                                        ((EnergyOrbPowerUI.UIData)objectInPathUIData).energyOrbPower.v = new ReferenceData<EnergyOrbPower>(energyOrbPower);
                                                    }
                                                }
                                                break;
                                            case ObjectInPath.Type.CocoonMantah:
                                                {
                                                    CocoonMantah cocoonMantah = objectInPath as CocoonMantah;
                                                    if (objectInPathUIData != null && objectInPathUIData is CocoonMantahUI.UIData)
                                                    {
                                                        ((CocoonMantahUI.UIData)objectInPathUIData).cocoonMantah.v = new ReferenceData<CocoonMantah>(cocoonMantah);
                                                    }
                                                }
                                                break;
                                        }                                        
                                    }
                                    // add
                                    if (needAdd)
                                    {
                                        newAds.Add(objectInPathUIData);
                                    }
                                }
                                // newAds
                                this.data.objectInPaths.add(newAds);
                            }
                            // remove old
                            foreach (UIData.ObjectInPathUI old in olds)
                            {
                                this.data.objectInPaths.remove(old);
                            }
                        }*/
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

        public Transform arenaCanvas;

        #region implement callBacks

        public CameraUI cameraUI;

        public MainCanvasUI mainCanvasPrefab;
        public Transform mainCanvasContainer;

        public HeroUI heroUI;

        public Transform stateContainer;
        public LoadUI loadPrefab;
        public StartUI startPrefab;
        public PlayUI playPrefab;
        public EndUI endPrefab;
        public EditUI editPrefab;

        public PlayerInputUI playerInputPrefab;
        public Transform playerInputContainer;

        public CoinUI coinPrefab;
        public EnergyOrbNormalUI energyOrbNormalPrefab;
        public TroopCageUI troopCagePrefab;
        public SawBladeUI sawBladePrefab;
        public BladeUI bladePrefab;
        public FireNozzleUI fireNozzlePrefab;
        public PikeUI pikePrefab;
        public EnergyOrbUpgradeUI energyOrbUpgradePrefab;
        public UpgradeGateFreeUI upgradeGateFreePrefab;
        public UpgradeGateChargeUI upgradeGateChargePrefab;
        public HammerUI hammerPrefab;
        public GrinderUI grinderPrefab;
        public EnergyOrbPowerUI energyOrbPowerPrefab;
        public CocoonMantahUI cocoonMantahPrefab;

        public override void onAddCallBack<T>(T data)
        {
            if (data is UIData)
            {
                UIData uiData = data as UIData;
                // Child
                {
                    uiData.battleRush.allAddCallBack(this);
                    uiData.camera.allAddCallBack(this);
                    uiData.mainCanvas.allAddCallBack(this);
                    uiData.stateUI.allAddCallBack(this);
                    uiData.hero.allAddCallBack(this);
                    uiData.playerInput.allAddCallBack(this);
                    uiData.objectInPaths.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            {
                if (data is BattleRush)
                {
                    BattleRush battleRush = data as BattleRush;
                    // Update
                    {
                        UpdateUtils.makeUpdate<BattleRushUpdate, BattleRush>(battleRush, this.transform);
                    }
                    dirty = true;
                    return;
                }
                if(data is CameraUI.UIData)
                {
                    CameraUI.UIData cameraUIData = data as CameraUI.UIData;
                    // UI
                    {
                        if (cameraUI != null)
                        {
                            cameraUI.setData(cameraUIData);
                        }
                        else
                        {
                            Logger.LogError("cameraUI null");
                        }
                    }
                    dirty = true;
                    return;
                }
                if(data is MainCanvasUI.UIData)
                {
                    MainCanvasUI.UIData mainCanvasUIData = data as MainCanvasUI.UIData;
                    // UI
                    {
                        UIUtils.Instantiate(mainCanvasUIData, mainCanvasPrefab, mainCanvasContainer);
                    }
                    dirty = true;
                    return;
                }
                if(data is UIData.StateUI)
                {
                    UIData.StateUI stateUI = data as UIData.StateUI;
                    // UI
                    {
                        switch (stateUI.getType())
                        {
                            case BattleRush.State.Type.Load:
                                {
                                    LoadUI.UIData loadUIData = stateUI as LoadUI.UIData;
                                    UIUtils.Instantiate(loadUIData, loadPrefab, stateContainer);
                                }
                                break;
                            case BattleRush.State.Type.Start:
                                {
                                    StartUI.UIData startUIData = stateUI as StartUI.UIData;
                                    UIUtils.Instantiate(startUIData, startPrefab, stateContainer);
                                }
                                break;
                            case BattleRush.State.Type.Play:
                                {
                                    PlayUI.UIData playUIData = stateUI as PlayUI.UIData;
                                    UIUtils.Instantiate(playUIData, playPrefab, stateContainer);
                                }
                                break;
                            case BattleRush.State.Type.End:
                                {
                                    EndUI.UIData endUIData = stateUI as EndUI.UIData;
                                    UIUtils.Instantiate(endUIData, endPrefab, stateContainer);
                                }
                                break;
                            case BattleRush.State.Type.Edit:
                                {
                                    EditUI.UIData editUIData = stateUI as EditUI.UIData;
                                    UIUtils.Instantiate(editUIData, editPrefab, stateContainer);
                                }
                                break;
                            default:
                                Logger.LogError("unknown type: " + stateUI.getType());
                                break;
                        }
                    }
                    dirty = true;
                    return;
                }
                if(data is HeroUI.UIData)
                {
                    HeroUI.UIData heroUIData = data as HeroUI.UIData;
                    // UI
                    {
                        if (heroUI != null)
                        {
                            heroUI.setData(heroUIData);
                        }
                    }
                    dirty = true;
                    return;
                }
                if(data is PlayerInputUI.UIData)
                {
                    PlayerInputUI.UIData playerInputUIData = data as PlayerInputUI.UIData;
                    // UI
                    {
                        UIUtils.Instantiate(playerInputUIData, playerInputPrefab, playerInputContainer);
                    }
                    dirty = true;
                    return;
                }
                if(data is UIData.ObjectInPathUI)
                {
                    UIData.ObjectInPathUI objectInPathUI = data as UIData.ObjectInPathUI;
                    // UI
                    /*{
                        switch (objectInPathUI.getType())
                        {
                            case ObjectInPath.Type.OkgCoin:
                                {
                                    CoinUI.UIData coinUIData = objectInPathUI as CoinUI.UIData;
                                    UIUtils.Instantiate(coinUIData, coinPrefab, this.transform);
                                }
                                break;
                            case ObjectInPath.Type.EnergyOrbNormal:
                                {
                                    EnergyOrbNormalUI.UIData energyOrbNormalUIData = objectInPathUI as EnergyOrbNormalUI.UIData;
                                    UIUtils.Instantiate(energyOrbNormalUIData, energyOrbNormalPrefab, this.transform);
                                }
                                break;
                            case ObjectInPath.Type.TroopCage:
                                {
                                    TroopCageUI.UIData troopCageUIData = objectInPathUI as TroopCageUI.UIData;
                                    UIUtils.Instantiate(troopCageUIData, troopCagePrefab, this.transform);
                                }
                                break;
                            case ObjectInPath.Type.SawBlade:
                                {
                                    SawBladeUI.UIData sawBladeUIData = objectInPathUI as SawBladeUI.UIData;
                                    UIUtils.Instantiate(sawBladeUIData, sawBladePrefab, this.transform);
                                }
                                break;
                            case ObjectInPath.Type.Blade:
                                {
                                    BladeUI.UIData bladeUIData = objectInPathUI as BladeUI.UIData;
                                    UIUtils.Instantiate(bladeUIData, bladePrefab, this.transform);
                                }
                                break;
                            case ObjectInPath.Type.FireNozzle:
                                {
                                    FireNozzleUI.UIData fireNozzleUIData = objectInPathUI as FireNozzleUI.UIData;
                                    UIUtils.Instantiate(fireNozzleUIData, fireNozzlePrefab, this.transform);
                                }
                                break;
                            case ObjectInPath.Type.Pike:
                                {
                                    PikeUI.UIData pikeUIData = objectInPathUI as PikeUI.UIData;
                                    UIUtils.Instantiate(pikeUIData, pikePrefab, this.transform);
                                }
                                break;
                            case ObjectInPath.Type.EnergyOrbUpgrade:
                                {
                                    EnergyOrbUpgradeUI.UIData energyOrbUpgradeUIData = objectInPathUI as EnergyOrbUpgradeUI.UIData;
                                    UIUtils.Instantiate(energyOrbUpgradeUIData, energyOrbUpgradePrefab, this.transform);
                                }
                                break;
                            case ObjectInPath.Type.UpgradeGateFree:
                                {
                                    UpgradeGateFreeUI.UIData upgradeGateFreeUIData = objectInPathUI as UpgradeGateFreeUI.UIData;
                                    UIUtils.Instantiate(upgradeGateFreeUIData, upgradeGateFreePrefab, this.transform);
                                }
                                break;
                            case ObjectInPath.Type.UpgradeGateCharge:
                                {
                                    UpgradeGateChargeUI.UIData upgradeGateChargeUIData = objectInPathUI as UpgradeGateChargeUI.UIData;
                                    UIUtils.Instantiate(upgradeGateChargeUIData, upgradeGateChargePrefab, this.transform);
                                }
                                break;
                            case ObjectInPath.Type.Hammer:
                                {
                                    HammerUI.UIData hammerUIData = objectInPathUI as HammerUI.UIData;
                                    UIUtils.Instantiate(hammerUIData, hammerPrefab, this.transform);
                                }
                                break;
                            case ObjectInPath.Type.Grinder:
                                {
                                    GrinderUI.UIData grinderUIData = objectInPathUI as GrinderUI.UIData;
                                    UIUtils.Instantiate(grinderUIData, grinderPrefab, this.transform);
                                }
                                break;
                            case ObjectInPath.Type.EnergyOrbPower:
                                {
                                    EnergyOrbPowerUI.UIData energyOrbPowerUIData = objectInPathUI as EnergyOrbPowerUI.UIData;
                                    UIUtils.Instantiate(energyOrbPowerUIData, energyOrbPowerPrefab, this.transform);
                                }
                                break;
                            case ObjectInPath.Type.CocoonMantah:
                                {
                                    CocoonMantahUI.UIData cocoonMantahUIData = objectInPathUI as CocoonMantahUI.UIData;
                                    UIUtils.Instantiate(cocoonMantahUIData, cocoonMantahPrefab, this.transform);
                                }
                                break;
                        }
                    }*/
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
                    uiData.battleRush.allRemoveCallBack(this);
                    uiData.camera.allRemoveCallBack(this);
                    uiData.mainCanvas.allRemoveCallBack(this);
                    uiData.stateUI.allRemoveCallBack(this);
                    uiData.hero.allRemoveCallBack(this);
                    uiData.playerInput.allRemoveCallBack(this);
                    uiData.objectInPaths.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // Child
            {
                if (data is BattleRush)
                {
                    BattleRush battleRush = data as BattleRush;
                    // Update
                    {
                        battleRush.removeCallBackAndDestroy(typeof(BattleRushUpdate));
                    }
                    return;
                }
                if (data is CameraUI.UIData)
                {
                    CameraUI.UIData cameraUIData = data as CameraUI.UIData;
                    // UI
                    {
                        if (cameraUI != null)
                        {
                            cameraUI.setDataNull(cameraUIData);
                        }
                        else
                        {
                            Logger.LogError("cameraUI null");
                        }
                    }
                    return;
                }
                if (data is MainCanvasUI.UIData)
                {
                    MainCanvasUI.UIData mainCanvasUIData = data as MainCanvasUI.UIData;
                    // UI
                    {
                        mainCanvasUIData.removeCallBackAndDestroy(typeof(MainCanvasUI));
                    }
                    return;
                }
                if (data is UIData.StateUI)
                {
                    UIData.StateUI stateUI = data as UIData.StateUI;
                    // UI
                    {
                        switch (stateUI.getType())
                        {
                            case BattleRush.State.Type.Load:
                                {
                                    LoadUI.UIData loadUIData = stateUI as LoadUI.UIData;
                                    loadUIData.removeCallBackAndDestroy(typeof(LoadUI));
                                }
                                break;
                            case BattleRush.State.Type.Start:
                                {
                                    StartUI.UIData startUIData = stateUI as StartUI.UIData;
                                    startUIData.removeCallBackAndDestroy(typeof(StartUI));
                                }
                                break;
                            case BattleRush.State.Type.Play:
                                {
                                    PlayUI.UIData playUIData = stateUI as PlayUI.UIData;
                                    playUIData.removeCallBackAndDestroy(typeof(PlayUI));
                                }
                                break;
                            case BattleRush.State.Type.End:
                                {
                                    EndUI.UIData endUIData = stateUI as EndUI.UIData;
                                    endUIData.removeCallBackAndDestroy(typeof(EndUI));
                                }
                                break;
                            case BattleRush.State.Type.Edit:
                                {
                                    EditUI.UIData editUIData = stateUI as EditUI.UIData;
                                    editUIData.removeCallBackAndDestroy(typeof(EditUI));
                                }
                                break;
                            default:
                                Logger.LogError("unknown type: " + stateUI.getType());
                                break;
                        }
                    }
                    return;
                }
                if (data is HeroUI.UIData)
                {
                    HeroUI.UIData heroUIData = data as HeroUI.UIData;
                    // UI
                    {
                        if (heroUI != null)
                        {
                            heroUI.setDataNull(heroUIData);
                        }
                    }
                    return;
                }
                if (data is PlayerInputUI.UIData)
                {
                    PlayerInputUI.UIData playerInputUIData = data as PlayerInputUI.UIData;
                    // UI
                    {
                        playerInputUIData.removeCallBackAndDestroy(typeof(PlayerInputUI));
                    }
                    return;
                }
                if (data is UIData.ObjectInPathUI)
                {
                    UIData.ObjectInPathUI objectInPathUI = data as UIData.ObjectInPathUI;
                    // UI
                    /*{
                        switch (objectInPathUI.getType())
                        {
                            case ObjectInPath.Type.OkgCoin:
                                {
                                    CoinUI.UIData coinUIData = objectInPathUI as CoinUI.UIData;
                                    coinUIData.removeCallBackAndDestroy(typeof(CoinUI));
                                }
                                break;
                            case ObjectInPath.Type.EnergyOrbNormal:
                                {
                                    EnergyOrbNormalUI.UIData energyOrbNormalUIData = objectInPathUI as EnergyOrbNormalUI.UIData;
                                    energyOrbNormalUIData.removeCallBackAndDestroy(typeof(EnergyOrbNormalUI));
                                }
                                break;
                            case ObjectInPath.Type.TroopCage:
                                {
                                    TroopCageUI.UIData troopCageUIData = objectInPathUI as TroopCageUI.UIData;
                                    troopCageUIData.removeCallBackAndDestroy(typeof(TroopCageUI));
                                }
                                break;
                            case ObjectInPath.Type.SawBlade:
                                {
                                    SawBladeUI.UIData sawBladeUIData = objectInPathUI as SawBladeUI.UIData;
                                    sawBladeUIData.removeCallBackAndDestroy(typeof(SawBladeUI));
                                }
                                break;
                            case ObjectInPath.Type.Blade:
                                {
                                    BladeUI.UIData bladeUIData = objectInPathUI as BladeUI.UIData;
                                    bladeUIData.removeCallBackAndDestroy(typeof(BladeUI));
                                }
                                break;
                            case ObjectInPath.Type.FireNozzle:
                                {
                                    FireNozzleUI.UIData fireNozzleUIData = objectInPathUI as FireNozzleUI.UIData;
                                    fireNozzleUIData.removeCallBackAndDestroy(typeof(FireNozzleUI));
                                }
                                break;
                            case ObjectInPath.Type.Pike:
                                {
                                    PikeUI.UIData pikeUIData = objectInPathUI as PikeUI.UIData;
                                    pikeUIData.removeCallBackAndDestroy(typeof(PikeUI));
                                }
                                break;
                            case ObjectInPath.Type.EnergyOrbUpgrade:
                                {
                                    EnergyOrbUpgradeUI.UIData energyOrbUpgradeUIData = objectInPathUI as EnergyOrbUpgradeUI.UIData;
                                    energyOrbUpgradeUIData.removeCallBackAndDestroy(typeof(EnergyOrbUpgradeUI));
                                }
                                break;
                            case ObjectInPath.Type.UpgradeGateFree:
                                {
                                    UpgradeGateFreeUI.UIData upgradeGateFreeUIData = objectInPathUI as UpgradeGateFreeUI.UIData;
                                    upgradeGateFreeUIData.removeCallBackAndDestroy(typeof(UpgradeGateFreeUI));
                                }
                                break;
                            case ObjectInPath.Type.UpgradeGateCharge:
                                {
                                    UpgradeGateChargeUI.UIData upgradeGateChargeUIData = objectInPathUI as UpgradeGateChargeUI.UIData;
                                    upgradeGateChargeUIData.removeCallBackAndDestroy(typeof(UpgradeGateChargeUI));
                                }
                                break;
                            case ObjectInPath.Type.Hammer:
                                {
                                    HammerUI.UIData hammerUIData = objectInPathUI as HammerUI.UIData;
                                    hammerUIData.removeCallBackAndDestroy(typeof(HammerUI));
                                }
                                break;
                            case ObjectInPath.Type.Grinder:
                                {
                                    GrinderUI.UIData grinderUIData = objectInPathUI as GrinderUI.UIData;
                                    grinderUIData.removeCallBackAndDestroy(typeof(GrinderUI));
                                }
                                break;
                            case ObjectInPath.Type.EnergyOrbPower:
                                {
                                    EnergyOrbPowerUI.UIData energyOrbPowerUIData = objectInPathUI as EnergyOrbPowerUI.UIData;
                                    energyOrbPowerUIData.removeCallBackAndDestroy(typeof(EnergyOrbPowerUI));
                                }
                                break;
                            case ObjectInPath.Type.CocoonMantah:
                                {
                                    CocoonMantahUI.UIData cocoonMantahUIData = objectInPathUI as CocoonMantahUI.UIData;
                                    cocoonMantahUIData.removeCallBackAndDestroy(typeof(CocoonMantahUI));
                                }
                                break;
                        }
                    }*/
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
                    case UIData.Property.battleRush:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case UIData.Property.camera:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case UIData.Property.mainCanvas:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case UIData.Property.hero:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case UIData.Property.stateUI:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case UIData.Property.playerInput:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case UIData.Property.objectInPaths:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    default:
                        Logger.LogError("Don't process: " + wrapProperty + "; " + this);
                        break;
                }
                return;
            }
            // Child
            {
                if (wrapProperty.p is BattleRush)
                {
                    switch ((BattleRush.Property)wrapProperty.n)
                    {
                        case BattleRush.Property.state:
                            dirty = true;
                            break;
                        case BattleRush.Property.mapData:
                            break;
                        case BattleRush.Property.hero:
                            dirty = true;
                            break;
                        case BattleRush.Property.laneObjects:
                            dirty = true;
                            break;
                        default:
                            //Logger.LogError("Don't process: " + wrapProperty + "; " + this);
                            break;
                    }
                    return;
                }
                if (wrapProperty.p is CameraUI.UIData)
                {
                    return;
                }
                if (wrapProperty.p is MainCanvasUI.UIData)
                {
                    return;
                }
                if (wrapProperty.p is UIData.StateUI)
                {
                    return;
                }
                if (wrapProperty.p is HeroUI.UIData)
                {
                    return;
                }
                if(wrapProperty.p is PlayerInputUI.UIData)
                {
                    return;
                }
                if (wrapProperty.p is UIData.ObjectInPathUI)
                {
                    return;
                }
            }
            Logger.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

        public Transform tempSegmentContainer;

    }
}
