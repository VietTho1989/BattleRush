using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ArenaS.TroopS
{
    public class Die : Troop.State
    {

        public VO<float> time;

        public VO<float> duration;


        #region Constructor

        public enum Property
        {
            time,
            duration
        }

        public Die() : base()
        {
            this.time = new VO<float>(this, (byte)Property.time, 0);
            this.duration = new VO<float>(this, (byte)Property.duration, 15);
        }

        #endregion

        public override Type getType()
        {
            return Type.Die;
        }

    }
}