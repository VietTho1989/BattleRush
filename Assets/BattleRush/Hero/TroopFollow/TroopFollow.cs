using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.HeroS
{
    public class TroopFollow : Data
    {

        #region troop type

        public enum TroopType
        {
            // Antuk
            AntukAir,
            AntukMage,
            AntukMelee,
            AntukRange,
            AntukTank,

            // Krakee
            KrakeeAir,
            KrakeeMage,
            KrakeeMelee,
            KrakeeRange,
            KrakeeTank,

            // Mantah
            MantahAir,
            MantahMage,
            MantahMelee,
            MantahRange,
            MantahTank,

            // Montak
            MontakAir,
            MontakMage,
            MontakMelee,
            MontakRange,
            MontakTank,

            // Muu
            MuuAir,
            MuuMage,
            MuuMelee,
            MuuRange,
            MuuTank
        }

        public VO<TroopType> troopType;

        #endregion

        /**
         * percent from 0 to 1
         * */
        public VO<float> hitPoint;

        #region Constructor

        public enum Property
        {
            troopType,
            hitPoint
        }

        public TroopFollow() : base()
        {
            this.troopType = new VO<TroopType>(this, (byte)Property.troopType, TroopType.AntukAir);
            this.hitPoint = new VO<float>(this, (byte)Property.hitPoint, 1);
        }

        #endregion

    }
}
