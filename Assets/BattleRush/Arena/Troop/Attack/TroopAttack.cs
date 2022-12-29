using System.Collections;
using System.Collections.Generic;
using BattleRushS.ArenaS.TroopS.TroopAttackS;
using UnityEngine;

namespace BattleRushS.ArenaS.TroopS
{
    public class TroopAttack : Data
    {

        #region state

        public abstract class State : Data
        {

            public enum Type
            {
                Normal,
                Animation
            }

            public abstract Type getType();

        }

        public VD<State> state;

        #endregion

        #region Constructor

        public enum Property
        {
            state
        }

        public TroopAttack() : base()
        {
            this.state = new VD<State>(this, (byte)Property.state, new Normal());
        }

        #endregion

    }
}