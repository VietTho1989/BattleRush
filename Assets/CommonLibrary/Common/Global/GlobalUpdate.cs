using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalUpdate : MonoBehaviour
{

    public RectTransform mainUI;

    private void Awake()
    {
        // Application.targetFrameRate = 300;

        // Time.timeScale = 0.25f;
    }

    public RectTransform getSizeUI;

    void Update()
    {
        Global.get().mainUI = mainUI;
        Global.get().networkReachability.v = Application.internetReachability;
        Global.get().deviceOrientation.v = Input.deviceOrientation;
        Global.get().screenOrientation.v = Screen.orientation;
        if (getSizeUI != null)
        {
            Global.get().width.v = getSizeUI.rect.size.x;
            Global.get().height.v = getSizeUI.rect.size.y;
            // Logger.Log("getSizeUI: " + Global.get().width.v + ", " + Global.get().height.v);
        }
        else
        {
            // Logger.LogError("getSizeUI null");
        }
        Global.get().screenWidth.v = Screen.width;
        Global.get().screenHeight.v = Screen.height;
        // event click
        /*if (Input.GetMouseButtonDown (0)) {
            if (EventSystem.current.IsPointerOverGameObject ()) {
                Debug.LogError ("left-click over a GUI element!: " + EventSystem.current.currentSelectedGameObject);
            } else {
                Debug.Log ("just a left-click!");
            }
        }*/
    }

    #region black screen

    public static bool IsBlackScreen = false;
    public static int BlackCount = 2;

    private void LateUpdate()
    {
        /*if (IsBlackScreen)
        {
            if (!AppDataS.AppLoginUI.UIData.get().isBlackScreen(false))
            {
                if (BlackCount <= 0)
                {
                    IsBlackScreen = false;
                }
                else
                {
                    BlackCount--;
                }
            }
        }*/
    }

    #endregion

}