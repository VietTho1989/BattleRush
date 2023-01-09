using System.Collections;
using System.Collections.Generic;
using BattleRushS.ArenaS;
using UnityEngine;


namespace BattleRushS.HeroS
{
    public class HeroInformation : MonoBehaviour, TroopType
    {

        #region information

        public string id;
        public string heroName;
        public string race;
        public string skinPortraitName;
        public string skinModelName;
        public string weapon;
        public string vfxDeadName;
        public string unLockType;
        public int unlockPrice;

        public TroopType.Type getType()
        {
            return TroopType.Type.Hero;
        }

        public GameObject getGameObject()
        {
            return this.gameObject;
        }

        #endregion

        public Animator myAnimator;

        public void playAnimation(string animationName)
        {
            // find
            if (myAnimator == null)
            {
                myAnimator = GetComponentInChildren<Animator>();
            }
            // play
            if (myAnimator != null)
            {
                // fix name
                {
                    switch (animationName)
                    {
                        case "Attack":
                            animationName = "Cast spell";
                            break;
                        case "Move":
                            animationName = "Run";
                            break;
                        case "Die":
                            animationName = "Death";
                            break;
                        default:
                            break;
                    }
                }
                myAnimator.Play(animationName);
            }
        }

        public void setUIByTeam(int teamId)
        {
            
        }
    }
}