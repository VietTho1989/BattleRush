using System.Collections;
using System.Collections.Generic;
using BattleRushS.HeroS;
using UnityEngine;

namespace BattleRushS.ArenaS.TroopS
{
    public class TroopAnimationUI : UIBehavior<TroopAnimationUI.UIData>
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

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    TroopUI.UIData troopUIData = this.data.findDataInParent<TroopUI.UIData>();
                    if (troopUIData != null)
                    {
                        TroopUI troopUI = troopUIData.findCallBack<TroopUI>();
                        if (troopUI != null)
                        {
                            TroopInformation troopInformation = troopUI.getCurrentTroopTypeModel();
                            if (troopInformation != null)
                            {
                                if (troopInformation.troopAnimator != null)
                                {
                                    Troop troop = troopUIData.troop.v.data;
                                    if (troop != null)
                                    {
                                        switch (troop.state.v.getType())
                                        {
                                            case Troop.State.Type.Live:
                                                {
                                                    // Idle, Move, Attack
                                                    Live live = troop.state.v as Live;
                                                    // attack
                                                    {
                                                        // find
                                                        bool isAttack = false;
                                                        {
                                                            switch (live.troopAttack.v.state.v.getType())
                                                            {
                                                                case TroopAttack.State.Type.Normal:
                                                                    break;
                                                                case TroopAttack.State.Type.Animation:
                                                                    isAttack = true;
                                                                    break;
                                                                default:
                                                                    Logger.LogError("unknown type: "+ live.troopAttack.v.state.v.getType());
                                                                    break;
                                                            }
                                                        }
                                                        // process
                                                        {
                                                            if (isAttack)
                                                            {
                                                                troopInformation.troopAnimator.Play("Attack");
                                                            }
                                                            else
                                                            {
                                                                // Move or Idle
                                                                bool isMove = false;
                                                                {
                                                                    // TODO can hoan thien
                                                                }
                                                                // process
                                                                if (isMove)
                                                                {
                                                                    troopInformation.troopAnimator.Play("Move");
                                                                }
                                                                else
                                                                {
                                                                    troopInformation.troopAnimator.Play("Idle");
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                break;
                                            case Troop.State.Type.Die:
                                                {
                                                    troopInformation.troopAnimator.Play("Die");
                                                }
                                                break;
                                            default:
                                                Logger.LogError("unknown state: " + troop.state.v.getType());
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
                                    Logger.LogError("troopAnimator null");
                                }                               
                            }
                            else
                            {
                                Logger.LogError("currentTroopTypeModel null");
                            }
                        }
                        else
                        {
                            Logger.LogError("troopUI null");
                        }
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
        }

        public override bool isShouldDisableUpdate()
        {
            return true;
        }

        #endregion

        #region implement callBacks

        private TroopUI.UIData troopUIData = null;

        public override void onAddCallBack<T>(T data)
        {
            if(data is UIData)
            {
                UIData uiData = data as UIData;
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
                {
                    if(data is Troop)
                    {
                        Troop troop = data as Troop;
                        // Child
                        {
                            troop.state.allAddCallBack(this);
                        }
                        dirty = true;
                        return;
                    }
                    // Child
                    {
                        if(data is Troop.State)
                        {
                            Troop.State state = data as Troop.State;
                            // Child
                            {
                                switch (state.getType())
                                {
                                    case Troop.State.Type.Live:
                                        {
                                            Live live = state as Live;
                                            live.troopAttack.allAddCallBack(this);
                                        }
                                        break;
                                    case Troop.State.Type.Die:
                                        break;
                                    default:
                                        Logger.LogError("unknown type: " + state.getType());
                                        break;
                                }
                            }
                            dirty = true;
                            return;
                        }
                        // Child
                        if(data is TroopAttack)
                        {
                            dirty = true;
                            return;
                        }
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
                {
                    if (data is Troop)
                    {
                        Troop troop = data as Troop;
                        // Child
                        {
                            troop.state.allRemoveCallBack(this);
                        }
                        return;
                    }
                    // Child
                    {
                        if (data is Troop.State)
                        {
                            Troop.State state = data as Troop.State;
                            // Child
                            {
                                switch (state.getType())
                                {
                                    case Troop.State.Type.Live:
                                        {
                                            Live live = state as Live;
                                            live.troopAttack.allRemoveCallBack(this);
                                        }
                                        break;
                                    case Troop.State.Type.Die:
                                        break;
                                    default:
                                        Logger.LogError("unknown type: " + state.getType());
                                        break;
                                }
                            }
                            return;
                        }
                        // Child
                        if (data is TroopAttack)
                        {
                            return;
                        }
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
                {
                    if (wrapProperty.p is Troop)
                    {
                        switch ((Troop.Property)wrapProperty.n)
                        {
                            case Troop.Property.state:
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
                        if (wrapProperty.p is Troop.State)
                        {
                            Troop.State state = wrapProperty.p as Troop.State;
                            // Child
                            {
                                switch (state.getType())
                                {
                                    case Troop.State.Type.Live:
                                        {
                                            switch ((Live.Property)wrapProperty.n)
                                            {
                                                case Live.Property.troopAttack:
                                                    {
                                                        ValueChangeUtils.replaceCallBack(this, syncs);
                                                        dirty = true;
                                                    }
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                        break;
                                    case Troop.State.Type.Die:
                                        break;
                                    default:
                                        Logger.LogError("unknown type: " + state.getType());
                                        break;
                                }
                            }
                            dirty = true;
                            return;
                        }
                        // Child
                        if (wrapProperty.p is TroopAttack)
                        {
                            switch ((TroopAttack.Property)wrapProperty.n)
                            {
                                case TroopAttack.Property.state:
                                    dirty = true;
                                    break;
                                default:
                                    break;
                            }
                            return;
                        }
                    }
                }
            }
            Logger.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

    }
}