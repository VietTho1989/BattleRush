using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Dreamteck.Forever;
using BattleRushS.StateS;
using BattleRushS.StateS.LoadS;

namespace BattleRushS
{
    [CustomEditor(typeof(Segment))]
    public class SegmentEditor : Editor
    {
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            // add object
            {
                GUILayout.Space(100);
                Rect lastRect = GUILayoutUtility.GetLastRect();
                Rect buttonRect = new Rect(lastRect.x, lastRect.y + 2 * EditorGUIUtility.singleLineHeight, 160, 60);
                if (GUI.Button(buttonRect, "Reload"))
                {
                    Segment segment = this.target as Segment;
                    BattleRushUI battleRushUI = segment.GetComponentInParent<BattleRushUI>();
                    // process
                    if (battleRushUI != null)
                    {
                        BattleRushUI.UIData battleRushUIData = battleRushUI.data;
                        if (battleRushUIData != null)
                        {
                            BattleRush battleRush = battleRushUIData.battleRush.v.data;
                            if (battleRush != null)
                            {
                                switch (battleRush.state.v.getType())
                                {
                                    case BattleRush.State.Type.Edit:
                                        {
                                            Logger.Log("reload: " + segment);
                                            LevelSegment levelSegment = segment.GetComponent<LevelSegment>();
                                            if (levelSegment != null)
                                            {
                                                int level = battleRush.level.v;
                                                battleRush.save();
                                                battleRush.reset();
                                                // load
                                                {
                                                    Load load = battleRush.state.newOrOld<Load>();
                                                    {
                                                        // sub
                                                        {
                                                            LoadLevel loadLevel = load.sub.newOrOld<LoadLevel>();
                                                            {
                                                                loadLevel.level.v = level;
                                                            }
                                                            load.sub.v = loadLevel;
                                                        }
                                                    }
                                                    battleRush.state.v = load;
                                                }
                                            }
                                            else
                                            {
                                                Logger.LogError("levelSegment null");
                                            }
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                Logger.LogError("battleRush null");
                            }
                        }
                        else
                        {
                            Logger.LogError("battleRushUIData null");
                        }
                    }
                    else
                    {
                        Logger.LogError("battleRushUI null");
                    }
                }
            }
        }

    }
}