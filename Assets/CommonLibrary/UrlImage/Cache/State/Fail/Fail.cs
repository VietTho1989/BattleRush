using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Images.Cache
{
    public class Fail : ImageCache.State
    {

        #region Sub

        public abstract class Sub : Data
        {

            public enum Type
            {
                Cannot,
                RegetAfterTime
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

        public Fail() : base()
        {
            this.sub = new VD<Sub>(this, (byte)Property.sub, null);
        }

        #endregion

        public override Type getType()
        {
            return Type.Fail;
        }

    }
}