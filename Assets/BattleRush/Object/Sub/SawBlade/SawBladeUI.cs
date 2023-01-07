using System.Collections;
using System.Collections.Generic;
using BattleRushS.StateS;
using Dreamteck.Forever;
using UnityEngine;

namespace BattleRushS.ObjectS
{
    public class SawBladeUI : UIBehavior<SawBladeUI.UIData>, BattleRushUI.UIData.ObjectInPathUIInterface
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

            public VO<ReferenceData<SawBlade>> sawBlade;

            public VD<ObstructionUI.UIData> obstruction;

            #region Constructor

            public enum Property
            {
                sawBlade,
                obstruction
            }

            public UIData() : base()
            {
                this.sawBlade = new VO<ReferenceData<SawBlade>>(this, (byte)Property.sawBlade, new ReferenceData<SawBlade>(null));
                this.obstruction = new VD<ObstructionUI.UIData>(this, (byte)Property.obstruction, new ObstructionUI.UIData());
            }

            #endregion

            public override ObjectInPath.Type getType()
            {
                return ObjectInPath.Type.SawBlade;
            }

            public override ObjectInPath getObjectInPath()
            {
                return this.sawBlade.v.data;
            }

        }

        #endregion

        #region Refresh

        public Animator sawBladeAnimator;
        public Animator sawPivotAnimator;

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    SawBlade sawBlade = this.data.sawBlade.v.data;
                    if (sawBlade != null)
                    {
                        // Animator
                        {
                            if (sawBladeAnimator != null && sawPivotAnimator)
                            {
                                // find
                                bool needAnimation = false;
                                {
                                    BattleRush battleRush = sawBlade.findDataInParent<BattleRush>();
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
                                    sawBladeAnimator.enabled = needAnimation;
                                    sawPivotAnimator.enabled = needAnimation;
                                }
                            }
                            else
                            {
                                Logger.LogError("bladeAnimator null");
                            }
                        }
                        // obstruction
                        {
                            ObstructionUI.UIData obstructionUIData = this.data.obstruction.v;
                            if (obstructionUIData != null)
                            {
                                obstructionUIData.obstruction.v = new ReferenceData<Obstruction>(sawBlade.obstruction.v);
                            }
                            else
                            {
                                Logger.LogError("obstructionUIData null");
                            }
                        }
                    }
                    else
                    {
                        Logger.LogError("sawBlade null");
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
                    SawBlade sawBlade = this.data.sawBlade.v.data;
                    if (sawBlade != null)
                    {
                        BattleRush battleRush = sawBlade.findDataInParent<BattleRush>();
                        if (battleRush != null)
                        {
                            battleRush.laneObjects.remove(sawBlade);
                        }
                    }
                    else
                    {
                        Logger.LogError("sawBlade null");
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
                    uiData.sawBlade.allAddCallBack(this);
                    uiData.obstruction.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            {
                // sawBlade
                {
                    if (data is SawBlade)
                    {
                        SawBlade sawBlade = data as SawBlade;
                        // Parent
                        {
                            DataUtils.addParentCallBack(sawBlade, this, ref this.battleRush);
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
                    uiData.sawBlade.allRemoveCallBack(this);
                    uiData.obstruction.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // Child
            {
                // sawBlade
                {
                    if (data is SawBlade)
                    {
                        SawBlade sawBlade = data as SawBlade;
                        // Parent
                        {
                            DataUtils.removeParentCallBack(sawBlade, this, ref this.battleRush);
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
                    case UIData.Property.sawBlade:
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
                // sawBlade
                {
                    if (wrapProperty.p is SawBlade)
                    {
                        switch ((SawBlade.Property)wrapProperty.n)
                        {
                            case SawBlade.Property.position:
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
