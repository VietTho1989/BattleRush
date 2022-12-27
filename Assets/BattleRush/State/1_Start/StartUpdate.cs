using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.StateS
{
    class StartUpdate : UpdateBehavior<Start>
    {

        #region update

        public override void update()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    // count down or wait click to start
                    bool isFinishStart = false;
                    {
                        // TODO can hoan thien
                        //isFinishStart = true;
                    }
                    // process: change to state play
                    if (isFinishStart)
                    {
                        BattleRush battleRush = this.data.findDataInParent<BattleRush>();
                        if (battleRush != null)
                        {
                            Play play = battleRush.state.newOrOld<Play>();
                            {

                            }
                            battleRush.state.v = play;
                        }
                        else
                        {
                            Logger.LogError("battleRush null");
                        }
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
            if (data is Start)
            {
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if (data is Start)
            {
                Start start = data as Start;
                this.setDataNull(start);
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
            if (wrapProperty.p is Start)
            {
                switch ((Start.Property)wrapProperty.n)
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
