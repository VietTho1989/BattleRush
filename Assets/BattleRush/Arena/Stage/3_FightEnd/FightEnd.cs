using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ArenaS
{
    public class FightEnd : Arena.Stage
    {

        public VO<int> teamWin;

        #region Constructor

        public enum Property
        {
            teamWin
        }

        public FightEnd() : base()
        {
            this.teamWin = new VO<int>(this, (byte)Property.teamWin, 0);
        }

        #endregion

        public override Type getType()
        {
            return Type.FightEnd;
        }

    }
}