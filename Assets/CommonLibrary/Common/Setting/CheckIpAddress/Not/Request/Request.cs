using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CheckIpAddressS.NotS
{
    public class Request : Not.Sub
    {

        #region Constructor

        public enum Property
        {

        }

        public Request() : base()
        {

        }

        #endregion

        public override Type getType()
        {
            return Type.Request;
        }

    }
}