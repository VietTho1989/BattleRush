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

        public GameObject[] segments;
        public GameObject arenaSegment;

        #endregion

        #region battleRush

        private BattleRush battleRush;

        public void setBattleRush(BattleRush battleRush)
        {
            this.battleRush = battleRush;
        }

        #endregion

        public override GameObject[] GetAllSegments()
        {
            return segments;
        }

        public override void Initialize()
        {
            isDone = false;
        }

        public override GameObject Next()
        {
            Logger.Log("Choose next sequence");
            GameObject ret = segments[0];
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
                            if(battleRush.makeSegmentManager.v.assetIndex.v>= battleRush.makeSegmentManager.v.mapAsset.v.segments.Count)
                            {
                                isArena = true;
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
                        ret = Instantiate(segments[0], battleRushUI.tempSegmentContainer);
                        LevelSegment levelSegment = ret.GetComponent<LevelSegment>();
                        if (levelSegment != null)
                        {
                            /**
                             * cac object them vao can them segmentobjectsetting, chon bound no di, de tranh choan qua segment
                             * */ 
                            // add object to segment
                            {
                                int lastAddIndex = battleRush.mapData.v.lastAddIndex.v;
                                {
                                    for (int i = battleRush.mapData.v.lastAddIndex.v + 1; i < battleRush.mapData.v.objectDatas.vs.Count; i++)
                                    {
                                        ObjectData objectData = battleRush.mapData.v.objectDatas.vs[i];
                                        // check is in segment or not
                                        bool isInSegment = false;
                                        {
                                            Logger.Log("check object is in segment or not: " + objectData.position.v + ", " + battleRush.makeSegmentManager.v.currentNextSegmentIndex.v);
                                            if (objectData.position.v.z > battleRush.makeSegmentManager.v.currentNextSegmentIndex.v && objectData.position.v.z <= (battleRush.makeSegmentManager.v.currentNextSegmentIndex.v + 1))
                                            {
                                                isInSegment = true;
                                            }
                                        }
                                        // process
                                        if (isInSegment)
                                        {
                                            lastAddIndex = i;
                                            Logger.Log("create object: " + objectData.I.v + ", " + objectData.position.v);
                                            // make data and UI
                                            Transform objectUI = null;
                                            {
                                                switch (objectData.I.v)
                                                {
                                                    case "okg_coin":
                                                        {
                                                            // make data
                                                            Coin coin = new Coin();
                                                            {
                                                                coin.uid = battleRush.laneObjects.makeId();
                                                                coin.position.v = objectData.position.v;
                                                            }
                                                            battleRush.laneObjects.add(coin);
                                                            // make UI
                                                            {
                                                                CoinUI.UIData objectUIData = new CoinUI.UIData();
                                                                {
                                                                    objectUIData.coin.v = new ReferenceData<Coin>(coin);
                                                                }
                                                                battleRushUI.data.objectInPaths.add(objectUIData);
                                                                objectUI = Instantiate(battleRushUI.coinPrefab, ret.transform).transform;
                                                                // add component
                                                                {
                                                                    AddObjectToBattleRushUI addObjectToBattleRushUI = objectUI.gameObject.AddComponent<AddObjectToBattleRushUI>();
                                                                    addObjectToBattleRushUI.objectDataId = coin.uid;
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    case "cocoon_mantah":// "cocoon_mantah":
                                                        {
                                                            Logger.LogWarning("create cocoon mantah");
                                                            CocoonMantah cocoonMantah = new CocoonMantah();
                                                            {
                                                                cocoonMantah.uid = battleRush.laneObjects.makeId();
                                                                cocoonMantah.position.v = objectData.position.v;
                                                            }
                                                            battleRush.laneObjects.add(cocoonMantah);
                                                            // make UI
                                                            {
                                                                CocoonMantahUI.UIData objectUIData = new CocoonMantahUI.UIData();
                                                                {
                                                                    objectUIData.cocoonMantah.v = new ReferenceData<CocoonMantah>(cocoonMantah);
                                                                }
                                                                battleRushUI.data.objectInPaths.add(objectUIData);
                                                                objectUI = Instantiate(battleRushUI.cocoonMantahPrefab, ret.transform).transform;
                                                                // add component
                                                                {
                                                                    AddObjectToBattleRushUI addObjectToBattleRushUI = objectUI.gameObject.AddComponent<AddObjectToBattleRushUI>();
                                                                    addObjectToBattleRushUI.objectDataId = cocoonMantah.uid;
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    case "energy_orb_normal":
                                                        {
                                                            EnergyOrbNormal energyOrbNormal = new EnergyOrbNormal();
                                                            {
                                                                energyOrbNormal.uid = battleRush.laneObjects.makeId();
                                                                energyOrbNormal.position.v = objectData.position.v;
                                                            }
                                                            battleRush.laneObjects.add(energyOrbNormal);
                                                            // make UI
                                                            {
                                                                EnergyOrbNormalUI.UIData objectUIData = new EnergyOrbNormalUI.UIData();
                                                                {
                                                                    objectUIData.energyOrbNormal.v = new ReferenceData<EnergyOrbNormal>(energyOrbNormal);
                                                                }
                                                                battleRushUI.data.objectInPaths.add(objectUIData);
                                                                objectUI = Instantiate(battleRushUI.energyOrbNormalPrefab, ret.transform).transform;
                                                                // add component
                                                                {
                                                                    AddObjectToBattleRushUI addObjectToBattleRushUI = objectUI.gameObject.AddComponent<AddObjectToBattleRushUI>();
                                                                    addObjectToBattleRushUI.objectDataId = energyOrbNormal.uid;
                                                                }
                                                            }
                                                        }
                                                        break;

                                                    case "troop_cage_1":
                                                    case "troop_cage_2":
                                                    case "troop_cage_3":
                                                        {
                                                            Logger.LogWarning("make troop cage");
                                                            TroopCage troopCage = new TroopCage();
                                                            {
                                                                troopCage.uid = battleRush.laneObjects.makeId();
                                                                troopCage.position.v = objectData.position.v;
                                                                // troop follows
                                                                {
                                                                    int troopNumber = Random.Range(5, 15);// 10;// Random.Range(1, 5)
                                                                    for (int troopFollowIndex=0; troopFollowIndex < troopNumber; troopFollowIndex++)
                                                                    {
                                                                        TroopFollow troopFollow = new TroopFollow();
                                                                        {
                                                                            troopFollow.uid = troopCage.troops.makeId();
                                                                            // troopType
                                                                            {
                                                                                System.Array values = System.Enum.GetValues(typeof(TroopFollow.TroopType));
                                                                                troopFollow.troopType.v = (TroopFollow.TroopType)values.GetValue(Random.Range(0, values.Length));
                                                                            }
                                                                            // TODO can hoan thien them thong tin
                                                                            {

                                                                            }
                                                                        }
                                                                        troopCage.troops.add(troopFollow);
                                                                    }
                                                                }
                                                            }
                                                            battleRush.laneObjects.add(troopCage);
                                                            // make UI
                                                            {
                                                                TroopCageUI.UIData objectUIData = new TroopCageUI.UIData();
                                                                {
                                                                    objectUIData.troopCage.v = new ReferenceData<TroopCage>(troopCage);
                                                                }
                                                                battleRushUI.data.objectInPaths.add(objectUIData);
                                                                objectUI = Instantiate(battleRushUI.troopCagePrefab, ret.transform).transform;
                                                                // add component
                                                                {
                                                                    AddObjectToBattleRushUI addObjectToBattleRushUI = objectUI.gameObject.AddComponent<AddObjectToBattleRushUI>();
                                                                    addObjectToBattleRushUI.objectDataId = troopCage.uid;
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    case "saw_blade":
                                                        {
                                                            SawBlade sawBlade = new SawBlade();
                                                            {
                                                                sawBlade.uid = battleRush.laneObjects.makeId();
                                                                sawBlade.position.v = objectData.position.v;
                                                            }
                                                            battleRush.laneObjects.add(sawBlade);
                                                            // make UI
                                                            {
                                                                SawBladeUI.UIData objectUIData = new SawBladeUI.UIData();
                                                                {
                                                                    objectUIData.sawBlade.v = new ReferenceData<SawBlade>(sawBlade);
                                                                }
                                                                battleRushUI.data.objectInPaths.add(objectUIData);
                                                                objectUI = Instantiate(battleRushUI.sawBladePrefab, ret.transform).transform;
                                                                // add component
                                                                {
                                                                    AddObjectToBattleRushUI addObjectToBattleRushUI = objectUI.gameObject.AddComponent<AddObjectToBattleRushUI>();
                                                                    addObjectToBattleRushUI.objectDataId = sawBlade.uid;
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    case "blade":
                                                        {
                                                            Blade blade = new Blade();
                                                            {
                                                                blade.uid = battleRush.laneObjects.makeId();
                                                                blade.position.v = objectData.position.v;
                                                            }
                                                            battleRush.laneObjects.add(blade);
                                                            // make UI
                                                            {
                                                                BladeUI.UIData objectUIData = new BladeUI.UIData();
                                                                {
                                                                    objectUIData.blade.v = new ReferenceData<Blade>(blade);
                                                                }
                                                                battleRushUI.data.objectInPaths.add(objectUIData);
                                                                objectUI = Instantiate(battleRushUI.bladePrefab, ret.transform).transform;
                                                                // add component
                                                                {
                                                                    AddObjectToBattleRushUI addObjectToBattleRushUI = objectUI.gameObject.AddComponent<AddObjectToBattleRushUI>();
                                                                    addObjectToBattleRushUI.objectDataId = blade.uid;
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    case "fire_nozzle":
                                                        {
                                                            FireNozzle fireNozzle = new FireNozzle();
                                                            {
                                                                fireNozzle.uid = battleRush.laneObjects.makeId();
                                                                fireNozzle.position.v = objectData.position.v;
                                                            }
                                                            battleRush.laneObjects.add(fireNozzle);
                                                            // make UI
                                                            {
                                                                FireNozzleUI.UIData objectUIData = new FireNozzleUI.UIData();
                                                                {
                                                                    objectUIData.fireNozzle.v = new ReferenceData<FireNozzle>(fireNozzle);
                                                                }
                                                                battleRushUI.data.objectInPaths.add(objectUIData);
                                                                objectUI = Instantiate(battleRushUI.fireNozzlePrefab, ret.transform).transform;
                                                                // add component
                                                                {
                                                                    AddObjectToBattleRushUI addObjectToBattleRushUI = objectUI.gameObject.AddComponent<AddObjectToBattleRushUI>();
                                                                    addObjectToBattleRushUI.objectDataId = fireNozzle.uid;
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    case "pike":
                                                        {
                                                            Pike pike = new Pike();
                                                            {
                                                                pike.uid = battleRush.laneObjects.makeId();
                                                                pike.position.v = objectData.position.v;
                                                            }
                                                            battleRush.laneObjects.add(pike);
                                                            // make UI
                                                            {
                                                                PikeUI.UIData objectUIData = new PikeUI.UIData();
                                                                {
                                                                    objectUIData.pike.v = new ReferenceData<Pike>(pike);
                                                                }
                                                                battleRushUI.data.objectInPaths.add(objectUIData);
                                                                objectUI = Instantiate(battleRushUI.pikePrefab, ret.transform).transform;
                                                                // add component
                                                                {
                                                                    AddObjectToBattleRushUI addObjectToBattleRushUI = objectUI.gameObject.AddComponent<AddObjectToBattleRushUI>();
                                                                    addObjectToBattleRushUI.objectDataId = pike.uid;
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    case "energy_orb_upgrade":
                                                        {
                                                            EnergyOrbUpgrade energyOrbUpgrade = new EnergyOrbUpgrade();
                                                            {
                                                                energyOrbUpgrade.uid = battleRush.laneObjects.makeId();
                                                                energyOrbUpgrade.position.v = objectData.position.v;
                                                            }
                                                            battleRush.laneObjects.add(energyOrbUpgrade);
                                                            // make UI
                                                            {
                                                                EnergyOrbUpgradeUI.UIData objectUIData = new EnergyOrbUpgradeUI.UIData();
                                                                {
                                                                    objectUIData.energyOrbUpgrade.v = new ReferenceData<EnergyOrbUpgrade>(energyOrbUpgrade);
                                                                }
                                                                battleRushUI.data.objectInPaths.add(objectUIData);
                                                                objectUI = Instantiate(battleRushUI.energyOrbUpgradePrefab, ret.transform).transform;
                                                                // add component
                                                                {
                                                                    AddObjectToBattleRushUI addObjectToBattleRushUI = objectUI.gameObject.AddComponent<AddObjectToBattleRushUI>();
                                                                    addObjectToBattleRushUI.objectDataId = energyOrbUpgrade.uid;
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    case "upgrade_gate_free":
                                                        {
                                                            UpgradeGateFree upgradeGateFree = new UpgradeGateFree();
                                                            {
                                                                upgradeGateFree.uid = battleRush.laneObjects.makeId();
                                                                upgradeGateFree.position.v = objectData.position.v;
                                                            }
                                                            battleRush.laneObjects.add(upgradeGateFree);
                                                            // make UI
                                                            {
                                                                UpgradeGateFreeUI.UIData objectUIData = new UpgradeGateFreeUI.UIData();
                                                                {
                                                                    objectUIData.upgradeGateFree.v = new ReferenceData<UpgradeGateFree>(upgradeGateFree);
                                                                }
                                                                battleRushUI.data.objectInPaths.add(objectUIData);
                                                                objectUI = Instantiate(battleRushUI.upgradeGateFreePrefab, ret.transform).transform;
                                                                // add component
                                                                {
                                                                    AddObjectToBattleRushUI addObjectToBattleRushUI = objectUI.gameObject.AddComponent<AddObjectToBattleRushUI>();
                                                                    addObjectToBattleRushUI.objectDataId = upgradeGateFree.uid;
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    case "upgrade_gate_charge":
                                                        {
                                                            UpgradeGateCharge upgradeGateCharge = new UpgradeGateCharge();
                                                            {
                                                                upgradeGateCharge.uid = battleRush.laneObjects.makeId();
                                                                upgradeGateCharge.position.v = objectData.position.v;
                                                            }
                                                            battleRush.laneObjects.add(upgradeGateCharge);
                                                            // make UI
                                                            {
                                                                UpgradeGateChargeUI.UIData objectUIData = new UpgradeGateChargeUI.UIData();
                                                                {
                                                                    objectUIData.upgradeGateCharge.v = new ReferenceData<UpgradeGateCharge>(upgradeGateCharge);
                                                                }
                                                                battleRushUI.data.objectInPaths.add(objectUIData);
                                                                objectUI = Instantiate(battleRushUI.upgradeGateChargePrefab, ret.transform).transform;
                                                                // add component
                                                                {
                                                                    AddObjectToBattleRushUI addObjectToBattleRushUI = objectUI.gameObject.AddComponent<AddObjectToBattleRushUI>();
                                                                    addObjectToBattleRushUI.objectDataId = upgradeGateCharge.uid;
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    case "hammer":
                                                        {
                                                            Hammer hammer = new Hammer();
                                                            {
                                                                hammer.uid = battleRush.laneObjects.makeId();
                                                                hammer.position.v = objectData.position.v;
                                                            }
                                                            battleRush.laneObjects.add(hammer);
                                                            // make UI
                                                            {
                                                                HammerUI.UIData objectUIData = new HammerUI.UIData();
                                                                {
                                                                    objectUIData.hammer.v = new ReferenceData<Hammer>(hammer);
                                                                }
                                                                battleRushUI.data.objectInPaths.add(objectUIData);
                                                                objectUI = Instantiate(battleRushUI.hammerPrefab, ret.transform).transform;
                                                                // add component
                                                                {
                                                                    AddObjectToBattleRushUI addObjectToBattleRushUI = objectUI.gameObject.AddComponent<AddObjectToBattleRushUI>();
                                                                    addObjectToBattleRushUI.objectDataId = hammer.uid;
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    case "grinder":
                                                        {
                                                            Grinder grinder = new Grinder();
                                                            {
                                                                grinder.uid = battleRush.laneObjects.makeId();
                                                                grinder.position.v = objectData.position.v;
                                                            }
                                                            battleRush.laneObjects.add(grinder);
                                                            // make UI
                                                            {
                                                                GrinderUI.UIData objectUIData = new GrinderUI.UIData();
                                                                {
                                                                    objectUIData.grinder.v = new ReferenceData<Grinder>(grinder);
                                                                }
                                                                battleRushUI.data.objectInPaths.add(objectUIData);
                                                                objectUI = Instantiate(battleRushUI.grinderPrefab, ret.transform).transform;
                                                                // add component
                                                                {
                                                                    AddObjectToBattleRushUI addObjectToBattleRushUI = objectUI.gameObject.AddComponent<AddObjectToBattleRushUI>();
                                                                    addObjectToBattleRushUI.objectDataId = grinder.uid;
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    case "energy_orb_power":
                                                        {
                                                            EnergyOrbPower energyOrbPower = new EnergyOrbPower();
                                                            {
                                                                energyOrbPower.uid = battleRush.laneObjects.makeId();
                                                                energyOrbPower.position.v = objectData.position.v;
                                                            }
                                                            battleRush.laneObjects.add(energyOrbPower);
                                                            // make UI
                                                            {
                                                                EnergyOrbPowerUI.UIData objectUIData = new EnergyOrbPowerUI.UIData();
                                                                {
                                                                    objectUIData.energyOrbPower.v = new ReferenceData<EnergyOrbPower>(energyOrbPower);
                                                                }
                                                                battleRushUI.data.objectInPaths.add(objectUIData);
                                                                objectUI = Instantiate(battleRushUI.energyOrbPowerPrefab, ret.transform).transform;
                                                                // add component
                                                                {
                                                                    AddObjectToBattleRushUI addObjectToBattleRushUI = objectUI.gameObject.AddComponent<AddObjectToBattleRushUI>();
                                                                    addObjectToBattleRushUI.objectDataId = energyOrbPower.uid;
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    default:
                                                        Logger.LogError("unknown object data: " + objectData.I.v);
                                                        break;
                                                }
                                            }
                                            // set position
                                            {
                                                if (objectUI != null)
                                                {
                                                    float x = objectData.position.v.x * Position.DefaultSegmentWidth - Position.DefaultSegmentWidth / 2;
                                                    float z = (objectData.position.v.z % 1) * Position.DefaultSegmentHeight - Position.DefaultSegmentHeight / 2;
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
                                            Logger.Log("not in segment index");
                                            break;
                                        }
                                    }
                                }
                                Logger.Log("lastAddIndex: " + lastAddIndex + ", " + battleRush.mapData.v.objectDatas.vs.Count);
                                battleRush.mapData.v.lastAddIndex.v = lastAddIndex;
                                // level segment update
                                {
                                    levelSegment.UpdateReferences();
                                }
                            }
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
            battleRush.makeSegmentManager.v.currentNextSegmentIndex.v++;
            return ret;
        }

        public override void Stop()
        {
            base.stopped = true;
        }

    }
}
