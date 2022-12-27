using System.Collections;
using System.Collections.Generic;
using Dreamteck.Forever;
using UnityEngine;

namespace BattleRushS.ObjectS
{
    public class CoinUI : UIBehavior<CoinUI.UIData>, BattleRushUI.UIData.ObjectInPathUIInterface
    {

        public void setObjectInPathUIData(Data data)
        {
            if (data is UIData)
            {
                this.setData((UIData)data);
            }
        }

        #region UIData

        public class UIData : BattleRushUI.UIData.ObjectInPathUI
        {

            public VO<ReferenceData<Coin>> coin;

            public LO<Collider> colliderEnters;

            #region Constructor

            public enum Property
            {
                coin,
                colliderEnters
            }

            public UIData() : base()
            {
                this.coin = new VO<ReferenceData<Coin>>(this, (byte)Property.coin, new ReferenceData<Coin>(null));
                this.colliderEnters = new LO<Collider>(this, (byte)Property.colliderEnters);
            }

            #endregion

            public override ObjectInPath.Type getType()
            {
                return ObjectInPath.Type.OkgCoin;
            }

            public override ObjectInPath getObjectInPath()
            {
                return this.coin.v.data;
            }

        }

        #endregion

        #region Refresh

        private LevelGenerator levelGenerator = null;

