using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ArenaS
{
    public class PreBattle : Arena.Stage
    {

        #region Constructor

        public enum Property
        {

        }

        public PreBattle() : base()
        {

        }

        #endregion

        public override Type getType()
        {
            return Type.PreBattle;
        }

    }
}