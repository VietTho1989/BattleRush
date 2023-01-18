using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.HeroS
{
    public class HeroVfxUpgradeUI : UIBehavior<HeroVfxUpgradeUI.UIData>
    {

        #region UIData

        public class UIData : Data
        {

            #region Constructor

            public enum Property
            {

            }

            public UIData() : base()
            {

            }

            #endregion

        }

        #endregion

        #region Refresh

        public GameObject vfxPrefab;
        private List<GameObject> vfxs = new List<GameObject>();

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {

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

        public override void OnDestroy()
        {
            base.OnDestroy();
            destroyAllVfx();
        }

        private void destroyAllVfx()
        {
            foreach(GameObject vfx in vfxs)
            {
                if (vfx)
                {
                    Destroy(vfx);
                }
            }
            vfxs.Clear();
        }

        #endregion

        #region implement callBacks

        private HeroUI.UIData heroUIData = null;

        public override void onAddCallBack<T>(T data)
        {
            if(data is UIData)
            {
                UIData uiData = data as UIData;
                // Parent
                {
                    DataUtils.addParentCallBack(uiData, this, ref this.heroUIData);
                }
                destroyAllVfx();
                dirty = true;
                return;
            }
            // Parent
            {
                if(data is HeroUI.UIData)
                {
                    HeroUI.UIData heroUIData = data as HeroUI.UIData;
                    // Child
                    {
                        heroUIData.hero.allAddCallBack(this);
                    }
                    dirty = true;
                    return;
                }
                // Child
                if(data is Hero)
                {
                    destroyAllVfx();
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
                // Parent
                {
                    DataUtils.removeParentCallBack(uiData, this, ref this.heroUIData);
                }
                destroyAllVfx();
                this.setDataNull(uiData);
                return;
            }
            // Parent
            {
                if (data is HeroUI.UIData)
                {
                    HeroUI.UIData heroUIData = data as HeroUI.UIData;
                    // Child
                    {
                        heroUIData.hero.allRemoveCallBack(this);
                    }
                    return;
                }
                // Child
                if (data is Hero)
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
                return;
            }
            // Parent
            {
                if (wrapProperty.p is HeroUI.UIData)
                {
                    switch ((HeroUI.UIData.Property)wrapProperty.n)
                    {
                        case HeroUI.UIData.Property.hero:
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
                        case Hero.Property.level:
                            {
                                if (vfxPrefab != null)
                                {
                                    // remove old
                                    {
                                        for(int i=vfxs.Count-1; i>=0; i--)
                                        {
                                            if (!vfxs[i])
                                            {
                                                vfxs.RemoveAt(i);
                                            }
                                        }
                                    }
                                    // find
                                    bool canVfx = false;
                                    {
                                        Hero hero = wrapProperty.p as Hero;
                                        BattleRush battleRush = hero.findDataInParent<BattleRush>();
                                        if (battleRush != null)
                                        {
                                            switch (battleRush.state.v.getType())
                                            {
                                                case BattleRush.State.Type.Load:
                                                case BattleRush.State.Type.Edit:
                                                    break;
                                                case BattleRush.State.Type.Start:
                                                case BattleRush.State.Type.Play:
                                                    canVfx = true;
                                                    break;
                                                case BattleRush.State.Type.End:
                                                    break;
                                                default:
                                                    Logger.LogError("unknown type: " + battleRush.state.v.getType());
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            Logger.LogError("battleRush null");
                                        }
                                    }
                                    // process
                                    if(canVfx)
                                    {
                                        GameObject vfx = Instantiate(vfxPrefab, this.transform);
                                        vfxs.Add(vfx);
                                    }
                                }
                                else
                                {
                                    Logger.LogError("vfxPrefab null");
                                }                                
                            }
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
