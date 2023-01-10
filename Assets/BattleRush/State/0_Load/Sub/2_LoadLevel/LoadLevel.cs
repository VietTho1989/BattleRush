using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.StateS.LoadS
{
    public class LoadLevel : Load.Sub
    {

        public VO<int> level;

        #region sub

        public abstract class Sub : Data
        {

            public enum Type
            {
                Excel,
                ScriptableObject
            }

            public abstract Type getType();

        }

        public VD<Sub> sub;

        #endregion

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
            level,
            sub,
            state
        }

        public LoadLevel() : base()
        {
            this.level = new VO<int>(this, (byte)Property.level, 0);
            this.sub = new VD<Sub>(this, (byte)Property.sub, new LoadLevelByScriptableObject());
            this.state = new VO<State>(this, (byte)Property.state, State.Start);
        }

        #endregion

        public override Type getType()
        {
            return Type.LoadLevel;
        }

    }
}
