using System.Collections;
using System.Collections.Generic;
using BattleRushS.ArenaS.TroopS.TroopMoveS;
using UnityEngine;

namespace BattleRushS.ArenaS.TroopS
{
    public class TroopMove : Data
    {

        #region sub

        public abstract class Sub : Data
        {

            public enum Type
            {
                Idle,
                MoveToDest
            }

            public abstract Type getType();

        }

        public VD<Sub> sub;

        #endregion

        #region Constructor

        public enum Property
        {
            sub
        }

        public TroopMove() : base()
        {
            this.sub = new VD<Sub>(this, (byte)Property.sub, new Idle());
        }

        #endregion

    }
}