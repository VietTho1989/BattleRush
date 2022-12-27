using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ObjectS.TroopCageS
{
    public class PrisonAnimationUI : UIBehavior<PrisonAnimationUI.UIData>
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

        public Animator prisonAnimator;

        public const string AnimationHit = "hit";
        public const string AnimationIdle = "idle";
        public const string AnimationDie = "die";

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    // dirty = true;
                    if (prisonAnimator != null)
                    {
                        // find
                        string currentAnimation = "";
                        {
                            AnimatorClipInfo[] clipInfo = prisonAnimator.GetCurrentAnimatorClipInfo(0);
                            if(clipInfo!=null && clipInfo.Length > 0)
                            {
                                currentAnimation = clipInfo[0].clip.name;
                            }
                            Logger.Log("prisonAnimator: " + currentAnimation);
                        }                        
                        TroopCageUI.UIData troopCageUIData = this.data.findDataInParent<TroopCageUI.UIData>();
                        if (troopCageUIData != null)
                        {
                            TroopCage troopCage = troopCageUIData.troopCage.v.data;
                            if (troopCage != null)
                            {
                                switch (troopCage.state.v.getType())
                                {
                                    case TroopCage.State.Type.Live:
                                        {
                                            Live live = troopCage.state.v as Live;
                                            // find
                                            bool isHit = false;
                                            {
                                                foreach(TroopCageShootProjectile projectile in live.projectiles.vs)
                                                {
                                                    if(projectile.state.v.getType()== TroopCageShootProjectile.State.Type.Hit)
                                                    {
                                                        isHit = true;
                                                        break;
                                                    }
                                                }
                                            }
                                            // process
                                            {
                                                if (isHit)
                                                {
                                                    Logger.Log("troop cage is hit, show animation");
                                                    prisonAnimator.Play(AnimationHit);
                                                }
                                                else
                                                {
                                                    // TODO can check dang play animation hit hay khong
                                                    if (currentAnimation != AnimationHit)
                                                    {
                                                        prisonAnimator.Play(AnimationIdle);
                                                    }
                                                    else
                                                    {
                                                        Logger.Log("You are hitting");
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case TroopCage.State.Type.ReleaseTroop:
                                        {
                                            prisonAnimator.Play(AnimationDie);
                                        }
                                        break;
                                    case TroopCage.State.Type.Destroyed:
                                        {
                                            // dat o cuoi animation die
                                            prisonAnimator.Play(AnimationDie, 0, 1);
                                        }
                                        break;
                                    default:
                                        Logger.LogError("unknown state: " + troopCage.state.v.getType());
                                        break;
                                }
                            }
                            else
                            {
                                Logger.LogError("troopCage null");
                            }
                        }
                        else
                        {
                            Logger.LogError("troopCageUIData null");
                        }
                    }
                    else
                    {
                        Logger.LogError("prisonAnimator null");
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

        #region implement callBacks

        private TroopCageUI.UIData troopCageUIData = null;

        public override void onAddCallBack<T>(T data)
        {
            if(data is UIData)
            {
                UIData uiData = data as UIData;
                // Parent
                {
                    DataUtils.addParentCallBack(uiData, this, ref this.troopCageUIData);
                }
                dirty = true;
                return;
            }
            // Parent
            {
                if(data is TroopCageUI.UIData)
                {
                    TroopCageUI.UIData troopCageUIData = data as TroopCageUI.UIData;
                    // Child
                    {
                        troopCageUIData.troopCage.allAddCallBack(this);
                    }
                    dirty = true;
                    return;
                }
                // Child
                {
                    if(data is TroopCage)
                    {
                        TroopCage troopCage = data as TroopCage;
                        // Child
                        {
                            troopCage.state.allAddCallBack(this);
                        }
                        dirty = true;
                        return;
                    }
                    // Child
                    {
                        if(data is TroopCage.State)
                        {
                            TroopCage.State state = data as TroopCage.State;
                            // Child
                            {
                                switch (state.getType())
                                {
                                    case TroopCage.State.Type.Live:
                                        {
                                            Live live = state as Live;
                                            live.projectiles.allAddCallBack(this);
                                        }
                                        break;
                                    case TroopCage.State.Type.ReleaseTroop:
                                        break;
                                    case TroopCage.State.Type.Destroyed:
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
                        if(data is TroopCageShootProjectile)
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
                    DataUtils.removeParentCallBack(uiData, this, ref this.troopCageUIData);
                }
                this.setDataNull(uiData);
                return;
            }
            // Parent
            {
                if (data is TroopCageUI.UIData)
                {
                    TroopCageUI.UIData troopCageUIData = data as TroopCageUI.UIData;
                    // Child
                    {
                        troopCageUIData.troopCage.allRemoveCallBack(this);
                    }
                    return;
                }
                // Child
                {
                    if (data is TroopCage)
                    {
                        TroopCage troopCage = data as TroopCage;
                        // Child
                        {
                            troopCage.state.allRemoveCallBack(this);
                        }
                        return;
                    }
                    // Child
                    {
                        if (data is TroopCage.State)
                        {
                            TroopCage.State state = data as TroopCage.State;
                            // Child
                            {
                                switch (state.getType())
                                {
                                    case TroopCage.State.Type.Live:
                                        {
                                            Live live = state as Live;
                                            live.projectiles.allRemoveCallBack(this);
                                        }
                                        break;
                                    case TroopCage.State.Type.ReleaseTroop:
                                        break;
                                    case TroopCage.State.Type.Destroyed:
                                        break;
                                    default:
                                        Logger.LogError("unknown type: " + state.getType());
                                        break;
                                }
                            }
                            return;
                        }
                        // Child
                        if (data is TroopCageShootProjectile)
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
                switch((UIData.Property)wrapProperty.n)
                {
                    default:
                        break;
                }
                return;
            }
            // Parent
            {
                if (wrapProperty.p is TroopCageUI.UIData)
                {
                    switch ((TroopCageUI.UIData.Property)wrapProperty.n)
                    {
                        case TroopCageUI.UIData.Property.troopCage:
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
                    if (wrapProperty.p is TroopCage)
                    {
                        switch ((TroopCage.Property)wrapProperty.n)
                        {
                            case TroopCage.Property.state:
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
                        if (wrapProperty.p is TroopCage.State)
                        {
                            TroopCage.State state = wrapProperty.p as TroopCage.State;
                            switch (state.getType())
                            {
                                case TroopCage.State.Type.Live:
                                    {
                                        switch ((Live.Property)wrapProperty.n)
                                        {
                                            case Live.Property.projectiles:
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
                                case TroopCage.State.Type.ReleaseTroop:
                                    break;
                                case TroopCage.State.Type.Destroyed:
                                    break;
                                default:
                                    Logger.LogError("unknown type: " + state.getType());
                                    break;
                            }
                            return;
                        }
                        // Child
                        if (wrapProperty.p is TroopCageShootProjectile)
                        {
                            switch ((TroopCageShootProjectile.Property)wrapProperty.n)
                            {
                                case TroopCageShootProjectile.Property.state:
                                    Logger.Log("TroopCageShootProjectile state change: " + wrapProperty.getValue());
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