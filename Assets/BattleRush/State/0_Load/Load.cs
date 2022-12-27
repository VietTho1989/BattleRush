using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleRushS.StateS.LoadS;

namespace BattleRushS.StateS
{
    public class Load : BattleRush.State
    {

        #region sub

        public abstract class Sub : Data
        {
            public enum Type
            {
                Reset,
                ChooseLevel,
                LoadLevel
            }

            public abstract Type getType();
        }

        public VD<Sub> sub;

        #endregion

        #region Constructor

        public enum Property
        {
            sub
        }

        public Load() : base()
        {
            this.sub = new VD<Sub>(this, (byte)Property.sub, new Reset());
        }

        #endregion

        public override Type getType()
        {
            return Type.Load;
        }
    }
}
