using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattleRushS
{
    public class AdsBannerController : Data
    {

        public const string REMOVE_AD = "REMOVE_AD";

        public VO<bool> removeAds;

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
            removeAds,
            state
        }

        public AdsBannerController() : base()
        {
            // removeAds
            {
                bool removeAds = false;
                {
                    try
                    {
                        removeAds = PlayerPrefs.GetInt(AdsBannerController.REMOVE_AD, 0) != 0;
                    }
                    catch(System.Exception e)
                    {
                        Logger.LogError(e);
                    }
                }
                this.removeAds = new VO<bool>(this, (byte)Property.removeAds, removeAds);
            }            
            this.state = new VD<State>(this, (byte)Property.state, new State.NotShow());
        }

        #endregion

    }
}