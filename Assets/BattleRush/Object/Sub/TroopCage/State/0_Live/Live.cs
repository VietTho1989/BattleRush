using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ObjectS.TroopCageS
{
    public class Live : TroopCage.State
    {

        public VO<int> hitpoint;

        public LD<TroopCageShootProjectile> projectiles;

        #region Constructor

        public enum Property
        {
            hitpoint,
            projectiles
        }

        public Live() : base()
        {
            this.hitpoint = new VO<int>(this, (byte)Property.hitpoint, 5);
            this.projectiles = new LD<TroopCageShootProjectile>(this, (byte)Property.projectiles);
        }

        #endregion

        public override Type getType()
        {
            return Type.Live;
        }

    }
}