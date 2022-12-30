using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.StateS
{
    public class End : BattleRush.State
    {

        public VO<bool> isWin;

        #region Constructor

        public enum Property
        {
            isWin
        }

        public End() : base()
        {
            this.isWin = new VO<bool>(this, (byte)Property.isWin, false);
        }

        #endregion

        public override Type getType()
        {
            return Type.End;
        }
    }
}
