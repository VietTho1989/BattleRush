using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ObjectS
{
    public class UpgradeGateCharge : ObjectInPath
    {

        public VO<Vector3> P;

        public VO<Vector3> R;

        #region Constructor

        public enum Property
        {
            P,
            R
        }

        public UpgradeGateCharge() : base()
        {
            this.P = new VO<Vector3>(this, (byte)Property.P, Vector3.zero);
            this.R = new VO<Vector3>(this, (byte)Property.R, Vector3.zero);
        }

        #endregion

        public override Type getType()
        {
            return Type.UpgradeGateCharge;
        }
    }
}
