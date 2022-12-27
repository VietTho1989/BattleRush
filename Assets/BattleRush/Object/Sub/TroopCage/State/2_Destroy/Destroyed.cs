using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ObjectS.TroopCageS
{
    public class Destroyed : TroopCage.State
    {

        #region Constructor

        public enum Property
        {

        }

        public Destroyed() : base()
        {

        }

        #endregion

        public override Type getType()
        {
            return Type.Destroyed;
        }

    }
}