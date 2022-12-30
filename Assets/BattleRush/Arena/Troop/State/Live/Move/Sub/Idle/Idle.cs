using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ArenaS.TroopS.TroopMoveS
{
    public class Idle : TroopMove.Sub
    {

        #region Constructor

        public enum Property
        {

        }

        public Idle() : base()
        {

        }

        #endregion

        public override Type getType()
        {
            return Type.Idle;
        }

    }
}