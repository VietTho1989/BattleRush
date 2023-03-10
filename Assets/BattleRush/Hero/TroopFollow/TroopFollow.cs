using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.HeroS
{
    public class TroopFollow : Data
    {

        #region troop type

        public VO<TroopInformation> troopType;

        #endregion

        /**
         * percent from 0 to 1
         * */
        public VO<float> hitPoint;

        public VO<float> timeDead;

        public VO<int> level;

        #region Constructor

        public enum Property
        {
            troopType,
            hitPoint,
            timeDead,
            level
        }

        public TroopFollow() : base()
        {
            this.troopType = new VO<TroopInformation>(this, (byte)Property.troopType, null);
            this.hitPoint = new VO<float>(this, (byte)Property.hitPoint, 1);
            this.timeDead = new VO<float>(this, (byte)Property.timeDead, 0);
            // level
            {
                this.level = new VO<int>(this, (byte)Property.level, 1);
            }           
        }

        #endregion

    }
}
