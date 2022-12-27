using UnityEngine;
using System.Collections;

public class UpdateUtils
{

    public static T makeComponentUpdate<T, K>(K data, Transform transform) where T : UpdateBehavior<K> where K : Data
    {
        if (data != null)
        {
            T old = data.findCallBack<T>();
            if (old == null)
            {
                // Create new 
                T newUpdate = transform.gameObject.AddComponent<T>();
                {
                    newUpdate.setData(data);
                }
                return newUpdate;
            }
            else
            {
                Logger.Log("already have");
                return old;
            }
        }
        else
        {
            Logger.LogError("why data null: " + data);
        }
        Logger.LogError("Cannot make update: " + data + ", " + transform);
        return null;
    }

    public static T makeUpdate<T, K>(K data, Transform transform) where T : UpdateBehavior<K> where K : Data
    {
        if (data != null)
        {
            T old = data.findCallBack<T>();
            if (old == null)
            {
                // Create new 
                GameObject gameObject = GameObject.Instantiate<GameObject>(EmptyGameObject.emptyGameObject, transform, false);
                gameObject.name = "" + typeof(T);
                {
                    T newUpdate = gameObject.AddComponent<T>();
                    newUpdate.setData(data);
                    return newUpdate;
                }
            }
            else
            {
                Logger.Log("already have");
                return old;
            }
        }
        else
        {
            Logger.LogError("why data null: " + data);
        }
        Logger.LogError("Cannot make update: " + data + ", " + transform);
        return null;
    }

    public static T makeTreeUpdate<T, K>(K data, Transform transform) where T : UpdateBehavior<K> where K : Data
    {
        if (data != null)
        {
            T old = data.findCallBack<T>();
            if (old == null)
            {
                // Create new 
                GameObject gameObject = GameObject.Instantiate<GameObject>(EmptyGameObject.emptyTree, transform, false);
                gameObject.name = "" + typeof(T);
                {
                    T newUpdate = gameObject.AddComponent<T>();
                    newUpdate.setData(data);
                    return newUpdate;
                }
            }
            else
            {
                Logger.Log("already have");
                return old;
            }
        }
        else
        {
            Logger.LogError("why data null: " + data);
        }
        Logger.LogError("Cannot make update: " + data + ", " + transform);
        return null;
    }

    public static UpdateBehavior<T> Instantiate<T>(T data, UpdateBehavior<T> prefab, Transform transform) where T : Data
    {
        if (transform != null && prefab != null)
        {
            UpdateBehavior<T> old = data.findCallBack<UpdateBehavior<T>>(prefab.GetType());
            if (old == null)
            {
                UpdateBehavior<T> newUI = TrashMan.normalSpawn(prefab, transform);
                newUI.setData(data);
                return newUI;
            }
            else
            {
                Logger.LogError("already have");
                return old;
            }
        }
        else
        {
            Logger.LogError("transform or prefab null");
            return null;
        }
    }

    #region instantiate

    /*public static UpdateBehavior<T> Instantiate<T>(T data, UpdateBehavior<T> prefab, Transform transform) where T : Data
    {
        if (data != null)
        {
            if (transform != null && prefab != null)
            {
                UIBehavior<T> old = data.findCallBack<UIBehavior<T>>(prefab.GetType());
                if (old == null)
                {
                    UIBehavior<T> newUI = TrashMan.normalSpawn(prefab, transform);
                    newUI.setData(data);
                    data.uiGameObject = newUI.gameObject;
                    // haveTransformData
                    {
                        if (newUI is HaveTransformData)
                        {
                            data.haveTransformData = (HaveTransformData)newUI;
                        }
                    }
                    return newUI;
                }
                else
                {
                    Debug.LogError("already have");
                    return old;
                }
            }
            else
            {
                Debug.LogError("transform or prefab null");
                return null;
            }
        }
        else
        {
            Debug.LogError("error, Instantiate: why data null");
            return null;
        }
    }*/

    #endregion

}