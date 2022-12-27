using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.StateS
{
    public class Play : BattleRush.State
    {

        #region State

        public enum State
        {
            Normal,
            Pause
        }

        public VO<State> state;

        #endregion

        #region Constructor

        public enum Property
        {
            state
        }

        public Play() : base()
        {
            this.state = new VO<State>(this, (byte)Property.state, State.Normal);
        }

        #endregion

        public override Type getType()
        {
            return Type.Play;
        }
    }
}
