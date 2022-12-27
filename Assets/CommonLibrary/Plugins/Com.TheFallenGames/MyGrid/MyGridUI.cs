using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using Com.TheFallenGames.OSA.DataHelpers;
using UnityEngine;

namespace Com.TheFallenGames.OSA
{
    [RequireComponent(typeof(MyGrid))]
    public abstract class MyGridUI<K, T> : UIBehavior<K>, GridUIInterface where K : Data
    {

        public SimpleDataHelper<T> DataList { get; private set; }

        private MyGrid MyGrid;
        public MyGrid myGrid
        {
            get
            {
                if (MyGrid == null)
                {
                    MyGrid = GetComponent<MyGrid>();
                }
                return MyGrid;
            }
        }

        #region implement interface

        public virtual void MyStart(IOSA iAdapter)
        {
            DataList = new SimpleDataHelper<T>(iAdapter);
        }

        public abstract void OnCellViewsHolderCreated(MyCellViewsHolder cellVH, CellGroupViewsHolder<MyCellViewsHolder> cellGroup);

        public abstract void UpdateCellViewsHolder(MyCellViewsHolder viewsHolder);

        #endregion

    }

    public interface GridUIInterface
    {

        void MyStart(IOSA iAdapter);

        void OnCellViewsHolderCreated(MyCellViewsHolder cellVH, CellGroupViewsHolder<MyCellViewsHolder> cellGroup);

        void UpdateCellViewsHolder(MyCellViewsHolder viewsHolder);

    }
}