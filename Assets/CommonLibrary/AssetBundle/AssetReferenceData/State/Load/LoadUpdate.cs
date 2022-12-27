using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CommonLibrary.AssetBunbleS
{
    public abstract class LoadUpdate<T> : UpdateBehavior<Load<T>>
    {

        #region Update

        public override void update()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {

                }
                else
                {
                    Logger.LogError("data null");
                }
            }
        }

        public override bool isShouldDisableUpdate()
        {
            return false;
        }

        private AsyncOperationHandle<T> progress;

        public override void Update()
        {
            base.Update();
            if (this.data != null)
            {
                // start load
                {
                    if (this.data.time.v == 0)
                    {
                        AssetReferenceData<T> assetReferenceData = this.data.findDataInParent<AssetReferenceData<T>>();
                        if (assetReferenceData != null)
                        {
                            progress = Addressables.LoadAssetAsync<T>(assetReferenceData.assetReference);
                            progress.Completed += OnLoadDone;
                        }
                        else
                        {
                            Logger.LogError("assetReferenceData null");
                        }
                    }
                }
                // update
                {
                    if (this.data != null)
                    {
                        // progress
                        {
                            this.data.progress.v = progress.PercentComplete;
                        }
                        this.data.time.v += Time.deltaTime;
                        Logger.Log("Load asset progress: " + this.data.time.v + ", " + this.data.progress.v);
                    }
                }
            }
            else
            {
                Logger.LogError("data null");
            }
        }

        private void OnLoadDone(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<T> asyncOperationHandle)
        {
            // find
            bool needRelease = true;
            {
                if (this)
                {
                    if (this.data != null)
                    {
                        AssetReferenceData<T> assetReferenceData = this.data.findDataInParent<AssetReferenceData<T>>();
                        if (assetReferenceData != null)
                        {
                            needRelease = false;
                            // notify download
                            {
                                AssetReferenceManager<T> assetReferenceManager = this.data.findDataInParent<AssetReferenceManager<T>>();
                                if (assetReferenceManager != null)
                                {
                                    assetReferenceManager.latestLoad.v = assetReferenceData.assetReference.AssetGUID;
                                    Logger.Log("load success: " + assetReferenceManager.latestLoad.v);
                                }
                                else
                                {
                                    Logger.LogError("assetReferenceManager null");
                                }
                            }
                            // change state
                            {
                                Success<T> success = new Success<T>();
                                {
                                    success.asset = asyncOperationHandle.Result;
                                }
                                assetReferenceData.state.v = success;
                            }
                        }
                        else
                        {
                            Logger.LogError("assetReferenceData null");
                        }
                    }
                    else
                    {
                        Logger.LogError("data null");
                    }
                }
            }
            // process
            if (needRelease)
            {
                Logger.LogError("LoadUpdate already deleted");
                Addressables.Release<T>(asyncOperationHandle.Result);
            }
        }

        #endregion

        #region implement callBacks

        public override void onAddCallBack<M>(M data)
        {
            if (data is Load<T>)
            {
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<M>(M data, bool isHide)
        {
            if (data is Load<T>)
            {
                Load<T> load = data as Load<T>;
                this.setDataNull(load);
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onUpdateSync<M>(WrapProperty wrapProperty, List<Sync<M>> syncs)
        {
            if (WrapProperty.checkError(wrapProperty))
            {
                return;
            }
            if (wrapProperty.p is Load<T>)
            {
                switch ((Load<T>.Property)wrapProperty.n)
                {
                    case Load<T>.Property.time:
                        break;
                    case Load<T>.Property.progress:
                        break;
                    default:
                        Logger.LogError("Don't process: " + wrapProperty + "; " + this);
                        break;
                }
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + syncs + "; " + this);
        }

        #endregion

    }
}