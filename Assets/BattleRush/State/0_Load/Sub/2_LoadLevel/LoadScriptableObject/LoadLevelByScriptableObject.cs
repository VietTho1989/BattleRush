using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.StateS.LoadS
{
    public class LoadLevelByScriptableObject : LoadLevel.Sub
    {

        #region Constructor

        public enum Property
        {

        }

        public LoadLevelByScriptableObject() : base()
        {

        }

        #endregion

        public override Type getType()
        {
            return Type.ScriptableObject;
        }

    }
}