using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Images.Cache
{
    public class Get : ImageCache.State
    {

        #region Constructor

        public enum Property
        {

        }

        public Get() : base()
        {

        }

        #endregion

        public override Type getType()
        {
            return Type.Get;
        }

    }
}