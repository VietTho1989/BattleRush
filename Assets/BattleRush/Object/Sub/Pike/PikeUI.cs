using System.Collections;
using System.Collections.Generic;
using Dreamteck.Forever;
using UnityEngine;

namespace BattleRushS.ObjectS
{
    public class PikeUI : UIBehavior<PikeUI.UIData>, BattleRushUI.UIData.ObjectInPathUIInterface
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
            return ObjectInPath.Type.Pike;
        }

        #region UIData

        public class UIData : BattleRushUI.UIData.ObjectInPathUI
        {

            public VO<ReferenceData<Pike>> pike;

            public VD<ObstructionUI.UIData> obstruction;

            #region Constructor

            public enum Property
            {
                pike,
                obstruction
            }

            public UIData() : base()
            {
                this.pike = new VO<ReferenceData<Pike>>(this, (byte)Property.pike, new ReferenceData<Pike>(null));
                this.obstruction = new VD<ObstructionUI.UIData>(this, (byte)Property.obstruction, new ObstructionUI.UIData());
            }

            #endregion

            public override ObjectInPath.Type getType()
            {
                return ObjectInPath.Type.Pike;
            }

            public override ObjectInPath getObjectInPath()
            {
                return this.pike.v.data;
            }

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
                    Pike pike = this.data.pike.v.data;
                    if (pike != null)
                    {
                        // obstruction
                        {
                            ObstructionUI.UIData obstructionUIData = this.data.obstruction.v;
                            if (obstructionUIData != null)
                            {
                                obstructionUIData.obstruction.v = new ReferenceData<Obstruction>(pike.obstruction.v);
                            }
                            else
                            {
                                Logger.LogError("obstructionUIData null");
                            }
                        }
                    }
                    else
                    {
                        Logger.LogError("pike null");
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
                    Pike pike = this.data.pike.v.data;
                    if (pike != null)
                    {
                        BattleRush battleRush = pike.findDataInParent<BattleRush>();
                        if (battleRush != null)
                        {
                            battleRush.laneObjects.remove(pike);
                        }
                    }
                    else
                    {
                        Logger.LogError("pike null");
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

        public ObstructionUI obstructionUI;

        public override void onAddCallBack<T>(T data)
        {
            if (data is UIData)
            {
                UIData uiData = data as UIData;
                // Child
                {
                    uiData.pike.allAddCallBack(this);
                    uiData.obstruction.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            {
                // pike
                {
                    if (data is Pike)
                    {
                        Pike pike = data as Pike;
                        // Parent
                        {
                            DataUtils.addParentCallBack(pike, this, ref this.battleRush);
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
                if(data is ObstructionUI.UIData)
                {
                    ObstructionUI.UIData obstructionUIData = data as ObstructionUI.UIData;
                    // UI
                    {
                        if (obstructionUI != null)
                        {
                            obstructionUI.setData(obstructionUIData);
                        }
                        else
                        {
                            Logger.LogError("obstructionUI null");
                        }
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
                    uiData.pike.allRemoveCallBack(this);
                    uiData.obstruction.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // Child
            {
                // pike
                {
                    if (data is Pike)
                    {
                        Pike pike = data as Pike;
                        // Parent
                        {
                            DataUtils.removeParentCallBack(pike, this, ref this.battleRush);
                        }
                        return;
                    }
                    // Parent
                    if (data is BattleRush)
                    {
                        return;
                    }
                }
                if (data is ObstructionUI.UIData)
                {
                    ObstructionUI.UIData obstructionUIData = data as ObstructionUI.UIData;
                    // UI
                    {
                        if (obstructionUI != null)
                        {
                            obstructionUI.setDataNull(obstructionUIData);
                        }
                        else
                        {
                            Logger.LogError("obstructionUI null");
                        }
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
                    case UIData.Property.pike:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case UIData.Property.obstruction:
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
                // pike
                {
                    if (wrapProperty.p is Pike)
                    {
                        switch ((Pike.Property)wrapProperty.n)
                        {
                            case Pike.Property.position:
                                dirty = true;
                                break;
                            case Pike.Property.obstruction:
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
                if (wrapProperty.p is ObstructionUI.UIData)
                {
                    return;
                }
            }
            Logger.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

    }
}
