using System.Collections;
using System.Collections.Generic;
using BattleRushS.HeroS;
using UnityEngine;

namespace BattleRushS.ObjectS.TroopCageS
{
    public class ReleaseTroopUpdate : UpdateBehavior<ReleaseTroop>
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
                // add troop follow to hero
                if (!this.data.alreadyRelease.v)
                {
                    this.data.alreadyRelease.v = true;
                    BattleRush battleRush = this.data.findDataInParent<BattleRush>();
                    if (battleRush != null)
                    {
                        Hero hero = battleRush.hero.v;
                        if (hero != null)
                        {
                            TroopCage troopCage = this.data.findDataInParent<TroopCage>();
                            if (troopCage != null)
                            {
                                foreach (TroopFollow troopFollow in troopCage.troops.vs)
                                {
                                    TroopFollow newTroopFollow = DataUtils.cloneData(troopFollow) as TroopFollow;
                                    {
                                        newTroopFollow.uid = hero.troopFollows.makeId();
                                    }
                                    hero.troopFollows.add(newTroopFollow);
                                }
                            }
                            else
                            {
                                Logger.LogError("troopCage null");
                            }
                        }
                        else
                        {
                            Logger.LogError("hero null");
                        }
                    }
                    else
                    {
                        Logger.LogError("battleRush null");
                    }
                }
                // time
                {
                    this.data.time.v += Time.deltaTime;
                    // change to destroyed or not
                    if (this.data.time.v >= this.data.duration.v)
                    {
                        // change to destroyed
                        {
                            TroopCage troopCage = this.data.findDataInParent<TroopCage>();
                            if (troopCage != null)
                            {
                                Destroyed destroyed = troopCage.state.newOrOld<Destroyed>();
                                {

                                }
                                troopCage.state.v = destroyed;
                            }
                            else
                            {
                                Logger.LogError("troopCage null");
                            }
                        }
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
            if(data is ReleaseTroop)
            {
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if (data is ReleaseTroop)
            {
                ReleaseTroop releaseTroop = data as ReleaseTroop;
                this.setDataNull(releaseTroop);
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
            if(wrapProperty.p is ReleaseTroop)
            {
                switch ((ReleaseTroop.Property)wrapProperty.n)
                {
                    case ReleaseTroop.Property.time:
                        dirty = true;
                        break;
                    case ReleaseTroop.Property.duration:
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