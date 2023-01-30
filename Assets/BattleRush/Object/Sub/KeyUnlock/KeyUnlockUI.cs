using System.Collections;
using System.Collections.Generic;
using BattleRushS.HeroS;
using Dreamteck.Forever;
using UnityEngine;

namespace BattleRushS.ObjectS
{
    public class KeyUnlockUI : UIBehavior<KeyUnlockUI.UIData>, BattleRushUI.UIData.ObjectInPathUIInterface
    {

        public void setObjectInPathUIData(Data data)
        {
            if (data is UIData)
            {
                this.setData((UIData)data);
            }
        }

        public GameObject getMyGameObject()
        {
            return this.gameObject;
        }

        public ObjectInPath.Type getType()
        {
            return ObjectInPath.Type.KeyUnlock;
        }

        #region UIData

        public class UIData : BattleRushUI.UIData.ObjectInPathUI
        {

            public VO<ReferenceData<KeyUnlock>> keyUnlock;

            public LO<Collider> colliderEnters;

            #region Constructor

            public enum Property
            {
                keyUnlock,
                colliderEnters
            }

            public UIData() : base()
            {
                this.keyUnlock = new VO<ReferenceData<KeyUnlock>>(this, (byte)Property.keyUnlock, new ReferenceData<KeyUnlock>(null));
                this.colliderEnters = new LO<Collider>(this, (byte)Property.colliderEnters);
            }

            #endregion

            public override ObjectInPath.Type getType()
            {
                return ObjectInPath.Type.KeyUnlock;
            }

            public override ObjectInPath getObjectInPath()
            {
                return this.keyUnlock.v.data;
            }

        }

        #endregion

        #region Refresh

