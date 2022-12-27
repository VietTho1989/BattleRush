using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.DataHelpers;
using UnityEngine;

namespace Com.TheFallenGames.OSA
{
    [RequireComponent(typeof(MyOSA))]
    public abstract class MyOSAUI<K, T> : UIBehavior<K>, OSAUIInterface where K : Data
    {

        public SimpleDataHelper<T> DataList { get; private set; }

        private MyOSA MyOSA;
        public MyOSA myOSA
        {
            get
            {
                if (MyOSA == null)
                {
                    MyOSA = GetComponent<MyOSA>();
                }
                return MyOSA;
            }
        }

        #region implement interface

        public virtual void MyStart(IOSA iAdapter)
        {
            DataList = new SimpleDataHelper<T>(iAdapter);
        }

        public abstract void CreateViewsHolder(BaseItemViewsHolder viewHolder, int itemIndex);

        public abstract void UpdateViewsHolder(BaseItemViewsHolder newOrRecycled);

        #endregion

    }

    public interface OSAUIInterface
    {

        void MyStart(IOSA iAdapter);

        void CreateViewsHolder(BaseItemViewsHolder viewHolder, int itemIndex);

        void UpdateViewsHolder(BaseItemViewsHolder newOrRecycled);

    }

    /*#region implement callBacks

    public override void onAddCallBack<T>(T data)
    {
        Logger.LogError("Don't process: " + data + "; " + this);
    }

    public override void onRemoveCallBack<T>(T data, bool isHide)
    {
        Logger.LogError("Don't process: " + data + "; " + this);
    }

    public override void onUpdateSync<T>(WrapProperty wrapProperty, List<Sync<T>> syncs)
    {
        if (WrapProperty.checkError(wrapProperty))
        {
            return;
        }
        Logger.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
    }

    #endregion*/

}