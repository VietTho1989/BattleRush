using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.HeroS.RunS
{
    public class HeroRunShoot : Data
    {

        #region state

        public abstract class State : Data
        {

            public enum Type
            {
                Normal,
                CoolDown
            }

            public abstract Type getType();

            #region Normal

            public class Normal : State
            {

                #region Constructor

                public enum Property
                {

                }

                public Normal() : base()
                {

                }

                #endregion

                public override Type getType()
                {
                    return Type.Normal;
                }

            }

            #endregion

            #region CoolDown

            public class CoolDown : State
            {

                public VO<float> time;

                public VO<float> duration;

                #region Constructor

                public enum Property
                {
                    time,
                    duration
                }

                public CoolDown() : base()
                {
                    this.time = new VO<float>(this, (byte)Property.time, 0);
                    this.duration = new VO<float>(this, (byte)Property.duration, 0.3f);
                }

                #endregion

                public override Type getType()
                {
                    return Type.CoolDown;
                }

            }

            #endregion

        }

        public VD<State> state;

        #endregion

        #region Constructor

        public enum Property
        {
            state
        }

        public HeroRunShoot() : base()
        {
            this.state = new VD<State>(this, (byte)Property.state, new State.Normal());
        }

        #endregion

    }
}