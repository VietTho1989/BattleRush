using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ArenaS
{
    public class TakeDamage : Data
    {

        public LD<Damage> damages;

        #region Constructor

        public enum Property
        {
            damages
        }

        public TakeDamage() : base()
        {
            this.damages = new LD<Damage>(this, (byte)Property.damages);
        }

        #endregion

    }
}