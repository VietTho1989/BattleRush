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

        private void reload(BattleRush battleRush)
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
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            // add object
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
                                        switch (segment.segmentType)
                                        {
                                            case Segment.SegmentType.Run:
                                                {
                                                    Logger.Log("reload: " + segment);
                                                    LevelSegment levelSegment = segment.GetComponent<LevelSegment>();
                                                    if (levelSegment != null)
                                                    {
                                                        // btnReload
                                                        {
                                                            GUILayout.Space(100);
                                                            Rect lastRect = GUILayoutUtility.GetLastRect();
                                                            Rect buttonRect = new Rect(lastRect.x, lastRect.y + 2 * EditorGUIUtility.singleLineHeight, 160, 60);
                                                            if (GUI.Button(buttonRect, "Reload"))
                                                            {
                                                                reload(battleRush);
                                                            }
                                                        }
                                                        // btnDelete
                                                        {
                                                            GUILayout.Space(100);
                                                            Rect lastRect = GUILayoutUtility.GetLastRect();
                                                            Rect buttonRect = new Rect(lastRect.x, lastRect.y + 2 * EditorGUIUtility.singleLineHeight, 160, 60);
                                                            if (GUI.Button(buttonRect, "Delete"))
                                                            {
                                                                segment.isDeleted = true;
                                                                reload(battleRush);
                                                            }
                                                        }
                                                        // btnCopy
                                                        {
                                                            GUILayout.Space(100);
                                                            Rect lastRect = GUILayoutUtility.GetLastRect();
                                                            Rect buttonRect = new Rect(lastRect.x, lastRect.y + 2 * EditorGUIUtility.singleLineHeight, 160, 60);
                                                            if (GUI.Button(buttonRect, "Copy"))
                                                            {
                                                                Instantiate(segment, segment.transform);
                                                                reload(battleRush);
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Logger.LogError("levelSegment null");
                                                    }
                                                }
                                                break;
                                            case Segment.SegmentType.Arena:
                                                {
                                                    Logger.LogError("arena null");
                                                }
                                                break;
                                            default:
                                                break;
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