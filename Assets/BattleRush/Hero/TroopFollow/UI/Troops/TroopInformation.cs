using System.Collections;
using System.Collections.Generic;
using BattleRushS.ArenaS;
using UnityEngine;

namespace BattleRushS.HeroS
{
    public class TroopInformation : MonoBehaviour, TroopType
    {

        public Animator troopAnimator;
        public AnimationClip animationIdle;
        public AnimationClip animationMove;

        public TroopType.Type getType()
        {
            return TroopType.Type.Normal;
        }

        public void playAnimation(string animationName)
        {
            if (troopAnimator != null)
            {
                troopAnimator.Play(animationName);
            }
            else
            {
                Logger.LogError("troopAnimator null");
            }            
        }

        public GameObject getGameObject()
        {
            return this.gameObject;
        }

        #region information

        public string troopName;

        #region attack type

        public Troop.AttackType attackType = Troop.AttackType.Range;

        #endregion

        public int unlockPriority;
        public string modelName;
        public string armyMaterial;
        public string enemyMaterial;
        public string weapon;
        public string vfxDeadName;
        public string portraitName;

        #region level

        [System.Serializable]
        public class Level
        {
            public int level;
            public float hp;
            public float attack;
            public float attackRange;
            public float scale;
            public string vfxLevelAuraName;
        }

        public List<Level> levels = new List<Level>();

        #endregion

        public string potraitTroopUnlockName;

        #endregion
        
    }
}