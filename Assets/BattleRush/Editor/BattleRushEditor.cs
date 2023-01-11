using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BattleRushS
{
    [CustomEditor(typeof(BattleRushUI))]
    public class BattleRushEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(100);
            Rect lastRect = GUILayoutUtility.GetLastRect();
            Rect buttonRect = new Rect(lastRect.x, lastRect.y + 2 * EditorGUIUtility.singleLineHeight, 160, 60);
            if (GUI.Button(buttonRect, "Refresh"))
            {
                BattleRushUI battleRushUI = this.target as BattleRushUI;
                if (battleRushUI != null)
                {
                    Logger.Log("battleRushUI: " + battleRushUI.data);
                }
                else
                {
                    Logger.LogError("battleRushUI null");
                }               
            }
        }
    }
}