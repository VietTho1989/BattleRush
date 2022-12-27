using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.StateS
{
    public class End : BattleRush.State
    {

        #region Constructor

        public enum Property
        {

        }

        public End() : base()
        {

        }

        #endregion

        public override Type getType()
        {
            return Type.End;
        }
    }
}
