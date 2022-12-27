using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ArenaS
{
    public class FightEnd : Arena.Stage
    {

        #region Constructor

        public enum Property
        {

        }

        public FightEnd() : base()
        {

        }

        #endregion

        public override Type getType()
        {
            return Type.FightEnd;
        }

    }
}