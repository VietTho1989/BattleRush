using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommonLibrary.AssetBunbleS
{
    public abstract class Remove : Data
    {

        public enum Type
        {
            Immediately,
            MaxCount
        }

        public abstract Type getType();

        #region Immendidately

        public class Immediately : Remove
        {

            #region Constructor

            public enum Property
            {

            }

            public Immediately() : base()
            {

            }

            #endregion

            public override Type getType()
            {
                return Type.Immediately;
            }

        }

        #endregion

        #region max count

        public class MaxCount : Remove
        {

            public VO<int> max;

            #region Constructor

            public enum Property
            {
                max
            }

            public MaxCount() : base()
            {
                this.max = new VO<int>(this, (byte)Property.max, 5);
            }

            #endregion

            public override Type getType()
            {
                return Type.MaxCount;
            }

        }

        #endregion

    }
}