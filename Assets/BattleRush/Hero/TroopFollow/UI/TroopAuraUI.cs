using System.Collections;
using System.Collections.Generic;
using BattleRushS.ArenaS;
using UnityEngine;

namespace BattleRushS.HeroS
{
    public class TroopAuraUI : UIBehavior<TroopAuraUI.UIData>
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

        public GameObject oldPrefab;
        public GameObject oldAura;

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    // find prefab
                    GameObject auraPrefab = null;
                    {
                        // troop follow
                        if (auraPrefab == null)
                        {
                            TroopFollowUI.UIData troopFollowUIData = this.data.findDataInParent<TroopFollowUI.UIData>();
                            if (troopFollowUIData != null)
                            {
                                TroopFollow troopFollow = troopFollowUIData.troopFollow.v.data;
                                if (troopFollow != null)
                                {
                                    TroopInformation.Level level = troopFollow.troopType.v.levels.Find(check => check.level == troopFollow.level.v);
                                    if (level != null)
                                    {
                                        auraPrefab = level.auraPrefab;
                                    }
                                    else
                                    {
                                        Logger.LogError("level null");
                                    }
                                }
                                else
                                {
                                    Logger.LogError("troopFollow null");
                                }
                            }
                            else
                            {
                                // Logger.LogError("troopFollowUIData null");
                            }
                        }
                        // troop in arena
                        if (auraPrefab == null)
                        {
                            TroopUI.UIData troopUIData = this.data.findDataInParent<TroopUI.UIData>();
                            if (troopUIData != null)
                            {                             
                                Troop troop = troopUIData.troop.v.data;
                                if (troop != null)
                                {
                                    // Logger.Log("TroopAuraUI: check troop: " + troop.uid);
                                    switch (troop.state.v.getType())
                                    {
                                        case Troop.State.Type.Live:
                                            {
                                                switch (troop.troopType.v.getType())
                                                {
                                                    case TroopType.Type.Hero:
                                                        break;
                                                    case TroopType.Type.Normal:
                                                        {
                                                            TroopInformation troopInformation = troop.troopType.v as TroopInformation;
                                                            TroopInformation.Level level = troopInformation.levels.Find(check => check.level == troop.level.v);
                                                            if (level != null)
                                                            {
                                                                auraPrefab = level.auraPrefab;
                                                            }
                                                            else
                                                            {
                                                                Logger.LogError("level null");
                                                            }
                                                        }
                                                        break;
                                                    case TroopType.Type.Monster:
                                                        break;
                                                    default:
                                                        Logger.LogError("unknown type: "+troop.troopType.v.getType());
                                                        break;
                                                }
                                            }
                                            break;
                                        case Troop.State.Type.Die:
                                            break;
                                        default:
                                            Logger.LogError("unknown type: " + troop.state.v.getType());
                                            break;
                                    }
                                }
                                else
                                {
                                    Logger.LogError("troop null");
                                }
                            }
                            else
                            {
                                //Logger.LogError("troopUIData null");
                            }
                        }
                    }
                    // process
                    if (auraPrefab != null)
                    {
                        // find
                        bool needMakeNew = true;
                        {
                            if (oldPrefab == auraPrefab)
                            {
                                needMakeNew = false;
                            }
                        }
                        // make new
                        if(needMakeNew)
                        {
                            oldPrefab = auraPrefab;
                            oldAura = Instantiate(auraPrefab, this.transform);
                            oldAura.transform.localPosition = Vector3.zero;
                        }
                    }
                    else
                    {
                        oldPrefab = null;
                        if (oldAura != null)
                        {
                            Destroy(oldAura.gameObject);
                            oldAura = null;
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

        private TroopFollowUI.UIData troopFollowUIData = null;
        private TroopUI.UIData troopUIData = null;

        public override void onAddCallBack<T>(T data)
        {
            if(data is UIData)
            {
                UIData uiData = data as UIData;
                // Parent
                {
                    DataUtils.addParentCallBack(uiData, this, ref this.troopFollowUIData);
                    DataUtils.addParentCallBack(uiData, this, ref this.troopUIData);
                }
                dirty = true;
                return;
            }
            // Parent
            {
                // troopFollowUIData
                {
                    if (data is TroopFollowUI.UIData)
                    {
                        TroopFollowUI.UIData troopFollowUIData = data as TroopFollowUI.UIData;
                        // Child
                        {
                            troopFollowUIData.troopFollow.allAddCallBack(this);
                        }
                        dirty = true;
                        return;
                    }
                    // Child
                    if (data is TroopFollow)
                    {
                        dirty = true;
                        return;
                    }
                }
                // troopUIData
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
                    DataUtils.removeParentCallBack(uiData, this, ref this.troopFollowUIData);
                    DataUtils.removeParentCallBack(uiData, this, ref this.troopUIData);
                }
                this.setDataNull(uiData);
                return;
            }
            // Parent
            {
                // troopFollowUIData
                {
                    if (data is TroopFollowUI.UIData)
                    {
                        TroopFollowUI.UIData troopFollowUIData = data as TroopFollowUI.UIData;
                        // Child
                        {
                            troopFollowUIData.troopFollow.allRemoveCallBack(this);
                        }
                        return;
                    }
                    // Child
                    if (data is TroopFollow)
                    {
                        return;
                    }
                }
                // troopUIData
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
                // troopFollowUIData
                {
                    if (wrapProperty.p is TroopFollowUI.UIData)
                    {
                        switch ((TroopFollowUI.UIData.Property)wrapProperty.n)
                        {
                            case TroopFollowUI.UIData.Property.troopFollow:
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
                    if (wrapProperty.p is TroopFollow)
                    {
                        switch ((TroopFollow.Property)wrapProperty.n)
                        {
                            case TroopFollow.Property.level:
                                dirty = true;
                                break;
                            case TroopFollow.Property.troopType:
                                dirty = true;
                                break;
                            default:
                                break;
                        }
                        return;
                    }
                }
                // troopUIData
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
                            case Troop.Property.startPosition:
                                break;
                            case Troop.Property.formationPosition:
                                break;
                            case Troop.Property.teamId:
                                break;
                            case Troop.Property.level:
                                dirty = true;
                                break;
                            case Troop.Property.troopType:
                                dirty = true;
                                break;
                            case Troop.Property.worldPosition:
                                break;
                            case Troop.Property.state:
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