using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BattleRushS
{
    public class MyMapEditor : EditorWindow
    {

        // The window is selected if it already exists, else it's created.
        [MenuItem("Window/My Map Editor")]
        private static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(MyMapEditor));
        }

        void OnDestroy()
        {
            SceneView.duringSceneGui -= this.OnSceneGUI;
        }

        private bool paintMode = false;

        private Vector2 cellSize = new Vector2(2f, 2f);

        private void DisplayVisualHelp(Vector2 cellCenter)
        {
            // Get the mouse position in world space such as z = 0
            //Ray guiRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            //Vector3 mousePosition = guiRay.origin - guiRay.direction * (guiRay.origin.z / guiRay.direction.z);

            // Get the corresponding cell on our virtual grid
            //Vector2Int cell = new Vector2Int(Mathf.RoundToInt(mousePosition.x / cellSize.x), Mathf.RoundToInt(mousePosition.y / cellSize.y));
            //Vector2 cellCenter = cell * cellSize;

            // Vertices of our square
            Vector3 topLeft = cellCenter + Vector2.left * cellSize * 0.5f + Vector2.up * cellSize * 0.5f;
            Vector3 topRight = cellCenter - Vector2.left * cellSize * 0.5f + Vector2.up * cellSize * 0.5f;
            Vector3 bottomLeft = cellCenter + Vector2.left * cellSize * 0.5f - Vector2.up * cellSize * 0.5f;
            Vector3 bottomRight = cellCenter - Vector2.left * cellSize * 0.5f - Vector2.up * cellSize * 0.5f;

            // Rendering
            Handles.color = Color.green;
            Vector3[] lines = { topLeft, topRight, topRight, bottomRight, bottomRight, bottomLeft, bottomLeft, topLeft };
            Handles.DrawLines(lines);
        }

        void OnFocus()
        {
            SceneView.duringSceneGui -= this.OnSceneGUI; // Don't add twice
            SceneView.duringSceneGui += this.OnSceneGUI;

            RefreshPalette(); // Refresh the palette (can be called uselessly, but there is no overhead.)
        }

        // A list containing the available prefabs.
        [SerializeField]
        private List<GameObject> palette = new List<GameObject>();


        private void RefreshPalette()
        {
            palette.Clear();

            BattleRushUI battleRushUI = GameObject.FindObjectOfType<BattleRushUI>();
            if (battleRushUI != null)
            {
                palette.Add(battleRushUI.coinPrefab.gameObject);
                palette.Add(battleRushUI.energyOrbNormalPrefab.gameObject);
                palette.Add(battleRushUI.troopCagePrefab.gameObject);
                palette.Add(battleRushUI.sawBladePrefab.gameObject);
                palette.Add(battleRushUI.bladePrefab.gameObject);
                palette.Add(battleRushUI.fireNozzlePrefab.gameObject);
                palette.Add(battleRushUI.pikePrefab.gameObject);
                palette.Add(battleRushUI.energyOrbUpgradePrefab.gameObject);
                palette.Add(battleRushUI.upgradeGateFreePrefab.gameObject);
                palette.Add(battleRushUI.upgradeGateChargePrefab.gameObject);
                palette.Add(battleRushUI.hammerPrefab.gameObject);
                palette.Add(battleRushUI.grinderPrefab.gameObject);
                palette.Add(battleRushUI.energyOrbPowerPrefab.gameObject);
                palette.Add(battleRushUI.cocoonMantahPrefab.gameObject);
            }
            else
            {
                Logger.LogError("battleRushUI null");
            }
        }

        [SerializeField]
        private int paletteIndex;

        // Called to draw the MapEditor windows.
        private void OnGUI()
        {
            paintMode = GUILayout.Toggle(paintMode, "Start painting", "Button", GUILayout.Height(60f));

            // Get a list of previews, one for each of our prefabs
            List<GUIContent> paletteIcons = new List<GUIContent>();
            foreach (GameObject prefab in palette)
            {
                // Get a preview for the prefab
                Texture2D texture = AssetPreview.GetAssetPreview(prefab);
                paletteIcons.Add(new GUIContent(texture));
            }

            // Display the grid
            paletteIndex = GUILayout.SelectionGrid(paletteIndex, paletteIcons.ToArray(), 6);
        }

        private Vector2 GetSelectedCell()
        {
            // Get the mouse position in world space such as z = 0
            Ray guiRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            Vector3 mousePosition = guiRay.origin - guiRay.direction * (guiRay.origin.z / guiRay.direction.z);

            // Get the corresponding cell on our virtual grid
            Vector2Int cell = new Vector2Int(Mathf.RoundToInt(mousePosition.x / cellSize.x), Mathf.RoundToInt(mousePosition.y / cellSize.y));
            return cell * cellSize;
        }

        // Does the rendering of the map editor in the scene view.
        private void OnSceneGUI(SceneView sceneView)
        {
            if (paintMode)
            {
                Vector2 cellCenter = GetSelectedCell(); // Refactoring, I moved some code in this function


                DisplayVisualHelp(cellCenter);
                HandleSceneViewInputs(cellCenter);

                // Refresh the view
                sceneView.Repaint();
            }
        }

        private void HandleSceneViewInputs(Vector2 cellCenter)
        {
            // Filter the left click so that we can't select objects in the scene
            if (Event.current.type == EventType.Layout)
            {
                HandleUtility.AddDefaultControl(0); // Consume the event
            }
        }

    }
}