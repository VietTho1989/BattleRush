using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommonLibrary.AssetBunbleS
{
    public class Success<T> : State<T>
    {

        public T asset;

        #region Constructor

        public enum Property
        {

        }

        public Success() : base()
        {

        }

        #endregion

        public override Type getType()
        {
            return Type.Success;
        }

    }
}