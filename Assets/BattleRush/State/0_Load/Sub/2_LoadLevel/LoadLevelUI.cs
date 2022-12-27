using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.StateS.LoadS
{
    public class LoadLevelUI : UIBehavior<LoadLevelUI.UIData>
    {

        #region UIData

        public class UIData : LoadUI.UIData.Sub
        {

            public VO<ReferenceData<LoadLevel>> loadLevel;

            #region Constructor

            public enum Property
            {
                loadLevel
            }

            public UIData() : base()
            {
                this.loadLevel = new VO<ReferenceData<LoadLevel>>(this, (byte)Property.loadLevel, new ReferenceData<LoadLevel>(null));
            }

            #endregion

            public override Load.Sub.Type getType()
            {
                return Load.Sub.Type.LoadLevel;
            }

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
                    LoadLevel loadLevel = this.data.loadLevel.v.data;
                    if (loadLevel != null)
                    {

                    }
                    else
                    {
                        Logger.LogError("loadLevel null");
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
            if (data is UIData)
            {
                UIData uiData = data as UIData;
                // Child
                {
                    uiData.loadLevel.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            if (data is LoadLevel)
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
                // Child
                {
                    uiData.loadLevel.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // Child
            if (data is LoadLevel)
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
                    case UIData.Property.loadLevel:
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
            if (wrapProperty.p is LoadLevel)
            {
                return;
            }
            Logger.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

    }
}
