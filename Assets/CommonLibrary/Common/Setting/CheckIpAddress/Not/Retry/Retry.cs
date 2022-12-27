using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CheckIpAddressS.NotS
{
    public class Retry : Not.Sub
    {

        #region Constructor

        public enum Property
        {

        }

        public Retry() : base()
        {

        }

        #endregion

        public override Type getType()
        {
            return Type.Retry;
        }

    }
}