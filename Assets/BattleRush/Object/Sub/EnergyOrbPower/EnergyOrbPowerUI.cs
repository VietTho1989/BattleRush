using System.Collections;
using System.Collections.Generic;
using BattleRushS.HeroS;
using Dreamteck.Forever;
using UnityEngine;

namespace BattleRushS.ObjectS
{
    public class EnergyOrbPowerUI : UIBehavior<EnergyOrbPowerUI.UIData>, BattleRushUI.UIData.ObjectInPathUIInterface
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
            return ObjectInPath.Type.EnergyOrbPower;
        }

        #region UIData

        public class UIData : BattleRushUI.UIData.ObjectInPathUI
        {

            public VO<ReferenceData<EnergyOrbPower>> energyOrbPower;

            public LO<Collider> colliderEnters;

            #region Constructor

            public enum Property
            {
                energyOrbPower,
                colliderEnters
            }

            public UIData() : base()
            {
                this.energyOrbPower = new VO<ReferenceData<EnergyOrbPower>>(this, (byte)Property.energyOrbPower, new ReferenceData<EnergyOrbPower>(null));
                this.colliderEnters = new LO<Collider>(this, (byte)Property.colliderEnters);
            }

            #endregion

            public override ObjectInPath.Type getType()
            {
                return ObjectInPath.Type.EnergyOrbPower;
            }

            public override ObjectInPath getObjectInPath()
            {
                return this.energyOrbPower.v.data;
            }

        }

        #endregion

        #region Refresh

        public GameObject orb;
        public ParticleSystem pickUpEffect;

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    EnergyOrbPower energyOrbPower = this.data.energyOrbPower.v.data;
                    if (energyOrbPower != null)
                    {                      
                        // collider enter
                        {
                            switch (energyOrbPower.state.v)
                            {
                                case EnergyOrbPower.State.Normal:
                                    {
                                        // check is picked up
                                        bool isPickedUp = false;
                                        {
                                            foreach (Collider collider in this.data.colliderEnters.vs)
                                            {
                                                // get hero
                                                if(!isPickedUp){
                                                    HeroUI heroUI = collider.GetComponent<HeroUI>();
                                                    if (heroUI != null)
                                                    {
                                                        HeroUI.UIData heroUIData = heroUI.data;
                                                        if (heroUIData != null)
                                                        {
                                                            Hero hero = heroUIData.hero.v.data;
                                                            if (hero != null)
                                                            {
                                                                BattleRush battleRush = energyOrbPower.findDataInParent<BattleRush>();
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
                                                                            Logger.Log("CoinUI: troop follow pick coin: " + this.gameObject);
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
                                                }
                                            }
                                        }
                                        // process
                                        if (isPickedUp)
                                        {
                                            Logger.Log("energyOrbNormal pickup");
                                            energyOrbPower.state.v = EnergyOrbPower.State.PickUp;
                                        }
                                    }
                                    break;
                                case EnergyOrbPower.State.PickUp:
                                    {

                                    }
                                    break;
                                case EnergyOrbPower.State.PickedUpFinish:
                                    break;
                            }
                        }
                        // orb
                        {
                            if (orb != null)
                            {
                                switch (energyOrbPower.state.v)
                                {
                                    case EnergyOrbPower.State.Normal:
                                        {
                                            orb.SetActive(true);
                                        }
                                        break;
                                    case EnergyOrbPower.State.PickUp:
                                    case EnergyOrbPower.State.PickedUpFinish:
                                        {
                                            orb.SetActive(false);
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                Logger.LogError("orb null");
                            }
                        }
                        // pickUpEffect
                        {
                            if (pickUpEffect != null)
                            {
                                switch (energyOrbPower.state.v)
                                {
                                    case EnergyOrbPower.State.Normal:
                                        {
                                            pickUpEffect.gameObject.SetActive(false);
                                        }
                                        break;
                                    case EnergyOrbPower.State.PickUp:
                                        {
                                            pickUpEffect.gameObject.SetActive(true);
                                            pickUpEffect.Play();
                                            Logger.Log("energyOrbPower pickup effect");
                                            energyOrbPower.state.v = EnergyOrbPower.State.PickedUpFinish;
                                        }
                                        break;
                                    case EnergyOrbPower.State.PickedUpFinish:
                                        {
                                            pickUpEffect.gameObject.SetActive(true);
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                Logger.LogError("pickUpEffect null");
                            }
                        }
                    }
                    else
                    {
                        Logger.LogError("energyOrbPower null");
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
                    EnergyOrbPower energyOrbPower = this.data.energyOrbPower.v.data;
                    if (energyOrbPower != null)
                    {
                        BattleRush battleRush = energyOrbPower.findDataInParent<BattleRush>();
                        if (battleRush != null)
                        {
                            battleRush.laneObjects.remove(energyOrbPower);
                        }
                    }
                    else
                    {
                        Logger.LogError("energyOrbPower null");
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
                    uiData.energyOrbPower.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            {
                if (data is EnergyOrbPower)
                {
                    EnergyOrbPower energyOrbPower = data as EnergyOrbPower;
                    // Parent
                    {
                        DataUtils.addParentCallBack(energyOrbPower, this, ref this.battleRush);
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
                    uiData.energyOrbPower.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // Child
            {
                if (data is EnergyOrbPower)
                {
                    EnergyOrbPower energyOrbPower = data as EnergyOrbPower;
                    // Parent
                    {
                        DataUtils.removeParentCallBack(energyOrbPower, this, ref this.battleRush);
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
                    case UIData.Property.energyOrbPower:
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
                if (wrapProperty.p is EnergyOrbPower)
                {
                    switch ((EnergyOrbPower.Property)wrapProperty.n)
                    {
                        case EnergyOrbPower.Property.state:
                            dirty = true;
                            break;
                        case EnergyOrbPower.Property.position:
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
            Logger.Log("EnergyOrbPower onTriggerEnter: " + collider);
            if (this.data != null)
            {
                this.data.colliderEnters.add(collider);
                refresh();
            }
        }

        private void OnTriggerExit(Collider collider)
        {
            Logger.Log("EnergyOrbPower onTriggerEnter: " + collider);
            if (this.data != null)
            {
                this.data.colliderEnters.remove(collider);
                refresh();
            }
        }

    }
}
