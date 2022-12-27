using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ObjectS.TroopCageS
{
    public class ReleaseTroop : TroopCage.State
    {

        public VO<bool> alreadyRelease;

        public VO<float> time;

        public VO<float> duration;

        #region Constructor

        public enum Property
        {
            alreadyRelease,
            time,
            duration
        }

        public ReleaseTroop() : base()
        {
            this.alreadyRelease = new VO<bool>(this, (byte)Property.alreadyRelease, false);
            this.time = new VO<float>(this, (byte)Property.time, 0);
            this.duration = new VO<float>(this, (byte)Property.duration, 3);
        }

        #endregion

        public override Type getType()
        {
            return Type.ReleaseTroop;
        }

    }
}