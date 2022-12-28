using System.Collections;
using System.Collections.Generic;
using Dreamteck.Forever;
using UnityEngine;

namespace BattleRushS.HeroS
{
    public class HeroMoveUpdate : UpdateBehavior<HeroMove>
    {

        public override void Awake()
        {
            base.Awake();
            // level generator: check when enter segment
            {
                LevelGenerator.onLevelEntered += LevelEnterHandler;
                LevelSegment.onSegmentEntered += SegmentEnterHandler;
            }
        }

        public override void OnDestroy()
        {
            // level generator: check when enter segment
            {
                LevelGenerator.onLevelEntered -= LevelEnterHandler;
                LevelSegment.onSegmentEntered -= SegmentEnterHandler;
            }
        }

        public void LevelEnterHandler(ForeverLevel oldLevel, ForeverLevel level, int index)
        {
            Logger.Log("LevelEnterHandler: " + oldLevel + ", " + level + ", " + index);
        }

        public void SegmentEnterHandler(LevelSegment segment)
        {
            Logger.Log("SegmentEnterHandler: " + segment);
            if (this.data != null)
            {
                // check segment is in game
                bool isSegmentInGame = false;
                {
                    BattleRushUI parentBattleRushUI = segment.GetComponentInParent<BattleRushUI>();
                    if (parentBattleRushUI != null)
                    {
                        BattleRushUI.UIData battleRushUIData = parentBattleRushUI.data;
                        if (battleRushUIData != null)
                        {
                            BattleRush battleRush = battleRushUIData.battleRush.v.data;
                            if (battleRush != null)
                            {
                                if (battleRush == this.data.findDataInParent<BattleRush>())
                                {
                                    isSegmentInGame = true;
                                }
                            }
                            else
                            {
                                Logger.LogError("battleRush null");
                            }
                        }
                        else
                        {
                            Logger.LogError("battleRushUIData null");
                        }
                    }
                    else
                    {
                        Logger.LogError("parentBattleRushUI null");
                    }
                }
                // process
                if (isSegmentInGame)
                {
                    Segment mySegment = segment.GetComponent<Segment>();
                    if (mySegment != null)
                    {
                        this.data.currentSegment.v = mySegment;
                    }
                    else
                    {
                        Logger.LogError("mySegment null");
                    }
                }
            }
            else
            {
                Logger.LogError("data null");
            }
        }

        #region Update

        public override void update()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    // sub
                    {
                        // find
                        bool isRun = true;
                        {
                            // TODO can hoan thien
                            if (this.data.currentSegment.v != null)
                            {
                                if(this.data.currentSegment.v.segmentType == Segment.SegmentType.Arena)
                                {
                                    isRun = false;
                                }
                            }
                        }
                        // process
                        {
                            if (isRun)
                            {
                                HeroMoveRun heroMoveRun = this.data.sub.newOrOld<HeroMoveRun>();
                                {

                                }
                                this.data.sub.v = heroMoveRun;
                            }
                            else
                            {
                                HeroMoveArena heroMoveArena = this.data.sub.newOrOld<HeroMoveArena>();
                                {

                                }
                                this.data.sub.v = heroMoveArena;
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

        public override void onAddCallBack<T>(T data)
        {
            if(data is HeroMove)
            {
                HeroMove heroMove = data as HeroMove;
                // Child
                {
                    heroMove.sub.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            if(data is HeroMove.Sub)
            {
                HeroMove.Sub sub = data as HeroMove.Sub;
                // Update
                {
                    switch (sub.getType())
                    {
                        case HeroMove.Sub.Type.Run:
                            {
                                HeroMoveRun heroMoveRun = sub as HeroMoveRun;
                                UpdateUtils.makeUpdate<HeroMoveRunUpdate, HeroMoveRun>(heroMoveRun, this.transform);
                            }
                            break;
                        case HeroMove.Sub.Type.Arena:
                            {
                                HeroMoveArena heroMoveArena = sub as HeroMoveArena;
                                UpdateUtils.makeUpdate<HeroMoveArenaUpdate, HeroMoveArena>(heroMoveArena, this.transform);
                            }
                            break;
                        default:
                            Logger.LogError("unknown type: " + sub.getType());
                            break;
                    }
                }
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if(data is HeroMove)
            {
                HeroMove heroMove = data as HeroMove;
                // Child
                {
                    heroMove.sub.allRemoveCallBack(this);
                }
                this.setDataNull(heroMove);
                return;
            }
            // Child
            if (data is HeroMove.Sub)
            {
                HeroMove.Sub sub = data as HeroMove.Sub;
                // Update
                {
                    switch (sub.getType())
                    {
                        case HeroMove.Sub.Type.Run:
                            {
                                HeroMoveRun heroMoveRun = sub as HeroMoveRun;
                                heroMoveRun.removeCallBackAndDestroy(typeof(HeroMoveRunUpdate));
                            }
                            break;
                        case HeroMove.Sub.Type.Arena:
                            {
                                HeroMoveArena heroMoveArena = sub as HeroMoveArena;
                                heroMoveArena.removeCallBackAndDestroy(typeof(HeroMoveArenaUpdate));
                            }
                            break;
                        default:
                            Logger.LogError("unknown type: " + sub.getType());
                            break;
                    }
                }
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
            if(wrapProperty.p is HeroMove)
            {
                switch ((HeroMove.Property)wrapProperty.n)
                {
                    case HeroMove.Property.sub:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case HeroMove.Property.currentSegment:
                        dirty = true;
                        break;
                    default:
                        break;
                }
                return;
            }
            // Child
            if (wrapProperty.p is HeroMove.Sub)
            {
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + syncs + "; " + this);
        }

        #endregion

    }
}