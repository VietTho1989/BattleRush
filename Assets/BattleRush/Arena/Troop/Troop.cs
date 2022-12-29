using System.Collections;
using System.Collections.Generic;
using BattleRushS.ArenaS.TroopS;
using UnityEngine;

namespace BattleRushS.ArenaS
{
    public class Troop : Data
    {

        public VO<int> teamId;

        #region stat

        public VO<HeroS.TroopFollow.TroopType> troopType;

        public VO<int> level;

        public VO<float> hitpoint;

        public VO<int> attack;

        public VO<float> attackRange;

        #endregion

        #region world data

        public VO<Vector3> worldPosition;

        #endregion

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
            teamId,
            troopType,
            level,
            hitpoint,
            attack,
            attackRange,
            worldPosition,
            takeDamage,
            intention,
            troopAttack
        }

        public Troop() : base()
        {
            this.teamId = new VO<int>(this, (byte)Property.teamId, 0);
            this.troopType = new VO<HeroS.TroopFollow.TroopType>(this, (byte)Property.troopType, HeroS.TroopFollow.TroopType.AntukAir);
            this.level = new VO<int>(this, (byte)Property.level, 0);
            this.hitpoint = new VO<float>(this, (byte)Property.hitpoint, 0);
            this.attack = new VO<int>(this, (byte)Property.attack, 1);
            this.attackRange = new VO<float>(this, (byte)Property.attackRange, 1);
            this.worldPosition = new VO<Vector3>(this, (byte)Property.worldPosition, Vector3.zero);
            this.takeDamage = new VD<TakeDamage>(this, (byte)Property.takeDamage, new TakeDamage());
            this.intention = new VD<TroopIntention>(this, (byte)Property.intention, new TroopIntention());
            this.troopAttack = new VD<TroopAttack>(this, (byte)Property.troopAttack, new TroopAttack());
        }

        #endregion

    }
}
