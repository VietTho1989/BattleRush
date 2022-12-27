using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CommonLibrary.AssetBunbleS
{
    public class AssetReferenceData<T> : Data, GetKeyInterface<AssetReference>
    {

        public AssetReference assetReference;

        #region state

        public VD<State<T>> state;

        #endregion

        #region use

        public HashSet<object> use = new HashSet<object>();
        public VO<int> useChange;

        #endregion

        #region Constructor

        public enum Property
        {
            state,
            useChange
        }

        public AssetReferenceData() : base()
        {
            this.state = new VD<State<T>>(this, (byte)Property.state, new Load<T>());
            this.useChange = new VO<int>(this, (byte)Property.useChange, 0);
        }

        #endregion

        public AssetReference getKey()
        {
            return assetReference;
        }

    }
}