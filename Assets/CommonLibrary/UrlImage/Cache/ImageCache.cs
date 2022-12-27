using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Images.Cache;

namespace Images
{
    public class ImageCache : Data
    {

        public VO<string> url;

        #region State

        public abstract class State : Data
        {

            public enum Type
            {
                Get,
                Success,
                Fail
            }

            public abstract Type getType();

        }

        public VD<State> state;

        #endregion

        #region Constructor

        public enum Property
        {
            url,
            state
        }

        public ImageCache() : base()
        {
            this.url = new VO<string>(this, (byte)Property.url, "");
            this.state = new VD<State>(this, (byte)Property.state, new Get());
        }

        #endregion

    }
}