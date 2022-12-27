using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ObjectS
{
    public class Pike : ObjectInPath
    {

        public VO<Vector3> P;

        public VO<Vector3> R;

        public VD<Obstruction> obstruction;

        #region Constructor

        public enum Property
        {
            P,
            R,
            obstruction
        }

        public Pike() : base()
        {
            this.P = new VO<Vector3>(this, (byte)Property.P, Vector3.zero);
            this.R = new VO<Vector3>(this, (byte)Property.R, Vector3.zero);
            this.obstruction = new VD<Obstruction>(this, (byte)Property.obstruction, new Obstruction());
        }

        #endregion

        public override Type getType()
        {
            return Type.Pike;
        }
    }
}
