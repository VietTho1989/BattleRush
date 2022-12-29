using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ArenaS.TroopS
{
    public class DieUpdate : UpdateBehavior<Die>
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

        public override void Update()
        {
            base.Update();
            if (this.data != null)
            {
                this.data.time.v += Time.deltaTime;
                // remove troop
                if (this.data.time.v >= this.data.duration.v)
                {
                    Troop troop = this.data.findDataInParent<Troop>();
                    if (troop != null)
                    {
                        Arena arena = this.data.findDataInParent<Arena>();
                        if (arena != null)
                        {
                            arena.troops.remove(troop);
                        }
                        else
                        {
                            Logger.LogError("arena null");
                        }
                    }
                    else
                    {
                        Logger.LogError("troop null");
                    }                    
                }
            }
            else
            {
                Logger.LogError("data null");
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
            if(data is Die)
            {
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if(data is Die)
            {
                Die die = data as Die;
                this.setDataNull(die);
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
            if(wrapProperty.p is Die)
            {
                switch ((Die.Property)wrapProperty.n)
                {
                    case Die.Property.time:
                        dirty = true;
                        break;
                    case Die.Property.duration:
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