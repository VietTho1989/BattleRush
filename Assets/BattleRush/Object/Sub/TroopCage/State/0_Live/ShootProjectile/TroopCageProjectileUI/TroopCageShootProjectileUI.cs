using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ObjectS.TroopCageS
{
    public class TroopCageShootProjectileUI : UIBehavior<TroopCageShootProjectileUI.UIData>
    {

        #region UIData

        public class UIData : Data
        {

            public VO<ReferenceData<TroopCageShootProjectile>> projectile;

            #region type

            public enum Type
            {
                Normal,
                Power,
                Upgrade
            }

            public VO<Type> type;

            #endregion

            #region Constructor

            public enum Property
            {
                projectile,
                type
            }

            public UIData() : base()
            {
                this.projectile = new VO<ReferenceData<TroopCageShootProjectile>>(this, (byte)Property.projectile, new ReferenceData<TroopCageShootProjectile>(null));
                this.type = new VO<Type>(this, (byte)Property.type, Type.Normal);
            }

            #endregion

        }

        #endregion

        #region Refresh

        public GameObject normal;
        public GameObject power;
        public GameObject upgrade;

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    TroopCageShootProjectile troopCageShootProjectile = this.data.projectile.v.data;
                    if (troopCageShootProjectile != null)
                    {
                        // set position
                        {
                            float t = 0;
                            {
                                switch (troopCageShootProjectile.state.v.getType())
                                {
                                    case TroopCageShootProjectile.State.Type.Move:
                                        {
                                            TroopCageShootProjectile.State.Move move = troopCageShootProjectile.state.v as TroopCageShootProjectile.State.Move;
                                            t = move.time.v / move.duration.v;
                                        }
                                        break;
                                    case TroopCageShootProjectile.State.Type.Hit:
                                        t = 1;
                                        break;
                                    case TroopCageShootProjectile.State.Type.Finish:
                                        t = 1;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            this.transform.position = Vector3.Lerp(troopCageShootProjectile.startPosition.v, this.transform.parent.position, t);
                        }
                        // type
                        {
                            if(normal!=null && power!=null && upgrade != null)
                            {
                                switch (this.data.type.v)
                                {
                                    case UIData.Type.Normal:
                                        {
                                            normal.SetActive(true);
                                            power.SetActive(false);
                                            upgrade.SetActive(false);
                                        }
                                        break;
                                    case UIData.Type.Power:
                                        {
                                            normal.SetActive(false);
                                            power.SetActive(true);
                                            upgrade.SetActive(false);
                                        }
                                        break;
                                    case UIData.Type.Upgrade:
                                        {
                                            normal.SetActive(false);
                                            power.SetActive(false);
                                            upgrade.SetActive(true);
                                        }
                                        break;
                                    default:
                                        Logger.LogError("unknown type: " + this.data.type.v);
                                        break;
                                }
                            }
                            else
                            {
                                Logger.LogError("type null");
                            }
                        }
                    }
                    else
                    {
                        Logger.LogError("troopCageShootProjectile null");
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

        public override void onAddCallBack<T>(T data)
        {
            if (data is UIData)
            {
                UIData uiData = data as UIData;
                // Child
                {
                    uiData.projectile.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            {
                if (data is TroopCageShootProjectile)
                {
                    TroopCageShootProjectile troopCageShootProjectile = data as TroopCageShootProjectile;
                    // Child
                    {
                        troopCageShootProjectile.state.allAddCallBack(this);
                    }
                    dirty = true;
                    return;
                }
                // Child
                if(data is TroopCageShootProjectile.State)
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
                    uiData.projectile.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // Child
            {
                if (data is TroopCageShootProjectile)
                {
                    TroopCageShootProjectile troopCageShootProjectile = data as TroopCageShootProjectile;
                    // Child
                    {
                        troopCageShootProjectile.state.allRemoveCallBack(this);
                    }
                    return;
                }
                // Child
                if (data is TroopCageShootProjectile.State)
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
                    case UIData.Property.projectile:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case UIData.Property.type:
                        dirty = true;
                        break;
                    default:
                        break;
                }
                return;
            }
            // Child
            {
                if (wrapProperty.p is TroopCageShootProjectile)
                {
                    switch ((TroopCageShootProjectile.Property)wrapProperty.n)
                    {
                        case TroopCageShootProjectile.Property.startPosition:
                            dirty = true;
                            break;
                        case TroopCageShootProjectile.Property.state:
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
                if(wrapProperty.p is TroopCageShootProjectile.State)
                {
                    TroopCageShootProjectile.State state = wrapProperty.p as TroopCageShootProjectile.State;
                    switch (state.getType())
                    {
                        case TroopCageShootProjectile.State.Type.Move:
                            {
                                switch ((TroopCageShootProjectile.State.Move.Property)wrapProperty.n)
                                {
                                    case TroopCageShootProjectile.State.Move.Property.time:
                                        dirty = true;
                                        break;
                                    case TroopCageShootProjectile.State.Move.Property.duration:
                                        dirty = true;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case TroopCageShootProjectile.State.Type.Hit:
                            break;
                        case TroopCageShootProjectile.State.Type.Finish:
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