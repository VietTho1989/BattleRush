using System.Collections;
using System.Collections.Generic;
using Dreamteck.Forever;
using UnityEngine;

namespace BattleRushS.StateS.LoadS
{
    public class ResetUpdate : UpdateBehavior<Reset>
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
                        battleRush.mapData.v.reset();
                        battleRush.hero.v.reset();
                        battleRush.laneObjects.clear();
                        // levelGenerator
                        {
                            BattleRushUI battleRushUI = battleRush.findCallBack<BattleRushUI>();
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
                                            mySequence.setCurrentNextSegmentIdex(0);
                                        }
                                    }
                                    // tempSegmentContainer
                                    {
                                        if (battleRushUI.tempSegmentContainer != null)
                                        {
                                            for(int i=0; i< battleRushUI.tempSegmentContainer.childCount; i++)
                                            {
                                                Transform tempSegment = battleRushUI.tempSegmentContainer.GetChild(i);
                                                Destroy(tempSegment.gameObject);
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
                            Hero hero = battleRush.hero.v;
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

                        // change to choose level
                        {
                            Load load = this.data.findDataInParent<Load>();
                            if (load != null)
                            {
                                ChooseLevel chooseLevel = load.sub.newOrOld<ChooseLevel>();
                                {
                                    
                                }
                                load.sub.v = chooseLevel;
                            }
                            else
                            {
                                Logger.LogError("load null");
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

        public override void onAddCallBack<T>(T data)
        {
            if(data is Reset)
            {
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if(data is Reset)
            {
                Reset reset = data as Reset;
                this.setDataNull(reset);
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onUpdateSync<T>(WrapProperty wrapProperty, List<Sync<T>> syncs)
        {
            if (WrapProperty.checkError(wrapProperty))
            {
                return;
            }
            if(wrapProperty.p is Reset)
            {
                switch ((Reset.Property)wrapProperty.n)
                {
                    default:
                        Logger.LogError("Don't process: " + wrapProperty + "; " + this);
                        break;
                }
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + syncs + "; " + this);
        }

        #endregion

    }
}