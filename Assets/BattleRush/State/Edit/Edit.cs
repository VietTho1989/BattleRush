using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.StateS
{
    public class Edit : BattleRush.State
    {

        #region Constructor

        public enum Property
        {

        }

        public Edit() : base()
        {

        }

        #endregion

        public override Type getType()
        {
            return Type.Edit;
        }

    }
}