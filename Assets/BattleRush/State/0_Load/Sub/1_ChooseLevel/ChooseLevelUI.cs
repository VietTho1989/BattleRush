using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BattleRushS.StateS.LoadS
{
    public class ChooseLevelUI : UIBehavior<ChooseLevelUI.UIData>
    {

        #region UIData

        public class UIData : LoadUI.UIData.Sub
        {

            public VO<ReferenceData<ChooseLevel>> chooseLevel;

            #region Constructor

            public enum Property
            {
                chooseLevel
            }

            public UIData() : base()
            {
                this.chooseLevel = new VO<ReferenceData<ChooseLevel>>(this, (byte)Property.chooseLevel, new ReferenceData<ChooseLevel>(null));
            }

            #endregion

            public override Load.Sub.Type getType()
            {
                return Load.Sub.Type.ChooseLevel;
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
                    ChooseLevel chooseLevel = this.data.chooseLevel.v.data;
                    if (chooseLevel != null)
                    {

                    }
                    else
                    {
                        Logger.LogError("chooseLevel null");
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
                UIData uiData = data as UIData;
                // Child
                {
                    uiData.chooseLevel.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            if(data is ChooseLevel)
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
                    uiData.chooseLevel.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // Child
            if (data is ChooseLevel)
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
                    case UIData.Property.chooseLevel:
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
            if (wrapProperty.p is ChooseLevel)
            {
                return;
            }
            Logger.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

        public TMP_InputField edtLevel;

        public void onClickBtnChooseLevel()
        {
            if (this.data != null)
            {
                ChooseLevel chooseLevel = this.data.chooseLevel.v.data;
                if (chooseLevel != null)
                {
                    // find
                    int level = -1;
                    {
                        if (edtLevel != null)
                        {
                            int.TryParse(edtLevel.text, out level);
                        }
                        else
                        {
                            Logger.LogError("edtLevel null");
                        }
                    }
                    // process
                    if (level >= 0)
                    {
                        // change to load level
                        Load load = chooseLevel.findDataInParent<Load>();
                        if (load != null)
                        {
                            LoadLevel loadLevel = load.sub.newOrOld<LoadLevel>();
                            {
                                loadLevel.level.v = level;
                            }
                            load.sub.v = loadLevel;
                        }
                        else
                        {
                            Logger.LogError("load null");
                        }
                    }
                    else
                    {
                        Logger.LogError("level error: " + level);
                    }
                }
                else
                {
                    Logger.LogError("chooseLevel null");
                }
            }
            else
            {
                Logger.LogError("data null");
            }
        }

    }
}
