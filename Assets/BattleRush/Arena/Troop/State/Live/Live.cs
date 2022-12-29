using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ArenaS.TroopS
{
    public class Live : Troop.State
    {

        public VO<float> hitpoint;

        public VD<TakeDamage> takeDamage;

        #region troop action

        /**
         * ai: what troop will do
         * */
        public VD<TroopIntention> intention;

        public VD<TroopAttack> troopAttack;

        #endregion

        #region Constructor

        public enum Property
        {
            hitpoint,
            takeDamage,
            intention,
            troopAttack
        }

        public Live() : base()
        {
            this.hitpoint = new VO<float>(this, (byte)Property.hitpoint, 1.0f);
            this.takeDamage = new VD<TakeDamage>(this, (byte)Property.takeDamage, new TakeDamage());
            this.intention = new VD<TroopIntention>(this, (byte)Property.intention, new TroopIntention());
            this.troopAttack = new VD<TroopAttack>(this, (byte)Property.troopAttack, new TroopAttack());
        }

        #endregion

        public override Type getType()
        {
            return Type.Live;
        }

    }
}