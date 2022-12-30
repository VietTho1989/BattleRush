using System.Collections;
using System.Collections.Generic;
using BattleRushS.ArenaS.TroopS;
using BattleRushS.ArenaS.TroopS.IntentionS;
using UnityEngine;

namespace BattleRushS.ArenaS
{
    public class TroopIntention : Data
    {

        #region intention

        public abstract class Intention : Data
        {

            public enum Type
            {
                Rest,
                Attack
            }

            public abstract Type getType();

        }

        public VD<Intention> intention;

        #endregion

        #region Constructor

        public enum Property
        {
            intention
        }

        public TroopIntention() : base()
        {
            this.intention = new VD<Intention>(this, (byte)Property.intention, new Rest());
        }

        #endregion

    }
}