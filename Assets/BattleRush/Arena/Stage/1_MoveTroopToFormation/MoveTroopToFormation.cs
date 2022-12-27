using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ArenaS
{
    public class MoveTroopToFormation : Arena.Stage
    {

        #region Constructor

        public enum Property
        {

        }

        public MoveTroopToFormation() : base()
        {

        }

        #endregion

        public override Type getType()
        {
            return Type.MoveTroopToFormation;
        }

    }
}