using System.Collections;
using System.Collections.Generic;
using BattleRushS.HeroS;
using UnityEngine;

namespace BattleRushS
{
    public class HeroSkinUI : UIBehavior<HeroSkinUI.UIData>
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

        public HeroInformation currentSkin;

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
                        // find
                        bool needMakeNew = true;
                        {
                            if (currentSkin != null && hero.heroInformation.v != null)
                            {
                                if (currentSkin.id == hero.heroInformation.v.id)
                                {
                                    needMakeNew = false;
                                }
                            }
                        }
                        // make new
                        if (needMakeNew)
                        {
                            if (hero.heroInformation.v != null)
                            {
                                // delete current
                                {
                                    if (currentSkin != null)
                                    {
                                        Destroy(currentSkin.gameObject);
                                    }
                                }
                                // instantiate
                                {
                                    currentSkin = Instantiate(hero.heroInformation.v, this.transform);
                                    currentSkin.transform.localPosition = Vector3.zero;
                                }                              
                            }
                            else
                            {
                                Logger.LogError("heroInformation null");
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
            if(data is Hero)
            {
                dirty = true;
                return;
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
            if (data is Hero)
            {
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
                        break;
                }
                return;
            }
            // Child
            if (wrapProperty.p is Hero)
            {
                switch ((Hero.Property)wrapProperty.n)
                {
                    case Hero.Property.heroInformation:
                        dirty = true;
                        break;
                    default:
                        break;
                }
                return;
            }
            Logger.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

    }
}
