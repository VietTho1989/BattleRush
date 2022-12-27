using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommonLibrary.AssetBunbleS
{
    public class Fail<T> : State<T>
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
}