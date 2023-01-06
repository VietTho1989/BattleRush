using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using AdvancedCoroutines;
using System.IO;

namespace BattleRushS.StateS.LoadS
{
    public class LoadLocalUpdate : UpdateBehavior<LoadLocal>
    {

        #region Update

        public override void update()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    switch (this.data.state.v.getType())
                    {
                        case LoadLocal.State.Type.Load:
                            {
                                startRoutine(ref this.loadDataByFileExcel, TaskLoadDataByFileExcel());
                            }
                            break;
                        case LoadLocal.State.Type.Success:
                            {
                                destroyRoutine(this.loadDataByFileExcel);
                            }
                            break;
                        case LoadLocal.State.Type.Fail:
                            {
                                destroyRoutine(this.loadDataByFileExcel);
                            }
                            break;
                    }
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

        #region Routine Load Data By File Excel

        private Routine loadDataByFileExcel;

        public IEnumerator TaskLoadDataByFileExcel()
        {
            yield return new Wait();
            // find
            bool isSuccess = false;
            MapData mapData = new MapData();
            {
                try
                {
                    // load file excel
                    ES3Spreadsheet list = new ES3Spreadsheet();
                    {
                        // load file
                        {
                            // get path
                            string path = path = Path.Combine(Application.dataPath, "Resources", LoadLevelByFileExcel.FileLevel); ;// Path.Combine(Application.persistentDataPath, LoadLevelByFileExcel.FileLevel);
                            {

                            }
                            Logger.LogError("load excel file: " + path);
                            // TODO Test textAsset
                            {
                                if (Logger.IsEditor())
                                {
                                    TextAsset textAsset = Resources.Load<TextAsset>(path);
                                    if (textAsset != null)
                                    {
                                        Logger.LogError("load textAsset success: " + textAsset.text);
                                    }
                                    else
                                    {
                                        Logger.LogError("load textAsset error: " + textAsset);
                                    }
                                }                                
                            }
                            // make settings
                            ES3Settings settings = new ES3Settings();
                            {
                                settings.location = ES3.Location.File;// .Resources;
                            }
                            list.Load(path, settings);
                        }
                        // process file
                        {
                            // get level
                            int level = 0;
                            {
                                LoadLevel loadLevel = this.data.findDataInParent<LoadLevel>();
                                if (loadLevel != null)
                                {
                                    level = loadLevel.level.v;
                                }
                                else
                                {
                                    Logger.LogError("loadLevel null");
                                }
                                level = Math.Clamp(level, 0, list.RowCount - 1);
                            }
                            // process
                            if(level>=0 && level < list.RowCount)
                            {
                                // items
                                {
                                    String strItem = list.GetCell<string>(8, level + 1);
                                    Logger.Log("strItem: " + strItem);
                                    mapData.parse(strItem);
                                }
                                // special items
                                {
                                    String strItem = list.GetCell<string>(7, level + 1);
                                    Logger.Log("strSpecialItem: " + strItem);
                                    mapData.parse(strItem);
                                }
                            }
                        }
                    }
                    isSuccess = true;
                }
                catch (Exception e)
                {
                    Logger.LogError(e);
                }
            }
            // process
            {
                if (this.data != null)
                {
                    if (isSuccess)
                    {
                        LoadLocal.State.Success success = this.data.state.newOrOld<LoadLocal.State.Success>();
                        {
                            success.mapData.v = new ReferenceData<MapData>(mapData);
                        }
                        this.data.state.v = success;
                    }
                    else
                    {
                        LoadLocal.State.Fail fail = this.data.state.newOrOld<LoadLocal.State.Fail>();
                        {

                        }
                        this.data.state.v = fail;
                    }
                }
            }
        }

        #endregion

        #region implement callBacks

        public override void onAddCallBack<T>(T data)
        {
            if(data is LoadLocal)
            {
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if(data is LoadLocal)
            {
                LoadLocal loadLocal = data as LoadLocal;
                this.setDataNull(loadLocal);
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
            if(wrapProperty.p is LoadLocal)
            {
                switch ((LoadLocal.Property)wrapProperty.n)
                {
                    default:
                        Logger.LogError("Don't process: " + wrapProperty + "; " + this);
                        break;
                }
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + syncs + "; " + this);
        }

        #endregion

    }
}