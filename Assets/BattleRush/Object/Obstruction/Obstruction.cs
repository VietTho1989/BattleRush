using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ObjectS
{
    public class Obstruction : Data
    {

        #region damage type

        public enum DamageType
        {
            OneHitDie,
            HalfHitPoint
        }

        public VO<DamageType> damageType;

        #endregion

        #region taking damage

        public LO<uint> takingDamageHeroIds;

        public LO<uint> takingDamageTroopIds;

        #endregion

        #region id already take damage

        public LO<uint> alreadyTakeDamageHeroIds;

        /**
         * TODO troop id the nay chua tot, vi troop thuoc ve hero, nen sua de troop id unique
         * */
        public LO<uint> alreadyTakeDamageTroopIds;

        #endregion

        #region Constructor

        public enum Property
        {
            damageType,

            takingDamageHeroIds,
            takingDamageTroopIds,

            alreadyTakeDamageHeroIds,
            alreadyTakeDamageTroopIds
        }

        public Obstruction() : base()
        {
            this.damageType = new VO<DamageType>(this, (byte)Property.damageType, DamageType.OneHitDie);
            // taking damange
            {
                this.takingDamageHeroIds = new LO<uint>(this, (byte)Property.takingDamageHeroIds);
                this.takingDamageTroopIds = new LO<uint>(this, (byte)Property.takingDamageTroopIds);
            }
            // already take damage
            {
                this.alreadyTakeDamageHeroIds = new LO<uint>(this, (byte)Property.alreadyTakeDamageHeroIds);
                this.alreadyTakeDamageTroopIds = new LO<uint>(this, (byte)Property.alreadyTakeDamageTroopIds);
            }           
        }

        #endregion

    }
}