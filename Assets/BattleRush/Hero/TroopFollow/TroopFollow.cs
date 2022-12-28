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
            MuuTank,

            /** hero type*/
            Antuk_1, 
            Antuk_2, 
            Antuk_3, 
            Antuk_4, 
            Krakee_1, 
            Krakee_2, 
            Krakee_3, 
            Krakee_4, 
            Mantah_1, 
            Mantah_2, 
            Mantah_3, 
            Mantah_4, 
            Muu_1, 
            Muu_2, 
            Muu_3, 
            Muu_4,
        }

        public static bool IsHero(TroopType heroType)
        {
            switch (heroType)
            {
                case TroopType.Antuk_1:
                    return true;
                case TroopType.Antuk_2:
                    return true;
                case TroopType.Antuk_3:
                    return true;
                case TroopType.Antuk_4:
                    return true;
                case TroopType.Krakee_1:
                    return true;
                case TroopType.Krakee_2:
                    return true;
                case TroopType.Krakee_3:
                    return true;
                case TroopType.Krakee_4:
                    return true;
                case TroopType.Mantah_1:
                    return true;
                case TroopType.Mantah_2:
                    return true;
                case TroopType.Mantah_3:
                    return true;
                case TroopType.Mantah_4:
                    return true;
                case TroopType.Muu_1:
                    return true;
                case TroopType.Muu_2:
                    return true;
                case TroopType.Muu_3:
                    return true;
                case TroopType.Muu_4:
                    return true;
                default:
                    return false;
            }
        }

        public static string getHeroSkin(TroopType heroType)
        {
            switch (heroType)
            {
                case TroopType.Antuk_1:
                    return "Antuk_1";
                case TroopType.Antuk_2:
                    return "Antuk_2";
                case TroopType.Antuk_3:
                    return "Antuk_3";
                case TroopType.Antuk_4:
                    return "Antuk_4";
                case TroopType.Krakee_1:
                    return "Krakee_1";
                case TroopType.Krakee_2:
                    return "Krakee_2";
                case TroopType.Krakee_3:
                    return "Krakee_3";
                case TroopType.Krakee_4:
                    return "Krakee_4";
                case TroopType.Mantah_1:
                    return "Mantah_1";
                case TroopType.Mantah_2:
                    return "Mantah_2";
                case TroopType.Mantah_3:
                    return "Mantah_3";
                case TroopType.Mantah_4:
                    return "Mantah_4";
                case TroopType.Muu_1:
                    return "Muu_1";
                case TroopType.Muu_2:
                    return "Muu_2";
                case TroopType.Muu_3:
                    return "Muu_3";
                case TroopType.Muu_4:
                    return "Muu_4";
                default:
                    Logger.LogError("not hero, don't have skin");
                    return "";
            }
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
