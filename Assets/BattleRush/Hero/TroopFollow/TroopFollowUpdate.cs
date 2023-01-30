using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.HeroS
{
    public class TroopFollowUpdate : UpdateBehavior<TroopFollow>
    {

        #region Update

        public override void update()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    // remove troop when hitpoint <= 0
                    {
                        if (this.data.hitPoint.v <= 0 && this.data.timeDead.v >= 3.5f)
                        {
                            Hero hero = this.data.findDataInParent<Hero>();
                            if (hero != null)
                            {
                                hero.troopFollows.remove(this.data);
                            }
                            else
                            {
                                Logger.LogError("hero null");
                            }
                        }
                    }
                }
                else
                {
                    Logger.LogError("data null");
                }
            }
        }

        private void FixedUpdate()
        {
            base.Update();
            if (this.data != null)
            {
                if (this.data.hitPoint.v == 0)
                    this.data.timeDead.v += Time.deltaTime;
            }
        }

        public override bool isShouldDisableUpdate()
        {
            return false;
        }

        #endregion

        #region implement callBacks

        public override void onAddCallBack<T>(T data)
        {
            if(data is TroopFollow)
            {
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if(data is TroopFollow)
            {
                TroopFollow troopFollow = data as TroopFollow;
                this.setDataNull(troopFollow);
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
            if(wrapProperty.p is TroopFollow)
            {
                switch ((TroopFollow.Property)wrapProperty.n)
                {
                    case TroopFollow.Property.hitPoint:
                        dirty = true;
                        break;
                    case TroopFollow.Property.timeDead:
                        dirty = true;
                        break;
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