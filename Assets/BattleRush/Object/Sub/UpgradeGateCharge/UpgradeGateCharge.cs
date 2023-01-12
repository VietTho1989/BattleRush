using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ObjectS
{
    public class UpgradeGateCharge : ObjectInPath
    {

        public VO<Position> position;

        #region Constructor

        public enum Property
        {
            position
        }

        public UpgradeGateCharge() : base()
        {
            this.position = new VO<Position>(this, (byte)Property.position, Position.Zero);
        }

        #endregion

        public override Type getType()
        {
            return Type.UpgradeGateCharge;
        }

        public override Position getPosition()
        {
            return this.position.v;
        }

    }
}
