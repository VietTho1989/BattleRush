using System.Collections;
using System.Collections.Generic;
using BattleRushS.StateS;
using Dreamteck.Forever;
using UnityEngine;

namespace BattleRushS.ObjectS
{
    public class BladeUI : UIBehavior<BladeUI.UIData>, BattleRushUI.UIData.ObjectInPathUIInterface
    {

        public void setObjectInPathUIData(Data data)
        {
            if(data is UIData)
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
            return ObjectInPath.Type.Blade;
        }

        #region UIData

        public class UIData : BattleRushUI.UIData.ObjectInPathUI
        {

            public VO<ReferenceData<Blade>> blade;

            public VD<ObstructionUI.UIData> obstruction;

            #region Constructor

            public enum Property
            {
                blade,
                obstruction
            }

            public UIData() : base()
            {
                this.blade = new VO<ReferenceData<Blade>>(this, (byte)Property.blade, new ReferenceData<Blade>(null));
                this.obstruction = new VD<ObstructionUI.UIData>(this, (byte)Property.obstruction, new ObstructionUI.UIData());
            }

            #endregion

            public override ObjectInPath.Type getType()
            {
                return ObjectInPath.Type.Blade;
            }

            public override ObjectInPath getObjectInPath()
            {
                return this.blade.v.data;
            }

        }

        #endregion

        #region Refresh

        public Animator bladeAnimator;
        public ParticleSystem bladeSpinParticleSystem;

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    Blade blade = this.data.blade.v.data;
                    if (blade != null)
                    {
                        // bladeAnimator
                        {
                            if (bladeAnimator != null && bladeSpinParticleSystem!=null)
                            {
                                // find
                                bool needAnimation = false;
                                {
                                    BattleRush battleRush = blade.findDataInParent<BattleRush>();
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
                                    bladeAnimator.enabled = needAnimation;
                                    bladeSpinParticleSystem.gameObject.SetActive(needAnimation);
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
                                obstructionUIData.obstruction.v = new ReferenceData<Obstruction>(blade.obstruction.v);
                            }
                            else
                            {
                                Logger.LogError("obstructionUIData null");
                            }
                        }
                    }
                    else
                    {
                        Logger.LogError("blade null");
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
                    Blade blade = this.data.blade.v.data;
                    if (blade != null)
                    {
                        BattleRush battleRush = blade.findDataInParent<BattleRush>();
                        if (battleRush != null)
                        {
                            battleRush.laneObjects.remove(blade);
                        }
                    }
                    else
                    {
                        Logger.LogError("blade null");
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
                    uiData.blade.allAddCallBack(this);
                    uiData.obstruction.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            {
                // blade
                {
                    if (data is Blade)
                    {
                        Blade blade = data as Blade;
                        // Parent
                        {
                            DataUtils.addParentCallBack(blade, this, ref this.battleRush);
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
                    uiData.blade.allRemoveCallBack(this);
                    uiData.obstruction.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // Child
            {
                // blade
                {
                    if (data is Blade)
                    {
                        Blade blade = data as Blade;
                        // Parent
                        {
                            DataUtils.removeParentCallBack(blade, this, ref this.battleRush);
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
                    case UIData.Property.blade:
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
                // blade
                {
                    if (wrapProperty.p is Blade)
                    {
                        switch ((Blade.Property)wrapProperty.n)
                        {
                            case Blade.Property.position:
                                dirty = true;
                                break;
                            case Blade.Property.obstruction:
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
                        if(wrapProperty.p is BattleRush.State)
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
                                    Logger.LogError("unknown type: "+state.getType());
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
