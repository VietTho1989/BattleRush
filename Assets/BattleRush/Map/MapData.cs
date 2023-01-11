using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

namespace BattleRushS
{
    public class MapData : Data
    {

        #region objectDatas

        public LD<ObjectData> objectDatas;

        #endregion

        #region Constructor

        public enum Property
        {
            objectDatas,
        }

        public MapData() : base()
        {
            this.objectDatas = new LD<ObjectData>(this, (byte)Property.objectDatas);
        }

        public void reset()
        {
            this.objectDatas.clear();
        }

        #endregion

        public static int CompareByDistance(ObjectData A, ObjectData B)
        {
            return A.position.v.z.CompareTo(B.position.v.z);
        }

        public void parse(string dataStr)
        {
            try
            {
                JsonData jsObjectDatas = JsonMapper.ToObject(dataStr);
                // make list
                List<ObjectData> objectDatas = new List<ObjectData>();
                {
                    for (int i = 0; i < jsObjectDatas.Count; i++)
                    {
                        JsonData jsObjectData = jsObjectDatas[i];
                        ObjectData objectData = new ObjectData();
                        {
                            objectData.uid = this.objectDatas.makeId();
                            objectData.parseJson(jsObjectData);
                        }
                        objectDatas.Add(objectData);
                    }
                }                
                // add
                this.objectDatas.add(objectDatas);
                // sort
                {
                    this.objectDatas.vs.Sort(CompareByDistance);
                }

            }
            catch (System.Exception e)
            {
                Logger.LogWarning("error premiums: " + e);
            }
        }

    }
}