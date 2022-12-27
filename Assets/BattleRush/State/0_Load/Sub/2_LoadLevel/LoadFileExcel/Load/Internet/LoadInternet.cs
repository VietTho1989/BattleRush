using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BattleRushS.StateS.LoadS
{
    public class LoadInternet : LoadLevelByFileExcel.Step
    {

        public const int TimeOut = 90;// timeout in seconds

        #region Constructor

        public enum Property
        {

        }

        public LoadInternet() : base()
        {

        }

        #endregion

        public override Type getType()
        {
            return Type.Internet;
        }

    }
}