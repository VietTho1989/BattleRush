using System.Collections;
using System.Collections.Generic;
using BattleRushS.HeroS;
using Dreamteck.Forever;
using UnityEngine;

namespace BattleRushS
{
    public class HeroUI : UIBehavior<HeroUI.UIData>
    {

        #region UIData

        public class UIData : Data
        {
            public VO<ReferenceData<Hero>> hero;

            public VD<HeroAnimation.UIData> heroAnimation;

            public VD<HeroSkinUI.UIData> heroSkin;

            public VD<HeroVfxUpgradeUI.UIData> heroUpgrade;

            public LD<TroopFollowUI.UIData> troops;

            #region Constructor

            public enum Property
            {
                hero,
                heroAnimation,
                heroSkin,
                heroUpgrade,
                troops
            }

            public UIData() : base()
            {
                this.hero = new VO<ReferenceData<Hero>>(this, (byte)Property.hero, new ReferenceData<Hero>(null));
                this.heroAnimation = new VD<HeroAnimation.UIData>(this, (byte)Property.heroAnimation, new HeroAnimation.UIData());
                this.heroSkin = new VD<HeroSkinUI.UIData>(this, (byte)Property.heroSkin, new HeroSkinUI.UIData());
                this.heroUpgrade = new VD<HeroVfxUpgradeUI.UIData>(this, (byte)Property.heroUpgrade, new HeroVfxUpgradeUI.UIData());
                this.troops = new LD<TroopFollowUI.UIData>(this, (byte)Property.troops);
            }

            #endregion

        }

        #endregion

