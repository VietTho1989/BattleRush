using System.Collections;
using System.Collections.Generic;
using Dreamteck.Forever;
using UnityEngine;

namespace BattleRushS
{
    public class BattleRushSegmentIndexUpdate : MonoBehaviour
    {

        public LevelGenerator levelGenerator;
        public BattleRushUI battleRushUI;

        private void Update()
        {
            if(levelGenerator!=null && battleRushUI != null)
            {
                if (battleRushUI.data != null)
                {
                    if (battleRushUI.data.battleRush.v.data != null)
                    {
                        // find
                        int lastIndex = -1;
                        {
                            if (levelGenerator.segments.Count > 0)
                            {
                                lastIndex = levelGenerator.segments[levelGenerator.segments.Count - 1].index;
                            }
                            //Logger.Log("last segment index: " + lastIndex);
                        }
                        // process
                        if (lastIndex != -1)
                        {
                            battleRushUI.data.battleRush.v.data.segmentIndex.v = lastIndex;
                        }
                        else
                        {
                            // Logger.LogError("lastIndex -1");
                        }
                    }
                    else
                    {
                        Logger.LogError("battleRush null");
                    }
                }
                else
                {
                    Logger.LogError("battleRush data null");
                }
            }
            else
            {
                Logger.LogError("UI null");
            }
        }

    }
}
