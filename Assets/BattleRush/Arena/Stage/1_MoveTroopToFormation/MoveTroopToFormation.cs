using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ArenaS
{
    public class MoveTroopToFormation : Arena.Stage
    {

        public VO<float> time;

        public VO<float> duration;

        #region state

        public enum State
        {
            Start,
            Move,
            Came,
            Ready,
        }

        public VO<State> state;

        #endregion

        #region Constructor

        public enum Property
        {
            time,
            duration,
            state
        }

        public MoveTroopToFormation() : base()
        {
            this.time = new VO<float>(this, (byte)Property.time, 0);
            this.duration = new VO<float>(this, (byte)Property.duration, 10);
            this.state = new VO<State>(this, (byte)Property.state, State.Start);
        }

        #endregion

        public override Type getType()
        {
            return Type.MoveTroopToFormation;
        }

    }
}