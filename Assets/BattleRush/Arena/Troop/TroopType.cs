using System.Collections;
using System.Collections.Generic;
using BattleRushS.HeroS;
using UnityEngine;

namespace BattleRushS.ArenaS
{
    public interface TroopType
    {

        public enum Type
        {
            Hero,
            Normal,
            Monster
        }

        public abstract Type getType();

        public abstract void playAnimation(string animationName);

        public abstract GameObject getGameObject();

        public abstract void setUIByTeam(int teamId);

    }
}