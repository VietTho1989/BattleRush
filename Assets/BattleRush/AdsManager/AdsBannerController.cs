using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattleRushS
{
    public class AdsBannerController : Data
    {

        #region state

        public abstract class State : Data
        {

            public enum Type
            {
                Show,
                NotShow
            }

            public abstract Type getType();

            #region Show

            public class Show : State
            {

                public VO<bool> alreadyRequest;

                #region Constructor

                public enum Property
                {
                    alreadyRequest
                }

                public Show() : base()
                {
                    this.alreadyRequest = new VO<bool>(this, (byte)Property.alreadyRequest, false);
                }

                #endregion

                public override Type getType()
                {
                    return Type.Show;
                }

            }

            #endregion

            #region NotShow

            public class NotShow : State
            {

                #region Constructor

                public enum Property
                {

                }

                public NotShow() : base()
                {

                }

                #endregion

                public override Type getType()
                {
                    return Type.NotShow;
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

        public AdsBannerController() : base()
        {
            this.state = new VD<State>(this, (byte)Property.state, new State.NotShow());
        }

        #endregion

    }
}