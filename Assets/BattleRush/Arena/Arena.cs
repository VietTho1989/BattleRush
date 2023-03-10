using System.Collections;
using System.Collections.Generic;
using BattleRushS.ArenaS;
using UnityEngine;

namespace BattleRushS
{
    public class Arena : Data
    {

        #region stage

        public abstract class Stage : Data
        {

            public enum Type
            {
                /** init troop for battle*/
                PreBattle,
                MoveTroopToFormation,
                AutoFight,
                FightEnd
            }

            public abstract Type getType();

        }

        public VD<Stage> stage;

        #endregion

        public LD<Troop> troops;

        public LD<Projectile> projectiles;

        #region Constructor

        public enum Property
        {
            stage,
            troops,
            projectiles
        }

        public Arena() : base()
        {
            this.stage = new VD<Stage>(this, (byte)Property.stage, new PreBattle());
            this.troops = new LD<Troop>(this, (byte)Property.troops);
            this.projectiles = new LD<Projectile>(this, (byte)Property.projectiles);
        }

        #endregion

    }
}