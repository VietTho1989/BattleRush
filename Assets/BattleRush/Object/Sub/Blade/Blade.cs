using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ObjectS
{
    public class Blade : ObjectInPath
    {

        public VO<Position> position;

        public VD<Obstruction> obstruction;

        #region Constructor

        public enum Property
        {
            position,
            obstruction
        }

        public Blade() : base()
        {
            this.position = new VO<Position>(this, (byte)Property.position, Position.Zero);
            this.obstruction = new VD<Obstruction>(this, (byte)Property.obstruction, new Obstruction());
        }

        #endregion

        public override Type getType()
        {
            return Type.Blade;
        }

    }
}
