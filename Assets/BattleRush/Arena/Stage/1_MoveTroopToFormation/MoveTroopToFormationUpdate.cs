using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ArenaS
{
    public class MoveTroopToFormationUpdate : UpdateBehavior<MoveTroopToFormation>
    {

        #region update

        public override void update()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    switch (this.data.state.v)
                    {
                        case MoveTroopToFormation.State.Move:
                            {
                                if (this.data.time.v >= this.data.duration.v)
                                {
                                    this.data.state.v = MoveTroopToFormation.State.Came;
                                }
                            }
                            break;
                        case MoveTroopToFormation.State.Came:
                            {
                                if (this.data.time.v >= this.data.duration.v + 1.0f)
                                {
                                    this.data.state.v = MoveTroopToFormation.State.Ready;
                                }
                            }
                            break;
                        case MoveTroopToFormation.State.Ready:
                            {
                                if (this.data.time.v >= this.data.duration.v + 2.0f)
                                {
                                    // change to auto fight
                                    Arena arena = this.data.findDataInParent<Arena>();
                                    if (arena != null)
                                    {
                                        AutoFight autoFight = arena.stage.newOrOld<AutoFight>();
                                        {

                                        }
                                        arena.stage.v = autoFight;
                                    }
                                    else
                                    {
                                        Logger.LogError("arena null");
                                    }
                                }
                            }
                            break;
                        default:
                            Logger.LogError("unknown state: " + this.data.state.v);
                            break;
                    }
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
            if(data is MoveTroopToFormation)
            {
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if (data is MoveTroopToFormation)
            {
                MoveTroopToFormation moveTroopToFormation = data as MoveTroopToFormation;
                this.setDataNull(moveTroopToFormation);
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
            if(wrapProperty.p is MoveTroopToFormation)
            {
                switch ((MoveTroopToFormation.Property)wrapProperty.n)
                {
                    case MoveTroopToFormation.Property.time:
                        dirty = true;
                        break;
                    case MoveTroopToFormation.Property.duration:
                        dirty = true;
                        break;
                    case MoveTroopToFormation.Property.state:
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