using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS
{
    [System.Serializable]
    public class ItemAsset
    {

        public ObjectInPath.Type type;

        public Position position;

        public uint row = 1;
        public uint col = 1;

        public float distanceBetweenRow = 0.1f;
        public float distanceBetweenCol = 0.1f;

    }
}