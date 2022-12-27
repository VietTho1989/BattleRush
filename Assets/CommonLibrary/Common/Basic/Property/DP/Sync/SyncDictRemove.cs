using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SyncDictRemove<T> : Sync<T>
{

    public List<T> values = new List<T>();

    public override Type getType()
    {
        return Type.DictRemove;
    }

    #region makeBinary

    public override void makeBinary(BinaryWriter writer)
    {
        // values
        {
            writer.Write(values.Count);
            foreach (T value in values)
            {
                DataUtils.writeBinary(writer, value);
            }
        }
    }

    public override void parse(BinaryReader reader)
    {
        {
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                this.values.Add(DataUtils.readBinary<T>(reader));
            }
        }
    }

    #endregion

}