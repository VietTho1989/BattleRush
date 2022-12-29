using System.Collections;
using System.Collections.Generic;
using BattleRushS.ArenaS.ProjectileS;
using UnityEngine;

namespace BattleRushS.ArenaS
{
    public class ProjectileUI : UIBehavior<ProjectileUI.UIData>
    {

        #region UIData

        public class UIData : Data
        {

            public VO<ReferenceData<Projectile>> projectile;

            #region Constructor

            public enum Property
            {
                projectile
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
                    Projectile projectile = this.data.projectile.v.data;
                    if (projectile != null)
                    {

                    }
                    else
                    {
                        Logger.LogError("projectile null");
                    }
                }
                else
                {
                    Logger.LogError("data null");
                }
            }
        }

        private TroopUI targetTroopUI = null;

        public override void Update()
        {
            base.Update();
            if (this.data != null)
            {
                Projectile projectile = this.data.projectile.v.data;
                if (projectile != null)
                {
                    switch (projectile.state.v.getType())
                    {
                        case Projectile.State.Type.Move:
                            {
                                Move move = projectile.state.v as Move;
                                this.transform.position = move.position.v;
                            }
                            break;
                        case Projectile.State.Type.DealDamage:
                            {
                                // find target troop
                                if (targetTroopUI == null)
                                {
                                    Arena arena = projectile.findDataInParent<Arena>();
                                    if (arena != null)
                                    {
                                        Troop targetTroop = arena.troops.vs.Find(check => check.uid == projectile.targetId.v);
                                        if (targetTroop != null)
                                        {
                                            targetTroopUI = targetTroop.findCallBack<TroopUI>();
                                        }
                                        else
                                        {
                                            Logger.LogError("targetTroop null");
                                        }
                                    }
                                    else
                                    {
                                        Logger.LogError("arena null");
                                    }
                                }
                                // process
                                if (targetTroopUI != null)
                                {
                                    this.transform.position = targetTroopUI.transform.position;
                                }
                            }
                            break;
                        default:
                            Logger.LogError("unknown type: " + projectile.state.v.getType());
                            break;
                    }
                }
                else
                {
                    Logger.LogError("projectile null");
                }
            }
            else
            {
                Logger.LogError("data null");
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
            if (data is Projectile)
            {
                dirty = true;
                return;
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
            if (data is Projectile)
            {
                return;
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
                    default:
                        break;
                }
                return;
            }
            // Child
            if (wrapProperty.p is Projectile)
            {
                switch ((Projectile.Property)wrapProperty.n)
                {
                    case Projectile.Property.damage:
                        break;
                    case Projectile.Property.originId:
                        dirty = true;
                        break;
                    case Projectile.Property.targetId:
                        dirty = true;
                        break;
                    case Projectile.Property.state:
                        dirty = true;
                        break;
                    default:
                        break;
                }
                return;
            }
            Logger.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

    }
}