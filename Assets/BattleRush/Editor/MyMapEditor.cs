using System.Collections;
using System.Collections.Generic;
using Dreamteck.Forever;
using Dreamteck.Splines;
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

        private Vector3 cellSize = new Vector3(2f, 0, 2f);

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
                // state
                {
                    // find
                    bool canEdit = false;
                    {
                        BattleRushUI battleRushUI = GameObject.FindObjectOfType<BattleRushUI>();
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
                                        case BattleRush.State.Type.Load:                                        
                                        case BattleRush.State.Type.Start:
                                        case BattleRush.State.Type.Play:
                                        case BattleRush.State.Type.End:
                                            break;
                                        case BattleRush.State.Type.Edit:
                                            canEdit = true;
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
                                Logger.LogError("battleRushUIData null");
                            }
                        }
                        else
                        {
                            Logger.LogError("battleRushUI null");
                        }
                    }
                    // process
                    if (canEdit)
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
                // get txt
                string txt = "";
                {
                    switch(controller.state.v.getType())
                    {
                        case Controller.State.Type.None:
                            {
                                txt = "Not in edit mode, cannot paint";
                            }
                            break;
                        case Controller.State.Type.Paint:
                            {
                                txt = "Paint";
                            }
                            break;
                        default:
                            Logger.LogError("unknown type: " + controller.state.v.getType());
                            break;
                    }
                }
                // button
                GUILayout.Toggle(controller.state.v.getType() == Controller.State.Type.Paint, txt, "Button", GUILayout.Height(60f));               
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

        private Vector3 GetSelectedCell()
        {
            // Get the mouse position in world space such as y = 0
            Ray guiRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            Vector3 mousePosition = guiRay.origin - guiRay.direction * (guiRay.origin.y / guiRay.direction.y);

            // Get the corresponding cell on our virtual grid
            return new Vector3(mousePosition.x, 1, mousePosition.z);
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
                        Vector3 cellCenter = GetSelectedCell(); // Refactoring, I moved some code in this function

                        // DisplayVisualHelp(cellCenter);
                        {
                            // Vertices of our square
                            Vector3 topLeft = cellCenter + Vector3.Scale(Vector3.left, cellSize) * 0.5f + Vector3.Scale(Vector3.forward, cellSize) * 0.5f;
                            Vector3 topRight = cellCenter - Vector3.Scale(Vector3.left, cellSize) * 0.5f + Vector3.Scale(Vector3.forward, cellSize) * 0.5f;
                            Vector3 bottomLeft = cellCenter + Vector3.Scale(Vector3.left, cellSize) * 0.5f - Vector3.Scale(Vector3.forward, cellSize) * 0.5f;
                            Vector3 bottomRight = cellCenter - Vector3.Scale(Vector3.left, cellSize) * 0.5f - Vector3.Scale(Vector3.forward, cellSize) * 0.5f;

                            // Rendering
                            Handles.color = Color.green;
                            Vector3[] lines = { topLeft, topRight, topRight, bottomRight, bottomRight, bottomLeft, bottomLeft, topLeft };
                            Handles.DrawLines(lines);
                        }

                        // HandleSceneViewInputs(cellCenter);
                        {
                            // Filter the left click so that we can't select objects in the scene
                            if (Event.current.type == EventType.Layout)
                            {
                                HandleUtility.AddDefaultControl(0); // Consume the event
                            }

                            // We have a prefab selected and we are clicking in the scene view with the left button
                            if (controller.paletteIndex.v >= 0 && controller.paletteIndex.v < controller.palette.vs.Count)
                            {
                                if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                                {
                                    Logger.Log("MyMapEditor: cell center position: " + cellCenter);
                                    // find level segment contain
                                    LevelSegment levelSegment = null;
                                    {
                                        SplineSample evalResult = new SplineSample();
                                        LevelGenerator.instance.Project(cellCenter, evalResult);
                                        int segmentIndex;

                                        LevelGenerator.instance.GlobalToLocalPercent(evalResult.percent, out segmentIndex);
                                        if (segmentIndex >= 0 && segmentIndex < LevelGenerator.instance.segments.Count)
                                        {
                                            Logger.Log("MyMapEditor: paint in segment: " + LevelGenerator.instance.segments[segmentIndex].name);
                                            levelSegment = LevelGenerator.instance.segments[segmentIndex];
                                        }
                                        else
                                        {
                                            Logger.LogError("segmentIndex error: " + segmentIndex + ", " + LevelGenerator.instance.segments.Count);
                                        }
                                    }
                                    // make game object
                                    if (levelSegment != null)
                                    {
                                        Segment segment = levelSegment.GetComponent<Segment>();
                                        if (segment != null)
                                        {
                                            switch (segment.segmentType)
                                            {
                                                case Segment.SegmentType.Run:
                                                    {
                                                        // Create the prefab instance while keeping the prefab link
                                                        GameObject prefab = controller.palette.vs[controller.paletteIndex.v].getMyGameObject();
                                                        GameObject gameObject = PrefabUtility.InstantiatePrefab(prefab, levelSegment.transform) as GameObject;
                                                        gameObject.transform.position = cellCenter;

                                                        // Allow the use of Undo (Ctrl+Z, Ctrl+Y).
                                                        Undo.RegisterCreatedObjectUndo(gameObject, "");
                                                    }
                                                    break;
                                                case Segment.SegmentType.Arena:
                                                    break;
                                                default:
                                                    Logger.LogError("unknown type: "+segment.segmentType);
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            Logger.LogError("segment null");
                                        }                                        
                                    }
                                    else
                                    {
                                        Logger.LogError("levelSegment null");
                                    }                                   
                                }
                            }
                        }

                        // Refresh the view
                        sceneView.Repaint();
                    }
                    break;
                default:
                    Logger.LogError("unknown type: " + controller.state.v.getType());
                    break;
            }
        }

    }

}