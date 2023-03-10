using System.Collections;
using System.Collections.Generic;
using BattleRushS.StateS;
using Dreamteck.Forever;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BattleRushS
{
    public class BattleRush : Data
    {

        public VO<bool> isInEditMode;

        #region ads

        public VD<AdsBannerController> adsBanner;

        #endregion

        #region information

        public VO<int> level;

        public VO<int> levelCoin;

        public VO<int> levelNormalOrb;

        public VO<int> levelEnergyOrb;

        public VO<int> totalCoin;

        #endregion

        #region state

        public abstract class State : Data
        {

            public enum Type
            {
                Load,
                Edit,
                Start,
                Play,
                End
            }

            public abstract Type getType();

        }

        public VD<State> state;

        #endregion

        public VD<MapData> mapData;

        public VD<MakeSegmentManager> makeSegmentManager;

        public VD<Hero> hero;

        public LD<ObjectInPath> laneObjects;

        /**
         * index of last segment in LevelGenerator
         */
        public VO<int> segmentIndex;

        public LD<Arena> arenas;

        public enum Property
        {
            isInEditMode,
            adsBanner,

            level,
            levelCoin,
            levelNormalOrb,
            levelEnergyOrb,
            totalCoin,

            state,
            mapData,
            makeSegmentManager,
            hero,
            laneObjects,
            segmentIndex,
            arenas
        }

        public BattleRush() : base()
        {
            this.isInEditMode = new VO<bool>(this, (byte)Property.isInEditMode, false);
            this.adsBanner = new VD<AdsBannerController>(this, (byte)Property.adsBanner, new AdsBannerController());
            // information
            {
                this.level = new VO<int>(this, (byte)Property.level, 1);
                this.levelCoin = new VO<int>(this, (byte)Property.levelCoin, 0);
                this.levelNormalOrb = new VO<int>(this, (byte)Property.levelNormalOrb, 0);
                this.levelEnergyOrb = new VO<int>(this, (byte)Property.levelEnergyOrb, 0);
                this.totalCoin = new VO<int>(this, (byte)Property.totalCoin, 18000);
            }
            this.state = new VD<State>(this, (byte)Property.state, new Load());
            this.mapData = new VD<MapData>(this, (byte)Property.mapData, new MapData());
            this.makeSegmentManager = new VD<MakeSegmentManager>(this, (byte)Property.makeSegmentManager, new MakeSegmentManager());
            this.hero = new VD<Hero>(this, (byte)Property.hero, new Hero());
            this.laneObjects = new LD<ObjectInPath>(this, (byte)Property.laneObjects);
            this.segmentIndex = new VO<int>(this, (byte)Property.segmentIndex, -1);
            this.arenas = new LD<Arena>(this, (byte)Property.arenas);
        }

        public void reset()
        {
            // state
            {
                Load load = this.state.newOrOld<Load>();
                {

                }
                this.state.v = load;
            }
            this.mapData.v.reset();
            this.hero.v.reset();
            this.makeSegmentManager.v.reset();
            this.laneObjects.clear();
            // levelGenerator
            {
                BattleRushUI battleRushUI = this.findCallBack<BattleRushUI>();
                if (battleRushUI != null)
                {
                    LevelGenerator levelGenerator = battleRushUI.GetComponent<LevelGenerator>();
                    if (levelGenerator != null)
                    {
                        levelGenerator.Clear();
                        // set my custom sequence: TODO tam viet vay
                        {
                            CustomSequence customSequence = levelGenerator.currentLevel.sequenceCollection.sequences[0].customSequence;
                            Logger.Log("myCustomSequence: " + customSequence);
                            if (customSequence != null && customSequence is MySequence)
                            {
                                MySequence mySequence = customSequence as MySequence;
                                mySequence.stopped = false;
                            }
                        }
                        // tempSegmentContainer
                        {
                            if (battleRushUI.tempSegmentContainer != null)
                            {
                                for (int i = 0; i < battleRushUI.tempSegmentContainer.childCount; i++)
                                {
                                    Transform tempSegment = battleRushUI.tempSegmentContainer.GetChild(i);
                                    GameObject.Destroy(tempSegment.gameObject);
                                }
                            }
                            else
                            {
                                Logger.LogError("tempSegmentContainer null");
                            }
                        }
                    }
                    else
                    {
                        Logger.LogError("levelGenerator null");
                    }
                }
                else
                {
                    Logger.LogError("battleRushUI null");
                }
            }
            // heroUI
            {
                Hero hero = this.hero.v;
                if (hero != null)
                {
                    HeroUI heroUI = hero.findCallBack<HeroUI>();
                    if (heroUI != null)
                    {
                        heroUI.transform.localPosition = Vector3.zero;
                        heroUI.transform.localRotation = Quaternion.identity;
                    }
                    else
                    {
                        Logger.LogError("heroUI null");
                    }
                }
                else
                {
                    Logger.LogError("hero null");
                }
            }
            // vfx
            {
                BattleRushUI battleRushUI = this.findCallBack<BattleRushUI>();
                if (battleRushUI != null)
                {
                    if (battleRushUI.vfxContainer != null)
                    {
                        for(int i=0; i< battleRushUI.vfxContainer.childCount; i++)
                        {
                            GameObject vfx = battleRushUI.vfxContainer.GetChild(i).gameObject;
                            GameObject.Destroy(vfx);
                        }
                    }
                }
                else
                {
                    Logger.LogError("battleRushUI null");
                }
            }
        }

#if UNITY_EDITOR
        public void save()
        {
            // item
            {
                ItemMap itemMap = this.mapData.v.itemMap.v;
                if (itemMap != null)
                {
                    itemMap.items.Clear();
                    foreach (ObjectInPath objectInPath in this.laneObjects.vs)
                    {
                        ItemAsset itemAsset = new ItemAsset();
                        {
                            itemAsset.type = objectInPath.getType();
                            itemAsset.position = objectInPath.getPosition();
                            //public uint row = 1;
                            //public uint col = 1;
                            //public float distanceBetweenRow = 0.1f;
                            //public float distanceBetweenCol = 0.1f;
                        }
                        itemMap.items.Add(itemAsset);
                    }
                    EditorUtility.SetDirty(itemMap);
                }
                else
                {
                    Logger.LogError("itemMap null");
                }
            }
            // map
            {
                MapAsset mapAsset = this.makeSegmentManager.v.mapAsset.v;
                if (mapAsset != null)
                {
                    BattleRushUI battleRushUI = this.findCallBack<BattleRushUI>();
                    if (battleRushUI != null)
                    {
                        Segment[] segments = battleRushUI.GetComponentsInChildren<Segment>();
                        mapAsset.segments.Clear();
                        foreach (Segment segment in segments)
                        {
                            if (segment.segmentType == Segment.SegmentType.Run && !segment.isDeleted)
                            {
                                mapAsset.segments.Add(segment.segmentAsset);
                            }                                
                        }
                        EditorUtility.SetDirty(mapAsset);
                    }
                    else
                    {
                        Logger.LogError("battleRushUI null");
                    }
                }
                else
                {
                    Logger.LogError("mapAsset null");
                }
            }
        }
#endif

    }
}