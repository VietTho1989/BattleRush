using System.Collections;
using System.Collections.Generic;
using BattleRushS.ArenaS.ProjectileS;
using UnityEngine;

namespace BattleRushS.ArenaS
{
    /**
     * the attack to enemy
     * */ 
    public class Projectile : Data
    {

        public VO<Damage> damage;

        public VO<uint> originId;

        public VO<uint> targetId;

        #region state

        public abstract class State : Data
        {

            public enum Type
            {
                Move,
                DealDamage
            }

            public abstract Type getType();

        }

        public VD<State> state;

        #endregion

        #region Constructor

        public enum Property
        {
            damage,
            originId,
            targetId,
            state
        }

        public Projectile() : base()
        {
            this.damage = new VO<Damage>(this, (byte)Property.damage, new Damage());
            this.originId = new VO<uint>(this, (byte)Property.originId, 0);
            this.targetId = new VO<uint>(this, (byte)Property.targetId, 0);
            this.state = new VD<State>(this, (byte)Property.state, new Move());
        }

        #endregion

    }
}