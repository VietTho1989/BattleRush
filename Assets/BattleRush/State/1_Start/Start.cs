using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.StateS
{
    public class Start : BattleRush.State
    {

        #region Constructor

        public enum Property
        {

        }

        public Start() : base()
        {

        }

        #endregion

        public override Type getType()
        {
            return Type.Start;
        }
    }
}
