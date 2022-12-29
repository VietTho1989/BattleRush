using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattleRushS.ArenaS.TroopS.IntentionS
{
    public class Rest : TroopIntention.Intention
    {

        #region Constructor

        public enum Property
        {

        }

        public Rest()
        {

        }

        #endregion

        public override Type getType()
        {
            return Type.Rest;
        }

    }
}