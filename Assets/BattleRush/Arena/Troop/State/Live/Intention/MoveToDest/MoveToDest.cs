using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ArenaS.TroopS.IntentionS
{
    public class MoveToDest : TroopIntention.Intention
    {

        public VO<Vector3> dest;

        #region Constructor

        public enum Property
        {
            dest
        }

        public MoveToDest() : base()
        {
            this.dest = new VO<Vector3>(this, (byte)Property.dest, Vector3.zero);
        }

        #endregion

        public override Type getType()
        {
            return Type.MoveToDest;
        }

    }
}