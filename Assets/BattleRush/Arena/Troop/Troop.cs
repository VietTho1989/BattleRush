using System.Collections;
using System.Collections.Generic;
using BattleRushS.ArenaS.TroopS;
using UnityEngine;

namespace BattleRushS.ArenaS
{
    public class Troop : Data
    {

        #region position

        public VO<Vector3> startPosition;

        public VO<Vector3> formationPosition;

        #endregion

        public VO<int> teamId;

        #region troop type

        public VO<HeroS.TroopFollow.TroopType> troopType;

        public bool isRange()
        {
            // find
            bool ret = true;
            {
                switch (this.troopType.v)
                {
                    // Antuk
                    case HeroS.TroopFollow.TroopType.AntukAir:
                        break;
                    case HeroS.TroopFollow.TroopType.AntukMage:
                        break;
                    case HeroS.TroopFollow.TroopType.AntukMelee:
                        ret = false;
                        break;
                    case HeroS.TroopFollow.TroopType.AntukRange:
                        break;
                    case HeroS.TroopFollow.TroopType.AntukTank:
                        ret = false;
                        break;

                    // Krakee
                    case HeroS.TroopFollow.TroopType.KrakeeAir:
                        break;
                    case HeroS.TroopFollow.TroopType.KrakeeMage:
                        break;
                    case HeroS.TroopFollow.TroopType.KrakeeMelee:
                        ret = false;
                        break;
                    case HeroS.TroopFollow.TroopType.KrakeeRange:
                        break;
                    case HeroS.TroopFollow.TroopType.KrakeeTank:
                        ret = false;
                        break;

                    // Mantah
                    case HeroS.TroopFollow.TroopType.MantahAir:
                        break;
                    case HeroS.TroopFollow.TroopType.MantahMage:
                        break;
                    case HeroS.TroopFollow.TroopType.MantahMelee:
                        ret = false;
                        break;
                    case HeroS.TroopFollow.TroopType.MantahRange:
                        break;
                    case HeroS.TroopFollow.TroopType.MantahTank:
                        ret = false;
                        break;

                    // Montak
                    case HeroS.TroopFollow.TroopType.MontakAir:
                        break;
                    case HeroS.TroopFollow.TroopType.MontakMage:
                        break;
                    case HeroS.TroopFollow.TroopType.MontakMelee:
                        ret = false;
                        break;
                    case HeroS.TroopFollow.TroopType.MontakRange:
                        break;
                    case HeroS.TroopFollow.TroopType.MontakTank:
                        ret = false;
                        break;

                    // Muu
                    case HeroS.TroopFollow.TroopType.MuuAir:
                        break;
                    case HeroS.TroopFollow.TroopType.MuuMage:
                        break;
                    case HeroS.TroopFollow.TroopType.MuuMelee:
                        ret = false;
                        break;
                    case HeroS.TroopFollow.TroopType.MuuRange:
                        break;
                    case HeroS.TroopFollow.TroopType.MuuTank:
                        ret = false;
                        break;

                    /** hero type*/
                    case HeroS.TroopFollow.TroopType.Antuk_1:
                        break;
                    case HeroS.TroopFollow.TroopType.Antuk_2:
                        break;
                    case HeroS.TroopFollow.TroopType.Antuk_3:
                        break;
                    case HeroS.TroopFollow.TroopType.Antuk_4:
                        break;
                    case HeroS.TroopFollow.TroopType.Krakee_1:
                        break;
                    case HeroS.TroopFollow.TroopType.Krakee_2:
                        break;
                    case HeroS.TroopFollow.TroopType.Krakee_3:
                        break;
                    case HeroS.TroopFollow.TroopType.Krakee_4:
                        break;
                    case HeroS.TroopFollow.TroopType.Mantah_1:
                        break;
                    case HeroS.TroopFollow.TroopType.Mantah_2:
                        break;
                    case HeroS.TroopFollow.TroopType.Mantah_3:
                        break;
                    case HeroS.TroopFollow.TroopType.Mantah_4:
                        break;
                    case HeroS.TroopFollow.TroopType.Muu_1:
                        break;
                    case HeroS.TroopFollow.TroopType.Muu_2:
                        break;
                    case HeroS.TroopFollow.TroopType.Muu_3:
                        break;
                    case HeroS.TroopFollow.TroopType.Muu_4:
                        break;
                    default:
                        Logger.LogError("unknown troop type: " + this.troopType.v);
                        break;
                }
            }
            // return
            return ret;
        }

        #endregion

        #region world data

        public VO<Vector3> worldPosition;

        #endregion

        #region state

        public abstract class State : Data
        {

            public enum Type
            {
                Live,
                Die
            }

            public abstract Type getType();

        }

        public VD<State> state;

        #endregion      

        #region Constructor

        public enum Property
        {
            startPosition,
            formationPosition,

            teamId,
            troopType,
            worldPosition,
            state
        }

        public Troop() : base()
        {
            // position
            {
                this.startPosition = new VO<Vector3>(this, (byte)Property.startPosition, Vector3.zero);
                this.formationPosition = new VO<Vector3>(this, (byte)Property.formationPosition, Vector3.zero);
            }
            this.teamId = new VO<int>(this, (byte)Property.teamId, 0);
            this.troopType = new VO<HeroS.TroopFollow.TroopType>(this, (byte)Property.troopType, HeroS.TroopFollow.TroopType.AntukAir);
            this.worldPosition = new VO<Vector3>(this, (byte)Property.worldPosition, Vector3.zero);
            this.state = new VD<State>(this, (byte)Property.state, new Live());
        }

        #endregion

    }
}
