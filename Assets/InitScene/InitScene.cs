using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class InitScene : MonoBehaviour
{

    private static bool alreadyInit = false;

    // Disables analytics before any code runs that sends analytic events
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void OnRuntimeMethodBeforeSceneLoad()
    {
        if (!alreadyInit)
        {
            alreadyInit = true;
            // instantiate globalGameObject
            {
                GameObject globalGameObject = Resources.Load("GlobalGameObject") as GameObject;
                Instantiate(globalGameObject);
            }
        }
    }

}