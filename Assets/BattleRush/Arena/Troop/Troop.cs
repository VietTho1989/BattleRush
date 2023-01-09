using System.Collections;
using System.Collections.Generic;
using BattleRushS.ArenaS.TroopS;
using BattleRushS.HeroS;
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

        /**
         * level of troop, not const, get from server
         * */
        public VO<int> level;

        #region troop type

        public enum AttackType
        {
            Range,
            Melee
        }

        public VO<TroopType> troopType;

        public AttackType getAttackType()
        {
            // find
            AttackType ret = AttackType.Range;
            {
                switch (this.troopType.v.getType())
                {
                    case TroopType.Type.Hero:
                        ret = AttackType.Range;
                        break;
                    case TroopType.Type.Normal:
                        {
                            TroopInformation troopInformation = this.troopType.v as TroopInformation;
                            ret = troopInformation.attackType;
                        }
                        break;
                    case TroopType.Type.Monster:
                        {
                            // TODO Can hoan thien
                        }
                        break;
                    default:
                        Logger.LogError("unknown type: " + this.troopType.v);
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
            level,
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
            this.level = new VO<int>(this, (byte)Property.level, 0);
            this.troopType = new VO<TroopType>(this, (byte)Property.troopType, null);
            this.worldPosition = new VO<Vector3>(this, (byte)Property.worldPosition, Vector3.zero);
            this.state = new VD<State>(this, (byte)Property.state, new Live());
        }

        #endregion

    }
}
