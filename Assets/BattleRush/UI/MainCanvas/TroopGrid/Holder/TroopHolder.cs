using System.Collections;
using System.Collections.Generic;
using BattleRushS.HeroS;
using Com.TheFallenGames.OSA;
using UnityEngine;
using UnityEngine.UI;

namespace BattleRushS.MainCanvasS
{
    public class TroopHolder : UIBehavior<TroopHolder.UIData>
    {

        #region UIData

        public class UIData : Data
        {

            public MyCellViewsHolder viewsHolder = null;

            public VO<TroopInformation> troopInformation;

            #region Constructor

            public enum Property
            {
                troopInformation
            }

            public UIData() : base()
            {
                this.troopInformation = new VO<TroopInformation>(this, (byte)Property.troopInformation, null);
            }

            #endregion

        }

        #endregion

        #region Refresh

        public TMPro.TextMeshProUGUI tvTroopName;

        public Image troopImage;

        public Image imgBorder;
        public Sprite spriteBorderLock;
        public Sprite spriteBorderReady;
        public Sprite spriteBorderUnlock;

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    TroopInformation troopInformation = this.data.troopInformation.v;
                    if (troopInformation != null)
                    {
                        // tvTroopName
                        if (tvTroopName != null)
                        {
                            // find
                            string troopName = troopInformation.troopName;
                            {
                                if (!string.IsNullOrEmpty(troopName))
                                {
                                    troopName = troopName.ToLower();
                                    troopName = troopName.Substring(0, 1).ToUpper() + troopName.Substring(1);
                                }
                            }
                            // set
                            tvTroopName.text = troopName;
                        }
                        else
                        {
                            Logger.LogError("tvTroopName null");
                        }
                        // troopImage
                        if (troopImage != null)
                        {
                            troopImage.sprite = troopInformation.portraitSprite;
                        }
                        else
                        {
                            Logger.LogError("troopImage null");
                        }
                        // imgBorder
                        if (imgBorder != null)
                        {
                            switch (troopInformation.unlockPriority)
                            {
                                case 0:
                                    imgBorder.sprite = spriteBorderUnlock;
                                    break;
                                default:
                                    imgBorder.sprite = spriteBorderLock;
                                    break;
                            }
                        }
                        else
                        {
                            Logger.LogError("imgBorder null");
                        }
                    }
                    else
                    {
                        Logger.LogError("troopInformation null");
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
                    case UIData.Property.troopInformation:
                        dirty = true;
                        break;
                    default:
                        break;
                }
                return;
            }
            Logger.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

    }
}