using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BattleRushS.HeroS
{
    [CustomEditor(typeof(HeroInformation))]
    public class HeroInformationEditor : Editor
    {

        void OnEnable()
        {
            
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(100);
            Rect lastRect = GUILayoutUtility.GetLastRect();
            Rect buttonRect = new Rect(lastRect.x, lastRect.y + 2 * EditorGUIUtility.singleLineHeight, 160, 60);
            if (GUI.Button(buttonRect, "Set Data"))
            {
                HeroInformation heroInformation = this.target as HeroInformation;
                //Debug.Log("set data from file excel: " + heroInformation.id);
                string[] parts = Array.ConvertAll(heroInformation.id.Split('\t'), p => p.Trim());
                //Debug.Log("parts: " + parts.Length);
                if (parts.Length == 9)
                {
                    heroInformation.id = parts[0];
                    heroInformation.heroName = parts[1];
                    heroInformation.race = parts[2];
                    heroInformation.skinPortraitName = parts[3];
                    heroInformation.skinModelName = parts[4];
                    heroInformation.weapon = parts[5];
                    heroInformation.vfxDeadName = parts[6];
                    heroInformation.unLockType = parts[7];
                    // unlockPrice
                    {
                        try
                        {
                            heroInformation.unlockPrice = int.Parse(parts[8]);
                        }
                        catch(Exception e)
                        {
                            Debug.LogError(e);
                        }                       
                    }
                }
                else
                {
                    Logger.LogError("why not 9");
                }
            }
        }

    }
}