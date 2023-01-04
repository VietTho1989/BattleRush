using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.HeroS.RunS
{
    public class HeroRunShootUpdate : UpdateBehavior<HeroRunShoot>
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

        public void FixedUpdate()
        {
            // increase time when cool down
            {
                if (this.data != null)
                {
                    switch (this.data.state.v.getType())
                    {
                        case HeroRunShoot.State.Type.Normal:
                            break;
                        case HeroRunShoot.State.Type.CoolDown:
                            {
                                HeroRunShoot.State.CoolDown coolDown = this.data.state.v as HeroRunShoot.State.CoolDown;
                                coolDown.time.v += Time.fixedDeltaTime;
                                // change back to normal
                                {
                                    if (coolDown.time.v >= coolDown.duration.v)
                                    {
                                        HeroRunShoot.State.Normal normal = this.data.state.newOrOld<HeroRunShoot.State.Normal>();
                                        {

                                        }
                                        this.data.state.v = normal;
                                    }
                                }                                
                            }
                            break;
                    }
                }
                else
                {
                    Logger.LogError("data null");
                }
            }
            base.Update();
        }

        public override bool isShouldDisableUpdate()
        {
            return false;
        }

        #endregion

        #region implement callBacks

        public override void onAddCallBack<T>(T data)
        {
            if(data is HeroRunShoot)
            {
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if(data is HeroRunShoot)
            {
                HeroRunShoot heroRunShoot = data as HeroRunShoot;
                this.setDataNull(heroRunShoot);
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
            if(wrapProperty.p is HeroRunShoot)
            {
                switch ((HeroRunShoot.Property)wrapProperty.n)
                {
                    case HeroRunShoot.Property.state:
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
            if(wrapProperty.p is HeroRunShoot.State)
            {
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + syncs + "; " + this);
        }

        #endregion

    }
}