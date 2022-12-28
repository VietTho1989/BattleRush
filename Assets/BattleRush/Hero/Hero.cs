using System.Collections;
using System.Collections.Generic;
using BattleRushS.HeroS;
using Dreamteck.Forever;
using UnityEngine;

namespace BattleRushS
{
    public class Hero : Data
    {

        #region hit point

        /**
         * hit point percent: from 0 to 1
         * */
        public VO<float> hitPoint;

        #endregion

        #region skin

        public VO<TroopFollow.TroopType> troopType;

        #endregion

        public VD<HeroMove> heroMove;

        public LD<TroopFollow> troopFollows;

        #region Constructor

        public enum Property
        {
            hitPoint,
            troopType,
            heroMove,
            troopFollows
        }

        public Hero() : base()
        {
            this.hitPoint = new VO<float>(this, (byte)Property.hitPoint, 1);
            // skin
            {
                TroopFollow.TroopType skin = TroopFollow.TroopType.AntukAir;
                {
                    // find
                    List<TroopFollow.TroopType> allHeroesType = new List<TroopFollow.TroopType>();
                    {
                        foreach (TroopFollow.TroopType check in System.Enum.GetValues(typeof(TroopFollow.TroopType)))
                        {
                            if (TroopFollow.IsHero(check))
                            {
                                allHeroesType.Add(check);
                            }
                        }
                    }
                    // process
                    if (allHeroesType.Count > 0)
                    {
                        skin = allHeroesType[Random.Range(0, allHeroesType.Count)];
                    }
                    Logger.Log("Hero random skin: " + skin);
                }                
                this.troopType = new VO<TroopFollow.TroopType>(this, (byte)Property.troopType, skin);
            }
            this.heroMove = new VD<HeroMove>(this, (byte)Property.heroMove, new HeroMove());
            this.troopFollows = new LD<TroopFollow>(this, (byte)Property.troopFollows);
        }

        #endregion

        public void reset()
        {
            this.hitPoint.v = 1;
            this.heroMove.v.reset();
            this.troopFollows.clear();
        }

    }
}
