using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.StateS
{
    public class End : BattleRush.State
    {

        public VO<bool> isWin;

        public VO<float> time;

        #region Constructor

        public enum Property
        {
            isWin,
            time
        }

        public End() : base()
        {
            this.isWin = new VO<bool>(this, (byte)Property.isWin, false);
            this.time = new VO<float>(this, (byte)Property.time, 0);
        }

        #endregion

        public override Type getType()
        {
            return Type.End;
        }
    }
}
