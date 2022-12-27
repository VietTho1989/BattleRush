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
                    // skin
                    {
                        if (string.IsNullOrEmpty(this.data.skin.v))
                        {
                            string[] skins = { "Antuk_1", "Antuk_2", "Antuk_3", "Antuk_4", "Krakee_1", "Krakee_2", "Krakee_3", "Krakee_4", "Mantah_1", "Mantah_2", "Mantah_3", "Mantah_4", "Muu_1", "Muu_2", "Muu_3", "Muu_4" };
                            this.data.skin.v = skins[Random.Range(0, skins.Length)];
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
                    case Hero.Property.skin:
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
