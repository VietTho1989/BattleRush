using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ObjectS.TroopCageS
{
    public class TroopCageShootProjectile : Data
    {

        public VO<Vector3> startPosition;

        #region state

        public abstract class State : Data
        {

            public enum Type
            {
                Move,
                Hit,
                Finish
            }

            public abstract Type getType();

            #region move

            public class Move : State
            {

                public VO<float> time;

                public VO<float> duration;

                #region Constructor

                public enum Property
                {
                    time,
                    duration
                }

                public Move() : base()
                {
                    this.time = new VO<float>(this, (byte)Property.time, 0);
                    this.duration = new VO<float>(this, (byte)Property.duration, 1.0f);
                }

                #endregion

                public override Type getType()
                {
                    return Type.Move;
                }

            }

            #endregion

            #region hit

            public class Hit : State
            {

                #region Constructor

                public enum Property
                {

                }

                public Hit() : base()
                {

                }

                #endregion

                public override Type getType()
                {
                    return Type.Hit;
                }

            }

            #endregion

            #region Finish

            public class Finish : State
            {

                #region Constructor

                public enum Property
                {

                }

                public Finish() : base()
                {

                }

                #endregion

                public override Type getType()
                {
                    return Type.Finish;
                }

            }

            #endregion

        }

        public VD<State> state;

        #endregion

        #region Constructor

        public enum Property
        {
            startPosition,
            state
        }

        public TroopCageShootProjectile() : base()
        {
            this.startPosition = new VO<Vector3>(this, (byte)Property.startPosition, Vector3.zero);
            this.state = new VD<State>(this, (byte)Property.state, new State.Move());
        }

        #endregion

    }
}
