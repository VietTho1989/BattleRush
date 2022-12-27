using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BattleRushS.StateS.LoadS
{
    public class LoadLevelByFileExcel : Data
    {

        public const string FileLevel = "GameplayMapLevel.csv";// "MapLevel.csv";

        #region step

        public abstract class Step : Data
        {

            public enum Type
            {
                Internet,
                Local
            }

            public abstract Type getType();

        }

        public VD<Step> step;

        #endregion

        #region Constructor

        public enum Property
        {
            step
        }

        public LoadLevelByFileExcel() : base()
        {
            this.step = new VD<Step>(this, (byte)Property.step, null);
        }

        #endregion

    }
}