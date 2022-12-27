using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.StateS.LoadS
{
    public class LoadLevel : Load.Sub
    {

        public VO<int> level;

        public VD<LoadLevelByFileExcel> loadLevelByFileExcel;

        #region Constructor

        public enum Property
        {
            level,
            loadLevelByFileExcel
        }

        public LoadLevel() : base()
        {
            this.level = new VO<int>(this, (byte)Property.level, 0);
            this.loadLevelByFileExcel = new VD<LoadLevelByFileExcel>(this, (byte)Property.loadLevelByFileExcel, new LoadLevelByFileExcel());
        }

        #endregion

        public override Type getType()
        {
            return Type.LoadLevel;
        }

    }
}
