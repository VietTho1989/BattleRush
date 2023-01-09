using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using BattleRushS.ArenaS;
using UnityEditor;
using UnityEngine;
using static BattleRushS.HeroS.TroopInformation;

namespace BattleRushS.HeroS
{
    [CustomEditor(typeof(TroopInformation))]
    public class TroopInformationEditor : Editor
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
                TroopInformation troopInformation = this.target as TroopInformation;
                //Debug.Log("set data from file excel: " + heroInformation.id);
                string[] parts = Array.ConvertAll(troopInformation.troopName.Split('\t'), p => p.Trim());
                Debug.Log("parts: " + parts.Length);
                if (parts.Length == 46)
                {
                    troopInformation.troopName = parts[0];
                    // attack type
                    {
                        // public Troop.AttackType attackType = Troop.AttackType.Range;
                        Troop.AttackType attackType = Troop.AttackType.Range;
                        {
                            switch (parts[1].ToLower())
                            {
                                case "range":
                                    attackType = Troop.AttackType.Range;
                                    break;
                                case "melee":
                                    attackType = Troop.AttackType.Melee;
                                    break;
                                default:
                                    Debug.LogError("unknown " + parts[1]);
                                    break;
                            }
                        }
                        troopInformation.attackType = attackType;
                    }
                    // unlockPriority
                    {
                        try
                        {
                            troopInformation.unlockPriority = int.Parse(parts[2]);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError(e);
                        }
                    }
                    troopInformation.modelName = parts[3];
                    troopInformation.armyMaterial = parts[4];
                    troopInformation.enemyMaterial = parts[5];
                    troopInformation.weapon = parts[6];
                    troopInformation.vfxDeadName = parts[7];
                    troopInformation.portraitName = parts[8];
                    // level
                    {
                        troopInformation.levels.Clear();
                        for(int i=0; i<3; i++)
                        {
                            Level level = new Level();
                            {
                                int.TryParse(parts[9 + 15 * i], out level.level);
                                float.TryParse(parts[10 + 15 * i], NumberStyles.Float, CultureInfo.GetCultureInfo("en-EN"), out level.hp);
                                float.TryParse(parts[11 + 15 * i], NumberStyles.Float, CultureInfo.GetCultureInfo("en-EN"), out level.attack);
                                float.TryParse(parts[12 + 15 * i], NumberStyles.Float, CultureInfo.GetCultureInfo("en-EN"), out level.attackRange);
                                float.TryParse(parts[13 + 15 * i], NumberStyles.Float, CultureInfo.GetCultureInfo("en-EN"), out level.scale);
                                level.vfxLevelAuraName = parts[14 + 15 * i];
                            }
                            troopInformation.levels.Add(level);
                        }
                    }
                    troopInformation.potraitTroopUnlockName = parts[15];
                }
                else
                {
                    Logger.LogError("why not 46");
                }
            }
        }

    }
}
