using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class DO<T, K> : WrapProperty where T : GetKeyInterface<K>
{

    #region isData

    private static readonly bool IsAddCallBackInterface = false;

    static DO()
    {
        IsAddCallBackInterface = Generic.IsAddCallBackInterface<T>();
    }

    public override uint getIndex()
    {
        Logger.LogError("why get index with LO");
        return 0;
    }

    public override void setIndex(uint i)
    {

    }

    #endregion

    #region Constructor

    public DO(Data parent, byte name) : base(parent, name)
    {
        //Debug.Log ("ListProperty Constructor: " + parent + ", " + properties);
    }

    public override string ToString()
    {
        return base.ToString();
    }

    public override System.Type getValueType()
    {
        return typeof(T);
    }

    public override object getValue()
    {
        return this.getValue(0);
    }

    public override object getValue(int index)
    {
        return null;
    }

    public override int getValueCount()
    {
        return this.dict.Count;
    }

    #region Process Value

    public override void addValue(object value, bool needOrder = false)
    {
        if (value is T)
        {
            T t = (T)value;
            this.add(t);
        }
        else
        {
            Logger.LogError("Why wrong value type: " + value);
        }
    }

    public override void removeValue(object value)
    {
        if (value is T)
        {
            T t = (T)value;
            List<Change<T>> changes = new List<Change<T>>();
            {
                ChangeDictRemove<T> changeDictRemove = new ChangeDictRemove<T>();
                {
                    changeDictRemove.values.Add(t);
                }
                changes.Add(changeDictRemove);
            }
            this.processChange(changes);
        }
        else
        {
            Logger.LogError("why wrong value type: " + value);
        }
    }

    #region RemoveAt

    public override void removeAt(int index)
    {
        Logger.Log("DO not remove at index");
    }


    #endregion

    #endregion

    public override Type getType()
    {
        return WrapProperty.Type.Dict;
    }

    #endregion

    #region value

    // [SerializeField]
    // private readonly Dictionary<K, T> Dict = new Dictionary<K, T>();
    public readonly Dictionary<K, T> dict = new Dictionary<K, T>();
    /*{
        get
        {
            return Dict;
        }

        set
        {
            if (Dict != value)
            {
                Dict = value;
            }
        }
    }*/

    #endregion

    #region Operation

    public override void clear()
    {
        if (dict.Count > 0)
        {
            List<Change<T>> changes = new List<Change<T>>();
            {
                ChangeDictRemove<T> changeDictRemove = new ChangeDictRemove<T>();
                {
                    changeDictRemove.values.AddRange(dict.Values);
                }
                changes.Add(changeDictRemove);
            }
            this.processChange(changes);
        }
        else
        {
            // Debug.LogError ("why don't have values: " + this);
        }
    }

    public void add(T property)
    {
        List<Change<T>> changes = new List<Change<T>>();
        {
            ChangeDictAdd<T> changeDictAdd = new ChangeDictAdd<T>();
            {
                changeDictAdd.values.Add(property);
            }
            changes.Add(changeDictAdd);
        }
        this.processChange(changes);
    }

    public void add(List<T> properties)
    {
        List<Change<T>> changes = new List<Change<T>>();
        {
            ChangeDictAdd<T> changeAdd = new ChangeDictAdd<T>();
            {
                changeAdd.values.AddRange(properties);
            }
            changes.Add(changeAdd);
        }
        this.processChange(changes);
    }

    public override void insertValue(object value, int index)
    {
        Logger.LogError("DO: not need to insert");
    }

    #endregion

    #region process change

    public override void copyWrapProperty(WrapProperty otherWrapProperty)
    {
        if (otherWrapProperty is DO<T, K>)
        {
            DO<T, K> otherDO = (DO<T, K>)otherWrapProperty;
            // find listChange
            List<Change<T>> changes = new List<Change<T>>();
            {
                // remove not in anymore
                {
                    ChangeDictRemove<T> changeDictRemove = new ChangeDictRemove<T>();
                    {
                        foreach (K key in dict.Keys)
                        {
                            if (!otherDO.dict.ContainsKey(key))
                            {
                                changeDictRemove.values.Add(dict[key]);
                            }
                        }
                    }
                    if (changeDictRemove.values.Count > 0)
                    {
                        changes.Add(changeDictRemove);
                    }
                }
                // add current not in
                {
                    ChangeDictAdd<T> changeDictAdd = new ChangeDictAdd<T>();
                    ChangeDictSet<T> changeDictSet = new ChangeDictSet<T>();
                    {
                        foreach (K key in otherDO.dict.Keys)
                        {
                            if (!dict.ContainsKey(key))
                            {
                                changeDictAdd.values.Add(otherDO.dict[key]);
                            }
                            else
                            {
                                T add = otherDO.dict[key];
                                T remove = dict[key];
                                if(!object.Equals(remove, add))
                                {
                                    changeDictSet.addValues.Add(add);
                                    changeDictSet.removeValues.Add(remove);
                                }
                            }
                        }
                    }
                    // process
                    {
                        if (changeDictAdd.values.Count > 0)
                        {
                            changes.Add(changeDictAdd);
                        }
                        if (changeDictSet.addValues.Count > 0)
                        {
                            changes.Add(changeDictSet);
                        }
                    }
                }
            }
            // Change
            if (changes.Count > 0)
            {
                this.processChange(changes);
            }
        }
        else
        {
            Logger.LogError("why not the same type wrapProperty: " + this + "; " + otherWrapProperty);
        }
    }

    public void processChange(List<Change<T>> changes)
    {
        if (changes.Count > 0)
        {
            // Make list change
            List<Sync<T>> syncs = new List<Sync<T>>();
            {
                for (int syncCount = 0; syncCount < changes.Count; syncCount++)
                {
                    Change<T> change = changes[syncCount];
                    switch (change.getType())
                    {
                        case Change<T>.Type.DictAdd:
                            {
                                ChangeDictAdd<T> changeDictAdd = change as ChangeDictAdd<T>;
                                // change
                                {
                                    foreach(T t in changeDictAdd.values)
                                    {
                                        dict[t.getKey()] = t;
                                    }
                                }
                                // Make Sync
                                {
                                    SyncDictAdd<T> syncDictAdd = new SyncDictAdd<T>();
                                    {
                                        syncDictAdd.values.AddRange(changeDictAdd.values);
                                    }
                                    syncs.Add(syncDictAdd);
                                }
                            }
                            break;
                        case Change<T>.Type.DictRemove:
                            {
                                ChangeDictRemove<T> changeDictRemove = change as ChangeDictRemove<T>;
                                // sync
                                {
                                    SyncDictRemove<T> syncDictRemove = new SyncDictRemove<T>();
                                    {
                                        foreach(T t in changeDictRemove.values)
                                        {
                                            dict.Remove(t.getKey());
                                            syncDictRemove.values.Add(t);
                                        }
                                    }
                                    if (syncDictRemove.values.Count > 0)
                                        syncs.Add(syncDictRemove);
                                }
                            }
                            break;
                        case Change<T>.Type.DictSet:
                            {
                                ChangeDictSet<T> changeDictSet = change as ChangeDictSet<T>;
                                // change
                                {
                                    for (int i = 0; i < changeDictSet.addValues.Count; i++)
                                    {
                                        dict[changeDictSet.addValues[i].getKey()] = changeDictSet.addValues[i];
                                    }
                                }
                                // sync
                                {
                                    SyncDictSet<T> syncDictSet = new SyncDictSet<T>();
                                    {
                                        syncDictSet.olds.AddRange(changeDictSet.removeValues);
                                        syncDictSet.news.AddRange(changeDictSet.addValues);
                                    }
                                    syncs.Add(syncDictSet);
                                }
                            }
                            break;
                        default:
                            Logger.LogError("unknown change type: " + change.getType() + "; " + this);
                            break;
                    }
                }
            }
            // CallBack
            if (syncs.Count > 0)
            {
                foreach (ValueChangeCallBack callBack in p.callBacks.ToArray())
                {
                    callBack.onUpdateSync(this, syncs);
                }
            }
            else
            {
                // Debug.LogError("why don't have syncCount: " + this);
            }
        }
        else
        {
            Logger.LogError("why don't have changes: " + this);
        }
    }

    #endregion

    private void undo(List<Sync<T>> syncs)
    {
        List<Change<T>> changes = new List<Change<T>>();
        {
            for (int syncCount = syncs.Count - 1; syncCount >= 0; syncCount--)
            {
                Sync<T> sync = syncs[syncCount];
                switch (sync.getType())
                {
                    case Sync<T>.Type.DictSet:
                        {
                            SyncDictSet<T> syncDictSet = (SyncDictSet<T>)sync;
                            // Make change
                            ChangeDictSet<T> changeDictSet = new ChangeDictSet<T>();
                            {
                                changeDictSet.removeValues.AddRange(syncDictSet.news);
                                changeDictSet.addValues.AddRange(syncDictSet.olds);
                            }
                            changes.Add(changeDictSet);
                        }
                        break;
                    case Sync<T>.Type.DictAdd:
                        {
                            SyncDictAdd<T> syncDictAdd = (SyncDictAdd<T>)sync;
                            // Make change
                            ChangeDictRemove<T> changeDictRemove = new ChangeDictRemove<T>();
                            {
                                changeDictRemove.values.AddRange(syncDictAdd.values);
                            }
                            changes.Add(changeDictRemove);
                        }
                        break;
                    case Sync<T>.Type.DictRemove:
                        {
                            SyncDictRemove<T> syncDictRemove = (SyncDictRemove<T>)sync;
                            // Make change
                            ChangeDictAdd<T> changeDictAdd = new ChangeDictAdd<T>();
                            {
                                changeDictAdd.values.AddRange(syncDictRemove.values);
                            }
                            changes.Add(changeDictAdd);
                        }
                        break;
                    default:
                        Logger.LogError("unknown type: " + sync.getType() + "; " + this);
                        break;
                }
            }
        }
        if (changes.Count > 0)
        {
            this.processChange(changes);
        }
    }

    private void redo(List<Sync<T>> syncs)
    {
        List<Change<T>> changes = new List<Change<T>>();
        {
            for (int syncCount = 0; syncCount < syncs.Count; syncCount++)
            {
                Sync<T> sync = syncs[syncCount];
                switch (sync.getType())
                {
                    case Sync<T>.Type.DictSet:
                        {
                            SyncDictSet<T> syncDictSet = (SyncDictSet<T>)sync;
                            // Make change
                            ChangeDictSet<T> changeDictSet = new ChangeDictSet<T>();
                            {
                                changeDictSet.removeValues.AddRange(syncDictSet.olds);
                                changeDictSet.addValues.AddRange(syncDictSet.news);
                            }
                            changes.Add(changeDictSet);
                        }
                        break;
                    case Sync<T>.Type.DictAdd:
                        {
                            SyncDictAdd<T> syncDictAdd = (SyncDictAdd<T>)sync;
                            // Make change
                            ChangeDictAdd<T> changeDictAdd = new ChangeDictAdd<T>();
                            {
                                changeDictAdd.values.AddRange(syncDictAdd.values);
                            }
                            changes.Add(changeDictAdd);
                        }
                        break;
                    case Sync<T>.Type.DictRemove:
                        {
                            SyncDictRemove<T> syncDictRemove = (SyncDictRemove<T>)sync;
                            // Make change
                            ChangeDictRemove<T> changeDictRemove = new ChangeDictRemove<T>();
                            {
                                changeDictRemove.values.AddRange(syncDictRemove.values);
                            }
                            changes.Add(changeDictRemove);
                        }
                        break;
                    default:
                        Logger.LogError("unknown type: " + sync.getType() + "; " + this);
                        break;
                }
            }
        }
        if (changes.Count > 0)
        {
            this.processChange(changes);
        }
    }

    #region History Undo/Redo

    public override void processUndo(WrapChange wrapChange)
    {
        List<Sync<T>> syncs = wrapChange.getSyncs<T>();
        if (syncs != null)
        {
            this.undo(syncs);
        }
        else
        {
            Logger.LogError("processUndo sync null: " + this);
        }
    }

    public override void processRedo(WrapChange wrapChange)
    {
        List<Sync<T>> syncs = wrapChange.getSyncs<T>();
        if (syncs != null)
        {
            this.redo(syncs);
        }
        else
        {
            Logger.LogError("sync null: " + this);
        }
    }

    #endregion

    public override void allAddCallBack(ValueChangeCallBack callBack)
    {
        if (IsAddCallBackInterface)
        {
            foreach (T t in this.dict.Values)
            {
                if (t != null)
                {
                    ((AddCallBackInterface)t).addCallBack(callBack);
                }
                else
                {
                    Logger.LogError("data null: " + callBack + "; " + this + "; " + typeof(T));
                }
            }
        }
        else
        {
            Logger.LogError("why not data: " + callBack + "; " + this + "; " + typeof(T));
        }
    }

    public override void allRemoveCallBack(ValueChangeCallBack callBack)
    {
        if (IsAddCallBackInterface)
        {
            foreach (T t in this.dict.Values)
            {
                if (t != null)
                {
                    ((AddCallBackInterface)t).removeCallBack(callBack);
                }
                else
                {
                    Logger.LogError("data null: " + callBack + "; " + this + "; " + typeof(T));
                }
            }
        }
        else
        {
            Logger.LogError("why not data: " + callBack + "; " + this + "; " + typeof(T));
        }
    }

    #region makeBinary

    public override void makeBinary(BinaryWriter writer)
    {
        // writer.Write(this.i);
        // vs
        {
            writer.Write(this.dict.Values.Count);
            foreach (T value in this.dict.Values)
            {
                DataUtils.writeBinary(writer, value);
            }
        }
    }

    public override void parse(BinaryReader reader)
    {
        // this.i = reader.ReadUInt32();
        // vs
        {
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                this.add(DataUtils.readBinary<T>(reader));
            }
        }
    }

    #endregion

    #region makeSqliteBinary

    public override void makeSqliteBinary(BinaryWriter writer)
    {
        // writer.Write(this.i);
        // vs
        {
            writer.Write(this.dict.Values.Count);
            foreach (T value in this.dict.Values)
            {
                DataUtils.writeBinary(writer, value);
            }
        }
    }

    public override void parseSqlite(BinaryReader reader)
    {
        // this.i = reader.ReadUInt32();
        // vs
        {
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                this.add(DataUtils.readBinary<T>(reader));
            }
        }
    }

    #endregion

}