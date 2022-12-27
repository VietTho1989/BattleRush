using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommonLibrary.AssetBunbleS
{
    public class Load<T> : State<T>
    {

        public VO<float> time;

        public VO<float> progress;

        #region Constructor

        public enum Property
        {
            time,
            progress
        }

        public Load() : base()
        {
            this.time = new VO<float>(this, (byte)Property.time, 0);
            this.progress = new VO<float>(this, (byte)Property.progress, 0);
        }

        #endregion

        public override Type getType()
        {
            return Type.Load;
        }

    }
}