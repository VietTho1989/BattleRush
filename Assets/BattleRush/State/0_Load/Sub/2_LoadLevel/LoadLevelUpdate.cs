using System.Collections;
using System.Collections.Generic;
using Dreamteck.Forever;
using UnityEngine;

namespace BattleRushS.StateS.LoadS
{
    public class LoadLevelUpdate : UpdateBehavior<LoadLevel>
    {

        #region Update

        public override void update()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    switch (this.data.state.v)
                    {
                        case LoadLevel.State.Start:
                            {
                                // find mapData
                                MapData mapData = null;
                                {
                                    switch (this.data.sub.v.getType())
                                    {
                                        case LoadLevel.Sub.Type.Excel:
                                            {
                                                LoadLevelByFileExcel loadLevelByFileExcel = this.data.sub.v as LoadLevelByFileExcel;
                                                if (loadLevelByFileExcel.step.v != null)
                                                {
                                                    switch (loadLevelByFileExcel.step.v.getType())
                                                    {
                                                        case LoadLevelByFileExcel.Step.Type.Internet:
                                                            break;
                                                        case LoadLevelByFileExcel.Step.Type.Local:
                                                            {
                                                                LoadLocal loadLocal = loadLevelByFileExcel.step.v as LoadLocal;
                                                                switch (loadLocal.state.v.getType())
                                                                {
                                                                    case LoadLocal.State.Type.Load:
                                                                        {

                                                                        }
                                                                        break;
                                                                    case LoadLocal.State.Type.Success:
                                                                        {
                                                                            LoadLocal.State.Success success = loadLocal.state.v as LoadLocal.State.Success;
                                                                            mapData = success.mapData.v.data;
                                                                        }
                                                                        break;
                                                                    case LoadLocal.State.Type.Fail:
                                                                        {
                                                                            Logger.LogError("load fail");
                                                                            Toast.showMessage("Load level fail");
                                                                        }
                                                                        break;
                                                                }
                                                            }
                                                            break;
                                                    }
                                                }
                                                else
                                                {
                                                    Logger.LogError("loadLevelByFileExcel null");
                                                }
                                            }
                                            break;
                                        case LoadLevel.Sub.Type.ScriptableObject:
                                            {
                                                LoadLevelByScriptableObject loadLevelByScriptableObject = this.data.sub.v as LoadLevelByScriptableObject;
                                                mapData = loadLevelByScriptableObject.mapData.v.data;
                                            }
                                            break;
                                        default:
                                            Logger.LogError("unknown type: " + this.data.sub.v.getType());
                                            break;
                                    }
                                }
                                // process
                                if (mapData != null)
                                {
                                    {
                                        // change state
                                        BattleRush battleRush = this.data.findDataInParent<BattleRush>();
                                        if (battleRush != null)
                                        {
                                            // map
                                            {
                                                {
                                                    mapData.uid = battleRush.mapData.makeId();
                                                }
                                                battleRush.mapData.v = mapData;
                                            }
                                            // levelGenerator
                                            {
                                                BattleRushUI battleRushUI = battleRush.findCallBack<BattleRushUI>();
                                                if (battleRushUI != null)
                                                {
                                                    LevelGenerator levelGenerator = battleRushUI.GetComponent<LevelGenerator>();
                                                    if (levelGenerator != null)
                                                    {
                                                        Logger.Log("LevelGenerator start");
                                                        // set custom path
                                                        {
                                                            if (levelGenerator.pathGenerator is MyCustomPath)
                                                            {
                                                                MyCustomPath myCustomPath = levelGenerator.pathGenerator as MyCustomPath;
                                                                myCustomPath.battleRush = battleRush;
                                                                // make map manager
                                                                {
                                                                    battleRush.makeSegmentManager.v.mapAsset.v = battleRushUI.level_0;
                                                                }
                                                            }
                                                        }
                                                        levelGenerator.StartGeneration(() =>
                                                        {
                                                            if (this.data != null)
                                                            {
                                                                this.data.state.v = LoadLevel.State.Ready;
                                                            }
                                                            else
                                                            {
                                                                Logger.LogError("data null");
                                                            }
                                                        }
                                                            );
                                                    }
                                                    else
                                                    {
                                                        Logger.LogError("levelGenerator null");
                                                    }
                                                }
                                                else
                                                {
                                                    Logger.LogError("battleRushUI null");
                                                }
                                            }
                                            // state
                                            {
                                                this.data.state.v = LoadLevel.State.WaitReady;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Logger.LogError("mapData null");
                                }                             
                            }
                            break;
                        case LoadLevel.State.WaitReady:
                            {

                            }
                            break;
                        case LoadLevel.State.Ready:
                            {
                                BattleRush battleRush = this.data.findDataInParent<BattleRush>();
                                if (battleRush != null)
                                {
                                    Start start = battleRush.state.newOrOld<Start>();
                                    {

                                    }
                                    battleRush.state.v = start;
                                }
                                else
                                {
                                    Logger.LogError("battleRush null");
                                }
                            }
                            break;
                        default:
                            Logger.LogError("unknown state: " + this.data.state.v);
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
            return true;
        }

        #endregion

        #region implement callBacks

        public override void onAddCallBack<T>(T data)
        {
            if (data is LoadLevel)
            {
                LoadLevel loadLevel = data as LoadLevel;
                // Child
                {
                    loadLevel.sub.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            {
                if(data is LoadLevel.Sub)
                {
                    LoadLevel.Sub sub = data as LoadLevel.Sub;
                    // Child
                    {
                        switch (sub.getType())
                        {
                            case LoadLevel.Sub.Type.Excel:
                                {
                                    LoadLevelByFileExcel loadLevelByFileExcel = data as LoadLevelByFileExcel;
                                    // Update
                                    {
                                        UpdateUtils.makeUpdate<LoadLevelByFileExcelUpdate, LoadLevelByFileExcel>(loadLevelByFileExcel, this.transform);
                                    }
                                    // Child
                                    {
                                        loadLevelByFileExcel.step.allAddCallBack(this);
                                    }
                                }
                                break;
                            case LoadLevel.Sub.Type.ScriptableObject:
                                {
                                    LoadLevelByScriptableObject loadLevelByScriptableObject = data as LoadLevelByScriptableObject;
                                    // Update
                                    {
                                        UpdateUtils.makeUpdate<LoadLevelByScriptableObjectUpdate, LoadLevelByScriptableObject>(loadLevelByScriptableObject, this.transform);
                                    }
                                }
                                break;
                            default:
                                Logger.LogError("unknown type: " + sub.getType());
                                break;
                        }
                    }
                    dirty = true;
                    return;
                }
                // Child
                {
                    if (data is LoadLevelByFileExcel.Step)
                    {
                        LoadLevelByFileExcel.Step step = data as LoadLevelByFileExcel.Step;
                        // Child
                        {
                            switch (step.getType())
                            {
                                case LoadLevelByFileExcel.Step.Type.Internet:
                                    break;
                                case LoadLevelByFileExcel.Step.Type.Local:
                                    {
                                        LoadLocal loadLocal = step as LoadLocal;
                                        loadLocal.state.allAddCallBack(this);
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
                    // Child
                    if (data is LoadLocal.State)
                    {
                        dirty = true;
                        return;
                    }
                }                
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if (data is LoadLevel)
            {
                LoadLevel loadLevel = data as LoadLevel;
                // Child
                {
                    loadLevel.sub.allRemoveCallBack(this);
                }
                this.setDataNull(loadLevel);
                return;
            }
            // Child
            {
                if (data is LoadLevel.Sub)
                {
                    LoadLevel.Sub sub = data as LoadLevel.Sub;
                    // Child
                    {
                        switch (sub.getType())
                        {
                            case LoadLevel.Sub.Type.Excel:
                                {
                                    LoadLevelByFileExcel loadLevelByFileExcel = data as LoadLevelByFileExcel;
                                    // Update
                                    {
                                        loadLevelByFileExcel.removeCallBackAndDestroy(typeof(LoadLevelByFileExcelUpdate));
                                    }
                                    // Child
                                    {
                                        loadLevelByFileExcel.step.allRemoveCallBack(this);
                                    }
                                }
                                break;
                            case LoadLevel.Sub.Type.ScriptableObject:
                                {
                                    LoadLevelByScriptableObject loadLevelByScriptableObject = data as LoadLevelByScriptableObject;
                                    // Update
                                    {
                                        loadLevelByScriptableObject.removeCallBackAndDestroy(typeof(LoadLevelByScriptableObjectUpdate));
                                    }
                                }
                                break;
                            default:
                                Logger.LogError("unknown type: " + sub.getType());
                                break;
                        }
                    }
                    return;
                }
                // Child
                {
                    if (data is LoadLevelByFileExcel.Step)
                    {
                        LoadLevelByFileExcel.Step step = data as LoadLevelByFileExcel.Step;
                        // Child
                        {
                            switch (step.getType())
                            {
                                case LoadLevelByFileExcel.Step.Type.Internet:
                                    break;
                                case LoadLevelByFileExcel.Step.Type.Local:
                                    {
                                        LoadLocal loadLocal = step as LoadLocal;
                                        loadLocal.state.allRemoveCallBack(this);
                                    }
                                    break;
                                default:
                                    Logger.LogError("unknown type: " + step.getType());
                                    break;
                            }
                        }
                        return;
                    }
                    // Child
                    if (data is LoadLocal.State)
                    {
                        return;
                    }
                }
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onUpdateSync<T>(WrapProperty wrapProperty, List<Sync<T>> syncs)
        {
            if (WrapProperty.checkError(wrapProperty))
            {
                return;
            }
            if (wrapProperty.p is LoadLevel)
            {
                switch ((LoadLevel.Property)wrapProperty.n)
                {
                    case LoadLevel.Property.level:
                        dirty = true;
                        break;
                    case LoadLevel.Property.sub:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case LoadLevel.Property.state:
                        dirty = true;
                        break;
                    default:
                        Logger.LogError("Don't process: " + wrapProperty + "; " + this);
                        break;
                }
                return;
            }
            // Child
            {
                if (wrapProperty.p is LoadLevel.Sub)
                {
                    LoadLevel.Sub sub = wrapProperty.p as LoadLevel.Sub;
                    // Child
                    {
                        switch (sub.getType())
                        {
                            case LoadLevel.Sub.Type.Excel:
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
                                            break;
                                    }
                                }
                                break;
                            case LoadLevel.Sub.Type.ScriptableObject:
                                {
                                    switch ((LoadLevelByScriptableObject.Property)wrapProperty.n)
                                    {
                                        case LoadLevelByScriptableObject.Property.mapData:
                                            dirty = true;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            default:
                                Logger.LogError("unknown type: " + sub.getType());
                                break;
                        }
                    }
                    return;
                }
                // Child
                {
                    if (wrapProperty.p is LoadLevelByFileExcel.Step)
                    {
                        LoadLevelByFileExcel.Step step = wrapProperty.p as LoadLevelByFileExcel.Step;
                        switch (step.getType())
                        {
                            case LoadLevelByFileExcel.Step.Type.Internet:
                                break;
                            case LoadLevelByFileExcel.Step.Type.Local:
                                {
                                    switch ((LoadLocal.Property)wrapProperty.n)
                                    {
                                        case LoadLocal.Property.state:
                                            {
                                                ValueChangeUtils.replaceCallBack(this, syncs);
                                                dirty = true;
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                        }
                        return;
                    }
                    // Child
                    if (wrapProperty.p is LoadLocal.State)
                    {
                        LoadLocal.State state = wrapProperty.p as LoadLocal.State;
                        switch (state.getType())
                        {
                            case LoadLocal.State.Type.Load:
                                break;
                            case LoadLocal.State.Type.Success:
                                {
                                    switch ((LoadLocal.State.Success.Property)wrapProperty.n)
                                    {
                                        case LoadLocal.State.Success.Property.mapData:
                                            dirty = true;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case LoadLocal.State.Type.Fail:
                                break;
                            default:
                                Logger.LogError("unknown type: " + state.getType());
                                break;
                        }
                        return;
                    }
                }               
            }
            Logger.LogError("Don't process: " + data + "; " + syncs + "; " + this);
        }

        #endregion

    }
}