        public Transform troopPoint;
        public CenterLaneForCamera centerLaneForCamera;

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
                        // animation
                        {
                            HeroAnimation.UIData heroAnimationUIData = this.data.heroAnimation.v;
                            if (heroAnimationUIData != null)
                            {
                                //Debug.Log("set hero for heroAnimation");
                                heroAnimationUIData.hero.v = new ReferenceData<Hero>(hero);
                            }
                            else
                            {
                                Logger.LogError("heroAnimationUIData null");
                            }
                        }
                        // skin
                        {
                            HeroSkinUI.UIData heroSkinUIData = this.data.heroSkin.v;
                            if (heroSkinUIData != null)
                            {
                                heroSkinUIData.hero.v = new ReferenceData<Hero>(hero);
                            }
                            else
                            {
                                Logger.LogError("heroSkinUIData null");
                            }
                        }
                        // troops
                        {
                            // find
                            List<TroopFollow> troops = new List<TroopFollow>();
                            {
                                troops.AddRange(hero.troopFollows.vs);
                            }
                            // process
                            {
                                // get old
                                List<TroopFollowUI.UIData> olds = new List<TroopFollowUI.UIData>();
                                {
                                    olds.AddRange(this.data.troops.vs);
                                }
                                // Update
                                {
                                    foreach (TroopFollow troop in troops)
                                    {
                                        // get UI
                                        TroopFollowUI.UIData troopFollowUIData = null;
                                        bool needAdd = false;
                                        {
                                            // find old
                                            foreach (TroopFollowUI.UIData check in olds)
                                            {
                                                if (check.troopFollow.v.data == troop)
                                                {
                                                    troopFollowUIData = check;
                                                    break;
                                                }
                                            }
                                            // make new
                                            if (troopFollowUIData == null)
                                            {
                                                troopFollowUIData = new TroopFollowUI.UIData();
                                                {
                                                    troopFollowUIData.uid = this.data.troops.makeId();
                                                }
                                                needAdd = true;
                                            }
                                            else
                                            {
                                                olds.Remove(troopFollowUIData);
                                            }
                                        }
                                        // update
                                        {
                                            troopFollowUIData.troopFollow.v = new ReferenceData<TroopFollow>(troop);
                                        }
                                        // add
                                        if (needAdd)
                                        {
                                            this.data.troops.add(troopFollowUIData);
                                        }
                                    }
                                }
                                // remove old
                                foreach (TroopFollowUI.UIData old in olds)
                                {
                                    this.data.troops.remove(old);
                                }
                            }
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

        public HeroAnimation heroAnimation;
        public HeroSkinUI heroSkin;
        public HeroVfxUpgradeUI heroUpgrade;
        public TroopFollowUI troopFollowPrefab;

        public override void onAddCallBack<T>(T data)
        {
            if(data is UIData)
            {
                UIData uiData = data as UIData;
                // Child
                {
                    uiData.hero.allAddCallBack(this);
                    uiData.heroAnimation.allAddCallBack(this);
                    uiData.heroSkin.allAddCallBack(this);
                    uiData.heroUpgrade.allAddCallBack(this);
                    uiData.troops.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            {
                if (data is Hero)
                {
                    dirty = true;
                    return;
                }
                if(data is HeroAnimation.UIData)
                {
                    HeroAnimation.UIData heroAnimationUIData = data as HeroAnimation.UIData;
                    // UI
                    {
                        if (heroAnimation != null)
                        {
                            heroAnimation.setData(heroAnimationUIData);
                        }
                        else
                        {
                            Logger.LogError("heroAnimation null");
                        }
                    }
                    dirty = true;
                    return;
                }
                if(data is HeroSkinUI.UIData)
                {
                    HeroSkinUI.UIData heroSkinUIData = data as HeroSkinUI.UIData;
                    // UI
                    {
                        if (heroSkin != null)
                        {
                            heroSkin.setData(heroSkinUIData);
                        }
                        else
                        {
                            Logger.LogError("heroSkinUIData null");
                        }
                    }
                    dirty = true;
                    return;
                }
                if(data is HeroVfxUpgradeUI.UIData)
                {
                    HeroVfxUpgradeUI.UIData heroVfxUpgradeUIData = data as HeroVfxUpgradeUI.UIData;
                    // UI
                    {
                        if (heroUpgrade != null)
                        {
                            heroUpgrade.setData(heroVfxUpgradeUIData);
                        }
                        else
                        {
                            Logger.LogError("heroUpgrade null");
                        }
                    }
                    dirty = true;
                    return;
                }
                if (data is TroopFollowUI.UIData)
                {
                    TroopFollowUI.UIData troopFollowUIData = data as TroopFollowUI.UIData;
                    // UI
                    {
                        UIUtils.Instantiate(troopFollowUIData, troopFollowPrefab, troopPoint);
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
                    uiData.hero.allRemoveCallBack(this);
                    uiData.heroAnimation.allRemoveCallBack(this);
                    uiData.heroSkin.allRemoveCallBack(this);
                    uiData.heroUpgrade.allRemoveCallBack(this);
                    uiData.troops.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // Child
            {
                if (data is Hero)
                {
                    return;
                }
                if (data is HeroAnimation.UIData)
                {
                    HeroAnimation.UIData heroAnimationUIData = data as HeroAnimation.UIData;
                    // UI
                    {
                        if (heroAnimation != null)
                        {
                            heroAnimation.setDataNull(heroAnimationUIData);
                        }
                        else
                        {
                            Logger.LogError("heroAnimation null");
                        }
                    }
                    return;
                }
                if (data is HeroSkinUI.UIData)
                {
                    HeroSkinUI.UIData heroSkinUIData = data as HeroSkinUI.UIData;
                    // UI
                    {
                        if (heroSkin != null)
                        {
                            heroSkin.setDataNull(heroSkinUIData);
                        }
                        else
                        {
                            Logger.LogError("heroSkinUIData null");
                        }
                    }
                    return;
                }
                if (data is HeroVfxUpgradeUI.UIData)
                {
                    HeroVfxUpgradeUI.UIData heroVfxUpgradeUIData = data as HeroVfxUpgradeUI.UIData;
                    // UI
                    {
                        if (heroUpgrade != null)
                        {
                            heroUpgrade.setDataNull(heroVfxUpgradeUIData);
                        }
                        else
                        {
                            Logger.LogError("heroUpgrade null");
                        }
                    }
                    return;
                }
                if (data is TroopFollowUI.UIData)
                {
                    TroopFollowUI.UIData troopFollowUIData = data as TroopFollowUI.UIData;
                    // UI
                    {
                        troopFollowUIData.removeCallBackAndDestroy(typeof(TroopFollowUI));
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
                    case UIData.Property.hero:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case UIData.Property.heroAnimation:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case UIData.Property.heroSkin:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case UIData.Property.heroUpgrade:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case UIData.Property.troops:
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
                        case Hero.Property.troopFollows:
                            dirty = true;
                            break;
                        default:
                            break;
                    }
                    return;
                }
                if (wrapProperty.p is HeroAnimation.UIData)
                {
                    return;
                }
                if (wrapProperty.p is HeroSkinUI.UIData)
                {
                    return;
                }
                if (wrapProperty.p is HeroVfxUpgradeUI.UIData)
                {
                    return;
                }
                if (wrapProperty.p is TroopFollowUI.UIData)
                {
                    return;
                }
            }
            Logger.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

    }

}