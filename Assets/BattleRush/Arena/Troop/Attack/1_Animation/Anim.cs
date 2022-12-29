using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ArenaS.TroopS.TroopAttackS
{
    public class Anim : TroopAttack.State
    {

        public VO<float> time;

        public VO<float> duration;

        public VO<uint> target;

        #region Constructor

        public enum Property
        {
            time,
            duration,
            target
        }

        public Anim() : base()
        {
            this.time = new VO<float>(this, (byte)Property.time, 0);
            this.duration = new VO<float>(this, (byte)Property.duration, 0.2f);
            this.target = new VO<uint>(this, (byte)Property.target, 0);
        }

        #endregion

        public override Type getType()
        {
            return Type.Animation;
        }

    }
}