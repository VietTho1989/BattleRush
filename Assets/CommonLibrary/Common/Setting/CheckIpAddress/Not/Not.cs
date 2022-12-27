using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CheckIpAddressS
{
    public class Not : Settings.CheckIpAddressCountry
    {

        #region sub

        public abstract class Sub : Data
        {

            public enum Type
            {
                Request,
                Retry
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

        public Not() : base()
        {
            this.sub = new VD<Sub>(this, (byte)Property.sub, new NotS.Request());
        }

        #endregion

        public override Type getType()
        {
            return Type.Not;
        }

    }
}