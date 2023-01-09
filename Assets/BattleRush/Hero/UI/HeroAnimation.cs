using System.Collections;
using System.Collections.Generic;
using BattleRushS.HeroS;
using BattleRushS.StateS;
using UnityEngine;

namespace BattleRushS
{
    public class HeroAnimation : UIBehavior<HeroAnimation.UIData>
    {

        #region UIData

        public class UIData : Data
        {

            public VO<ReferenceData<Hero>> hero;

            #region Constructor

            public enum Property
            {
                hero
            }

            public UIData() : base()
            {
                this.hero = new VO<ReferenceData<Hero>>(this, (byte)Property.hero, new ReferenceData<Hero>(null));
            }

            #endregion

        }

        #endregion

        #region Refresh

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    Hero hero = this.data.hero.v.data;
                    if (hero != null)
                    {
                        // find hero information
                        HeroInformation heroInformation = null;
                        {
                            HeroSkinUI heroSkinUI = hero.findCallBack<HeroSkinUI>();
                            if (heroSkinUI != null)
                            {
                                heroInformation = heroSkinUI.currentSkin;
                            }
                            else
                            {
                                Logger.LogError("heroSkinUI null");
                            }
                        }
                        // process
                        if (heroInformation != null)
                        {
                            BattleRush battleRush = hero.findDataInParent<BattleRush>();
                            if (battleRush != null)
                            {
                                Debug.Log("battleRush: " + battleRush.state.v.getType());
                                switch (battleRush.state.v.getType())
                                {
                                    case BattleRush.State.Type.Load:
                                        {
                                            heroInformation.playAnimation("Idle");
                                        }
                                        break;
                                    case BattleRush.State.Type.Start:
                                        {
                                            heroInformation.playAnimation("Idle");
                                        }
                                        break;
                                    case BattleRush.State.Type.Play:
                                        {
                                            Debug.Log("update hero animation: run");
                                            Play play = battleRush.state.v as Play;
                                            switch (play.state.v)
                                            {
                                                case Play.State.Normal:
                                                    {
                                                        if (hero.heroMove.v.currentSegment.v != null)
                                                        {
                                                            switch (hero.heroMove.v.currentSegment.v.segmentType)
                                                            {
                                                                case Segment.SegmentType.Run:
                                                                    heroInformation.playAnimation("Run");
                                                                    break;
                                                                case Segment.SegmentType.Arena:
                                                                    heroInformation.playAnimation("Idle");
                                                                    break;
                                                                default:
                                                                    Logger.LogError("unknown segmentType: "+ hero.heroMove.v.currentSegment.v.segmentType);
                                                                    break;
                                                            }                                                            
                                                        }
                                                        else
                                                        {
                                                            Logger.LogError("currentSegment null");
                                                            heroInformation.playAnimation("Idle");
                                                        }                                                      
                                                    }                                                   
                                                    break;
                                                case Play.State.Pause:
                                                    heroInformation.playAnimation("Idle");
                                                    break;
                                            }                                           
                                        }
                                        break;
                                    case BattleRush.State.Type.End:
                                        {
                                            heroInformation.playAnimation("Death");
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                Logger.LogError("battleRush null");
                            }
                        }
                        else
                        {
                            Logger.LogError("myAnimator null");
                            dirty = true;
                        }
                    }
                    else
                    {
                        Logger.LogError("hero null");
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

        public BattleRush battleRush = null;

        public override void onAddCallBack<T>(T data)
        {
            if(data is UIData)
            {
                UIData uiData = data as UIData;
                // Child
                {
                    uiData.hero.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            {
                if(data is Hero)
                {
                    Hero hero = data as Hero;
                    // Parent
                    {
                        DataUtils.addParentCallBack(hero, this, ref this.battleRush);
                    }
                    // Child
                    {
                        hero.heroMove.allAddCallBack(this);
                    }
                    dirty = true;
                    return;
                }
                // Parent
                {
                    if (data is BattleRush)
                    {
                        BattleRush battleRush = data as BattleRush;
                        // Child
                        {
                            battleRush.state.allAddCallBack(this);
                        }
                        dirty = true;
                        return;
                    }
                    // Child
                    if(data is BattleRush.State)
                    {
                        dirty = true;
                        return;
                    }
                }
                // Child
                if(data is HeroMove)
                {
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
                    uiData.hero.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // Child
            {
                if (data is Hero)
                {
                    Hero hero = data as Hero;
                    // Parent
                    {
                        DataUtils.removeParentCallBack(hero, this, ref this.battleRush);
                    }
                    // Child
                    {
                        hero.heroMove.allRemoveCallBack(this);
                    }
                    return;
                }
                // Parent
                {
                    if (data is BattleRush)
                    {
                        BattleRush battleRush = data as BattleRush;
                        // Child
                        {
                            battleRush.state.allRemoveCallBack(this);
                        }
                        return;
                    }
                    // Child
                    if (data is BattleRush.State)
                    {
                        return;
                    }
                }
                // Child
                if(data is HeroMove)
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
            if (wrapProperty.p is UIData)
            {
                switch ((UIData.Property)wrapProperty.n)
                {
                    case UIData.Property.hero:
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
                // Parent
                {
                    if (wrapProperty.p is BattleRush)
                    {
                        switch ((BattleRush.Property)wrapProperty.n)
                        {
                            case BattleRush.Property.state:
                                {
                                    ValueChangeUtils.replaceCallBack(this, syncs);
                                    dirty = true;
                                }
                                break;
                            case BattleRush.Property.mapData:
                                break;
                            case BattleRush.Property.hero:
                                break;
                            default:
                                //Logger.LogError("Don't process: " + wrapProperty + "; " + this);
                                break;
                        }
                        return;
                    }
                    // Child
                    if(wrapProperty.p is BattleRush.State)
                    {
                        BattleRush.State state = wrapProperty.p as BattleRush.State;
                        switch (state.getType())
                        {
                            case BattleRush.State.Type.Load:
                                break;
                            case BattleRush.State.Type.Start:
                                break;
                            case BattleRush.State.Type.Play:
                                {
                                    switch ((Play.Property)wrapProperty.n)
                                    {
                                        case Play.Property.state:
                                            dirty = true;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case BattleRush.State.Type.End:
                                break;
                        }
                        return;
                    }
                }
                // Child
                if(wrapProperty.p is HeroMove)
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
            Logger.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

    }
}
