using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattleRushS.StateS.LoadS
{
    public class ChooseLevelUpdate : UpdateBehavior<ChooseLevel>
    {

        #region Update

        public override void update()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    // tam thoi chuyen level ngay
                    /*{
                        Load load = this.data.findDataInParent<Load>();
                        if (load != null)
                        {
                            LoadLevel loadLevel = load.sub.newOrOld<LoadLevel>();
                            {
                                loadLevel.level.v = 20;
                            }
                            load.sub.v = loadLevel;
                        }
                        else
                        {
                            Logger.LogError("load null");
                        }
                    }*/
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
            if(data is ChooseLevel)
            {
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if(data is ChooseLevel)
            {
                ChooseLevel chooseLevel = data as ChooseLevel;
                this.setDataNull(chooseLevel);
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
            if(wrapProperty.p is ChooseLevel)
            {
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + syncs + "; " + this);
        }

        #endregion

    }
}
