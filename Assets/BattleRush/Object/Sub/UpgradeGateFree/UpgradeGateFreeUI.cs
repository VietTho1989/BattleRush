using System.Collections;
using System.Collections.Generic;
using Dreamteck.Forever;
using UnityEngine;

namespace BattleRushS.ObjectS
{
    public class UpgradeGateFreeUI : UIBehavior<UpgradeGateFreeUI.UIData>, BattleRushUI.UIData.ObjectInPathUIInterface
    {

        public void setObjectInPathUIData(Data data)
        {
            if (data is UIData)
            {
                this.setData((UIData)data);
            }
        }

        public GameObject getMyGameObject()
        {
            return this.gameObject;
        }

        public ObjectInPath.Type getType()
        {
            return ObjectInPath.Type.UpgradeGateFree;
        }

        #region UIData

        public class UIData : BattleRushUI.UIData.ObjectInPathUI
        {

            public VO<ReferenceData<UpgradeGateFree>> upgradeGateFree;

            #region Constructor

            public enum Property
            {
                upgradeGateFree
            }

            public UIData() : base()
            {
                this.upgradeGateFree = new VO<ReferenceData<UpgradeGateFree>>(this, (byte)Property.upgradeGateFree, new ReferenceData<UpgradeGateFree>(null));
            }

            #endregion

            public override ObjectInPath.Type getType()
            {
                return ObjectInPath.Type.UpgradeGateFree;
            }

            public override ObjectInPath getObjectInPath()
            {
                return this.upgradeGateFree.v.data;
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
                    UpgradeGateFree upgradeGateFree = this.data.upgradeGateFree.v.data;
                    if (upgradeGateFree != null)
                    {
                        // set position
                        {
                            // find levelGenerator
                            if (levelGenerator == null)
                            {
                                BattleRush battleRush = upgradeGateFree.findDataInParent<BattleRush>();
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
                                /*Vector3 position = MakeObjectUpdate.GetPositionInLevelGenerator(levelGenerator, upgradeGateFree.P.v.z);
                                this.transform.position = new Vector3(position.x, position.y + 0.9f, position.z);*/
                            }
                        }
                    }
                    else
                    {
                        Logger.LogError("upgradeGateFree null");
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
                    UpgradeGateFree upgradeGateFree = this.data.upgradeGateFree.v.data;
                    if (upgradeGateFree != null)
                    {
                        BattleRush battleRush = upgradeGateFree.findDataInParent<BattleRush>();
                        if (battleRush != null)
                        {
                            battleRush.laneObjects.remove(upgradeGateFree);
                        }
                    }
                    else
                    {
                        Logger.LogError("upgradeGateFree null");
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
                    uiData.upgradeGateFree.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            {
                if (data is UpgradeGateFree)
                {
                    UpgradeGateFree upgradeGateFree = data as UpgradeGateFree;
                    // Parent
                    {
                        DataUtils.addParentCallBack(upgradeGateFree, this, ref this.battleRush);
                    }
                    dirty = true;
                    return;
                }
                // Parent
                if (data is BattleRush)
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
                    uiData.upgradeGateFree.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // Child
            {
                if (data is UpgradeGateFree)
                {
                    UpgradeGateFree upgradeGateFree = data as UpgradeGateFree;
                    // Parent
                    {
                        DataUtils.removeParentCallBack(upgradeGateFree, this, ref this.battleRush);
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
                    case UIData.Property.upgradeGateFree:
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
                if (wrapProperty.p is UpgradeGateFree)
                {
                    switch ((UpgradeGateFree.Property)wrapProperty.n)
                    {
                        case UpgradeGateFree.Property.position:
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
