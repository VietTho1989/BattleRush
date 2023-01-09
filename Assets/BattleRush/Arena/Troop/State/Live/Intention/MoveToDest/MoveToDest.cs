using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ArenaS.TroopS.IntentionS
{
    public class MoveToDest : TroopIntention.Intention
    {

        #region time

        public VO<float> delay;

        public VO<float> time;

        #endregion

        public VO<Vector3> dest;

        #region Constructor

        public enum Property
        {
            delay,
            time,
            dest
        }

        public MoveToDest() : base()
        {
            this.delay = new VO<float>(this, (byte)Property.delay, 0);
            this.time = new VO<float>(this, (byte)Property.time, 0);
            this.dest = new VO<Vector3>(this, (byte)Property.dest, Vector3.zero);
        }

        #endregion

        public override Type getType()
        {
            return Type.MoveToDest;
        }

    }
}