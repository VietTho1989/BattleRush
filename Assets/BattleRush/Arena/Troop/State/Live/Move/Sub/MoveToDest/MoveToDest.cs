using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattleRushS.ArenaS.TroopS.TroopMoveS
{
    public class MoveToDest : TroopMove.Sub
    {

        public VO<Vector3> dest;

        public VO<bool> alreadyCallAgent;

        #region Constructor

        public enum Property
        {
            dest,
            alreadyCallAgent
        }

        public MoveToDest() : base()
        {
            this.dest = new VO<Vector3>(this, (byte)Property.dest, Vector3.zero);
            this.alreadyCallAgent = new VO<bool>(this, (byte)Property.alreadyCallAgent, false);
        }

        #endregion

        public override Type getType()
        {
            return Type.MoveToDest;
        }

    }
}