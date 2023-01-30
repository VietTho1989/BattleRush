using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.StateS
{
    public class StartUI : UIBehavior<StartUI.UIData>
    {

        #region UIData

        public class UIData : BattleRushUI.UIData.StateUI
        {

            public VO<ReferenceData<Start>> start;

            #region Constructor

            public enum Property
            {
                start
            }

            public UIData() : base()
            {
                this.start = new VO<ReferenceData<Start>>(this, (byte)Property.start, new ReferenceData<Start>(null));
            }

            #endregion

            public override BattleRush.State.Type getType()
            {
                return BattleRush.State.Type.Start;
            }
        }

        #endregion

        #region Refresh

        public GameObject[] contents;

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    Start start = this.data.start.v.data;
                    if (start != null)
                    {
                        // content show or not
                        if(contents!=null)
                        {
                            // find
                            bool show = true;
                            {
                                //show troops
                                if(show)
                                {
                                    BattleRushUI.UIData battleRushUIData = this.data.findDataInParent<BattleRushUI.UIData>();
                                    if (battleRushUIData != null)
                                    {
                                        MainCanvasUI.UIData mainCanvasUIData = battleRushUIData.mainCanvas.v;
                                        if (mainCanvasUIData != null)
                                        {
                                            if (mainCanvasUIData.troopGrid.v != null)
                                            {
                                                show = false;
                                            }
                                        }
                                        else
                                        {
                                            Logger.LogError("mainCanvasUIData null");
                                        }
                                    }
                                    else
                                    {
                                        Logger.LogError("battleRushUIData null");
                                    }
                                }
                            }
                            // process
                            foreach(GameObject content in contents)
                            {
                                if (content)
                                {
                                    content.SetActive(show);
                                }
                            }
                        }
                        else
                        {
                            Logger.LogError("contents null");
                        }
                    }
                    else
                    {
                        Logger.LogError("start null");
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

        private BattleRushUI.UIData battleRushUIData = null;
        public override void onAddCallBack<T>(T data)
        {
            if (data is UIData)
            {
                UIData uiData = data as UIData;
                // Parent
                {
                    DataUtils.addParentCallBack(uiData, this, ref this.battleRushUIData);
                }
                // Child
                {
                    uiData.start.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Parent
            {
                if(data is BattleRushUI.UIData)
                {
                    BattleRushUI.UIData battleRushUIData = data as BattleRushUI.UIData;
                    // Child
                    {
                        battleRushUIData.mainCanvas.allAddCallBack(this);
                    }
                    dirty = true;
                    return;
                }
                // Child
                if(data is MainCanvasUI.UIData)
                {
                    dirty = true;
                    return;
                }
            }
            // Child
            if (data is Start)
            {
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if (data is UIData)
            {
                UIData uiData = data as UIData;
                // Parent
                {
                    DataUtils.removeParentCallBack(uiData, this, ref this.battleRushUIData);
                }
                // Child
                {
                    uiData.start.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // Parent
            {
                if (data is BattleRushUI.UIData)
                {
                    BattleRushUI.UIData battleRushUIData = data as BattleRushUI.UIData;
                    // Child
                    {
                        battleRushUIData.mainCanvas.allRemoveCallBack(this);
                    }
                    return;
                }
                // Child
                if (data is MainCanvasUI.UIData)
                {
                    return;
                }
            }
            // Child
            if (data is Start)
            {
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
            if (wrapProperty.p is UIData)
            {
                switch ((UIData.Property)wrapProperty.n)
                {
                    case UIData.Property.start:
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
            // Parent
            {
                if (wrapProperty.p is BattleRushUI.UIData)
                {
                    switch ((BattleRushUI.UIData.Property)wrapProperty.n)
                    {
                        case BattleRushUI.UIData.Property.mainCanvas:
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
                if (wrapProperty.p is MainCanvasUI.UIData)
                {
                    switch ((MainCanvasUI.UIData.Property)wrapProperty.n)
                    {
                        case MainCanvasUI.UIData.Property.troopGrid:
                            dirty = true;
                            break;
                        default:
                            break;
                    }
                    return;
                }
            }
            // Child
            if (wrapProperty.p is Start)
            {
                switch ((Start.Property)wrapProperty.n)
                {
                    default:
                        Logger.LogError("Don't process: " + wrapProperty + "; " + this);
                        break;
                }
                return;
            }
            Logger.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

    }
}
