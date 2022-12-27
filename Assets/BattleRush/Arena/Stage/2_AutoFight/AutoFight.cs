using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ArenaS
{
    public class AutoFight : Arena.Stage
    {

        #region Constructor

        public enum Property
        {

        }

        public AutoFight() : base()
        {

        }

        #endregion

        public override Type getType()
        {
            return Type.AutoFight;
        }

    }
}