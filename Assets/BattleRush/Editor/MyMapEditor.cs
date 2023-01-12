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

        void OnFocus()
        {
            SceneView.duringSceneGui -= this.OnSceneGUI; // Just in case
            SceneView.duringSceneGui += this.OnSceneGUI;
        }

        void OnDestroy()
        {
            SceneView.duringSceneGui -= this.OnSceneGUI;
        }

        private bool paintMode = false;

        // Called to draw the MapEditor windows.
        private void OnGUI()
        {
            paintMode = GUILayout.Toggle(paintMode, "Start painting", "Button", GUILayout.Height(60f));
        }

        // Does the rendering of the map editor in the scene view.
        private void OnSceneGUI(SceneView sceneView)
        {
            if (paintMode)
            {
                DisplayVisualHelp();
            }
        }

        private Vector2 cellSize = new Vector2(2f, 2f);

        private void DisplayVisualHelp()
        {
            // Get the mouse position in world space such as z = 0
            Ray guiRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            Vector3 mousePosition = guiRay.origin - guiRay.direction * (guiRay.origin.z / guiRay.direction.z);

            // Get the corresponding cell on our virtual grid
            Vector2Int cell = new Vector2Int(Mathf.RoundToInt(mousePosition.x / cellSize.x), Mathf.RoundToInt(mousePosition.y / cellSize.y));
            Vector2 cellCenter = cell * cellSize;

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

    }
}