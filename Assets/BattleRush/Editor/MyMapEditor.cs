using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static BattleRushS.BattleRushUI.UIData;

namespace BattleRushS
{
    public class MyMapEditor : EditorWindow
    {

        #region controller

        public class Controller : Data
        {

            #region state

            public abstract class State : Data
            {

                public enum Type
                {
                    None,
                    Paint
                }

                public abstract Type getType();

                #region None

                public class None : State
                {

                    #region Constructor

                    public enum Property
                    {

                    }

                    public None() : base()
                    {

                    }

                    #endregion

                    public override Type getType()
                    {
                        return Type.None;
                    }

                }

                #endregion

                #region Paint

                public class Paint : State
                {                    

                    #region Constructor

                    public enum Property
                    {
                        
                    }

                    public Paint() : base()
                    {
                        
                    }

                    #endregion

                    public override Type getType()
                    {
                        return Type.Paint;
                    }

                }

                #endregion

            }

            public VD<State> state;

            #endregion

            public LO<ObjectInPathUIInterface> palette;

            public VO<int> paletteIndex;

            #region Constructor

            public enum Property
            {
                state,
                palette,
                paletteIndex
            }

            public Controller() : base()
            {
                this.state = new VD<State>(this, (byte)Property.state, new State.None());
                this.palette = new LO<ObjectInPathUIInterface>(this, (byte)Property.palette);
                this.paletteIndex = new VO<int>(this, (byte)Property.paletteIndex, 0);
            }

            #endregion


        }

        private Controller controller = new Controller();

        #endregion

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


        private void RefreshPalette()
        {
            controller.palette.clear();

            BattleRushUI battleRushUI = GameObject.FindObjectOfType<BattleRushUI>();
            if (battleRushUI != null)
            {
                controller.palette.add(battleRushUI.coinPrefab);
                controller.palette.add(battleRushUI.energyOrbNormalPrefab);
                controller.palette.add(battleRushUI.troopCagePrefab);
                controller.palette.add(battleRushUI.sawBladePrefab);
                controller.palette.add(battleRushUI.bladePrefab);
                controller.palette.add(battleRushUI.fireNozzlePrefab);
                controller.palette.add(battleRushUI.pikePrefab);
                controller.palette.add(battleRushUI.energyOrbUpgradePrefab);
                controller.palette.add(battleRushUI.upgradeGateFreePrefab);
                controller.palette.add(battleRushUI.upgradeGateChargePrefab);
                controller.palette.add(battleRushUI.hammerPrefab);
                controller.palette.add(battleRushUI.grinderPrefab);
                controller.palette.add(battleRushUI.energyOrbPowerPrefab);
                controller.palette.add(battleRushUI.cocoonMantahPrefab);
            }
            else
            {
                Logger.LogError("battleRushUI null");
            }
        }

        Vector2 scrollPosition;

        // Called to draw the MapEditor windows.
        private void OnGUI()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            // paint or not
            {
                // get txt
                string txt = "";
                {
                    switch(controller.state.v.getType())
                    {
                        case Controller.State.Type.None:
                            {
                                txt = "Start Painting";
                            }
                            break;
                        case Controller.State.Type.Paint:
                            {
                                txt = "Stop Painting";
                            }
                            break;
                        default:
                            Logger.LogError("unknown type: " + controller.state.v.getType());
                            break;
                    }
                }
                // button
                bool isPaint = GUILayout.Toggle(controller.state.v.getType() == Controller.State.Type.Paint, txt, "Button", GUILayout.Height(60f));
                if (isPaint)
                {
                    Controller.State.Paint paint = controller.state.newOrOld<Controller.State.Paint>();
                    {

                    }
                    controller.state.v = paint;
                }
                else
                {
                    Controller.State.None none = controller.state.newOrOld<Controller.State.None>();
                    {

                    }
                    controller.state.v = none;
                }
            }

            // Get a list of previews, one for each of our prefabs
            {
                List<GUIContent> paletteIcons = new List<GUIContent>();
                foreach (ObjectInPathUIInterface prefab in controller.palette.vs)
                {
                    // Get a preview for the prefab
                    Texture2D texture = AssetPreview.GetAssetPreview(prefab.getMyGameObject());
                    paletteIcons.Add(new GUIContent(prefab.getType().ToString(), texture));
                }
                // Display the grid
                {
                    controller.paletteIndex.v = GUILayout.SelectionGrid(controller.paletteIndex.v, paletteIcons.ToArray(), 2);
                }                
            }

            GUILayout.EndScrollView();
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
            switch (controller.state.v.getType())
            {
                case Controller.State.Type.None:
                    break;
                case Controller.State.Type.Paint:
                    {
                        Vector2 cellCenter = GetSelectedCell(); // Refactoring, I moved some code in this function

                        DisplayVisualHelp(cellCenter);
                        HandleSceneViewInputs(cellCenter);

                        // Refresh the view
                        sceneView.Repaint();
                    }
                    break;
                default:
                    Logger.LogError("unknown type: " + controller.state.v.getType());
                    break;
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