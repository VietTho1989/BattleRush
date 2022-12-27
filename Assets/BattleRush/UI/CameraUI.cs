using System.Collections;
using System.Collections.Generic;
using BattleRushS.HeroS;
using Cinemachine;
using UnityEngine;

namespace BattleRushS
{
    public class CameraUI : UIBehavior<CameraUI.UIData>
    {

        #region UIData

        public class UIData : Data
        {

            public VO<ReferenceData<BattleRush>> battleRush;

            #region Constructor

            public enum Property
            {
                battleRush
            }

            public UIData() : base()
            {
                this.battleRush = new VO<ReferenceData<BattleRush>>(this, (byte)Property.battleRush, new ReferenceData<BattleRush>(null));
            }

            #endregion

        }

        #endregion

        #region Refresh

        public CinemachineBrain mainCamera;
        public CinemachineVirtualCamera heroCamera;

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
                        if(mainCamera!=null && heroCamera != null)
                        {
                            // find
                            bool isRun = true;
                            {
                                Segment mySegment = battleRush.hero.v.heroMove.v.currentSegment.v;
                                if (mySegment != null)
                                {
                                    switch (mySegment.segmentType)
                                    {
                                        case Segment.SegmentType.Run:
                                            break;
                                        case Segment.SegmentType.Arena:
                                            isRun = false;
                                            break;
                                        default:
                                            Logger.LogError("unknown segmentType: " + mySegment.segmentType);
                                            break;
                                    }
                                }
                                else
                                {
                                    Logger.LogError("mySegment null");
                                }
                            }
                            // process
                            heroCamera.enabled = isRun;
                        }
                        else
                        {
                            Logger.LogError("camera null");
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
            if(data is UIData)
            {
                UIData uiData = data as UIData;
                // Child
                {
                    uiData.battleRush.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            {
                if(data is BattleRush)
                {
                    BattleRush battleRush = data as BattleRush;
                    // Child
                    {
                        battleRush.hero.allAddCallBack(this);
                    }
                    dirty = true;
                    return;
                }
                // Child
                {
                    if(data is Hero)
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
                }
                this.setDataNull(uiData);
                return;
            }
            // Child
            {
                if (data is BattleRush)
                {
                    BattleRush battleRush = data as BattleRush;
                    // Child
                    {
                        battleRush.hero.allRemoveCallBack(this);
                    }
                    return;
                }
                // Child
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
                    default:
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
                        case BattleRush.Property.hero:
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
            }
            Logger.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

    }
}
