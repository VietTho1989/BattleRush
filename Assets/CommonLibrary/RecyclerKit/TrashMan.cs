using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TrashMan : MonoBehaviour
{

    #region instance

    /// <summary>
    /// access to the singleton
    /// </summary>
    // public static TrashMan instance;

    #endregion

    /// <summary>
    /// stores the recycle bins and is used to populate the lookup Dictionaries at startup
    /// </summary>
    [HideInInspector]
    public List<TrashManRecycleBin> recycleBinCollection;

    /// <summary>
    /// this is how often in seconds TrashMan should cull excess objects. Setting this to 0 or a negative number will
    /// fully turn off automatic culling. You can then use the TrashManRecycleBin.cullExcessObjects method manually if
    /// you would still like to do any culling.
    /// </summary>
    public float cullExcessObjectsInterval = 10f;

    /// <summary>
    /// if true, DontDestroyOnLoad will be called on the TrashMan
    /// </summary>
    public bool persistBetweenScenes = false;

    /// <summary>
    /// uses the GameObject instanceId as its key for fast look-ups
    /// </summary>
    Dictionary<int, TrashManRecycleBin> _instanceIdToRecycleBin = new Dictionary<int, TrashManRecycleBin>();

    [HideInInspector]
    public new Transform transform;

    #region MonoBehaviour

    // public SettingPref settingPref;

    void Awake()
    {
        DontDestroyOnLoad(this);
        // Debug.unityLogger.logEnabled = false;
        // Debug.developerConsoleVisible = false;
        Logger.LogError("TrashMan Awake");
        /*if (settingPref != null)
        {
            settingPref.MyAwake();
        }
        else
        {
            Debug.LogError("settingPref null");
        }*/
        {
            RecyclerKit.instance = this;
            transform = gameObject.transform;
            initializePrefabPools();

            if (persistBetweenScenes)
                DontDestroyOnLoad(gameObject);
        }

        // only cull if we have an interval greater than 0
        if (cullExcessObjectsInterval > 0)
            StartCoroutine(cullExcessObjects());

        SceneManager.activeSceneChanged += activeSceneChanged;
    }

    void activeSceneChanged(Scene oldScene, Scene newScene)
    {
        if (oldScene.name == null)
            return;

        for (var i = recycleBinCollection.Count - 1; i >= 0; i--)
        {
            if (!recycleBinCollection[i].persistBetweenScenes)
                removeRecycleBin(recycleBinCollection[i]);
        }
    }

    void OnApplicationQuit()
    {
        // Co le ko can
    }

    #endregion

    #region Private

    /// <summary>
    /// coroutine that runs every couple seconds and removes any objects created over the recycle bins limit
    /// </summary>
    /// <returns>The excess objects.</returns>
    IEnumerator cullExcessObjects()
    {
        var waiter = new WaitForSeconds(cullExcessObjectsInterval);

        while (true)
        {
            for (var i = 0; i < recycleBinCollection.Count; i++)
                recycleBinCollection[i].cullExcessObjects();

            yield return waiter;
        }
    }


    /// <summary>
    /// populats the lookup dictionaries
    /// </summary>
    void initializePrefabPools()
    {
        if (recycleBinCollection == null)
            return;
        foreach (var recycleBin in recycleBinCollection)
        {
            if (recycleBin == null || recycleBin.prefab == null)
                continue;
            recycleBin.initialize();
            // _instanceIdToRecycleBin.Add(GetInstanceId(recycleBin.prefab), recycleBin);
            // add
            {
                int instanceId = GetInstanceId(recycleBin.prefab);
                if (instanceId == 0)
                {
                    Logger.LogError("why instanceId 0: " + recycleBin.prefab);
                }
                /*if (_instanceIdToRecycleBin.ContainsKey(instanceId))
                {
                    Debug.LogError("already contain: " + instanceId + ", " + recycleBin.prefab);
                }*/
                _instanceIdToRecycleBin[instanceId] = recycleBin;
            }
        }
    }

    #endregion


    #region Public

    /// <summary>
    /// tells TrashMan to start managing the recycle bin at runtime
    /// </summary>
    /// <param name="recycleBin">Recycle bin.</param>
    public static void manageRecycleBin(TrashManRecycleBin recycleBin)
    {
        if (!RecyclerKit.get()._instanceIdToRecycleBin.ContainsKey(GetInstanceId(recycleBin.prefab)))
        {
            RecyclerKit.get().recycleBinCollection.Add(recycleBin);
            recycleBin.initialize();
            RecyclerKit.get()._instanceIdToRecycleBin.Add(GetInstanceId(recycleBin.prefab), recycleBin);
        }
        else
        {
            Logger.LogError("Cannot manage the recycle bin because there is already a GameObject with the name (" + recycleBin.prefab.name + ") being managed");
        }
    }

    /// <summary>
    /// stops managing the recycle bin optionally destroying all managed objects
    /// </summary>
    /// <param name="recycleBin">Recycle bin.</param>
    /// <param name="shouldDestroyAllManagedObjects">If set to <c>true</c> should destroy all managed objects.</param>
    public static void removeRecycleBin(TrashManRecycleBin recycleBin, bool shouldDestroyAllManagedObjects = true)
    {
        if (RecyclerKit.get()._instanceIdToRecycleBin.ContainsKey(GetInstanceId(recycleBin.prefab)))
        {
            RecyclerKit.get()._instanceIdToRecycleBin.Remove(GetInstanceId(recycleBin.prefab));
            RecyclerKit.get().recycleBinCollection.Remove(recycleBin);
            recycleBin.clearBin(shouldDestroyAllManagedObjects);
        }
        else
        {
            Logger.LogError("removeRecycleBin: why not contains key");
        }
    }

    #endregion

    #region Normal Spawn

    public interface DespawnInterface
    {

        int getInstanceId();

        void setInstanceId(int newInstanceId);

        GameObject getGameObject();

        void onDespawn();

        int getStartAllocate();

    }

    public static int GetInstanceId(GameObject gameObject)
    {
        int ret = 0;
        {
            if (gameObject != null)
            {
                DespawnInterface despawnInterface = gameObject.GetComponent<DespawnInterface>();
                if (despawnInterface != null)
                {
                    ret = despawnInterface.getInstanceId();
                }
                else
                {
                    Logger.LogError("despawnInterface null");
                }
            }
            else
            {
                Logger.LogError("gameObject null");
            }
        }
        return ret;
    }

    public static void normalDespawn(DespawnInterface despawnInterface)
    {
        // Debug.LogError ("despawn: " + despawnInterface);
        if (despawnInterface == null || !RecyclerKit.get())
        {
            Logger.LogError("null error");
            GameObject go = despawnInterface.getGameObject();
            if (go != null)
            {
                Destroy(go);
            }
            else
            {
                Logger.LogError("go null");
            }
        }
        else
        {
            GameObject go = despawnInterface.getGameObject();
            if (go != null)
            {
                int instanceId = despawnInterface.getInstanceId();
                if (instanceId != 0)
                {
                    // despawn
                    TrashManRecycleBin recycleBin = null;
                    if (RecyclerKit.get()._instanceIdToRecycleBin.TryGetValue(instanceId, out recycleBin))
                    {
                        // Debug.LogError ("despawn: " + go);
                        // find
                        bool canMake = true;
                        {
                            /*if (despawnInterface is ISRIA)
                            {
                                canMake = false;
                                Debug.LogError("Why ISRIA: " + go);
                            }*/
                        }
                        if (canMake)
                        {
                            recycleBin.despawn(go);
                            go.transform.SetParent(RecyclerKit.get().transform, false);
                            // event
                            despawnInterface.onDespawn();
                        }
                        else
                        {
                            Destroy(go);
                        }
                    }
                    else
                    {
                        Destroy(go);
                    }
                }
                else
                {
                    Destroy(go);
                }
            }
            else
            {
                Logger.LogError("go null: " + despawnInterface);
            }
        }
    }

    public static T normalSpawn<T>(T go, Transform parent) where T : MonoBehaviour
    {
        T ret = null;
        // recycle
        {
            // find instanceId
            int instanceId = GetInstanceId(go.gameObject);
            if (instanceId != 0)
            {
                // find recycleBin
                TrashManRecycleBin recycleBin = null;
                if (RecyclerKit.get() != null)
                {
                    if (RecyclerKit.get()._instanceIdToRecycleBin.TryGetValue(instanceId, out recycleBin))
                    {
                        // find
                        bool canMake = true;
                        {
                            /*if (go is ISRIA)
                            {
                                canMake = false;
                                Debug.LogError("Why ISRIA: " + go);
                            }*/
                        }
                        // process
                        if (canMake)
                        {
                            GameObject newGo = recycleBin.spawn();
                            // Set transform
                            if (newGo != null)
                            {
                                ret = newGo.GetComponent<T>();
                                if (ret != null)
                                {
                                    newGo.SetActive(true);
                                    // Set Position
                                    newGo.transform.SetParent(parent, false);
                                }
                                else
                                {
                                    Logger.LogError("Cannot find behavior: " + newGo + "; " + go + "; " + parent);
                                }
                            }
                            else
                            {
                                // Debug.LogError("newGo null: " + go);
                            }
                        }
                    }
                }
                else
                {
                    Logger.LogError("instance null");
                }
            }
            else
            {
                Logger.LogError("instanceId 0: " + go);
            }
        }
        // Normal
        if (ret == null)
        {
            // Debug.LogError ("attempted to spawn go (" + go.name + ") but there is no recycle bin setup for it. Falling back to Instantiate");
            ret = Instantiate<T>(go, parent, false) as T;
        }
        return ret;
    }

    public static T normalSpawn<T>(T go, Transform parent, Vector3 globalScale) where T : MonoBehaviour
    {
        T ret = null;
        // recycle
        {
            // find instanceId
            int instanceId = GetInstanceId(go.gameObject);
            if (instanceId != 0)
            {
                // find recycleBin
                TrashManRecycleBin recycleBin = null;
                if (RecyclerKit.get() != null)
                {
                    if (RecyclerKit.get()._instanceIdToRecycleBin.TryGetValue(instanceId, out recycleBin))
                    {
                        // find
                        bool canMake = true;
                        {
                            /*if (go is ISRIA)
                            {
                                canMake = false;
                                Debug.LogError("Why ISRIA: " + go);
                            }*/
                        }
                        // process
                        if (canMake)
                        {
                            GameObject newGo = recycleBin.spawn();
                            // Set transform
                            if (newGo != null)
                            {
                                ret = newGo.GetComponent<T>();
                                if (ret != null)
                                {
                                    newGo.SetActive(true);
                                    // Set Position
                                    newGo.transform.SetParent(parent, false);
                                }
                                else
                                {
                                    Logger.LogError("Cannot find behavior: " + newGo + "; " + go + "; " + parent);
                                }
                            }
                            else
                            {
                                // Debug.LogError("newGo null");
                            }
                        }
                    }
                }
                else
                {
                    Logger.LogError("instance null");
                }
            }
            else
            {
                Logger.LogError("instanceId 0: " + go);
            }
        }
        // Normal
        if (ret == null)
        {
            // Debug.LogWarning ("attempted to spawn go (" + go.name + ") but there is no recycle bin setup for it. Falling back to Instantiate");
            ret = Instantiate<T>(go, parent, false) as T;
        }
        // set scale
        {
            if (ret != null)
            {
                UIUtils.SetGlobalScale(ret.transform, globalScale);
            }
            else
            {
                Logger.LogError("ret null");
            }
        }
        return ret;
    }

    #endregion

    #region clear bin by trashManInstanceId

    public void clearBin(List<int> trashManInstanceIds)
    {
        foreach(int instanceId in trashManInstanceIds)
        {
            TrashManRecycleBin recycleBin = null;
            if (RecyclerKit.get() != null)
            {
                if (RecyclerKit.get()._instanceIdToRecycleBin.TryGetValue(instanceId, out recycleBin))
                {
                    recycleBin.clearBin(true);
                }
                else
                {
                    Logger.LogError("why don't find recycleBin");
                }
            }
        }
    }

    #endregion

}