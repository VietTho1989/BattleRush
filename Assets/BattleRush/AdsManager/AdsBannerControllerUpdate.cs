using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS
{
    public class AdsBannerControllerUpdate : UpdateBehavior<AdsBannerController>
    {

        #region Update

        private bool needSavePref = false;

        public override void update()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    // save remove ads to preference
                    if (needSavePref)
                    {
                        PlayerPrefs.SetInt(AdsBannerController.REMOVE_AD, this.data.removeAds.v ? 1 : 0);
                        PlayerPrefs.Save();
                    }
                    // show or not
                    {
                        // find
                        bool isShow = false;
                        {
                            if (this.data.removeAds.v)
                            {
                                isShow = false;
                            }
                            else
                            {
                                BattleRush battleRush = this.data.findDataInParent<BattleRush>();
                                if (battleRush != null)
                                {
                                    switch (battleRush.state.v.getType())
                                    {
                                        case BattleRush.State.Type.Load:
                                            break;
                                        case BattleRush.State.Type.Edit:
                                            break;
                                        case BattleRush.State.Type.Start:
                                            isShow = true;
                                            break;
                                        case BattleRush.State.Type.Play:
                                            isShow = true;
                                            break;
                                        case BattleRush.State.Type.End:
                                            isShow = true;
                                            break;
                                        default:
                                            Logger.LogError("unknown type: " + battleRush.state.v.getType());
                                            break;
                                    }
                                }
                                else
                                {
                                    Logger.LogError("battleRush null");
                                }
                            }                            
                        }
                        // process
                        if (isShow)
                        {
                            AdsBannerController.State.Show show = this.data.state.newOrOld<AdsBannerController.State.Show>();
                            {

                            }
                            this.data.state.v = show;
                        }
                        else
                        {
                            AdsBannerController.State.NotShow notShow = this.data.state.newOrOld<AdsBannerController.State.NotShow>();
                            {

                            }
                            this.data.state.v = notShow;
                        }
                    }
                    // request ads
                    {
                        switch (this.data.state.v.getType())
                        {
                            case AdsBannerController.State.Type.NotShow:
                                {
                                    AdsManager.Instance.DestroyBanner();
                                }
                                break;
                            case AdsBannerController.State.Type.Show:
                                {
                                    AdsBannerController.State.Show show = this.data.state.v as AdsBannerController.State.Show;
                                    if (!show.alreadyRequest.v)
                                    {
                                        show.alreadyRequest.v = true;
                                        AdsManager.Instance.RequestBanner(this.data.removeAds.v);
                                    }
                                }
                                break;
                            default:
                                Logger.LogError("unknown type: " + this.data.state.v.getType());
                                break;
                        }
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

        private BattleRush battleRush = null;

        public override void onAddCallBack<T>(T data)
        {
            if(data is AdsBannerController)
            {
                AdsBannerController adsBannerController = data as AdsBannerController;
                // Parent
                {
                    DataUtils.addParentCallBack(adsBannerController, this, ref this.battleRush);
                }
                // Child
                {
                    adsBannerController.state.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Parent
            if(data is BattleRush)
            {
                dirty = true;
                return;
            }
            // Child
            if(data is AdsBannerController.State)
            {
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if (data is AdsBannerController)
            {
                AdsBannerController adsBannerController = data as AdsBannerController;
                // Parent
                {
                    DataUtils.removeParentCallBack(adsBannerController, this, ref this.battleRush);
                }
                // Child
                {
                    adsBannerController.state.allRemoveCallBack(this);
                }
                this.setDataNull(adsBannerController);
                return;
            }
            // Parent
            if (data is BattleRush)
            {
                return;
            }
            // Child
            if (data is AdsBannerController.State)
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
            if (wrapProperty.p is AdsBannerController)
            {
                switch ((AdsBannerController.Property)wrapProperty.n)
                {
                    case AdsBannerController.Property.removeAds:
                        needSavePref = true;
                        dirty = true;
                        break;
                    case AdsBannerController.Property.state:
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
            // Parent
            if (wrapProperty.p is BattleRush)
            {
                switch ((BattleRush.Property)wrapProperty.n)
                {
                    case BattleRush.Property.state:
                        dirty = true;
                        break;
                    default:
                        break;
                }
                return;
            }
            // Child
            if (wrapProperty.p is AdsBannerController.State)
            {
                AdsBannerController.State state = wrapProperty.p as AdsBannerController.State;
                switch (state.getType())
                {
                    case AdsBannerController.State.Type.Show:
                        {
                            switch ((AdsBannerController.State.Show.Property)wrapProperty.n)
                            {
                                case AdsBannerController.State.Show.Property.alreadyRequest:
                                    dirty = true;
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    case AdsBannerController.State.Type.NotShow:
                        break;
                    default:
                        break;
                }
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + syncs + "; " + this);
        }

        #endregion

    }
}