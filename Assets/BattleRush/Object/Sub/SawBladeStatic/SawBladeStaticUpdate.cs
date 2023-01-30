using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ObjectS
{
    public class SawBladeStaticUpdate : UpdateBehavior<SawBladeStatic>
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
            if (data is SawBladeStatic)
            {
                SawBladeStatic sawBladeStatic = data as SawBladeStatic;
                // Child
                {
                    sawBladeStatic.obstruction.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            if (data is Obstruction)
            {
                Obstruction obstruction = data as Obstruction;
                // Update
                {
                    UpdateUtils.makeUpdate<ObstructionUpdate, Obstruction>(obstruction, this.transform);
                }
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if (data is SawBladeStatic)
            {
                SawBladeStatic sawBladeStatic = data as SawBladeStatic;
                // Child
                {
                    sawBladeStatic.obstruction.allRemoveCallBack(this);
                }
                this.setDataNull(sawBladeStatic);
                return;
            }
            // Child
            if (data is Obstruction)
            {
                Obstruction obstruction = data as Obstruction;
                // Update
                {
                    obstruction.removeCallBackAndDestroy(typeof(ObstructionUpdate));
                }
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
            if (wrapProperty.p is SawBladeStatic)
            {
                switch ((SawBladeStatic.Property)wrapProperty.n)
                {
                    case SawBladeStatic.Property.obstruction:
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
            if (wrapProperty.p is Obstruction)
            {
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + syncs + "; " + this);
        }

        #endregion

    }
}