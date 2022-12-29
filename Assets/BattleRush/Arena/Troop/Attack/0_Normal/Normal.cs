using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ArenaS.TroopS.TroopAttackS
{
    public class Normal : TroopAttack.State
    {

        public const float CoolDown = 2.5f;

        public VO<float> coolDown;


        #region Constructor

        public enum Property
        {
            coolDown
        }

        public Normal() : base()
        {
            this.coolDown = new VO<float>(this, (byte)Property.coolDown, 0.0f);
        }

        #endregion

        public override Type getType()
        {
            return Type.Normal;
        }

    }
}