        public MeshRenderer[] keyUnlockMeshRenderer;
        public ParticleSystem keyUnlockPickUpEffect;

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    KeyUnlock keyUnlock = this.data.keyUnlock.v.data;
                    if (keyUnlock != null)
                    {
                        this.name = "KeyUnlock " + keyUnlock.position.v;
                        KeyUnlock.State beforeState = keyUnlock.state.v;
                        // collider enter
                        {
                            switch (keyUnlock.state.v)
                            {
                                case KeyUnlock.State.Normal:
                                    {
                                        // check is picked up
                                        bool isPickedUp = false;
                                        {
                                            foreach (Collider collider in this.data.colliderEnters.vs)
                                            {
                                                // get hero
                                                if (!isPickedUp)
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
                                                                BattleRush battleRush = keyUnlock.findDataInParent<BattleRush>();
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
                                                // get troop follow
                                                if (!isPickedUp)
                                                {
                                                    TroopInformation troopInformation = collider.GetComponent<TroopInformation>();
                                                    if (troopInformation != null)
                                                    {
                                                        TroopFollowUI troopFollowUI = troopInformation.GetComponentInParent<TroopFollowUI>();
                                                        if (troopFollowUI != null)
                                                        {
                                                            TroopFollowUI.UIData troopFollowUIData = troopFollowUI.data;
                                                            if (troopFollowUIData != null)
                                                            {
                                                                TroopFollow troopFollow = troopFollowUIData.troopFollow.v.data;
                                                                if (troopFollow != null)
                                                                {
                                                                    // check this troop is running, follow hero, not in cage
                                                                    bool isTroopRunFollowHero = false;
                                                                    {
                                                                        Hero hero = troopFollow.findDataInParent<Hero>();
                                                                        if (hero != null)
                                                                        {
                                                                            BattleRush battleRush = troopFollow.findDataInParent<BattleRush>();
                                                                            if (battleRush != null)
                                                                            {
                                                                                if (battleRush.hero.v == hero)
                                                                                {
                                                                                    isTroopRunFollowHero = true;
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                Logger.Log("battleRush null");
                                                                            }
                                                                        }
                                                                    }
                                                                    // process
                                                                    if (isTroopRunFollowHero)
                                                                    {
                                                                        if (troopFollow.hitPoint.v > 0)
                                                                        {
                                                                            // alive, so can pick
                                                                            Logger.Log("KeyUnlockUI: troop follow pick keyUnlock: " + this.gameObject);
                                                                            isPickedUp = true;
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    Logger.LogError("troopFollow null");
                                                                }
                                                            }
                                                            else
                                                            {
                                                                Logger.LogError("troopFollowUIData null");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Logger.LogError("troopFollowUI null");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Logger.LogError("troopInformation null");
                                                    }
                                                }
                                            }
                                        }
                                        // process
                                        if (isPickedUp)
                                        {
                                            keyUnlock.state.v = KeyUnlock.State.PickUp;
                                        }
                                    }
                                    break;
                                case KeyUnlock.State.PickUp:
                                    {

                                    }
                                    break;
                                case KeyUnlock.State.PickedUpFinish:
                                    break;
                            }
                        }
                        // keyMeshRenderer
                        {
                            if (keyUnlockMeshRenderer != null)
                            {
                                switch (keyUnlock.state.v)
                                {
                                    case KeyUnlock.State.Normal:
                                        {
                                            foreach(MeshRenderer renderer in keyUnlockMeshRenderer)
                                            {
                                                renderer.enabled = true;
                                            }                                           
                                        }
                                        break;
                                    case KeyUnlock.State.PickUp:
                                    case KeyUnlock.State.PickedUpFinish:
                                        {
                                            foreach (MeshRenderer renderer in keyUnlockMeshRenderer)
                                            {
                                                renderer.enabled = false;
                                            }
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                Logger.LogError("keyUnlockMeshRenderer null");
                            }
                        }
                        // keyUnlockPickUpEffect
                        {
                            if (keyUnlockPickUpEffect != null)
                            {
                                switch (keyUnlock.state.v)
                                {
                                    case KeyUnlock.State.Normal:
                                        {
                                            keyUnlockPickUpEffect.gameObject.SetActive(false);
                                        }
                                        break;
                                    case KeyUnlock.State.PickUp:
                                        {
                                            keyUnlockPickUpEffect.gameObject.SetActive(true);
                                            keyUnlockPickUpEffect.Play();
                                            keyUnlock.state.v = KeyUnlock.State.PickedUpFinish;
                                        }
                                        break;
                                    case KeyUnlock.State.PickedUpFinish:
                                        {
                                            keyUnlockPickUpEffect.gameObject.SetActive(true);
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                Logger.LogError("keyUnlockPickUpEffect null");
                            }
                        }
                        // add to level coin
                        {
                            if (beforeState == KeyUnlock.State.Normal)
                            {
                                switch (keyUnlock.state.v)
                                {
                                    case KeyUnlock.State.Normal:
                                        break;
                                    case KeyUnlock.State.PickUp:
                                    case KeyUnlock.State.PickedUpFinish:
                                        {
                                            /*BattleRush battleRush = keyUnlock.findDataInParent<BattleRush>();
                                            if (battleRush != null)
                                            {
                                                battleRush.levelCoin.v++;
                                            }
                                            else
                                            {
                                                Logger.LogError("battleRush null");
                                            }*/
                                        }
                                        break;
                                    default:
                                        Logger.LogError("unknown state: " + keyUnlock.state.v);
                                        break;
                                }
                            }
                        }
                    }
                    else
                    {
                        Logger.LogError("keyUnlock null");
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
                    KeyUnlock keyUnlock = this.data.keyUnlock.v.data;
                    if (keyUnlock != null)
                    {
                        BattleRush battleRush = keyUnlock.findDataInParent<BattleRush>();
                        if (battleRush != null)
                        {
                            battleRush.laneObjects.remove(keyUnlock);
                        }
                    }
                    else
                    {
                        Logger.LogError("keyUnlock null");
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
            if (data is UIData)
            {
                UIData uiData = data as UIData;
                // Child
                {
                    uiData.keyUnlock.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            {
                if (data is KeyUnlock)
                {
                    KeyUnlock keyUnlock = data as KeyUnlock;
                    // Parent
                    {
                        DataUtils.addParentCallBack(keyUnlock, this, ref this.battleRush);
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
                    uiData.keyUnlock.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // Child
            {
                if (data is KeyUnlock)
                {
                    KeyUnlock keyUnlock = data as KeyUnlock;
                    // Parent
                    {
                        DataUtils.removeParentCallBack(keyUnlock, this, ref this.battleRush);
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
                    case UIData.Property.keyUnlock:
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
                if (wrapProperty.p is KeyUnlock)
                {
                    switch ((KeyUnlock.Property)wrapProperty.n)
                    {
                        case KeyUnlock.Property.state:
                            dirty = true;
                            break;
                        case KeyUnlock.Property.position:
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
            //Logger.Log("onTriggerEnter: " + collider);
            if (this.data != null)
            {
                this.data.colliderEnters.add(collider);
                refresh();
            }
        }

        private void OnTriggerExit(Collider collider)
        {
            //Logger.Log("onTriggerEnter: " + collider);
            if (this.data != null)
            {
                this.data.colliderEnters.remove(collider);
                refresh();
            }
        }

    }
}
