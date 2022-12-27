using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using frame8.Logic.Misc.Other.Extensions;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.Util;
using UnityEngine.EventSystems;

namespace Com.TheFallenGames.OSA
{
    public class MyOSA : OSA<BaseParamsWithPrefab, BaseItemViewsHolder>
    {

        #region MyOSAUI

        private OSAUIInterface OsaUIInterface;

        private OSAUIInterface osaUIInterface
        {
            get
            {
                if (OsaUIInterface == null)
                {
                    OsaUIInterface = this.GetComponent<OSAUIInterface>();
                }
                return OsaUIInterface;
            }
        }

        #endregion

        protected override void Start()
        {
            if (osaUIInterface != null)
            {
                osaUIInterface.MyStart(this);
            }
            else
            {
                Logger.LogError("osaUIInterface null");
            }
            base.Start();
        }

        protected override BaseItemViewsHolder CreateViewsHolder(int itemIndex)
        {
            Logger.Log("CreateViewsHolder");
            var instance = new BaseItemViewsHolder();
            instance.Init(_Params.ItemPrefab, _Params.Content, itemIndex);
            // interface
            {
                if (osaUIInterface != null)
                {
                    osaUIInterface.CreateViewsHolder(instance, itemIndex);
                }
                else
                {
                    Logger.LogError("osaUIInterface null");
                }
            }
            return instance;
        }

        protected override void UpdateViewsHolder(BaseItemViewsHolder newOrRecycled)
        {
            if (osaUIInterface != null)
            {
                osaUIInterface.UpdateViewsHolder(newOrRecycled);
            }
            else
            {
                Logger.LogError("osaUIInterface null");
            }
        }

        #region onScroll

        public delegate void MyScroll(double normalPos);
        public MyScroll myScroll;

        public override void OnScrollPositionChanged(double normPos)
        {
            base.OnScrollPositionChanged(normPos);
            // Logger.Log("OnScrollPositionChanged: " + normPos);
            if (myScroll != null)
            {
                myScroll(normPos);
            }
            else
            {
                // Logger.LogError("myScroll null");
            }
        }

        #endregion

    }
}