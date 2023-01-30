using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.TheFallenGames.OSA;
using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using BattleRushS.HeroS;

namespace BattleRushS.MainCanvasS
{
    public class TroopGrid : MyGridUI<TroopGrid.UIData, TroopInformation>
    {

        #region UIData

        public class UIData : Data
        {

            public LD<TroopHolder.UIData> holders;

            #region Constructor

            public enum Property
            {
                holders,
            }

            public UIData() : base()
            {
                this.holders = new LD<TroopHolder.UIData>(this, (byte)Property.holders);
            }

            #endregion

        }

        #endregion

        #region implement myGrid interface

        public override void OnCellViewsHolderCreated(MyCellViewsHolder cellVH, CellGroupViewsHolder<MyCellViewsHolder> cellGroup)
        {
            if (this.data != null)
            {
                TroopHolder holder = cellVH.root.GetComponent<TroopHolder>();
                if (holder != null)
                {
                    // set data
                    TroopHolder.UIData holderUIData = new TroopHolder.UIData();
                    {
                        holderUIData.uid = this.data.holders.makeId();
                        holderUIData.viewsHolder = cellVH;
                        // adCreative
                        {
                            if (cellVH.ItemIndex >= 0 && cellVH.ItemIndex < DataList.Count)
                            {
                                holderUIData.troopInformation.v = DataList[cellVH.ItemIndex];
                            }
                            else
                            {
                                Logger.LogError("itemIndex out of range");
                                holderUIData.troopInformation.v = null;
                            }
                        }
                    }
                    // add to parent
                    this.data.holders.add(holderUIData);
                    // set data
                    holder.setData(holderUIData);
                }
                else
                {
                    Logger.LogError("moreGameHolder null");
                }
            }
            else
            {
                Logger.LogError("data null");
            }
        }

        public override void UpdateCellViewsHolder(MyCellViewsHolder viewsHolder)
        {
            if (this.data != null)
            {
                TroopHolder holder = viewsHolder.root.GetComponent<TroopHolder>();
                if (holder != null)
                {
                    if (holder.data != null)
                    {
                        holder.data.viewsHolder = viewsHolder;
                        // adCreative
                        {
                            if (viewsHolder.ItemIndex >= 0 && viewsHolder.ItemIndex < DataList.Count)
                            {
                                holder.data.troopInformation.v = DataList[viewsHolder.ItemIndex];
                            }
                            else
                            {
                                Logger.LogError("ItemIndex out of range");
                                holder.data.troopInformation.v = null;
                            }
                        }
                    }
                    else
                    {
                        Logger.LogError("moreGameHolder data null");
                    }
                }
                else
                {
                    Logger.LogError("moreGameHolder null");
                }
            }
            else
            {
                Logger.LogError("data null");
            }
        }

        #endregion

        #region refresh

        private bool needMoreToRow = false;

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    if (myGrid.IsInitialized)
                    {
                        // update
                        {
                            // get list
                            List<TroopInformation> troopInformations = new List<TroopInformation>();
                            {
                                BattleRushUI.UIData battleRushUIData = this.data.findDataInParent<BattleRushUI.UIData>();
                                if (battleRushUIData != null)
                                {
                                    BattleRushUI battleRushUI = battleRushUIData.findCallBack<BattleRushUI>();
                                    if (battleRushUI != null)
                                    {
                                        troopInformations = battleRushUI.troopInformations;
                                    }
                                    else
                                    {
                                        Logger.LogError("battleRushUI null");
                                    }
                                }
                                else
                                {
                                    Logger.LogError("battleRushUIData null");
                                }
                            }
                            // process
                            int min = Mathf.Min(troopInformations.Count, DataList.Count);
                            // Update
                            {
                                for (int i = 0; i < min; i++)
                                {
                                    if (troopInformations[i] != DataList[i])
                                    {
                                        // change param
                                        DataList.List[i] = troopInformations[i];
                                        // Update holder
                                        foreach (TroopHolder.UIData holder in this.data.holders.vs)
                                        {
                                            if (holder.viewsHolder != null && holder.viewsHolder.ItemIndex == i)
                                            {
                                                holder.troopInformation.v = troopInformations[i];
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            // Add or Remove
                            {
                                if (troopInformations.Count > min)
                                {
                                    // Add
                                    int insertCount = troopInformations.Count - min;
                                    List<TroopInformation> addItems = troopInformations.GetRange(min, insertCount);
                                    DataList.InsertItemsAtEnd(addItems);
                                }
                                else
                                {
                                    // Remove
                                    int deleteCount = DataList.Count - min;
                                    if (deleteCount > 0)
                                    {
                                        DataList.RemoveItemsFromEnd(deleteCount);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        dirty = true;
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
            if(data is UIData)
            {
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if(data is UIData)
            {
                UIData uiData = data as UIData;
                this.setDataNull(uiData);
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
            if(wrapProperty.p is UIData)
            {
                switch ((UIData.Property)wrapProperty.n)
                {
                    default:
                        break;
                }
                return;
            }
            Logger.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

        public void onClickBtnClose()
        {
            if (this.data != null)
            {
                MainCanvasUI.UIData mainCanvasUIData = this.data.findDataInParent<MainCanvasUI.UIData>();
                if (mainCanvasUIData != null)
                {
                    mainCanvasUIData.troopGrid.v = null;
                }
                else
                {
                    Logger.LogError("mainCanvasUIData null");
                }
            }
            else
            {
                Logger.LogError("data null");
            }
        }

    }
}