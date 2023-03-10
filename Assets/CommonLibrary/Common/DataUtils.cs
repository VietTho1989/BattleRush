using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class DataUtils
{

    public enum ServerState
    {
        Offline,
        Connect,
        Disconnect
    }

    #region compare

    public static bool IsDifferent(Data A, Data B)
    {
        if (A == B)
        {
            Logger.LogError("the same data");
            return false;
        }
        if (A != null && B != null)
        {
            if (A.GetType() == B.GetType())
            {
                bool ret = false;
                // compare each property
                foreach (WrapProperty AProperty in A.pts)
                {
                    WrapProperty BProperty = B.findProperty(AProperty.n);
                    if (BProperty != null)
                    {
                        if (WrapProperty.isDifferent(AProperty, BProperty))
                        {
                            ret = true;
                            break;
                        }
                    }
                    else
                    {
                        Logger.LogError("BProperty null: " + A + "; " + B);
                        ret = true;
                        break;
                    }
                }
                return ret;
            }
            else
            {
                return true;
            }
        }
        else
        {
            Logger.LogError("A, B null");
            return false;
        }
    }

    #endregion

    #region Copy

    public static void copyDataExcept(Data copyData, Data originData, List<byte> exceptNames)
    {
        if (copyData == null || originData == null || copyData.GetType() != originData.GetType())
        {
            Logger.LogError("cannot copy data null or wrong type");
            return;
        }
        // Field
        {
            FieldInfo[] fieldInfos = copyData.GetType().GetFields();
            for (int index = 0; index < fieldInfos.Length; index++)
            {
                FieldInfo fieldInfo = fieldInfos[index];
                if (fieldInfo.FieldType.IsSubclassOf(typeof(WrapProperty)))
                {
                    // Debug.Log ("this is wrapProperty: " + fieldInfo.FieldType);
                    object copyFieldValue = fieldInfo.GetValue(copyData);
                    object originFieldValue = fieldInfo.GetValue(originData);
                    if (copyFieldValue is WrapProperty && originFieldValue is WrapProperty)
                    {
                        WrapProperty copyWrapProperty = (WrapProperty)(object)copyFieldValue;
                        if (!exceptNames.Contains(copyWrapProperty.n))
                        {
                            WrapProperty originWrapProperty = (WrapProperty)(object)originFieldValue;
                            // copy index
                            copyWrapProperty.setIndex(originWrapProperty.getIndex());
                            // Copy content
                            copyWrapProperty.copyWrapProperty(originWrapProperty);
                        }
                        else
                        {
                            // Debug.LogError ("not allow copy wrapProperty: " + copyWrapProperty);
                        }
                    }
                    else
                    {
                        Logger.LogError("Why not wrap property");
                    }
                }
            }
        }
    }

    /**
	 * cai copy nay chua copy cai makeId: chua biet nen lam the nao
	 * */
    public static void copyData(Data copyData, Data originData, List<byte> allowNames = null)
    {
        if (copyData == null || originData == null || copyData.GetType() != originData.GetType())
        {
            Logger.LogError("cannot copy data null or wrong type");
            return;
        }
        if (copyData == originData)
        {
            Logger.LogError("the same data");
            return;
        }
        // Field
        {
            FieldInfo[] fieldInfos = copyData.GetType().GetFields();
            for (int index = 0; index < fieldInfos.Length; index++)
            {
                FieldInfo fieldInfo = fieldInfos[index];
                if (fieldInfo.FieldType.IsSubclassOf(typeof(WrapProperty)))
                {
                    // Debug.Log ("this is wrapProperty: " + fieldInfo.FieldType);
                    object copyFieldValue = fieldInfo.GetValue(copyData);
                    object originFieldValue = fieldInfo.GetValue(originData);
                    if (copyFieldValue is WrapProperty && originFieldValue is WrapProperty)
                    {
                        WrapProperty copyWrapProperty = (WrapProperty)(object)copyFieldValue;
                        if (allowNames == null || allowNames.Contains(copyWrapProperty.n))
                        {
                            WrapProperty originWrapProperty = (WrapProperty)(object)originFieldValue;
                            // Copy index
                            copyWrapProperty.setIndex(originWrapProperty.getIndex());
                            // Copy content
                            copyWrapProperty.copyWrapProperty(originWrapProperty);
                        }
                        else
                        {
                            // Debug.LogError ("not allow copy wrapProperty: " + copyWrapProperty);
                        }
                    }
                    else
                    {
                        Logger.LogError("Why not wrap property");
                    }
                }
            }
        }
    }

    public static Data cloneData(Data originData, List<byte> allowNames = null)
    {
        if (originData == null)
        {
            Logger.LogError("originData null");
            return null;
        }
        System.Type type = originData.GetType();
        Data cloneData = (Data)Activator.CreateInstance(type);
        // Id?
        {
            cloneData.uid = originData.uid;
        }
        DataUtils.copyData(cloneData, originData, allowNames);
        // return
        return cloneData;
    }

    #endregion

    #region Parent Add CallBack

    /** Add neu nhieu cai cung 1 bien thi co the co van de*/
    public static T addParentCallBack<T>(Data data, ValueChangeCallBack callBack, ref T thisParent) where T : Data
    {
        T ret = null;
        if (data != null)
        {
            T parent = data.findDataInParent<T>();
            if (parent != null)
            {
                // Set
                {
                    if (thisParent != parent)
                    {
                        // remove old
                        if (thisParent != null)
                        {
                            // Debug.LogError("Why have old parent: " + thisParent + "; " + data + "; " + callBack);
                            thisParent.removeCallBack(callBack);
                        }
                        // set new
                        thisParent = parent;
                    }
                    else
                    {
                        // Debug.LogError("the same: " + callBack);
                    }
                }
                // Add
                parent.addCallBack(callBack);
                // return
                ret = parent;
            }
            else
            {
                // Debug.LogError ("parent null: " + data + "; " + callBack);
            }
        }
        else
        {
            // Debug.LogError ("why data null: " + callBack + "; " + typeof(T));
        }
        return ret;
    }

    public static void removeParentCallBack<T>(Data data, ValueChangeCallBack callBack, ref T thisParent) where T : Data
    {
        if (thisParent != null)
        {
            thisParent.removeCallBack(callBack);
            thisParent = null;
        }
        else
        {
            // Debug.LogError ("parent null: " + data + "; " + callBack);
        }
    }

    #endregion

    #region writeBinary

    public static void writeBinary<T>(BinaryWriter writer, T value)
    {
        // byte
        if (typeof(T) == typeof(byte))
        {
            writer.Write((byte)(object)value);
            return;
        }
        // sbyte
        if (typeof(T) == typeof(sbyte))
        {
            writer.Write((sbyte)(object)value);
            return;
        }
        // short
        if (typeof(T) == typeof(short))
        {
            writer.Write((short)(object)value);
            return;
        }
        // ushort
        if (typeof(T) == typeof(ushort))
        {
            writer.Write((ushort)(object)value);
            return;
        }
        // bool
        if (typeof(T) == typeof(bool))
        {
            writer.Write((bool)(object)value);
            return;
        }
        // float
        if (typeof(T) == typeof(float))
        {
            writer.Write((float)(object)value);
            return;
        }
        // int
        if (typeof(T) == typeof(int))
        {
            writer.Write((int)(object)value);
            return;
        }
        // string
        if (typeof(T) == typeof(string))
        {
            writer.Write((string)(object)value);
            return;
        }
        // uint
        if (typeof(T) == typeof(uint))
        {
            writer.Write((uint)(object)value);
            return;
        }
        // ulong
        if (typeof(T) == typeof(ulong))
        {
            writer.Write((ulong)(object)value);
            return;
        }
        // long
        if (typeof(T) == typeof(long))
        {
            writer.Write((long)(object)value);
            return;
        }
        // enum
        if (typeof(T).IsEnum)
        {
            writer.Write((int)(object)value);
            return;
        }
        // vector2
        if (typeof(T) == typeof(Vector2))
        {
            Logger.LogError("write vector2");
            Vector2 vector2 = (Vector2)(object)value;
            writer.Write(vector2.x);
            writer.Write(vector2.y);
            return;
        }
        // Data
        if (value is Data)
        {
            Data data = value as Data;
            data.makeBinary(writer);
            return;
        }
        // byte array
        if (value is byte[])
        {
            byte[] byteArray = (byte[])(object)value;
            writer.Write(byteArray.Length);
            writer.Write(byteArray);
            return;
        }
        // HistoryChange
        {
            if (value is List<Data.SI>)
            {
                List<Data.SI> pi = value as List<Data.SI>;
                // write
                {
                    writer.Write(pi.Count);
                    foreach (Data.SI si in pi)
                    {
                        writer.Write(si.i);
                        writer.Write(si.n);
                    }
                }
                return;
            }
        }
        // DateTime
        {
            if(value is DateTime)
            {
                DateTime dateTime = (DateTime)(object)value;
                writer.Write(dateTime.Ticks);
                return;
            }
        }
        // unknown
        if (value != null)
        {
            Logger.LogError("unknown type: " + value + "; " + value.GetType());
        }
        else
        {
            Logger.LogError("why value null: " + typeof(T));
            if (typeof(T).IsSubclassOf(typeof(Data)))
            {
                writer.Write("null");
            }
            else
            {
                Logger.LogError("why value null and not data");
            }
        }
    }

    public static T readBinary<T>(BinaryReader reader)
    {
        // byte
        if (typeof(T) == typeof(byte))
        {
            return (T)(object)reader.ReadByte();
        }
        // sbyte
        if (typeof(T) == typeof(sbyte))
        {
            return (T)(object)reader.ReadSByte();
        }
        // short
        if (typeof(T) == typeof(short))
        {
            return (T)(object)reader.ReadInt16();
        }
        // ushort
        if (typeof(T) == typeof(ushort))
        {
            return (T)(object)reader.ReadUInt16();
        }
        // bool
        if (typeof(T) == typeof(bool))
        {
            return (T)(object)reader.ReadBoolean();
        }
        // float
        if (typeof(T) == typeof(float))
        {
            return (T)(object)reader.ReadSingle();
        }
        // int
        if (typeof(T) == typeof(int))
        {
            return (T)(object)reader.ReadInt32();
        }
        // string
        if (typeof(T) == typeof(string))
        {
            return (T)(object)reader.ReadString();
        }
        // uint
        if (typeof(T) == typeof(uint))
        {
            return (T)(object)reader.ReadUInt32();
        }
        // ulong
        if (typeof(T) == typeof(ulong))
        {
            return (T)(object)reader.ReadUInt64();
        }
        // long
        if (typeof(T) == typeof(long))
        {
            return (T)(object)reader.ReadInt64();
        }
        // enum
        if (typeof(T).IsEnum)
        {
            return (T)(object)reader.ReadInt32();
        }
        // vector2
        if (typeof(T) == typeof(Vector2))
        {
            Logger.LogError("read vector2");
            Vector2 vector2 = new Vector2();
            {
                vector2.x = reader.ReadSingle();
                vector2.y = reader.ReadSingle();
            }
            return (T)(object)vector2;
        }
        // Data
        if (typeof(T).IsSubclassOf(typeof(Data)))
        {
            return (T)(object)Data.parseBinary(reader);
        }
        // byte array
        if (typeof(T) == typeof(byte[]))
        {
            int count = reader.ReadInt32();
            return (T)(object)reader.ReadBytes(count);
        }
        // History
        {
            // ParentInfo
            if (typeof(T) == typeof(List<Data.SI>))
            {
                int count = reader.ReadInt32();
                List<Data.SI> pi = new List<Data.SI>();
                for (int i = 0; i < count; i++)
                {
                    Data.SI si = new Data.SI();
                    {
                        si.i = reader.ReadUInt32();
                        si.n = reader.ReadByte();
                    }
                    pi.Add(si);
                }
                return (T)(object)pi;
            }
        }
        // DateTime
        {
            if (typeof(T) == typeof(DateTime))
            {
                long tick = reader.ReadInt64();
                return (T)(object)(new DateTime(tick));
            }
        }
        Logger.LogError("unknown type: " + typeof(T));
        return default(T);
    }

    #endregion

}