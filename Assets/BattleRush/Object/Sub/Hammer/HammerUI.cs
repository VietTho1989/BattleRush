using System.Collections;
using System.Collections.Generic;
using BattleRushS.StateS;
using Dreamteck.Forever;
using UnityEngine;

namespace BattleRushS.ObjectS
{
    public class HammerUI : UIBehavior<HammerUI.UIData>, BattleRushUI.UIData.ObjectInPathUIInterface
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

            public VO<ReferenceData<Hammer>> hammer;

            public VD<ObstructionUI.UIData> obstruction;

            #region Constructor

            public enum Property
            {
                hammer,
                obstruction
            }

            public UIData() : base()
            {
                this.hammer = new VO<ReferenceData<Hammer>>(this, (byte)Property.hammer, new ReferenceData<Hammer>(null));
                this.obstruction = new VD<ObstructionUI.UIData>(this, (byte)Property.obstruction, new ObstructionUI.UIData());
            }

            #endregion

            public override ObjectInPath.Type getType()
            {
                return ObjectInPath.Type.Hammer;
            }

            public override ObjectInPath getObjectInPath()
            {
                return this.hammer.v.data;
            }

        }

        #endregion

        #region Refresh

        public Animator hammerAnimator;

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    Hammer hammer = this.data.hammer.v.data;
                    if (hammer != null)
                    {
                        // Animator
                        {
                            if (hammerAnimator != null)
                            {
                                // find
                                bool needAnimation = false;
                                {
                                    BattleRush battleRush = hammer.findDataInParent<BattleRush>();
                                    if (battleRush != null)
                                    {
                                        switch (battleRush.state.v.getType())
                                        {
                                            case BattleRush.State.Type.Load:
                                                break;
                                            case BattleRush.State.Type.Start:
                                                break;
                                            case BattleRush.State.Type.Play:
                                                {
                                                    Play play = battleRush.state.v as Play;
                                                    switch (play.state.v)
                                                    {
                                                        case Play.State.Normal:
                                                            needAnimation = true;
                                                            break;
                                                        case Play.State.Pause:
                                                            break;
                                                        default:
                                                            Logger.LogError("unknown state: " + play.state.v);
                                                            break;
                                                    }
                                                }
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
                                {
                                    hammerAnimator.enabled = needAnimation;
                                }
                            }
                            else
                            {
                                Logger.LogError("hammerAnimator null");
                            }
                        }
                        // obstruction
                        {
                            ObstructionUI.UIData obstructionUIData = this.data.obstruction.v;
                            if (obstructionUIData != null)
                            {
                                obstructionUIData.obstruction.v = new ReferenceData<Obstruction>(hammer.obstruction.v);
                            }
                            else
                            {
                                Logger.LogError("obstructionUIData null");
                            }
                        }
                    }
                    else
                    {
                        Logger.LogError("hammer null");
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
                    Hammer hammer = this.data.hammer.v.data;
                    if (hammer != null)
                    {
                        BattleRush battleRush = hammer.findDataInParent<BattleRush>();
                        if (battleRush != null)
                        {
                            battleRush.laneObjects.remove(hammer);
                        }
                    }
                    else
                    {
                        Logger.LogError("hammer null");
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
                    uiData.hammer.allAddCallBack(this);
                    uiData.obstruction.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            {
                // hammer
                {
                    if (data is Hammer)
                    {
                        Hammer hammer = data as Hammer;
                        // Parent
                        {
                            DataUtils.addParentCallBack(hammer, this, ref this.battleRush);
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
                }
                if(data is ObstructionUI.UIData)
                {
                    ObstructionUI.UIData obstractionUIData = data as ObstructionUI.UIData;
                    // UI
                    {
                        if (obstructionUI != null)
                        {
                            obstructionUI.setData(obstractionUIData);
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
                    uiData.hammer.allRemoveCallBack(this);
                    uiData.obstruction.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // Child
            {
                // hammer
                {
                    if (data is Hammer)
                    {
                        Hammer hammer = data as Hammer;
                        // Parent
                        {
                            DataUtils.removeParentCallBack(hammer, this, ref this.battleRush);
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
                        if(data is BattleRush.State)
                        {
                            return;
                        }
                    }
                }
                if (data is ObstructionUI.UIData)
                {
                    ObstructionUI.UIData obstractionUIData = data as ObstructionUI.UIData;
                    // UI
                    {
                        if (obstructionUI != null)
                        {
                            obstructionUI.setDataNull (obstractionUIData);
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
                    case UIData.Property.hammer:
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
                // hammer
                {
                    if (wrapProperty.p is Hammer)
                    {
                        switch ((Hammer.Property)wrapProperty.n)
                        {
                            case Hammer.Property.position:
                                dirty = true;
                                break;
                            case Hammer.Property.obstruction:
                                dirty = true;
                                break;
                            default:
                                Logger.LogError("Don't process: " + wrapProperty + "; " + this);
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
                                case BattleRush.Property.segmentIndex:
                                    dirty = true;
                                    break;
                                case BattleRush.Property.state:
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
                        if (wrapProperty.p is BattleRush.State)
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
                                default:
                                    Logger.LogError("unknown type: " + state.getType());
                                    break;
                            }
                            return;
                        }
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
