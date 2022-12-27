using System.Collections;
using System.Collections.Generic;
using BattleRushS.HeroS.RunS;
using UnityEngine;

namespace BattleRushS.HeroS
{
    public class HeroMoveRun : HeroMove.Sub
    {

        #region state

        public abstract class State : Data
        {

            public enum Type
            {
                None,
                CanPlay
            }

            public abstract Type getType();

        }

        #region state none

        public class StateNone : State
        {

            #region Constructor

            public enum Property
            {

            }

            public StateNone() : base()
            {

            }

            #endregion

            public override Type getType()
            {
                return Type.None;
            }

        }

        #endregion

        #region state Touch

        public class StateCanPlay : State
        {

            #region Sub

            public abstract class Sub : Data
            {

                public enum Type
                {
                    NotTouch,
                    Touch
                }

                public abstract Type getType();

            }

            #region notTouch

            public class NotTouch : Sub
            {

                #region Constructor

                public enum Property
                {

                }

                public NotTouch() : base()
                {

                }

                #endregion

                public override Type getType()
                {
                    return Type.NotTouch;
                }

            }

            #endregion

            #region touch

            public class Touch : Sub
            {

                #region player start value

                public VO<float> startInput;

                public VO<int> fingerId;

                #endregion

                #region touch position

                public VO<float> from;

                public VO<float> last;

                public VO<float> current;

                #endregion

                #region Constructor

                public enum Property
                {
                    startInput,
                    touchIndex,

                    from,
                    last,
                    current
                }

                public Touch() : base()
                {
                    this.startInput = new VO<float>(this, (byte)Property.startInput, 0);
                    this.fingerId = new VO<int>(this, (byte)Property.touchIndex, 0);
                    // touch position
                    {
                        this.from = new VO<float>(this, (byte)Property.from, 0);
                        this.last = new VO<float>(this, (byte)Property.last, 0);
                        this.current = new VO<float>(this, (byte)Property.current, float.MaxValue);
                    }
                }

                #endregion

                public override Type getType()
                {
                    return Type.Touch;
                }

            }

            #endregion

            public VD<Sub> sub;

            #endregion

            #region Constructor

            public enum Property
            {
                sub
            }

            public StateCanPlay() : base()
            {
                this.sub = new VD<Sub>(this, (byte)Property.sub, new NotTouch());
            }

            #endregion

            public override Type getType()
            {
                return Type.CanPlay;
            }

        }

        #endregion

        public VD<State> state;

        #endregion

        #region position

        public VO<float> move;

        /** tu -0.5 den 0.5 chieu ngang duong di*/
        public VO<float> newPos;

        #endregion

        public VD<HeroRunShoot> heroRunShoot;

        #region Constructor

        public enum Property
        {
            state,
            move,
            newPos,
            heroRunShoot
        }

        public HeroMoveRun() : base()
        {
            this.state = new VD<State>(this, (byte)Property.state, new StateNone());
            this.move = new VO<float>(this, (byte)Property.move, 0);
            this.newPos = new VO<float>(this, (byte)Property.newPos, 0);
            this.heroRunShoot = new VD<HeroRunShoot>(this, (byte)Property.heroRunShoot, new HeroRunShoot());
        }

        #endregion


        public override Type getType()
        {
            return Type.Run;
        }

    }
}
