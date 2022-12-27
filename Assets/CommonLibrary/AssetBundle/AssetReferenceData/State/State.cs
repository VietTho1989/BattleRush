using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommonLibrary.AssetBunbleS
{
    public abstract class State<T> : Data
    {

        public enum Type
        {
            Load,
            Success,
            Fail
        }

        public abstract Type getType();

    }
}