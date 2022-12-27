using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.HeroS
{
    public class TroopInformation : MonoBehaviour
    {

        public TroopFollow.TroopType troopType;

        public Animator troopAnimator;
        public AnimationClip animationIdle;
        public AnimationClip animationMove;

    }
}