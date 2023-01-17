using System.Collections;
using System.Collections.Generic;
using LeTai.Asset.TranslucentImage;
using UnityEngine;

namespace BattleRushS.StateS
{
    public class EndUI : UIBehavior<EndUI.UIData>
    {
        #region UIData

        public class UIData : BattleRushUI.UIData.StateUI
        {

            public VO<ReferenceData<End>> end;

            #region Constructor

            public enum Property
            {
                end
            }

            public UIData() : base()
            {
                this.end = new VO<ReferenceData<End>>(this, (byte)Property.end, new ReferenceData<End>(null));
            }

            #endregion

            public override BattleRush.State.Type getType()
            {
                return BattleRush.State.Type.End;
            }
        }

        #endregion

        #region Refresh

        public TranslucentImage translucentImage;

        public TMPro.TextMeshProUGUI tvCountDown;

        public GameObject btnAds;
        public GameObject btnAdsOutOfTime;

        public const int TimeDuration = 9;

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    End end = this.data.end.v.data;
                    if (end != null)
                    {
                        // translucentImage
                        {
                            if (translucentImage != null)
                            {
                                // find
                                TranslucentImageSource source = null;
                                {
                                    CameraUI cameraUI = this.GetComponentInParent<CameraUI>();
                                    if (cameraUI != null)
                                    {
                                        if (cameraUI.mainCamera != null)
                                        {
                                            source = cameraUI.mainCamera.GetComponent<TranslucentImageSource>();
                                        }
                                        else
                                        {
                                            Logger.LogError("mainCamera null");
                                        }
                                    }
                                    else
                                    {
                                        Logger.LogError("cameraUI null");
                                    }
                                }
                                // set
                                translucentImage.source = source;                                
                            }
                            else
                            {
                                Logger.LogError("translucentImage null");
                            }
                        }
                        // tvCountDown
                        {
                            if (tvCountDown != null)
                            {
                                int countDown = Mathf.Max(0, Mathf.FloorToInt(TimeDuration - end.time.v));
                                tvCountDown.text = countDown.ToString();
                            }
                            else
                            {
                                Logger.LogError("tvCountDown null");
                            }
                        }
                        // btnAds
                        {
                            if(btnAds!=null && btnAdsOutOfTime != null)
                            {
                                btnAds.SetActive(!(end.time.v >= TimeDuration));
                                btnAdsOutOfTime.SetActive(end.time.v >= TimeDuration);
                            }
                            else
                            {
                                Logger.LogError("btnAds null");
                            }
                        }
                    }
                    else
                    {
                        Logger.LogError("end null");
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
                    uiData.end.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            if (data is End)
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
                    uiData.end.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // Child
            if (data is End)
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
                    case UIData.Property.end:
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
            if (wrapProperty.p is End)
            {
                switch ((End.Property)wrapProperty.n)
                {
                    case End.Property.time:
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

        public void onClickBtnRevive()
        {
            Logger.Log("onClickBtnRevive");
            if (this.data != null)
            {
                End end = this.data.end.v.data;
                if (end != null)
                {
                    // find
                    bool removeAds = false;
                    {
                        BattleRush battleRush = end.findDataInParent<BattleRush>();
                        if (battleRush != null)
                        {
                            removeAds = battleRush.adsBanner.v.removeAds.v;
                        }
                        else
                        {
                            Logger.LogError("battleRush null");
                        }
                    }
                    // show
                    AdsManager.Instance.ShowAds(() =>
                    {
                        Logger.Log("EndUI show ads finish");
                        revive();
                    }, removeAds);
                }
                else
                {
                    Logger.LogError("end null");
                }
            }
            else
            {
                Logger.LogError("data null");
            }
        }

        private void revive()
        {
            if (this.data != null)
            {
                End end = this.data.end.v.data;
                if (end != null)
                {
                    BattleRush battleRush = end.findDataInParent<BattleRush>();
                    if (battleRush != null)
                    {
                        // restore hero hit point
                        {
                            battleRush.hero.v.hitPoint.v = 1.0f;
                        }
                        // change to play
                        {
                            Play play = battleRush.state.newOrOld<Play>();
                            {

                            }
                            battleRush.state.v = play;
                        }                        
                    }
                    else
                    {
                        Logger.LogError("battleRush null");
                    }
                }
                else
                {
                    Logger.LogError("end null");
                }
            }
            else
            {
                Logger.LogError("data null");
            }
        }

        public void onClickBtnReset()
        {
            if (this.data != null)
            {
                End end = this.data.end.v.data;
                if (end != null)
                {
                    BattleRush battleRush = end.findDataInParent<BattleRush>();
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