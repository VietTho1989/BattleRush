using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ArenaS
{
    public class Damage : Data
    {

        public VO<float> damage;

        #region Constructor

        public enum Property
        {
            damage
        }

        public Damage() : base()
        {
            this.damage = new VO<float>(this, (byte)Property.damage, 0.1f);
        }

        #endregion

    }
}