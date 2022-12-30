using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ArenaS.ProjectileS
{
    public class MoveUpdate : UpdateBehavior<Move>
    {

        #region Update

        public override void update()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {

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

        private TroopUI targetTroopUI = null;

        private void FixedUpdate()
        {
            if (this.data != null)
            {
                this.data.time.v += Time.fixedDeltaTime;
                // find targetTroopUI
                {
                    if (targetTroopUI == null)
                    {
                        Arena arena = this.data.findDataInParent<Arena>();
                        if (arena != null)
                        {
                            Projectile projectile = this.data.findDataInParent<Projectile>();
                            if (projectile != null)
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
                                Logger.LogError("projectile null");
                            }
                        }
                        else
                        {
                            Logger.LogError("arena null");
                        }
                    }
                }
                // update position
                {
                    if (targetTroopUI)
                    {
                        float speed = 2.5f;
                        this.data.position.v = Vector3.MoveTowards(this.data.position.v, targetTroopUI.transform.position, speed * Time.fixedDeltaTime);
                    }
                    else
                    {
                        Logger.LogError("targetTroopUI null");
                    }
                }
                // check reach target or not
                bool isReachTarget = false;
                {                    
                    // check time
                    if (!isReachTarget)
                    {
                        float MaxMoveDuration = 120.0f;
                        if (this.data.time.v > MaxMoveDuration)
                        {
                            isReachTarget = true;
                        }
                    }
                    // position
                    if (!isReachTarget)
                    {
                        if (targetTroopUI != null)
                        {
                            if(Vector3.Distance(this.data.position.v, targetTroopUI.transform.position) < 0.05f)
                            {
                                isReachTarget = true;
                            }
                        }
                        else
                        {
                            Logger.LogError("targetTroopUI null");
                        }
                    }
                    // check target exist anymore
                    if (!isReachTarget)
                    {
                        if (targetTroopUI.data != null)
                        {
                            Troop targetTroop = targetTroopUI.data.troop.v.data;
                            if(targetTroop.state.v.getType() != Troop.State.Type.Live)
                            {
                                isReachTarget = true;
                            }
                        }
                    }
                }
                // change to deal damage
                if (isReachTarget)
                {
                    Projectile projectile = this.data.findDataInParent<Projectile>();
                    if (projectile != null)
                    {
                        DealDamage dealDamage = projectile.state.newOrOld<DealDamage>();
                        {

                        }
                        projectile.state.v = dealDamage;
                    }
                    else
                    {
                        Logger.LogError("projectile null");
                    }
                }
            }
            else
            {
                Logger.LogError("data null");
            }
        }

        #endregion

        #region implement callBacks

        public override void onAddCallBack<T>(T data)
        {
            if(data is Move)
            {
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if(data is Move)
            {
                Move move = data as Move;
                this.setDataNull(move);
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
            if(wrapProperty.p is Move)
            {
                switch ((Move.Property)wrapProperty.n)
                {
                    default:
                        break;
                }
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + syncs + "; " + this);
        }

        #endregion

    }
}
