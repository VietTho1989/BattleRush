using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ObjectS
{
    public class CocoonMantah : ObjectInPath
    {

        public VO<Position> position;

        #region Constructor

        public enum Property
        {
            position
        }

        public CocoonMantah() : base()
        {
            this.position = new VO<Position>(this, (byte)Property.position, Position.Zero);
        }

        #endregion

        public override Type getType()
        {
            return Type.CocoonMantah;
        }

    }
}
