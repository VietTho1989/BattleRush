using System.Collections;
using System.Collections.Generic;
using Dreamteck.Forever;
using UnityEngine;

namespace BattleRushS.HeroS
{
    public class HeroMoveArenaUpdate : UpdateBehavior<HeroMoveArena>
    {

        #region Update

        public override void update()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    // disable run because hero is fighting in arena
                    {
                        Hero hero = this.data.findDataInParent<Hero>();
                        if (hero != null)
                        {
                            HeroUI heroUI = hero.findCallBack<HeroUI>();
                            if (heroUI != null)
                            {
                                LaneRunner laneRunner = heroUI.GetComponent<LaneRunner>();
                                if (laneRunner != null)
                                {
                                    laneRunner.enabled = false;
                                }
                                else
                                {
                                    Logger.LogError("laneRunner null");
                                }
                            }
                            else
                            {
                                Logger.LogError("heroUI null");
                            }
                        }
                        else
                        {
                            Logger.LogError("hero null");
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
            if (data is HeroMoveArena)
            {
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if (data is HeroMoveArena)
            {
                HeroMoveArena heroMoveRun = data as HeroMoveArena;
                this.setDataNull(heroMoveRun);
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
            if (wrapProperty.p is HeroMoveArena)
            {
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + syncs + "; " + this);
        }

        #endregion

    }
}