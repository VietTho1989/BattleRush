using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CommonLibrary.AssetBunbleS
{
    /**
     * TODO co the se them OnLowMemory nua
     * */
    public abstract class AssetReferenceManagerUpdate<T, LOAD_UPDATE> : UpdateBehavior<AssetReferenceManager<T>> where LOAD_UPDATE : LoadUpdate<T>
    {

        private void OnLowMemory()
        {
            Logger.LogError("onLownMemory");
            isLowMemory = true;
            dirty = true;
            update();
        }

        #region Update

        bool isLowMemory = false;

        public override void update()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    // find
                    bool canRemove = false;
                    {
                        if (isLowMemory)
                        {
                            isLowMemory = false;
                            canRemove = true;
                        }
                        else
                        {
                            switch (this.data.myRemove.v.getType())
                            {
                                case Remove.Type.Immediately:
                                    canRemove = true;
                                    break;
                                case Remove.Type.MaxCount:
                                    {
                                        Remove.MaxCount maxCount = this.data.myRemove.v as Remove.MaxCount;
                                        if (maxCount.max.v <= this.data.assetReferenceDatas.dict.Count)
                                        {
                                            canRemove = true;
                                        }
                                    }
                                    break;
                                default:
                                    Logger.LogError("unknown type: " + this.data.myRemove.v.getType());
                                    break;
                            }
                        }
                    }
                    // process
                    if (canRemove)
                    {
                        // remove not use anymore
                        {
                            List<AssetReferenceData<T>> removes = new List<AssetReferenceData<T>>();
                            {
                                foreach (AssetReferenceData<T> assetReferenceData in this.data.assetReferenceDatas.dict.Values)
                                {
                                    if (assetReferenceData.use.Count == 0)
                                    {
                                        switch (assetReferenceData.state.v.getType())
                                        {
                                            case State<T>.Type.Load:
                                                break;
                                            case State<T>.Type.Success:
                                                {
                                                    Success<T> success = assetReferenceData.state.v as Success<T>;
                                                    Addressables.Release<T>(success.asset);
                                                }
                                                break;
                                            case State<T>.Type.Fail:
                                                break;
                                            default:
                                                break;
                                        }
                                        removes.Add(assetReferenceData);
                                    }
                                }
                            }
                            this.data.assetReferenceDatas.remove(removes);
                        }
                        Logger.Log("AssetReferenceManager: " + this.data.assetReferenceDatas.dict.Count);
                    }
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

        #endregion

        #region implement callBacks

        public override void onAddCallBack<M>(M data)
        {
            if(data is AssetReferenceManager<T>)
            {
                AssetReferenceManager<T> assetReferenceManager = data as AssetReferenceManager<T>;
                // Child
                {
                    assetReferenceManager.assetReferenceDatas.allAddCallBack(this);
                    assetReferenceManager.myRemove.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            {
                // assetReferenceData
                {
                    if (data is AssetReferenceData<T>)
                    {
                        AssetReferenceData<T> assetReferenceData = data as AssetReferenceData<T>;
                        // Child
                        {
                            assetReferenceData.state.allAddCallBack(this);
                        }
                        dirty = true;
                        return;
                    }
                    // Child
                    if (data is State<T>)
                    {
                        State<T> state = data as State<T>;
                        // Update
                        {
                            switch (state.getType())
                            {
                                case State<T>.Type.Load:
                                    {
                                        Load<T> load = state as Load<T>;
                                        UpdateUtils.makeUpdate<LOAD_UPDATE, Load<T>>(load, this.transform);
                                    }
                                    break;
                                case State<T>.Type.Success:
                                    break;
                                case State<T>.Type.Fail:
                                    break;
                                default:
                                    Logger.LogError("unknown type: " + state.getType());
                                    break;
                            }
                        }
                        dirty = true;
                        return;
                    }
                }
                if(data is Remove)
                {
                    dirty = true;
                    return;
                }
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<M>(M data, bool isHide)
        {
            if (data is AssetReferenceManager<T>)
            {
                AssetReferenceManager<T> assetReferenceManager = data as AssetReferenceManager<T>;
                // Child
                {
                    assetReferenceManager.assetReferenceDatas.allRemoveCallBack(this);
                    assetReferenceManager.myRemove.allRemoveCallBack(this);
                }
                this.setDataNull(assetReferenceManager);
                return;
            }
            // Child
            {
                // assetReferenceData
                {
                    if (data is AssetReferenceData<T>)
                    {
                        AssetReferenceData<T> assetReferenceData = data as AssetReferenceData<T>;
                        // Child
                        {
                            assetReferenceData.state.allRemoveCallBack(this);
                        }
                        return;
                    }
                    // Child
                    if (data is State<T>)
                    {
                        State<T> state = data as State<T>;
                        // Update
                        {
                            switch (state.getType())
                            {
                                case State<T>.Type.Load:
                                    {
                                        Load<T> load = state as Load<T>;
                                        load.removeCallBackAndDestroy(typeof(LOAD_UPDATE));
                                    }
                                    break;
                                case State<T>.Type.Success:
                                    break;
                                case State<T>.Type.Fail:
                                    break;
                                default:
                                    Logger.LogError("unknown type: " + state.getType());
                                    break;
                            }
                        }
                        return;
                    }
                }
                if (data is Remove)
                {
                    return;
                }
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onUpdateSync<M>(WrapProperty wrapProperty, List<Sync<M>> syncs)
        {
            if (WrapProperty.checkError(wrapProperty))
            {
                return;
            }
            if (wrapProperty.p is AssetReferenceManager<T>)
            {
                switch ((AssetReferenceManager<T>.Property)wrapProperty.n)
                {
                    case AssetReferenceManager<T>.Property.assetReferenceDatas:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case AssetReferenceManager<T>.Property.latestLoad:
                        break;
                    case AssetReferenceManager<T>.Property.myRemove:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    default:
                        Logger.LogError("Don't process: " + wrapProperty + "; " + this);
                        break;
                }
                return;
            }
            // Child
            {
                // assetReferenceData
                {
                    if (wrapProperty.p is AssetReferenceData<T>)
                    {
                        switch ((AssetReferenceData<T>.Property)wrapProperty.n)
                        {
                            case AssetReferenceData<T>.Property.state:
                                {
                                    ValueChangeUtils.replaceCallBack(this, syncs);
                                    dirty = true;
                                }
                                break;
                            case AssetReferenceData<T>.Property.useChange:
                                dirty = true;
                                break;
                            default:
                                Logger.LogError("Don't process: " + wrapProperty + "; " + this);
                                break;
                        }
                        return;
                    }
                    // Child
                    if (wrapProperty.p is State<T>)
                    {
                        return;
                    }
                }
                if(wrapProperty.p is Remove)
                {
                    Remove remove = wrapProperty.p as Remove;
                    switch (remove.getType())
                    {
                        case Remove.Type.Immediately:
                            break;
                        case Remove.Type.MaxCount:
                            {
                                switch ((Remove.MaxCount.Property)wrapProperty.n)
                                {
                                    case Remove.MaxCount.Property.max:
                                        dirty = true;
                                        break;
                                    default:
                                        Logger.LogError("Don't process: " + wrapProperty + "; " + this);
                                        break;
                                }
                            }
                            break;
                        default:
                            Logger.LogError("unknown type: " + remove.getType());
                            break;
                    }
                    return;
                }
            }
            Logger.LogError("Don't process: " + data + "; " + syncs + "; " + this);
        }

        #endregion

    }
}