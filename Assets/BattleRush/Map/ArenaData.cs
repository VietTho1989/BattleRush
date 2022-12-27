using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS
{
    public class ArenaData : Data
    {

        public VO<int> segmentIndex;

        #region Constructor

        public enum Property
        {
            segmentIndex
        }

        public ArenaData() : base()
        {
            this.segmentIndex = new VO<int>(this, (byte)Property.segmentIndex, 11);
        }

        #endregion

    }
}
