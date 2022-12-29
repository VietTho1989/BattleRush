using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ArenaS.ProjectileS
{
    public class Move : Projectile.State
    {

        public VO<Vector3> position;

        public VO<float> time;

        #region Constructor

        public enum Property
        {
            position,
            time
        }

        public Move() : base()
        {
            this.position = new VO<Vector3>(this, (byte)Property.position, Vector3.zero);
            this.time = new VO<float>(this, (byte)Property.time, 0);
        }

        #endregion

        public override Type getType()
        {
            return Type.Move;
        }

    }
}