using System.Collections;
using System.Collections.Generic;
using BattleRushS.StateS.LoadS;
using UnityEngine;

namespace BattleRushS.StateS
{
    public class LoadUI : UIBehavior<LoadUI.UIData>
    {

        #region UIData

        public class UIData : BattleRushUI.UIData.StateUI
        {
            public VO<ReferenceData<Load>> load;

            #region sub

            public abstract class Sub : Data
            {
                public abstract Load.Sub.Type getType();
            }

            public VD<Sub> sub;

            #endregion

            #region Constructor

            public enum Property
            {
                load,
                sub
            }

            public UIData() : base()
            {
                this.load = new VO<ReferenceData<Load>>(this, (byte)Property.load, new ReferenceData<Load>(null));
                this.sub = new VD<Sub>(this, (byte)Property.sub, null);
            }

            public override BattleRush.State.Type getType()
            {
                return BattleRush.State.Type.Load;
            }

            #endregion

        }

        #endregion

        #region Refresh

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    Load load = this.data.load.v.data;
                    if (load != null)
                    {
                        // sub
                        {
                            if (load.sub.v != null)
                            {
                                switch (load.sub.v.getType())
                                {
                                    case Load.Sub.Type.ChooseLevel:
                                        {
                                            ChooseLevel chooseLevel = load.sub.v as ChooseLevel;
                                            // makeUI
                                            {
                                                ChooseLevelUI.UIData chooseLevelUIData = this.data.sub.newOrOld<ChooseLevelUI.UIData>();
                                                {
                                                    chooseLevelUIData.chooseLevel.v = new ReferenceData<ChooseLevel>(chooseLevel);
                                                }
                                                this.data.sub.v = chooseLevelUIData;
                                            }
                                        }
                                        break;
                                    case Load.Sub.Type.LoadLevel:
                                        {
                                            LoadLevel loadLevel = load.sub.v as LoadLevel;
                                            // makeUI
                                            {
                                                LoadLevelUI.UIData loadLevelUIData = this.data.sub.newOrOld<LoadLevelUI.UIData>();
                                                {
                                                    loadLevelUIData.loadLevel.v = new ReferenceData<LoadLevel>(loadLevel);
                                                }
                                                this.data.sub.v = loadLevelUIData;
                                            }
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                Logger.LogError("sub null");
                                this.data.sub.v = null;
                            }
                        }
                    }
                    else
                    {
                        Logger.LogError("load null");
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

        public ChooseLevelUI chooseLevelPrefab;
        public LoadLevelUI loadLevelPrefab;

        public override void onAddCallBack<T>(T data)
        {
            if(data is UIData)
            {
                UIData uiData = data as UIData;
                // Child
                {
                    uiData.load.allAddCallBack(this);
                    uiData.sub.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            {
                if (data is Load)
                {
                    dirty = true;
                    return;
                }
                if(data is UIData.Sub){
                    UIData.Sub sub = data as UIData.Sub;
                    // UI
                    {
                        switch (sub.getType())
                        {
                            case Load.Sub.Type.ChooseLevel:
                                {
                                    ChooseLevelUI.UIData chooseLevelUIData = sub as ChooseLevelUI.UIData;
                                    UIUtils.Instantiate(chooseLevelUIData, chooseLevelPrefab, this.transform);
                                }
                                break;
                            case Load.Sub.Type.LoadLevel:
                                {
                                    LoadLevelUI.UIData loadLevelUIData = sub as LoadLevelUI.UIData;
                                    UIUtils.Instantiate(loadLevelUIData, loadLevelPrefab, this.transform);
                                }
                                break;
                        }
                    }
                    dirty = true;
                    return;
                }
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if (data is UIData)
            {
                UIData uiData = data as UIData;
                // Child
                {
                    uiData.load.allRemoveCallBack(this);
                    uiData.sub.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // Child
            {
                if (data is Load)
                {
                    return;
                }
                if (data is UIData.Sub)
                {
                    UIData.Sub sub = data as UIData.Sub;
                    // UI
                    {
                        switch (sub.getType())
                        {
                            case Load.Sub.Type.ChooseLevel:
                                {
                                    ChooseLevelUI.UIData chooseLevelUIData = sub as ChooseLevelUI.UIData;
                                    chooseLevelUIData.removeCallBackAndDestroy(typeof(ChooseLevelUI));
                                }
                                break;
                            case Load.Sub.Type.LoadLevel:
                                {
                                    LoadLevelUI.UIData loadLevelUIData = sub as LoadLevelUI.UIData;
                                    loadLevelUIData.removeCallBackAndDestroy(typeof(LoadLevelUI));
                                }
                                break;
                        }
                    }
                    return;
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
            if (wrapProperty.p is UIData)
            {
                switch ((UIData.Property)wrapProperty.n)
                {
                    case UIData.Property.load:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case UIData.Property.sub:
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
                if (wrapProperty.p is Load)
                {
                    switch ((Load.Property)wrapProperty.n)
                    {
                        case Load.Property.sub:
                            dirty = true;
                            break;
                        default:
                            Logger.LogError("Don't process: " + wrapProperty + "; " + this);
                            break;
                    }
                    return;
                }
                if (wrapProperty.p is UIData.Sub)
                {
                    return;
                }
            }
            Logger.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

    }
}
