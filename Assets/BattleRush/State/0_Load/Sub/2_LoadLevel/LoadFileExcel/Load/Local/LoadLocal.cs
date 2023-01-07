using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BattleRushS.StateS.LoadS
{
    public class LoadLocal : LoadLevelByFileExcel.Step
    {

        #region state

        public abstract class State : Data
        {
            public enum Type
            {
                Load,
                Success,
                Fail
            }

            public abstract Type getType();

            #region Load

            public class Load : State
            {

                #region Constructor

                public enum Property
                {

                }

                public Load() : base()
                {

                }

                #endregion

                public override Type getType()
                {
                    return Type.Load;
                }
            }

            #endregion

            #region Success

            public class Success : State
            {

                public VO<ReferenceData<MapData>> mapData;

                #region state

                public enum State
                {
                    Start,
                    WaitReady,
                    Ready
                }

                public VO<State> state;

                #endregion

                #region Constructor

                public enum Property
                {
                    mapData,
                    state
                }

                public Success() : base()
                {
                    this.mapData = new VO<ReferenceData<MapData>>(this, (byte)Property.mapData, new ReferenceData<MapData>(null));
                    this.state = new VO<State>(this, (byte)Property.state, State.Start);
                }

                #endregion

                public override Type getType()
                {
                    return Type.Success;
                }
            }

            #endregion

            #region Fail

            public class Fail : State
            {

                #region Constructor

                public enum Property
                {

                }

                public Fail() : base()
                {

                }

                #endregion

                public override Type getType()
                {
                    return Type.Fail;
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

        public LoadLocal() : base()
        {
            this.state = new VD<State>(this, (byte)Property.state, new State.Load());
        }

        #endregion

        public override Type getType()
        {
            return Type.Local;
        }

    }
}