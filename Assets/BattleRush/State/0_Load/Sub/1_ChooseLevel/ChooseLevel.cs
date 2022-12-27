using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.StateS.LoadS
{
    public class ChooseLevel : Load.Sub
    {

        #region Constructor

        public enum Property
        {

        }

        public ChooseLevel() : base()
        {

        }

        #endregion

        public override Type getType()
        {
            return Type.ChooseLevel;
        }

    }
}
