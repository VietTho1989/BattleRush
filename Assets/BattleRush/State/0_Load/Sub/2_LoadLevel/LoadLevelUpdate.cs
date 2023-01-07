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
                    if (this.data.loadLevelByFileExcel.v.step.v != null)
                    {
                        switch (this.data.loadLevelByFileExcel.v.step.v.getType())
                        {
                            case LoadLevelByFileExcel.Step.Type.Internet:
                                break;
                            case LoadLevelByFileExcel.Step.Type.Local:
                                {
                                    LoadLocal loadLocal = this.data.loadLevelByFileExcel.v.step.v as LoadLocal;
                                    switch (loadLocal.state.v.getType())
                                    {
                                        case LoadLocal.State.Type.Load:
                                            {

                                            }
                                            break;
                                        case LoadLocal.State.Type.Success:
                                            {
                                                LoadLocal.State.Success success = loadLocal.state.v as LoadLocal.State.Success;
                                                switch (success.state.v)
                                                {
                                                    case LoadLocal.State.Success.State.Start:
                                                        {
                                                            // change state
                                                            BattleRush battleRush = this.data.findDataInParent<BattleRush>();
                                                            if (battleRush != null)
                                                            {
                                                                // map
                                                                {
                                                                    MapData mapData = success.mapData.v.data;
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
                                                                                success.state.v = LoadLocal.State.Success.State.Ready;
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
                                                                    success.state.v = LoadLocal.State.Success.State.WaitReady;                                                                   
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    case LoadLocal.State.Success.State.WaitReady:
                                                        {

                                                        }
                                                        break;
                                                    case LoadLocal.State.Success.State.Ready:
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
                                                        Logger.LogError("unknown state: " + success.state.v);
                                                        break;
                                                }                                                
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
                    loadLevel.loadLevelByFileExcel.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            {
                if (data is LoadLevelByFileExcel)
                {
                    LoadLevelByFileExcel loadLevelByFileExcel = data as LoadLevelByFileExcel;
                    // Update
                    {
                        UpdateUtils.makeUpdate<LoadLevelByFileExcelUpdate, LoadLevelByFileExcel>(loadLevelByFileExcel, this.transform);
                        loadLevelByFileExcel.step.allAddCallBack(this);
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
                    if(data is LoadLocal.State)
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
                    loadLevel.loadLevelByFileExcel.allRemoveCallBack(this);
                }
                this.setDataNull(loadLevel);
                return;
            }
            // Child
            {
                if (data is LoadLevelByFileExcel)
                {
                    LoadLevelByFileExcel loadLevelByFileExcel = data as LoadLevelByFileExcel;
                    // Update
                    {
                        loadLevelByFileExcel.removeCallBackAndDestroy(typeof(LoadLevelByFileExcelUpdate));
                        loadLevelByFileExcel.step.allRemoveCallBack(this);
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
                    case LoadLevel.Property.loadLevelByFileExcel:
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
            {
                if (wrapProperty.p is LoadLevelByFileExcel)
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
                                        case LoadLocal.State.Success.Property.state:
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
