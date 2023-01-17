using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS
{
    public class MainCanvasUI : UIBehavior<MainCanvasUI.UIData>
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

        public GameObject informationContent;
        public GameObject btnRemoveAds;

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    BattleRushUI.UIData battleRushUIData = this.data.findDataInParent<BattleRushUI.UIData>();
                    if (battleRushUIData != null)
                    {
                        BattleRush battleRush = battleRushUIData.battleRush.v.data;
                        if (battleRush != null)
                        {
                            // informationContent
                            {
                                if (informationContent != null)
                                {
                                    switch (battleRush.state.v.getType())
                                    {
                                        case BattleRush.State.Type.Load:
                                        case BattleRush.State.Type.Edit:
                                            informationContent.SetActive(false);
                                            break;
                                        case BattleRush.State.Type.Start:                                           
                                        case BattleRush.State.Type.Play:
                                        case BattleRush.State.Type.End:
                                            informationContent.SetActive(true);
                                            break;
                                        default:
                                            Logger.LogError("unknown type: " + battleRush.state.v.getType());
                                            break;
                                    }
                                }
                                else
                                {
                                    Logger.LogError("informationContent null");
                                }
                            }
                            // btnRemoveAds
                            {
                                if (btnRemoveAds != null)
                                {
                                    btnRemoveAds.SetActive(!battleRush.adsBanner.v.removeAds.v);
                                }
                                else
                                {
                                    Logger.LogError("btnRemoveAds null");
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
                        Logger.LogError("battleRushUIData null");
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

        private BattleRushUI.UIData battleRushUIData = null;

        public override void onAddCallBack<T>(T data)
        {
            if(data is UIData)
            {
                UIData uiData = data as UIData;
                // Parent
                {
                    DataUtils.addParentCallBack(uiData, this, ref this.battleRushUIData);
                }
                dirty = true;
                return;
            }
            // Parent
            {
                if(data is BattleRushUI.UIData)
                {
                    BattleRushUI.UIData battleRushUIData = data as BattleRushUI.UIData;
                    // Child
                    {
                        battleRushUIData.battleRush.allAddCallBack(this);
                    }
                    dirty = true;
                    return;
                }
                // Child
                {
                    if (data is BattleRush)
                    {
                        BattleRush battleRush = data as BattleRush;
                        // Child
                        {
                            battleRush.adsBanner.allAddCallBack(this);
                        }
                        dirty = true;
                        return;
                    }
                    // Child
                    if(data is AdsBannerController)
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
            if(data is UIData)
            {
                UIData uiData = data as UIData;
                // Parent
                {
                    DataUtils.removeParentCallBack(uiData, this, ref this.battleRushUIData);
                }
                this.setDataNull(uiData);
                return;
            }
            // Parent
            {
                if (data is BattleRushUI.UIData)
                {
                    BattleRushUI.UIData battleRushUIData = data as BattleRushUI.UIData;
                    // Child
                    {
                        battleRushUIData.battleRush.allRemoveCallBack(this);
                    }
                    return;
                }
                // Child
                {
                    if (data is BattleRush)
                    {
                        BattleRush battleRush = data as BattleRush;
                        // Child
                        {
                            battleRush.adsBanner.allRemoveCallBack(this);
                        }
                        return;
                    }
                    // Child
                    if (data is AdsBannerController)
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
            if(wrapProperty.p is UIData)
            {
                return;
            }
            // Parent
            {
                if (wrapProperty.p is BattleRushUI.UIData)
                {
                    switch ((BattleRushUI.UIData.Property)wrapProperty.n)
                    {
                        case BattleRushUI.UIData.Property.battleRush:
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
                            case BattleRush.Property.state:
                                dirty = true;
                                break;
                            case BattleRush.Property.adsBanner:
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
                    if(wrapProperty.p is AdsBannerController)
                    {
                        switch ((AdsBannerController.Property)wrapProperty.n)
                        {
                            case AdsBannerController.Property.removeAds:
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
        
        public void onClickBtnRemoveAds()
        {
            if (this.data != null)
            {
                BattleRushUI.UIData battleRushUIData = this.data.findDataInParent<BattleRushUI.UIData>();
                if (battleRushUIData != null)
                {
                    BattleRush battleRush = battleRushUIData.battleRush.v.data;
                    if (battleRush != null)
                    {
                        battleRush.adsBanner.v.removeAds.v = true;
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
                Logger.LogError("data null");
            }
        }

    }
}