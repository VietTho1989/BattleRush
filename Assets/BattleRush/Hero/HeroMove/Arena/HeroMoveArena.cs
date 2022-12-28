using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.HeroS
{
    public class HeroMoveArena : HeroMove.Sub
    {

        #region Constructor

        public enum Property
        {

        }

        public HeroMoveArena() : base()
        {

        }

        #endregion

        public override Type getType()
        {
            return Type.Arena;
        }

    }
}
