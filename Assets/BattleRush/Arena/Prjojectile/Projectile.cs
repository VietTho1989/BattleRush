using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ArenaS
{
    /**
     * the attack to enemy
     * */ 
    public class Projectile : Data
    {

        public VO<float> speed;

        public VO<Damage> damage;

        public VO<int> targetId;

        #region Constructor

        public enum Property
        {
            speed,
            damage,
            targetId
        }

        public Projectile() : base()
        {
            this.speed = new VO<float>(this, (byte)Property.speed, 0);
            this.damage = new VO<Damage>(this, (byte)Property.damage, new Damage());
            this.targetId = new VO<int>(this, (byte)Property.targetId, 0);
        }

        #endregion

    }
}