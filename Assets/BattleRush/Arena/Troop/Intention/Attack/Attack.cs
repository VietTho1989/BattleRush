using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ArenaS.TroopS.IntentionS
{
    public class Attack : TroopIntention.Intention
    {

        public VO<uint> targetId;

        public VO<float> time;

        #region Constructor

        public enum Property
        {
            targetId,
            time
        }

        public Attack() : base()
        {
            this.targetId = new VO<uint>(this, (byte)Property.targetId, 0);
            this.time = new VO<float>(this, (byte)Property.time, 0);
        }

        #endregion

        public override Type getType()
        {
            return Type.Attack;
        }

    }
}