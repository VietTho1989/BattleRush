using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ArenaS
{
    public class Troop : Data
    {

        #region stat

        #region race

        public enum Race
        {
            antuk,
            krakee,
            mantah,
            montak,
            muu
        }

        public VO<Race> race;

        #endregion

        public VO<int> level;

        public VO<int> hp;

        public VO<int> attack;

        public VO<float> attackRange;

        #endregion

        public VD<TakeDamage> takeDamage;

        #region Constructor

        public enum Property
        {
            race,
            level,
            hp,
            attack,
            attackRange,

            takeDamage,
        }

        public Troop() : base()
        {
            this.race = new VO<Race>(this, (byte)Property.race, Race.antuk);
            this.level = new VO<int>(this, (byte)Property.level, 0);
            this.hp = new VO<int>(this, (byte)Property.hp, 0);
            this.attack = new VO<int>(this, (byte)Property.attack, 1);
            this.attackRange = new VO<float>(this, (byte)Property.attackRange, 1);

            this.takeDamage = new VD<TakeDamage>(this, (byte)Property.takeDamage, new TakeDamage());
        }

        #endregion

    }
}
