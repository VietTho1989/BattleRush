using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.StateS
{
    public class PlayUI : UIBehavior<PlayUI.UIData>
    {

        #region UIData

        public class UIData : BattleRushUI.UIData.StateUI
        {

            public VO<ReferenceData<Play>> play;

            #region Constructor

            public enum Property
            {
                play
            }

            public UIData() : base()
            {
                this.play = new VO<ReferenceData<Play>>(this, (byte)Property.play, new ReferenceData<Play>(null));
            }

            #endregion

            public override BattleRush.State.Type getType()
            {
                return BattleRush.State.Type.Play;
            }
        }

        #endregion

        #region Refresh

        public GameObject blur;

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    Play play = this.data.play.v.data;
                    if (play != null)
                    {
                        // blur
                        {
                            if (blur != null)
                            {
                                switch (play.state.v)
                                {
                                    case Play.State.Normal:
                                        blur.SetActive(false);
                                        break;
                                    case Play.State.Pause:
                                        blur.SetActive(true);
                                        break;
                                }
                            }
                            else
                            {
                                Logger.LogError("blur null");
                            }
                        }
                    }
                    else
                    {
                        Logger.LogError("play null");
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
                    uiData.play.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            if (data is Play)
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
                    uiData.play.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // Child
            if (data is Play)
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
                    case UIData.Property.play:
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
            if (wrapProperty.p is Play)
            {
                switch ((Play.Property)wrapProperty.n)
                {
                    case Play.Property.state:
                        dirty = true;
                        break;
                    default:
                        Logger.LogError("Don't process: " + wrapProperty + "; " + this);
                        break;
                }
                return;
            }
            Logger.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

        public void onClickBtnReset()
        {
            if (this.data != null)
            {
                Play play = this.data.play.v.data;
                if (play != null)
                {
                    BattleRush battleRush = play.findDataInParent<BattleRush>();
                    if (battleRush != null)
                    {
                        Load load = battleRush.state.newOrOld<Load>();
                        {

                        }
                        battleRush.state.v = load;
                    }
                    else
                    {
                        Logger.LogError("battleRush null");
                    }
                }
                else
                {
                    Logger.LogError("play null");
                }
            }
            else
            {
                Logger.LogError("data null");
            }
        }

    }
}
