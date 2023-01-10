using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS
{
    [CreateAssetMenu(fileName = "ItemMapAsset", menuName = "BattleRush/ItemMapAsset", order = 2)]
    public class ItemMap : ScriptableObject
    {
        public List<ItemAsset> items = new List<ItemAsset>();
    }
}