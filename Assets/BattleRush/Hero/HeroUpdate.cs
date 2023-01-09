using System.Collections;
using System.Collections.Generic;
using BattleRushS.HeroS;
using UnityEngine;

namespace BattleRushS
{
    public class HeroUpdate : UpdateBehavior<Hero>
    {

        #region update

        public override void update()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    // prevent skin null
                    {
                        if (this.data.heroInformation.v == null)
                        {
                            BattleRush battleRush = this.data.findDataInParent<BattleRush>();
                            if (battleRush != null)
                            {
                                BattleRushUI battleRushUI = battleRush.findCallBack<BattleRushUI>();
                                if (battleRushUI != null)
                                {
                                    this.data.heroInformation.v = battleRushUI.heroInformations[Random.Range(0, battleRushUI.heroInformations.Count)];
                                }
                                else
                                {
                                    Logger.LogError("battleRushUI null");
                                }
                            }
                            else
                            {
                                Logger.LogError("battleRush null");
                            }
                        }
                        // prevent in another frame
                        if (this.data.heroInformation.v == null)
                        {
                            dirty = true;
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
            if(data is Hero)
            {
                Hero hero = data as Hero;
                // Child
                {
                    hero.heroMove.allAddCallBack(this);
                    hero.troopFollows.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            {
                if (data is HeroMove)
                {
                    HeroMove heroMove = data as HeroMove;
                    // Update
                    {
                        UpdateUtils.makeUpdate<HeroMoveUpdate, HeroMove>(heroMove, this.transform);
                    }
                    dirty = true;
                    return;
                }
                if(data is TroopFollow)
                {
                    TroopFollow troopFollow = data as TroopFollow;
                    // Update
                    {
                        UpdateUtils.makeUpdate<TroopFollowUpdate, TroopFollow>(troopFollow, this.transform);
                    }
                    dirty = true;
                    return;
                }
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if(data is Hero)
            {
                Hero hero = data as Hero;
                // Child
                {
                    hero.heroMove.allRemoveCallBack(this);
                    hero.troopFollows.allRemoveCallBack(this);
                }
                this.setDataNull(hero);
                return;
            }
            // Child
            {
                if (data is HeroMove)
                {
                    HeroMove heroMove = data as HeroMove;
                    // Update
                    {
                        heroMove.removeCallBackAndDestroy(typeof(HeroMoveUpdate));
                    }
                    return;
                }
                if (data is TroopFollow)
                {
                    TroopFollow troopFollow = data as TroopFollow;
                    // Update
                    {
                        troopFollow.removeCallBackAndDestroy(typeof(TroopFollowUpdate));
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
            if(wrapProperty.p is Hero)
            {
                switch ((Hero.Property)wrapProperty.n)
                {
                    case Hero.Property.heroInformation:
                        dirty = true;
                        break;
                    case Hero.Property.heroMove:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case Hero.Property.troopFollows:
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
                if (wrapProperty.p is HeroMove)
                {
                    return;
                }
                if(wrapProperty.p is TroopFollow)
                {
                    return;
                }
            }
            Logger.LogError("Don't process: " + data + "; " + syncs + "; " + this);
        }

        #endregion

    }
}
