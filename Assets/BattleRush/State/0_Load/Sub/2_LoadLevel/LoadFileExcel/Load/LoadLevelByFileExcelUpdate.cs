using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AdvancedCoroutines;
using System;

namespace BattleRushS.StateS.LoadS
{
    public class LoadLevelByFileExcelUpdate : UpdateBehavior<LoadLevelByFileExcel>
    {

        #region Update

        public override void update()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    if (this.data.step.v == null)
                    {
                        // find
                        bool needLoadInternet = false;
                        {
                            // TODO can hoan thien
                            needLoadInternet = false;
                        }
                        // process
                        if (needLoadInternet)
                        {
                            LoadInternet loadInternet = new LoadInternet();
                            {
                                loadInternet.uid = this.data.step.makeId();
                            }
                            this.data.step.v = loadInternet;
                        }
                        else
                        {
                            LoadLocal loadLocal = new LoadLocal();
                            {
                                loadLocal.uid = this.data.step.makeId();
                            }
                            this.data.step.v = loadLocal;
                        }
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
            return true;
        }

        #endregion

        #region implement callBacks

        public override void onAddCallBack<T>(T data)
        {
            if(data is LoadLevelByFileExcel)
            {
                LoadLevelByFileExcel load = data as LoadLevelByFileExcel;
                // Child
                {
                    load.step.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            if(data is LoadLevelByFileExcel.Step)
            {
                LoadLevelByFileExcel.Step step = data as LoadLevelByFileExcel.Step;
                // Update
                {
                    switch (step.getType())
                    {
                        case LoadLevelByFileExcel.Step.Type.Internet:
                            {
                                LoadInternet loadInternet = step as LoadInternet;
                                UpdateUtils.makeUpdate<LoadInternetUpdate, LoadInternet>(loadInternet, this.transform);
                            }
                            break;
                        case LoadLevelByFileExcel.Step.Type.Local:
                            {
                                LoadLocal loadLocal = step as LoadLocal;
                                UpdateUtils.makeUpdate<LoadLocalUpdate, LoadLocal>(loadLocal, this.transform);
                            }
                            break;
                        default:
                            Logger.LogError("unknown type: " + step.getType());
                            break;
                    }
                }
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if(data is LoadLevelByFileExcel)
            {
                LoadLevelByFileExcel load = data as LoadLevelByFileExcel;
                // Child
                {
                    load.step.allRemoveCallBack(this);
                }
                this.setDataNull(load);
                return;
            }
            // Child
            if (data is LoadLevelByFileExcel.Step)
            {
                LoadLevelByFileExcel.Step step = data as LoadLevelByFileExcel.Step;
                // Update
                {
                    switch (step.getType())
                    {
                        case LoadLevelByFileExcel.Step.Type.Internet:
                            {
                                LoadInternet loadInternet = step as LoadInternet;
                                loadInternet.removeCallBackAndDestroy(typeof(LoadInternetUpdate));
                            }
                            break;
                        case LoadLevelByFileExcel.Step.Type.Local:
                            {
                                LoadLocal loadLocal = step as LoadLocal;
                                loadLocal.removeCallBackAndDestroy(typeof(LoadLocalUpdate));
                            }
                            break;
                        default:
                            Logger.LogError("unknown type: " + step.getType());
                            break;
                    }
                }
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
            if(wrapProperty.p is LoadLevelByFileExcel)
            {
                switch ((LoadLevelByFileExcel.Property)wrapProperty.n)
                {
                    case LoadLevelByFileExcel.Property.step:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    default:
                        Logger.LogError("Don't process: " + wrapProperty + "; " + this);
                        break;
                }
                return;
            }
            // Child
            if (wrapProperty.p is LoadLevelByFileExcel.Step)
            {
                return;
            }
            Logger.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

    }
}