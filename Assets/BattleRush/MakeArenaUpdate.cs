using System.Collections;
using System.Collections.Generic;
using BattleRushS.HeroS;
using Dreamteck.Forever;
using UnityEngine;

namespace BattleRushS.ArenaS
{
    public class MakeArenaUpdate : UpdateBehavior<BattleRush>
    {

        #region Update

        public override void update()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    // make new
                    {
                        // find
                        Segment segment = null;
                        {
                            Segment currentSegment = this.data.hero.v.heroMove.v.currentSegment.v;
                            if(currentSegment!=null && currentSegment.segmentType== Segment.SegmentType.Arena)
                            {
                                segment = currentSegment;
                            }
                        }
                        // process
                        if (segment != null)
                        {
                            // find already make
                            bool alreadyMake = false;
                            {
                                foreach(Arena arena in this.data.arenas.vs)
                                {
                                    if (arena.segment.v == segment)
                                    {
                                        alreadyMake = true;
                                        break;
                                    }
                                }
                            }
                            // make
                            if (!alreadyMake)
                            {
                                Arena arena = new Arena();
                                {
                                    arena.uid = this.data.arenas.makeId();
                                    arena.segment.v = segment;
                                }
                                this.data.arenas.add(arena);
                            }
                        }
                    }
                    // remove not exist anymore
                    {
                        BattleRushUI battleRushUI = this.data.findCallBack<BattleRushUI>();
                        if (battleRushUI != null)
                        {
                            LevelGenerator levelGenerator = battleRushUI.GetComponent<LevelGenerator>();
                            if (levelGenerator != null)
                            {
                                List<Arena> needRemoves = new List<Arena>();
                                {
                                    foreach (Arena arena in this.data.arenas.vs)
                                    {
                                        if (arena.segment.v==null || !arena.segment.v.gameObject)
                                        {
                                            Logger.Log("arena segment is destroyed");
                                            needRemoves.Add(arena);
                                        }
                                    }
                                }
                                foreach(Arena needRemove in needRemoves)
                                {
                                    this.data.arenas.remove(needRemove);
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
            if(data is BattleRush)
            {
                BattleRush battleRush = data as BattleRush;
                // Child
                {
                    battleRush.hero.allAddCallBack(this);
                    battleRush.arenas.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            {
                // hero
                {
                    if (data is Hero)
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
                if(data is Arena)
                {
                    dirty = true;
                    return;
                }
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if (data is BattleRush)
            {
                BattleRush battleRush = data as BattleRush;
                // Child
                {
                    battleRush.hero.allRemoveCallBack(this);
                    battleRush.arenas.allRemoveCallBack(this);
                }
                this.setDataNull(battleRush);
                return;
            }
            // Child
            {
                // hero
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
                if (data is Arena)
                {
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
            if (wrapProperty.p is BattleRush)
            {
                switch ((BattleRush.Property)wrapProperty.n)
                {
                    case BattleRush.Property.segmentIndex:
                        dirty = true;
                        break;
                    case BattleRush.Property.hero:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case BattleRush.Property.arenas:
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
                // hero
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
                            default:
                                break;
                        }
                        return;
                    }
                }
                if (wrapProperty.p is Arena)
                {
                    switch ((Arena.Property)wrapProperty.n)
                    {
                        case Arena.Property.segment:
                            dirty = true;
                            break;
                        default:
                            break;
                    }
                    return;
                }
            }
            Logger.LogError("Don't process: " + data + "; " + syncs + "; " + this);
        }

        #endregion

    }
}
