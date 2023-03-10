using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ValueChangeUtils
{

    #region Update CallBack

    public static void replaceCallBack<T>(ValueChangeCallBack callBack, List<Sync<T>> syncs)
    {
        for (int syncCount = 0; syncCount < syncs.Count; syncCount++)
        {
            Sync<T> sync = syncs[syncCount];
            switch (sync.getType())
            {
                case Sync<T>.Type.DictSet:
                    {
                        SyncDictSet<T> syncDictSet = (SyncDictSet<T>)sync;
                        if (syncDictSet.olds.Count == syncDictSet.news.Count)
                        {
                            for (int i = 0; i < syncDictSet.olds.Count; i++)
                            {
                                // od value
                                RemoveCallBack(callBack, syncDictSet.olds[i]);
                                // add new value
                                AddCallBack(callBack, syncDictSet.news[i]);
                            }
                        }
                        else
                        {
                            Logger.LogError("count error: " + syncDictSet.olds.Count + "; " + syncDictSet.news.Count);
                        }
                    }
                    break;
                case Sync<T>.Type.DictAdd:
                    {
                        SyncDictAdd<T> syncDictAdd = (SyncDictAdd<T>)sync;
                        for (int i = 0; i < syncDictAdd.values.Count; i++)
                        {
                            T value = syncDictAdd.values[i];
                            AddCallBack(callBack, value);
                        }
                    }
                    break;
                case Sync<T>.Type.DictRemove:
                    {
                        SyncDictRemove<T> syncDictRemove = (SyncDictRemove<T>)sync;
                        for (int i = 0; i < syncDictRemove.values.Count; i++)
                        {
                            T value = syncDictRemove.values[i];
                            RemoveCallBack(callBack, value);
                        }
                    }
                    break;

                case Sync<T>.Type.Set:
                    {
                        SyncSet<T> syncSet = (SyncSet<T>)sync;
                        if (syncSet.olds.Count == syncSet.news.Count)
                        {
                            for (int i = 0; i < syncSet.olds.Count; i++)
                            {
                                // od value
                                RemoveCallBack(callBack, syncSet.olds[i]);
                                // add new value
                                AddCallBack(callBack, syncSet.news[i]);
                            }
                        }
                        else
                        {
                            Logger.LogError("count error: " + syncSet.olds.Count + "; " + syncSet.news.Count);
                        }
                    }
                    break;
                case Sync<T>.Type.Add:
                    {
                        SyncAdd<T> syncAdd = (SyncAdd<T>)sync;
                        for (int i = 0; i < syncAdd.values.Count; i++)
                        {
                            T value = syncAdd.values[i];
                            AddCallBack(callBack, value);
                        }
                    }
                    break;
                case Sync<T>.Type.Remove:
                    {
                        SyncRemove<T> syncRemove = (SyncRemove<T>)sync;
                        for (int i = 0; i < syncRemove.values.Count; i++)
                        {
                            T value = syncRemove.values[i];
                            RemoveCallBack(callBack, value);
                        }
                    }
                    break;
                default:
                    Logger.LogError("unknown type: " + sync.getType());
                    break;
            }
        }
    }

    public static void RemoveCallBack<T>(ValueChangeCallBack callBack, T value)
    {
        if (value != null)
        {
            if (Generic.IsAddCallBackInterface<T>())
            {
                ((AddCallBackInterface)value).removeCallBack(callBack);
            }
            else
            {
                Logger.LogError("value error: " + value + ", " + callBack);
            }
        }
        else
        {
            // Logger.Log ("value null: " + value + "; " + callBack);
        }
    }

    public static void AddCallBack<T>(ValueChangeCallBack callBack, T value)
    {
        if (value != null)
        {
            if (Generic.IsAddCallBackInterface<T>())
            {
                ((AddCallBackInterface)value).addCallBack(callBack);
            }
            else
            {
                Logger.LogError("value error: " + value + ", " + callBack);
            }
        }
        else
        {
            // Logger.Log("value null: " + value + "; " + callBack);
        }
    }

    #endregion

    public static void getAddAndRemoveValues<T>(List<Sync<T>> syncs, List<T> addValues, List<T> removeValues)
    {
        for (int syncCount = 0; syncCount < syncs.Count; syncCount++)
        {
            Sync<T> sync = syncs[syncCount];
            switch (sync.getType())
            {
                case Sync<T>.Type.Set:
                    {
                        SyncSet<T> syncSet = (SyncSet<T>)sync;
                        if (syncSet.olds.Count == syncSet.news.Count)
                        {
                            for (int i = 0; i < syncSet.olds.Count; i++)
                            {
                                // od value
                                removeValues.Add(syncSet.olds[i]);
                                // add new value
                                addValues.Add(syncSet.news[i]);
                            }
                        }
                        else
                        {
                            Logger.LogError("count error: " + syncSet.olds.Count + "; " + syncSet.news.Count);
                        }
                    }
                    break;
                case Sync<T>.Type.Add:
                    {
                        SyncAdd<T> syncAdd = (SyncAdd<T>)sync;
                        for (int i = 0; i < syncAdd.values.Count; i++)
                        {
                            T value = syncAdd.values[i];
                            addValues.Add(value);
                        }
                    }
                    break;
                case Sync<T>.Type.Remove:
                    {
                        SyncRemove<T> syncRemove = (SyncRemove<T>)sync;
                        for (int i = 0; i < syncRemove.values.Count; i++)
                        {
                            T value = syncRemove.values[i];
                            removeValues.Add(value);
                        }
                    }
                    break;
                default:
                    Logger.LogError("unknown type: " + sync.getType());
                    break;
            }
        }
    }

}