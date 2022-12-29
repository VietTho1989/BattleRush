using System.Collections;
using System.Collections.Generic;
using BattleRushS.ArenaS.TroopS;
using UnityEngine;

namespace BattleRushS.ArenaS
{
    public class Troop : Data
    {

        public VO<int> teamId;

        #region stat

        public VO<HeroS.TroopFollow.TroopType> troopType;

        public VO<int> level;       

        public VO<int> attack;

        public VO<float> attackRange;

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
            teamId,
            troopType,
            level,
            attack,
            attackRange,
            worldPosition,
            state
        }

        public Troop() : base()
        {
            this.teamId = new VO<int>(this, (byte)Property.teamId, 0);
            this.troopType = new VO<HeroS.TroopFollow.TroopType>(this, (byte)Property.troopType, HeroS.TroopFollow.TroopType.AntukAir);
            this.level = new VO<int>(this, (byte)Property.level, 0);
            this.attack = new VO<int>(this, (byte)Property.attack, 1);
            this.attackRange = new VO<float>(this, (byte)Property.attackRange, 1);
            this.worldPosition = new VO<Vector3>(this, (byte)Property.worldPosition, Vector3.zero);
            this.state = new VD<State>(this, (byte)Property.state, new Live());
        }

        #endregion

    }
}
