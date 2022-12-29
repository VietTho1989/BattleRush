using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ArenaS.ProjectileS
{
    public class DealDamage : Projectile.State
    {

        public VO<float> time;

        public VO<float> duration;

        #region Constructor

        public enum Property
        {
            time,
            duration
        }

        public DealDamage() : base()
        {
            this.time = new VO<float>(this, (byte)Property.time, 0);
            this.duration = new VO<float>(this, (byte)Property.duration, 1.0f);
        }

        #endregion

        public override Type getType()
        {
            return Type.DealDamage;
        }

    }
}