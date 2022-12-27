using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using AdvancedCoroutines;
using UnityEngine.Networking;

namespace BattleRushS.StateS.LoadS
{
    public class LoadInternetUpdate : UpdateBehavior<LoadInternet>
    {

        #region Update

        public override void update()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    startRoutine(ref this.task, TaskLoad());
                }
                else
                {
                    Logger.LogError("data null");
                }
            }
        }

        public override bool isShouldDisableUpdate()
        {
            return false;
        }

        #endregion

        #region task

        private Routine task;

        public IEnumerator TaskLoad()
        {
            if (this.data != null)
            {
                bool success = true;
                // find internet link
                string internetLink = "";
                {
                    // TODO can hoan thien
                }
                if (!string.IsNullOrEmpty(internetLink))
                {
                    // load beat file
                    {
                        if (!string.IsNullOrEmpty(internetLink))
                        {
                            UnityWebRequest www = UnityWebRequest.Get(internetLink);
                            www.timeout = LoadInternet.TimeOut;
                            yield return www.SendWebRequest();

                            if (www.isNetworkError || www.isHttpError)
                            {
                                success = false;
                                Logger.LogError("load file beat error: " + www.error);
                            }
                            else
                            {
                                // save to excel
                                try
                                {
                                    // in folder resources
                                    {
#if UNITY_EDITOR
                                        string path = Path.Combine(Application.dataPath, "Resources", "Internet" + LoadLevelByFileExcel.FileLevel);
                                        File.WriteAllText(path, www.downloadHandler.text);
#endif
                                    }
                                    // persistentDataPath
                                    {
                                        string path = Path.Combine(Application.persistentDataPath, "Internet" + LoadLevelByFileExcel.FileLevel);
                                        File.WriteAllText(path, www.downloadHandler.text);
                                    }
                                }
                                catch (Exception e)
                                {
                                    Logger.LogError(e);
                                }
                            }
                        }
                        else
                        {
                            Logger.LogError("beatLink empty");
                        }
                    }
                    // load music file
                    {
                        // TODO Can hoan thien
                    }
                }
                else
                {
                    Logger.LogError("internet null");
                }
                // change to load local
                {
                    LoadLevelByFileExcel load = this.data.findDataInParent<LoadLevelByFileExcel>();
                    if (load != null)
                    {
                        LoadLocal loadLocal = new LoadLocal();
                        {
                            loadLocal.uid = load.step.makeId();
                        }
                        load.step.v = loadLocal;
                    }
                    else
                    {
                        Logger.LogError("load null");
                    }
                }
                Logger.LogError("load internet success: " + success);
            }              
        }

        public override List<Routine> getRoutineList()
        {
            List<Routine> ret = new List<Routine>();
            {
                ret.Add(task);
            }
            return ret;
        }

        #endregion

        #region implement callBacks

        public override void onAddCallBack<T>(T data)
        {
            if(data is LoadInternet)
            {
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if(data is LoadInternet)
            {
                LoadInternet loadInternet = data as LoadInternet;
                this.setDataNull(loadInternet);
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onUpdateSync<T>(WrapProperty wrapProperty, List<Sync<T>> syncs)
        {
            if (WrapProperty.checkError(wrapProperty))
            {
                return;
            }
            if(wrapProperty.p is LoadInternet)
            {
                switch ((LoadInternet.Property)wrapProperty.n)
                {
                    default:
                        Logger.LogError("Don't process: " + wrapProperty + ", " + this);
                        break;
                }
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + syncs + "; " + this);
        }

        #endregion

    }
}