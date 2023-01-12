using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ObjectS
{
    public class EnergyOrbNormal : ObjectInPath
    {

        #region state

        public enum State
        {
            Normal,
            PickUp,
            PickedUpFinish,
        }

        public VO<State> state;

        #endregion

        public VO<Position> position;

        #region Constructor

        public enum Property
        {
            state,
            position
        }

        public EnergyOrbNormal() : base()
        {
            this.state = new VO<State>(this, (byte)Property.state, State.Normal);
            this.position = new VO<Position>(this, (byte)Property.position, Position.Zero);
        }

        #endregion


        public override Type getType()
        {
            return Type.EnergyOrbNormal;
        }

        public override Position getPosition()
        {
            return this.position.v;
        }

    }
}