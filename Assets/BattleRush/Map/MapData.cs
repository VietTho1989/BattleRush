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

        public VO<int> lastAddIndex;

        #endregion

        #region Constructor

        public enum Property
        {
            objectDatas,
            lastAddIndex,

            arenaDatas
        }

        public MapData() : base()
        {
            // objectDatas
            {
                this.objectDatas = new LD<ObjectData>(this, (byte)Property.objectDatas);
                this.lastAddIndex = new VO<int>(this, (byte)Property.lastAddIndex, -1);
            }
        }

        public void reset()
        {
            this.objectDatas.clear();
            this.lastAddIndex.v = -1;
        }

        #endregion

        public static int CompareByDistance(ObjectData A, ObjectData B)
        {
            return A.P.v.z.CompareTo(B.P.v.z);
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