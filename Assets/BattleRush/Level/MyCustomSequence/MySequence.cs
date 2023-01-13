using System.Collections;
using System.Collections.Generic;
using BattleRushS.HeroS;
using BattleRushS.ObjectS;
using Dreamteck.Forever;
using UnityEngine;

namespace BattleRushS
{
    public class MySequence : CustomSequence
    {

        #region segment

        public GameObject arenaSegment;

        #endregion

        #region battleRush

        private BattleRush battleRush;

        public void setBattleRush(BattleRush battleRush)
        {
            this.battleRush = battleRush;
        }

        #endregion

        public override void Initialize()
        {
            isDone = false;
        }

        public override GameObject Next()
        {
            Logger.Log("Choose next sequence");
            GameObject ret = null;
            if (battleRush != null)
            {                
                BattleRushUI battleRushUI = battleRush.findCallBack<BattleRushUI>();
                if (battleRushUI != null && battleRushUI.data!=null)
                {
                    // find
                    bool isArena = false;
                    {
                        if (battleRush.makeSegmentManager.v.mapAsset.v != null)
                        {
                            if (!isArena)
                            {
                                if (battleRush.makeSegmentManager.v.index.v >= battleRush.makeSegmentManager.v.mapAsset.v.segments.Count)
                                {
                                    Logger.Log("mySequence is arena: " + battleRush.makeSegmentManager.v.index.v + ", " + battleRush.makeSegmentManager.v.mapAsset.v.segments.Count);
                                    isArena = true;
                                }
                            }
                        }
                        else
                        {
                            Logger.LogError("mapAsset null");
                        }
                    }
                    // process
                    if (isArena)
                    {
                        ret = arenaSegment;
                        // TODO dung sinh khi da sinh arena
                        stopped = true;
                    }
                    else
                    {
                        // find segment prefab
                        Segment segmentPrefab = null;
                        {
                            // get by index
                            {
                                if (battleRush.makeSegmentManager.v.index.v >= 0 && battleRush.makeSegmentManager.v.index.v < battleRush.makeSegmentManager.v.mapAsset.v.segments.Count)
                                {
                                    SegmentAsset mapAsset = battleRush.makeSegmentManager.v.mapAsset.v.segments[battleRush.makeSegmentManager.v.index.v];
                                    segmentPrefab = mapAsset.segment;
                                }
                                // next
                                battleRush.makeSegmentManager.v.nextMakePrefab();
                            }
                            // prevent null
                            if (segmentPrefab == null)
                            {
                                segmentPrefab = battleRush.makeSegmentManager.v.mapAsset.v.defaultSegment;
                            }
                        }
                        ret = Instantiate(segmentPrefab.gameObject, battleRushUI.tempSegmentContainer);
                        LevelSegment levelSegment = ret.GetComponent<LevelSegment>();
                        if (levelSegment != null)
                        {
                            // find all object data in segment
                            List<ObjectData> objectDatas = new List<ObjectData>();
                            float segmentPosFrom = battleRush.makeSegmentManager.v.currentLength.v;
                            float segmentPosDes = battleRush.makeSegmentManager.v.currentLength.v;
                            {
                                // find segment position z                                
                                {
                                    Segment segment = levelSegment.GetComponent<Segment>();
                                    if (segment != null)
                                    {
                                        segmentPosDes = battleRush.makeSegmentManager.v.currentLength.v + segment.length;
                                        segment.segmentPos = segmentPosFrom;
                                    }
                                    else
                                    {
                                        Logger.LogError("segment null");
                                    }
                                }
                                // process
                                for (int i = 0; i < battleRush.mapData.v.objectDatas.vs.Count; i++)
                                {
                                    ObjectData objectData = battleRush.mapData.v.objectDatas.vs[i];
                                    // Logger.Log("MySequence: objectData check position: " + objectData.position.v.z + ", " + segmentPosFrom + ", " + segmentPosDes);
                                    if (objectData.position.v.z > segmentPosFrom && objectData.position.v.z <= segmentPosDes)
                                    {
                                        objectDatas.Add(objectData);
                                    }
                                }
                                // set current length for levelGenerator
                                battleRush.makeSegmentManager.v.currentLength.v = segmentPosDes;
                            }
                            // process
                            {
                                foreach(ObjectData objectData in objectDatas)
                                {
                                    // make
                                    Transform objectUI = null;
                                    ObjectInPath objectInPath;
                                    // process
                                    MakeObjectInPathDataAndUIFromObjectData(objectData, battleRush, levelSegment, out objectUI, out objectInPath);
                                    if (objectUI != null && objectInPath != null)
                                    {
                                        // add component
                                        {
                                            AddObjectToBattleRushUI addObjectToBattleRushUI = objectUI.gameObject.AddComponent<AddObjectToBattleRushUI>();
                                            addObjectToBattleRushUI.objectDataId = objectInPath.uid;
                                        }
                                        // set position
                                        {
                                            if (objectUI != null)
                                            {
                                                float x = objectData.position.v.x;
                                                float z = objectData.position.v.z - (segmentPosFrom + (segmentPosDes - segmentPosFrom) / 2.0f);
                                                Logger.Log("z local: " + z);
                                                // set
                                                objectUI.localPosition = new Vector3(x, 1, z);
                                            }
                                            else
                                            {
                                                Logger.LogError("coinUI null");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Logger.LogError("not create any thing");
                                    }
                                }
                                levelSegment.UpdateReferences();
                            }

                            /**
                             * cac object them vao can them segmentobjectsetting, chon bound no di, de tranh choan qua segment
                             * */ 
                        }
                        else
                        {
                            Logger.LogError("levelSegment null");
                        }
                    }                                   
                }
                else
                {
                    Logger.LogError("battleRushUI null");
                }
            }
            return ret;
        }

        public override void Stop()
        {
            base.stopped = true;
        }

        public static void MakeObjectInPathDataAndUIFromObjectData(ObjectData objectData, BattleRush battleRush, LevelSegment levelSegment, out Transform objectUI, out ObjectInPath objectInPath)
        {
            // init
            objectInPath = null;
            objectUI = null;
            // make
            BattleRushUI battleRushUI = battleRush.findCallBack<BattleRushUI>();
            if (battleRushUI != null)
            {
                {
                    switch (objectData.I.v)
                    {
                        case ObjectInPath.Type.OkgCoin:
                            {
                                // make data
                                Coin coin = new Coin();
                                {
                                    coin.uid = battleRush.laneObjects.makeId();
                                    coin.position.v = objectData.position.v;
                                }
                                objectInPath = coin;
                                battleRush.laneObjects.add(coin);
                                // make UI
                                {
                                    CoinUI.UIData objectUIData = new CoinUI.UIData();
                                    {
                                        objectUIData.coin.v = new ReferenceData<Coin>(coin);
                                    }
                                    battleRushUI.data.objectInPaths.add(objectUIData);
                                    objectUI = Instantiate(battleRushUI.coinPrefab, levelSegment.transform).transform;
                                }
                            }
                            break;
                        case ObjectInPath.Type.CocoonMantah:// "cocoon_mantah":
                            {
                                Logger.LogWarning("create cocoon mantah");
                                CocoonMantah cocoonMantah = new CocoonMantah();
                                {
                                    cocoonMantah.uid = battleRush.laneObjects.makeId();
                                    cocoonMantah.position.v = objectData.position.v;
                                }
                                objectInPath = cocoonMantah;
                                battleRush.laneObjects.add(cocoonMantah);
                                // make UI
                                {
                                    CocoonMantahUI.UIData objectUIData = new CocoonMantahUI.UIData();
                                    {
                                        objectUIData.cocoonMantah.v = new ReferenceData<CocoonMantah>(cocoonMantah);
                                    }
                                    battleRushUI.data.objectInPaths.add(objectUIData);
                                    objectUI = Instantiate(battleRushUI.cocoonMantahPrefab, levelSegment.transform).transform;
                                }
                            }
                            break;
                        case ObjectInPath.Type.EnergyOrbNormal:
                            {
                                EnergyOrbNormal energyOrbNormal = new EnergyOrbNormal();
                                {
                                    energyOrbNormal.uid = battleRush.laneObjects.makeId();
                                    energyOrbNormal.position.v = objectData.position.v;
                                }
                                objectInPath = energyOrbNormal;
                                battleRush.laneObjects.add(energyOrbNormal);
                                // make UI
                                {
                                    EnergyOrbNormalUI.UIData objectUIData = new EnergyOrbNormalUI.UIData();
                                    {
                                        objectUIData.energyOrbNormal.v = new ReferenceData<EnergyOrbNormal>(energyOrbNormal);
                                    }
                                    battleRushUI.data.objectInPaths.add(objectUIData);
                                    objectUI = Instantiate(battleRushUI.energyOrbNormalPrefab, levelSegment.transform).transform;
                                }
                            }
                            break;

                        case ObjectInPath.Type.TroopCage:
                            {
                                Logger.LogWarning("make troop cage");
                                TroopCage troopCage = new TroopCage();
                                {
                                    troopCage.uid = battleRush.laneObjects.makeId();
                                    troopCage.position.v = objectData.position.v;
                                    // troop follows
                                    {
                                        int troopNumber = Random.Range(5, 15);// 10;// Random.Range(1, 5)
                                        for (int troopFollowIndex = 0; troopFollowIndex < troopNumber; troopFollowIndex++)
                                        {
                                            TroopFollow troopFollow = new TroopFollow();
                                            {
                                                troopFollow.uid = troopCage.troops.makeId();
                                                // troopType
                                                {
                                                    // TODO tam de random
                                                    troopFollow.troopType.v = battleRushUI.troopInformations[Random.Range(0, battleRushUI.troopInformations.Count)];
                                                }
                                                // level
                                                {
                                                    // TODO de tam random
                                                    troopFollow.level.v = Random.Range(1, 3);
                                                }
                                                // TODO can hoan thien them thong tin
                                                {

                                                }
                                            }
                                            troopCage.troops.add(troopFollow);
                                        }
                                    }
                                }
                                objectInPath = troopCage;
                                battleRush.laneObjects.add(troopCage);
                                // make UI
                                {
                                    TroopCageUI.UIData objectUIData = new TroopCageUI.UIData();
                                    {
                                        objectUIData.troopCage.v = new ReferenceData<TroopCage>(troopCage);
                                    }
                                    battleRushUI.data.objectInPaths.add(objectUIData);
                                    objectUI = Instantiate(battleRushUI.troopCagePrefab, levelSegment.transform).transform;
                                }
                            }
                            break;
                        case ObjectInPath.Type.SawBlade:
                            {
                                SawBlade sawBlade = new SawBlade();
                                {
                                    sawBlade.uid = battleRush.laneObjects.makeId();
                                    sawBlade.position.v = objectData.position.v;
                                }
                                objectInPath = sawBlade;
                                battleRush.laneObjects.add(sawBlade);
                                // make UI
                                {
                                    SawBladeUI.UIData objectUIData = new SawBladeUI.UIData();
                                    {
                                        objectUIData.sawBlade.v = new ReferenceData<SawBlade>(sawBlade);
                                    }
                                    battleRushUI.data.objectInPaths.add(objectUIData);
                                    objectUI = Instantiate(battleRushUI.sawBladePrefab, levelSegment.transform).transform;
                                }
                            }
                            break;
                        case ObjectInPath.Type.Blade:
                            {
                                Blade blade = new Blade();
                                {
                                    blade.uid = battleRush.laneObjects.makeId();
                                    blade.position.v = objectData.position.v;
                                }
                                objectInPath = blade;
                                battleRush.laneObjects.add(blade);
                                // make UI
                                {
                                    BladeUI.UIData objectUIData = new BladeUI.UIData();
                                    {
                                        objectUIData.blade.v = new ReferenceData<Blade>(blade);
                                    }
                                    battleRushUI.data.objectInPaths.add(objectUIData);
                                    objectUI = Instantiate(battleRushUI.bladePrefab, levelSegment.transform).transform;
                                }
                            }
                            break;
                        case ObjectInPath.Type.FireNozzle:
                            {
                                FireNozzle fireNozzle = new FireNozzle();
                                {
                                    fireNozzle.uid = battleRush.laneObjects.makeId();
                                    fireNozzle.position.v = objectData.position.v;
                                }
                                objectInPath = fireNozzle;
                                battleRush.laneObjects.add(fireNozzle);
                                // make UI
                                {
                                    FireNozzleUI.UIData objectUIData = new FireNozzleUI.UIData();
                                    {
                                        objectUIData.fireNozzle.v = new ReferenceData<FireNozzle>(fireNozzle);
                                    }
                                    battleRushUI.data.objectInPaths.add(objectUIData);
                                    objectUI = Instantiate(battleRushUI.fireNozzlePrefab, levelSegment.transform).transform;
                                }
                            }
                            break;
                        case ObjectInPath.Type.Pike:
                            {
                                Pike pike = new Pike();
                                {
                                    pike.uid = battleRush.laneObjects.makeId();
                                    pike.position.v = objectData.position.v;
                                }
                                objectInPath = pike;
                                battleRush.laneObjects.add(pike);
                                // make UI
                                {
                                    PikeUI.UIData objectUIData = new PikeUI.UIData();
                                    {
                                        objectUIData.pike.v = new ReferenceData<Pike>(pike);
                                    }
                                    battleRushUI.data.objectInPaths.add(objectUIData);
                                    objectUI = Instantiate(battleRushUI.pikePrefab, levelSegment.transform).transform;
                                }
                            }
                            break;
                        case ObjectInPath.Type.EnergyOrbUpgrade:
                            {
                                EnergyOrbUpgrade energyOrbUpgrade = new EnergyOrbUpgrade();
                                {
                                    energyOrbUpgrade.uid = battleRush.laneObjects.makeId();
                                    energyOrbUpgrade.position.v = objectData.position.v;
                                }
                                objectInPath = energyOrbUpgrade;
                                battleRush.laneObjects.add(energyOrbUpgrade);
                                // make UI
                                {
                                    EnergyOrbUpgradeUI.UIData objectUIData = new EnergyOrbUpgradeUI.UIData();
                                    {
                                        objectUIData.energyOrbUpgrade.v = new ReferenceData<EnergyOrbUpgrade>(energyOrbUpgrade);
                                    }
                                    battleRushUI.data.objectInPaths.add(objectUIData);
                                    objectUI = Instantiate(battleRushUI.energyOrbUpgradePrefab, levelSegment.transform).transform;
                                }
                            }
                            break;
                        case ObjectInPath.Type.UpgradeGateFree:
                            {
                                UpgradeGateFree upgradeGateFree = new UpgradeGateFree();
                                {
                                    upgradeGateFree.uid = battleRush.laneObjects.makeId();
                                    upgradeGateFree.position.v = objectData.position.v;
                                }
                                objectInPath = upgradeGateFree;
                                battleRush.laneObjects.add(upgradeGateFree);
                                // make UI
                                {
                                    UpgradeGateFreeUI.UIData objectUIData = new UpgradeGateFreeUI.UIData();
                                    {
                                        objectUIData.upgradeGateFree.v = new ReferenceData<UpgradeGateFree>(upgradeGateFree);
                                    }
                                    battleRushUI.data.objectInPaths.add(objectUIData);
                                    objectUI = Instantiate(battleRushUI.upgradeGateFreePrefab, levelSegment.transform).transform;
                                }
                            }
                            break;
                        case ObjectInPath.Type.UpgradeGateCharge:
                            {
                                UpgradeGateCharge upgradeGateCharge = new UpgradeGateCharge();
                                {
                                    upgradeGateCharge.uid = battleRush.laneObjects.makeId();
                                    upgradeGateCharge.position.v = objectData.position.v;
                                }
                                objectInPath = upgradeGateCharge;
                                battleRush.laneObjects.add(upgradeGateCharge);
                                // make UI
                                {
                                    UpgradeGateChargeUI.UIData objectUIData = new UpgradeGateChargeUI.UIData();
                                    {
                                        objectUIData.upgradeGateCharge.v = new ReferenceData<UpgradeGateCharge>(upgradeGateCharge);
                                    }
                                    battleRushUI.data.objectInPaths.add(objectUIData);
                                    objectUI = Instantiate(battleRushUI.upgradeGateChargePrefab, levelSegment.transform).transform;
                                }
                            }
                            break;
                        case ObjectInPath.Type.Hammer:
                            {
                                Hammer hammer = new Hammer();
                                {
                                    hammer.uid = battleRush.laneObjects.makeId();
                                    hammer.position.v = objectData.position.v;
                                }
                                objectInPath = hammer;
                                battleRush.laneObjects.add(hammer);
                                // make UI
                                {
                                    HammerUI.UIData objectUIData = new HammerUI.UIData();
                                    {
                                        objectUIData.hammer.v = new ReferenceData<Hammer>(hammer);
                                    }
                                    battleRushUI.data.objectInPaths.add(objectUIData);
                                    objectUI = Instantiate(battleRushUI.hammerPrefab, levelSegment.transform).transform;
                                }
                            }
                            break;
                        case ObjectInPath.Type.Grinder:
                            {
                                Grinder grinder = new Grinder();
                                {
                                    grinder.uid = battleRush.laneObjects.makeId();
                                    grinder.position.v = objectData.position.v;
                                }
                                objectInPath = grinder;
                                battleRush.laneObjects.add(grinder);
                                // make UI
                                {
                                    GrinderUI.UIData objectUIData = new GrinderUI.UIData();
                                    {
                                        objectUIData.grinder.v = new ReferenceData<Grinder>(grinder);
                                    }
                                    battleRushUI.data.objectInPaths.add(objectUIData);
                                    objectUI = Instantiate(battleRushUI.grinderPrefab, levelSegment.transform).transform;
                                }
                            }
                            break;
                        case ObjectInPath.Type.EnergyOrbPower:
                            {
                                EnergyOrbPower energyOrbPower = new EnergyOrbPower();
                                {
                                    energyOrbPower.uid = battleRush.laneObjects.makeId();
                                    energyOrbPower.position.v = objectData.position.v;
                                }
                                objectInPath = energyOrbPower;
                                battleRush.laneObjects.add(energyOrbPower);
                                // make UI
                                {
                                    EnergyOrbPowerUI.UIData objectUIData = new EnergyOrbPowerUI.UIData();
                                    {
                                        objectUIData.energyOrbPower.v = new ReferenceData<EnergyOrbPower>(energyOrbPower);
                                    }
                                    battleRushUI.data.objectInPaths.add(objectUIData);
                                    objectUI = Instantiate(battleRushUI.energyOrbPowerPrefab, levelSegment.transform).transform;
                                }
                            }
                            break;
                        default:
                            {
                                Logger.LogError("unknown object data: " + objectData.I.v);                                
                            }                           
                            break;
                    }
                }
            }
            else
            {
                Logger.LogError("battleRushUI null");
            }
        }

    }
}