        public MeshRenderer coinMeshRenderer;
        public ParticleSystem coinPickUpEffect;

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    Coin coin = this.data.coin.v.data;
                    if (coin != null)
                    {
                        this.name = "Coin " + coin.P.v;
                        // set position
                        {
                            // find levelGenerator
                            if (levelGenerator == null)
                            {
                                BattleRush battleRush = coin.findDataInParent<BattleRush>();
                                if (battleRush != null)
                                {
                                    BattleRushUI battleRushUI = battleRush.findCallBack<BattleRushUI>();
                                    if (battleRushUI != null)
                                    {
                                        levelGenerator = battleRushUI.GetComponent<LevelGenerator>();
                                    }
                                    else
                                    {
                                        Logger.LogError("battleRushUI null");
                                    }
                                }
                                else
                                {
                                    Logger.LogError("battleRush null");
                                }
                            }
                            // set position
                            if (levelGenerator != null)
                            {
                                /*Vector3 position = MakeObjectUpdate.GetPositionInLevelGenerator(levelGenerator, coin.P.v.z);
                                this.transform.position = new Vector3(position.x, position.y + 0.9f, position.z);*/
                            }
                        }
                        // collider enter
                        {
                            switch (coin.state.v)
                            {
                                case Coin.State.Normal:
                                    {
                                        // check is picked up
                                        bool isPickedUp = false;
                                        {
                                            foreach(Collider collider in this.data.colliderEnters.vs)
                                            {
                                                // get hero
                                                {
                                                    HeroUI heroUI = collider.GetComponent<HeroUI>();
                                                    if (heroUI != null)
                                                    {
                                                        HeroUI.UIData heroUIData = heroUI.data;
                                                        if (heroUIData != null)
                                                        {
                                                            Hero hero = heroUIData.hero.v.data;
                                                            if (hero != null)
                                                            {
                                                                BattleRush battleRush = coin.findDataInParent<BattleRush>();
                                                                if (battleRush != null)
                                                                {
                                                                    if (battleRush.hero.v == hero)
                                                                    {
                                                                        isPickedUp = true;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    Logger.LogError("battleRush null");
                                                                }
                                                            }
                                                            else
                                                            {
                                                                Logger.LogError("hero null");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Logger.LogError("heroUIData null");
                                                        }
                                                    }
                                                }                                                
                                            }
                                        }
                                        // process
                                        if (isPickedUp)
                                        {
                                            coin.state.v = Coin.State.PickUp;
                                        }
                                    }
                                    break;
                                case Coin.State.PickUp:
                                    {

                                    }
                                    break;
                                case Coin.State.PickedUpFinish:
                                    break;
                            }
                        }
                        // coinMeshRenderer
                        {
                            if (coinMeshRenderer != null)
                            {
                                switch (coin.state.v)
                                {
                                    case Coin.State.Normal:
                                        {
                                            coinMeshRenderer.enabled = true;
                                        }
                                        break;
                                    case Coin.State.PickUp:
                                    case Coin.State.PickedUpFinish:
                                        {
                                            coinMeshRenderer.enabled = false;
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                Logger.LogError("coinMeshRenderer null");
                            }
                        }
                        // coinPickUpEffect
                        {
                            if (coinPickUpEffect != null)
                            {
                                switch (coin.state.v)
                                {
                                    case Coin.State.Normal:
                                        {
                                            coinPickUpEffect.gameObject.SetActive(false);
                                        }
                                        break;
                                    case Coin.State.PickUp:
                                        {
                                            coinPickUpEffect.gameObject.SetActive(true);
                                            coinPickUpEffect.Play();
                                            coin.state.v = Coin.State.PickedUpFinish;
                                        }
                                        break;
                                    case Coin.State.PickedUpFinish:
                                        {
                                            coinPickUpEffect.gameObject.SetActive(true);
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                Logger.LogError("coinPickUpEffect null");
                            }
                        }
                    }
                    else
                    {
                        Logger.LogError("coin null");
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
            return false;
        }

        #endregion

        public override void OnDestroy()
        {
            base.OnDestroy();
            // remove: auto delete
            {
                if (this.data != null)
                {
                    Coin coin = this.data.coin.v.data;
                    if (coin != null)
                    {
                        BattleRush battleRush = coin.findDataInParent<BattleRush>();
                        if (battleRush != null)
                        {
                            battleRush.laneObjects.remove(coin);
                        }
                    }
                    else
                    {
                        Logger.LogError("coin null");
                    }
                }
                else
                {
                    Logger.LogError("data null");
                }
            }
        }

        #region implement callBacks

        private BattleRush battleRush = null;

        public override void onAddCallBack<T>(T data)
        {
            if(data is UIData)
            {
                UIData uiData = data as UIData;
                // Child
                {
                    uiData.coin.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            {
                if (data is Coin)
                {
                    Coin coin = data as Coin;
                    // Parent
                    {
                        DataUtils.addParentCallBack(coin, this, ref this.battleRush);
                    }
                    dirty = true;
                    return;
                }
                // Parent
                if (data is BattleRush)
                {
                    dirty = true;
                    return;
                }
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
                    uiData.coin.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // Child
            {
                if (data is Coin)
                {
                    Coin coin = data as Coin;
                    // Parent
                    {
                        DataUtils.removeParentCallBack(coin, this, ref this.battleRush);
                    }
                    return;
                }
                // Parent
                if (data is BattleRush)
                {
                    return;
                }
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
                    case UIData.Property.coin:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case UIData.Property.colliderEnters:
                        dirty = true;
                        break;
                    default:
                        break;
                }
                return;
            }
            // Child
            {
                if (wrapProperty.p is Coin)
                {
                    switch ((Coin.Property)wrapProperty.n)
                    {
                        case Coin.Property.state:
                            dirty = true;
                            break;
                        case Coin.Property.P:
                            dirty = true;
                            break;
                        case Coin.Property.R:
                            dirty = true;
                            break;
                        default:
                            Logger.LogError("Don't process: " + wrapProperty + "; " + this);
                            break;
                    }
                    return;
                }
                // Parent
                if (wrapProperty.p is BattleRush)
                {
                    switch ((BattleRush.Property)wrapProperty.n)
                    {
                        case BattleRush.Property.segmentIndex:
                            dirty = true;
                            break;
                        default:
                            break;
                    }
                    return;
                }
            }
            Logger.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

        private void OnTriggerEnter(Collider collider)
        {
            Logger.Log("onTriggerEnter: " + collider);
            if (this.data != null)
            {
                this.data.colliderEnters.add(collider);
                refresh();
            }
        }

        private void OnTriggerExit(Collider collider)
        {
            Logger.Log("onTriggerEnter: " + collider);
            if (this.data != null)
            {
                this.data.colliderEnters.remove(collider);
                refresh();
            }
        }

    }
}
