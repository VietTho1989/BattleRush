using System.Collections;
using System.Collections.Generic;
using Dreamteck.Forever;
using UnityEngine;

namespace BattleRushS.ObjectS
{
    public class UpgradeGateChargeUI : UIBehavior<UpgradeGateChargeUI.UIData>, BattleRushUI.UIData.ObjectInPathUIInterface
    {

        public void setObjectInPathUIData(Data data)
        {
            if (data is UIData)
            {
                this.setData((UIData)data);
            }
        }

        #region UIData

        public class UIData : BattleRushUI.UIData.ObjectInPathUI
        {

            public VO<ReferenceData<UpgradeGateCharge>> upgradeGateCharge;

            #region Constructor

            public enum Property
            {
                upgradeGateCharge
            }

            public UIData() : base()
            {
                this.upgradeGateCharge = new VO<ReferenceData<UpgradeGateCharge>>(this, (byte)Property.upgradeGateCharge, new ReferenceData<UpgradeGateCharge>(null));
            }

            #endregion

            public override ObjectInPath.Type getType()
            {
                return ObjectInPath.Type.UpgradeGateCharge;
            }

            public override ObjectInPath getObjectInPath()
            {
                return this.upgradeGateCharge.v.data;
            }

        }

        #endregion

        #region Refresh

        private LevelGenerator levelGenerator = null;

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    UpgradeGateCharge upgradeGateCharge = this.data.upgradeGateCharge.v.data;
                    if (upgradeGateCharge != null)
                    {
                        // set position
                        {
                            // find levelGenerator
                            if (levelGenerator == null)
                            {
                                BattleRush battleRush = upgradeGateCharge.findDataInParent<BattleRush>();
                                if (battleRush != null)
                                {
                                    BattleRushUI battleRushUI = battleRush.findCallBack<BattleRushUI>();
                                    if (battleRushUI != null)
                                    {
                                        levelGenerator = battleRushUI.GetComponent<LevelGenerator>();
                                    }
                                    else
                                    {
                                        Logger.LogError("battleRushUI null");
                                    }
                                }
                                else
                                {
                                    Logger.LogError("battleRush null");
                                }
                            }
                            // set position
                            if (levelGenerator != null)
                            {
                                /*Vector3 position = MakeObjectUpdate.GetPositionInLevelGenerator(levelGenerator, upgradeGateCharge.P.v.z);
                                this.transform.position = new Vector3(position.x, position.y + 0.9f, position.z);*/
                            }
                        }
                    }
                    else
                    {
                        Logger.LogError("upgradeGateCharge null");
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

        public override void OnDestroy()
        {
            base.OnDestroy();
            // remove: auto delete
            {
                if (this.data != null)
                {
                    UpgradeGateCharge upgradeGateCharge = this.data.upgradeGateCharge.v.data;
                    if (upgradeGateCharge != null)
                    {
                        BattleRush battleRush = upgradeGateCharge.findDataInParent<BattleRush>();
                        if (battleRush != null)
                        {
                            battleRush.laneObjects.remove(upgradeGateCharge);
                        }
                    }
                    else
                    {
                        Logger.LogError("upgradeGateCharge null");
                    }
                }
                else
                {
                    Logger.LogError("data null");
                }
            }
        }

        #region implement callBacks

        private BattleRush battleRush = null;

        public override void onAddCallBack<T>(T data)
        {
            if (data is UIData)
            {
                UIData uiData = data as UIData;
                // Child
                {
                    uiData.upgradeGateCharge.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            {
                if (data is UpgradeGateCharge)
                {
                    UpgradeGateCharge upgradeGateCharge = data as UpgradeGateCharge;
                    // Parent
                    {
                        DataUtils.addParentCallBack(upgradeGateCharge, this, ref this.battleRush);
                    }
                    dirty = true;
                    return;
                }
                // Parent
                if(data is BattleRush)
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
                    uiData.upgradeGateCharge.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // Child
            {
                if (data is UpgradeGateCharge)
                {
                    UpgradeGateCharge upgradeGateCharge = data as UpgradeGateCharge;
                    // Parent
                    {
                        DataUtils.removeParentCallBack(upgradeGateCharge, this, ref this.battleRush);
                    }
                    return;
                }
                // Parent
                if (data is BattleRush)
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
                    case UIData.Property.upgradeGateCharge:
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
                if (wrapProperty.p is UpgradeGateCharge)
                {
                    switch ((UpgradeGateCharge.Property)wrapProperty.n)
                    {
                        case UpgradeGateCharge.Property.P:
                            dirty = true;
                            break;
                        case UpgradeGateCharge.Property.R:
                            dirty = true;
                            break;
                        default:
                            Logger.LogError("Don't process: " + wrapProperty + "; " + this);
                            break;
                    }
                    return;
                }
                // Parent
                if (wrapProperty.p is BattleRush)
                {
                    switch ((BattleRush.Property)wrapProperty.n)
                    {
                        case BattleRush.Property.segmentIndex:
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
