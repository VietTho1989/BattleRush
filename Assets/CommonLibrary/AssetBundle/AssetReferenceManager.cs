using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CommonLibrary.AssetBunbleS
{
    public class AssetReferenceManager<T> : Data
    {

        public DD<AssetReferenceData<T>, AssetReference> assetReferenceDatas;

        public T getAsset(AssetReference assetReference)
        {
            T ret = default(T);
            {
                if (assetReference != null)
                {
                    CommonLibrary.AssetBunbleS.AssetReferenceData<T> assetReferenceData = null;
                    if (this.assetReferenceDatas.dict.TryGetValue(assetReference, out assetReferenceData))
                    {
                        switch (assetReferenceData.state.v.getType())
                        {
                            case CommonLibrary.AssetBunbleS.State<T>.Type.Load:
                                break;
                            case CommonLibrary.AssetBunbleS.State<T>.Type.Success:
                                {
                                    Success<T> success = assetReferenceData.state.v as Success<T>;
                                    ret = success.asset;
                                }
                                break;
                            case CommonLibrary.AssetBunbleS.State<T>.Type.Fail:
                                break;
                            default:
                                Logger.LogError("unknown type: " + assetReferenceData.state.v.getType());
                                break;
                        }
                    }
                }
                else
                {
                    Logger.LogError("assetReference null");
                }
            }
            return ret;
        }

        /**
         * Latest load success
         * */
        public VO<string> latestLoad;

        #region remove

        public VD<Remove> myRemove;

        #endregion

        #region Constructor

        public enum Property
        {
            assetReferenceDatas,
            latestLoad,
            myRemove
        }

        public AssetReferenceManager() : base()
        {
            this.assetReferenceDatas = new DD<AssetReferenceData<T>, AssetReference>(this, (byte)Property.assetReferenceDatas);
            this.latestLoad = new VO<string>(this, (byte)Property.latestLoad, "");
            this.myRemove = new VD<Remove>(this, (byte)Property.myRemove, new Remove.Immediately());
        }

        #endregion

        public void add(AssetReference assetReference, object use)
        {
            if (assetReference != null)
            {
                // get old
                {
                    AssetReferenceData<T> assetReferenceData = null;
                    if (this.assetReferenceDatas.dict.TryGetValue(assetReference, out assetReferenceData))
                    {
                        if (assetReferenceData.use.Add(use))
                        {
                            assetReferenceData.useChange.v++;
                        }
                        return;
                    }
                }
                // make new
                {
                    AssetReferenceData<T> assetReferenceData = new AssetReferenceData<T>();
                    {
                        assetReferenceData.uid = this.assetReferenceDatas.makeId();
                        assetReferenceData.assetReference = assetReference;
                        assetReferenceData.use.Add(use);
                    }
                    this.assetReferenceDatas.add(assetReferenceData);
                }
            }
            else
            {
                Logger.LogError("assetReference null");
            }
        }

        public void remove(AssetReference assetReference, object use)
        {
            if (assetReference != null)
            {
                AssetReferenceData<T> assetReferenceData = null;
                if (this.assetReferenceDatas.dict.TryGetValue(assetReference, out assetReferenceData))
                {
                    if (assetReferenceData.use.Remove(use))
                    {
                        assetReferenceData.useChange.v++;
                    }
                }
            }
            else
            {
                Logger.LogError("assetReference null");
            }
        }

    }
}