using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleRushS.ArenaS
{
    public class HealthBarUI : UIBehavior<HealthBarUI.UIData>
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

        public Color team1Color = Color.white;
        public Color team2Color = Color.white;

        public Image fillImage;

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    // set transform parent in world canvas
                    {
                        BattleRushUI.UIData battleRushUIData = this.data.findDataInParent<BattleRushUI.UIData>();
                        if (battleRushUIData != null)
                        {
                            BattleRushUI battleRushUI = battleRushUIData.findCallBack<BattleRushUI>();
                            if (battleRushUI != null)
                            {
                                if (battleRushUI.worldCanvas != null)
                                {
                                    this.transform.SetParent(battleRushUI.worldCanvas);
                                }
                                else
                                {
                                    Logger.LogError("worldCanvas null");
                                }
                            }
                            else
                            {
                                Logger.LogError("battleRushUI null");
                            }
                        }
                        else
                        {
                            Logger.LogError("battleRushUIData null");
                        }
                    }
                    // fill image
                    {
                        if (fillImage != null)
                        {
                            TroopUI.UIData troopUIData = this.data.findDataInParent<TroopUI.UIData>();
                            if (troopUIData != null)
                            {
                                Troop troop = troopUIData.troop.v.data;
                                if (troop != null)
                                {
                                    // team color
                                    {
                                        switch (troop.teamId.v)
                                        {
                                            case 0:
                                                fillImage.color = team1Color;
                                                break;
                                            case 1:
                                                fillImage.color = team2Color;
                                                break;
                                            default:                                                
                                                Logger.LogError("unknown teamId: " + troop.teamId.v);
                                                fillImage.color = Color.white;
                                                break;
                                        }
                                    }
                                    // fill percent
                                    fillImage.fillAmount = Mathf.Clamp(troop.hitpoint.v, 0, 1);
                                }
                                else
                                {
                                    Logger.LogError("troop null");
                                }
                            }
                            else
                            {
                                Logger.LogError("troopUIData null");
                            }
                        }
                        else
                        {
                            Logger.LogError("fillImage null");
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
            return false;
        }

        #endregion

        #region position follow troop

        private TroopUI troopUI = null;

        public override void LateUpdate()
        {
            base.Update();
            // get troopUI
            if (troopUI == null)
            {
                if (this.data != null)
                {
                    TroopUI.UIData troopUIData = this.data.findDataInParent<TroopUI.UIData>();
                    if (troopUIData != null)
                    {
                        troopUI = troopUIData.findCallBack<TroopUI>();
                    }
                    else
                    {
                        Logger.LogError("troopUIData null");
                    }
                }
                else
                {
                    Logger.LogError("data null");
                }
            }
            // set
            {
                if (troopUI != null)
                {
                    this.transform.position = new Vector3(troopUI.transform.position.x, troopUI.transform.position.y + 1.5f, troopUI.transform.position.z);
                }
                else
                {
                    Logger.LogError("troopUI null");
                }                
            }
        }

        #endregion

        #region implement callBacks

        private TroopUI.UIData troopUIData = null;

        public override void onAddCallBack<T>(T data)
        {
            if(data is UIData)
            {
                UIData uiData = data as UIData;
                // reset
                {
                    troopUI = null;
                }
                // Parent
                {
                    DataUtils.addParentCallBack(uiData, this, ref this.troopUIData);
                }
                dirty = true;
                return;
            }
            // Parent
            {
                if(data is TroopUI.UIData)
                {
                    TroopUI.UIData troopUIData = data as TroopUI.UIData;
                    // Child
                    {
                        troopUIData.troop.allAddCallBack(this);
                    }
                    dirty = true;
                    return;
                }
                // Child
                if(data is Troop)
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
                // Parent
                {
                    DataUtils.removeParentCallBack(uiData, this, ref this.troopUIData);
                }
                this.setDataNull(uiData);
                return;
            }
            // Parent
            {
                if (data is TroopUI.UIData)
                {
                    TroopUI.UIData troopUIData = data as TroopUI.UIData;
                    // Child
                    {
                        troopUIData.troop.allRemoveCallBack(this);
                    }
                    return;
                }
                // Child
                if (data is Troop)
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
                    default:
                        break;
                }
                return;
            }
            // Parent
            {
                if (wrapProperty.p is TroopUI.UIData)
                {
                    switch ((TroopUI.UIData.Property)wrapProperty.n)
                    {
                        case TroopUI.UIData.Property.troop:
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
                if (wrapProperty.p is Troop)
                {
                    switch ((Troop.Property)wrapProperty.n)
                    {
                        case Troop.Property.teamId:
                            dirty = true;
                            break;
                        case Troop.Property.hitpoint:
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
