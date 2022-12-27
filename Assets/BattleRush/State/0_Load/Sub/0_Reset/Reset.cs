using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.StateS.LoadS
{
    public class Reset : Load.Sub
    {

        #region Constructor

        public enum Property
        {

        }

        public Reset() : base()
        {

        }

        #endregion

        public override Type getType()
        {
            return Type.Reset;
        }

    }
}