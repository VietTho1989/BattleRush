using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ObjectS
{
    public class EnergyOrbUpgrade : ObjectInPath
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

        public VO<Vector3> P;

        public VO<Vector3> R;

        #region Constructor

        public enum Property
        {
            state,
            P,
            R
        }

        public EnergyOrbUpgrade() : base()
        {
            this.state = new VO<State>(this, (byte)Property.state, State.Normal);
            this.P = new VO<Vector3>(this, (byte)Property.P, Vector3.zero);
            this.R = new VO<Vector3>(this, (byte)Property.R, Vector3.zero);
        }

        #endregion

        public override Type getType()
        {
            return Type.EnergyOrbUpgrade;
        }

    }
}
