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

        public VO<string> skin;

        #endregion

        public VD<HeroMove> heroMove;

        public LD<TroopFollow> troopFollows;

        #region Constructor

        public enum Property
        {
            hitPoint,
            skin,
            heroMove,
            troopFollows
        }

        public Hero() : base()
        {
            this.hitPoint = new VO<float>(this, (byte)Property.hitPoint, 1);
            this.skin = new VO<string>(this, (byte)Property.skin, "");
            this.heroMove = new VD<HeroMove>(this, (byte)Property.heroMove, new HeroMove());
            this.troopFollows = new LD<TroopFollow>(this, (byte)Property.troopFollows);
        }

        #endregion

        public void reset()
        {
            this.hitPoint.v = 1;
            this.skin.v = "";
            this.heroMove.v.reset();
            this.troopFollows.clear();
        }

    }
}
