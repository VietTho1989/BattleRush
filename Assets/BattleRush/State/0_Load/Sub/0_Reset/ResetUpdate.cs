using System.Collections;
using System.Collections.Generic;
using BattleRushS.HeroS;
using Dreamteck.Forever;
using UnityEngine;

namespace BattleRushS.StateS.LoadS
{
    public class ResetUpdate : UpdateBehavior<Reset>
    {

        #region Update

        public override void update()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    BattleRush battleRush = this.data.findDataInParent<BattleRush>();
                    if (battleRush != null)
                    {
                        // reset
                        battleRush.reset();
                        // change to choose level
                        {
                            Load load = this.data.findDataInParent<Load>();
                            if (load != null)
                            {
                                ChooseLevel chooseLevel = load.sub.newOrOld<ChooseLevel>();
                                {
                                    
                                }
                                load.sub.v = chooseLevel;
                            }
                            else
                            {
                                Logger.LogError("load null");
                            }                            
                        }
                    }
                    else
                    {
                        Logger.LogError("battleRush null");
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
            if(data is Reset)
            {
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if(data is Reset)
            {
                Reset reset = data as Reset;
                this.setDataNull(reset);
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
            if(wrapProperty.p is Reset)
            {
                switch ((Reset.Property)wrapProperty.n)
                {
                    default:
                        Logger.LogError("Don't process: " + wrapProperty + "; " + this);
                        break;
                }
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + syncs + "; " + this);
        }

        #endregion

    }
}