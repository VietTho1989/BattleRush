using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ArenaS
{
    public class TroopUpdate : UpdateBehavior<Troop>
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
            return true;
        }

        #endregion

        #region implement callBacks

        public override void onAddCallBack<T>(T data)
        {
            if(data is Troop)
            {
                Troop troop = data as Troop;
                // Child
                {
                    troop.takeDamage.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            {
                if(data is TakeDamage)
                {
                    TakeDamage takeDamage = data as TakeDamage;
                    // Update
                    {
                        UpdateUtils.makeUpdate<TakeDamageUpdate, TakeDamage>(takeDamage, this.transform);
                    }
                    dirty = true;
                    return;
                }
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if (data is Troop)
            {
                Troop troop = data as Troop;
                // Child
                {
                    troop.takeDamage.allRemoveCallBack(this);
                }
                this.setDataNull(troop);
                return;
            }
            // Child
            {
                if (data is TakeDamage)
                {
                    TakeDamage takeDamage = data as TakeDamage;
                    // Update
                    {
                        takeDamage.removeCallBackAndDestroy(typeof(TakeDamageUpdate));
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
            if (wrapProperty.p is Troop)
            {
                switch ((Troop.Property)wrapProperty.n)
                {
                    case Troop.Property.takeDamage:
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
                if (wrapProperty.p is TakeDamage)
                {
                    return;
                }
            }
            Logger.LogError("Don't process: " + data + "; " + syncs + "; " + this);
        }

        #endregion

    }
}