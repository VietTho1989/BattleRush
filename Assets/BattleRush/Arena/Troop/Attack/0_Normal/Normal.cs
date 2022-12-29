using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ArenaS.TroopS.TroopAttackS
{
    public class Normal : TroopAttack.State
    {

        #region Constructor

        public enum Property
        {

        }

        public Normal() : base()
        {

        }

        #endregion

        public override Type getType()
        {
            return Type.Normal;
        }

    }
}