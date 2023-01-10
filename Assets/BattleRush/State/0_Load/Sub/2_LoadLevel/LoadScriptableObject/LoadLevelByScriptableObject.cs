using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.StateS.LoadS
{
    public class LoadLevelByScriptableObject : LoadLevel.Sub
    {

        public VO<ReferenceData<MapData>> mapData;

        #region Constructor

        public enum Property
        {
            mapData
        }

        public LoadLevelByScriptableObject() : base()
        {
            this.mapData = new VO<ReferenceData<MapData>>(this, (byte)Property.mapData, new ReferenceData<MapData>(null));
        }

        #endregion

        public override Type getType()
        {
            return Type.ScriptableObject;
        }

    }
}