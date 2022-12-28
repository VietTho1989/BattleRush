using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ArenaS.TroopS.IntentionS
{
    public class AttackUpdate : UpdateBehavior<Attack>
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
                    Logger.Log("data null");
                }
            }
        }

        public override bool isShouldDisableUpdate()
        {
            return false;
        }

        public override void Update()
        {
            base.Update();
            if (this.data != null)
            {
                this.data.time.v += Time.deltaTime;
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
            if(data is Attack)
            {
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if(data is Attack)
            {
                Attack attack = data as Attack;
                this.setDataNull(attack);
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
            if(wrapProperty.p is Attack)
            {
                switch ((Attack.Property)wrapProperty.n)
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