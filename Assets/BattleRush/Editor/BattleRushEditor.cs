using System.Collections;
using System.Collections.Generic;
using BattleRushS.ObjectS;
using Dreamteck.Forever;
using UnityEditor;
using UnityEngine;

namespace BattleRushS
{
    [CustomEditor(typeof(BattleRushUI))]
    public class BattleRushEditor : Editor
    {

        /*public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            // add object
            {
                GUILayout.Space(100);
                Rect lastRect = GUILayoutUtility.GetLastRect();
                Rect buttonRect = new Rect(lastRect.x, lastRect.y + 2 * EditorGUIUtility.singleLineHeight, 160, 60);
                if (GUI.Button(buttonRect, "Add Object"))
                {
                    BattleRushUI battleRushUI = this.target as BattleRushUI;
                    // process
                    if (battleRushUI != null)
                    {
                        Logger.Log("battleRushUI: " + battleRushUI.data);
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
                                            // find
                                            List<Segment> segments = new List<Segment>();
                                            {
                                                foreach (Segment segment in GameObject.FindObjectsOfType<Segment>())
                                                {
                                                    if (segment.segmentType == Segment.SegmentType.Run)
                                                    {
                                                        segments.Add(segment);
                                                    }
                                                }
                                            }
                                            // process
                                            if (segments.Count > 0)
                                            {
                                                Segment segment = segments[Random.Range(0, segments.Count)];
                                                Logger.Log("battleRushEditor choose segment: " + segment.name);
                                                // make object in path
                                                {
                                                    // make data
                                                    Coin coin = new Coin();
                                                    {
                                                        coin.uid = battleRush.laneObjects.makeId();
                                                        coin.position.v = new Position(Random.Range(-4, 4), segment.segmentPos + Random.Range(0, segment.length));
                                                    }
                                                    battleRush.laneObjects.add(coin);
                                                    // make UI
                                                    {
                                                        CoinUI.UIData objectUIData = new CoinUI.UIData();
                                                        {
                                                            objectUIData.coin.v = new ReferenceData<Coin>(coin);
                                                        }
                                                        battleRushUI.data.objectInPaths.add(objectUIData);
                                                        Transform objectUI = UIUtils.Instantiate(objectUIData, battleRushUI.coinPrefab, segment.transform).transform;
                                                        // set position
                                                        {
                                                            if (objectUI != null)
                                                            {
                                                                float x = coin.position.v.x;
                                                                float z = coin.position.v.z - (segment.segmentPos + segment.length / 2.0f);
                                                                Logger.Log("z local: " + z);
                                                                // set
                                                                objectUI.localPosition = new Vector3(x, 1, z);
                                                            }
                                                            else
                                                            {
                                                                Logger.LogError("coinUI null");
                                                            }
                                                        }
                                                    }
                                                }
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
            // save
            {
                GUILayout.Space(100);
                Rect lastRect = GUILayoutUtility.GetLastRect();
                Rect buttonRect = new Rect(lastRect.x, lastRect.y + 2 * EditorGUIUtility.singleLineHeight, 160, 60);
                if (GUI.Button(buttonRect, "Save"))
                {
                    BattleRushUI battleRushUI = this.target as BattleRushUI;
                    // process
                    if (battleRushUI != null)
                    {
                        Logger.Log("battleRushUI: " + battleRushUI.data);
                        BattleRushUI.UIData battleRushUIData = battleRushUI.data;
                        if (battleRushUIData != null)
                        {
                            BattleRush battleRush = battleRushUIData.battleRush.v.data;
                            if (battleRush != null)
                            {
                                switch (battleRush.state.v.getType())
                                {
                                    case BattleRush.State.Type.Load:
                                        break;
                                    case BattleRush.State.Type.Edit:
                                        {
                                            ItemMap itemMap = battleRush.mapData.v.itemMap.v;
                                            if (itemMap != null)
                                            {
                                                itemMap.items.Clear();
                                                foreach(ObjectInPath objectInPath in battleRush.laneObjects.vs)
                                                {
                                                    ItemAsset itemAsset = new ItemAsset();
                                                    {
                                                        itemAsset.type = objectInPath.getType();
                                                        itemAsset.position = objectInPath.getPosition();
                                                        //public uint row = 1;
                                                        //public uint col = 1;
                                                        //public float distanceBetweenRow = 0.1f;
                                                        //public float distanceBetweenCol = 0.1f;
                                                    }
                                                    itemMap.items.Add(itemAsset);
                                                }
                                                EditorUtility.SetDirty(itemMap);
                                            }
                                            else
                                            {
                                                Logger.LogError("itemMap null");
                                            }
                                        }
                                        break;
                                    case BattleRush.State.Type.Start:
                                        break;
                                    case BattleRush.State.Type.Play:
                                        break;
                                    case BattleRush.State.Type.End:
                                        break;
                                    default:
                                        Logger.LogError("unknown type: " + battleRush.state.v.getType());
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
                            Logger.LogError("battleRush null");
                        }
                    }
                    else
                    {
                        Logger.LogError("battleRushUI null");
                    }
                }
            }
        }*/
    }
}