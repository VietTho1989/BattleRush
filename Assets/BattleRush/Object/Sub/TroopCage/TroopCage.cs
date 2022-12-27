using System.Collections;
using System.Collections.Generic;
using BattleRushS.HeroS;
using BattleRushS.ObjectS.TroopCageS;
using UnityEngine;

namespace BattleRushS.ObjectS
{
    public class TroopCage : ObjectInPath
    {

        public VO<Vector3> P;

        public VO<Vector3> R;

        #region state

        public abstract class State : Data
        {

            public enum Type
            {
                Live,
                ReleaseTroop,
                Destroyed
            }

            public abstract Type getType();


        }

        public VD<State> state;

        #endregion

        public LD<TroopFollow> troops;

        public VD<Obstruction> obstruction;

        #region Constructor

        public enum Property
        {
            P,
            R,
            state,
            troops,
            obstruction
        }

        public TroopCage() : base()
        {
            this.P = new VO<Vector3>(this, (byte)Property.P, Vector3.zero);
            this.R = new VO<Vector3>(this, (byte)Property.R, Vector3.zero);
            this.state = new VD<State>(this, (byte)Property.state, new Live());
            this.troops = new LD<TroopFollow>(this, (byte)Property.troops);
            this.obstruction = new VD<Obstruction>(this, (byte)Property.obstruction, new Obstruction());
        }

        #endregion

        public override Type getType()
        {
            return Type.TroopCage;
        }
    }
}
