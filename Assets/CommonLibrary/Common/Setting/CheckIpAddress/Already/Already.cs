using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CheckIpAddressS
{
    public class Already : Settings.CheckIpAddressCountry
    {

        #region Constructor

        public enum Property
        {

        }

        public Already() : base()
        {

        }

        #endregion

        public override Type getType()
        {
            return Type.Already;
        }

    }
}