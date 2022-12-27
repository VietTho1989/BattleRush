using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RecyclerKit
{

    public static TrashMan instance;

    static RecyclerKit()
    {
        Object recyclerPrefab = Resources.Load("RecyclerKit");
        if (recyclerPrefab != null)
        {
            GameObject recyclerKit = Object.Instantiate(recyclerPrefab) as GameObject;
            if (recyclerKit != null)
            {
                instance = recyclerKit.GetComponent<TrashMan>();
            }
            else
            {
                Logger.LogError("recyclerKit null");
            }
        }
        else
        {
            Logger.LogError("recyclerPrefab null");
        }
        // Debug.LogError("load recyclerKit: " + instance);
    }

    public static TrashMan get()
    {
        return instance;
    }

}