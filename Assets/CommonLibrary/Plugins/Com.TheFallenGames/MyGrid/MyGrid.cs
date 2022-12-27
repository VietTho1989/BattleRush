using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using Com.TheFallenGames.OSA.Util.IO;
using Com.TheFallenGames.OSA.DataHelpers;
using Com.TheFallenGames.OSA.Util.IO.Pools;

namespace Com.TheFallenGames.OSA
{
    public class MyGrid : GridAdapter<GridParams, MyCellViewsHolder>
    {

        #region MyOSAUI

        private GridUIInterface GridUIInterface;

        private GridUIInterface gridUIInterface
        {
            get
            {
                if (GridUIInterface == null)
                {
                    GridUIInterface = this.GetComponent<GridUIInterface>();
                }
                return GridUIInterface;
            }
        }

        #endregion

        protected override void Start()
        {
            if (gridUIInterface != null)
            {
                gridUIInterface.MyStart(this);
            }
            else
            {
                Logger.LogError("gridUIInterface null");
            }
            base.Start();
        }

        #region create, update

        protected override void OnCellViewsHolderCreated(MyCellViewsHolder cellVH, CellGroupViewsHolder<MyCellViewsHolder> cellGroup)
        {
            base.OnCellViewsHolderCreated(cellVH, cellGroup);
            if (gridUIInterface != null)
            {
                gridUIInterface.OnCellViewsHolderCreated(cellVH, cellGroup);
            }
            else
            {
                Logger.LogError("gridUIInterface null");
            }
        }

        protected override void UpdateCellViewsHolder(MyCellViewsHolder viewsHolder)
        {
            if (gridUIInterface != null)
            {
                gridUIInterface.UpdateCellViewsHolder(viewsHolder);
            }
            else
            {
                Logger.LogError("gridUIInterface null");
            }
        }

        #endregion

    }

    public class MyCellViewsHolder : CellViewsHolder
    {

        /*protected override RectTransform GetViews()
        {
            Logger.Log("need override this method");
            return (RectTransform)this.root.gameObject.transform;
        }*/

    }

}