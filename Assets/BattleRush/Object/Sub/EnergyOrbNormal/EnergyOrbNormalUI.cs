using System.Collections;
using System.Collections.Generic;
using Dreamteck.Forever;
using UnityEngine;

namespace BattleRushS.ObjectS
{
    public class EnergyOrbNormalUI : UIBehavior<EnergyOrbNormalUI.UIData>, BattleRushUI.UIData.ObjectInPathUIInterface
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

            public VO<ReferenceData<EnergyOrbNormal>> energyOrbNormal;

            public LO<Collider> colliderEnters;

            #region Constructor

            public enum Property
            {
                energyOrbNormal,
                colliderEnters
            }

            public UIData() : base()
            {
                this.energyOrbNormal = new VO<ReferenceData<EnergyOrbNormal>>(this, (byte)Property.energyOrbNormal, new ReferenceData<EnergyOrbNormal>(null));
                this.colliderEnters = new LO<Collider>(this, (byte)Property.colliderEnters);
            }

            #endregion

            public override ObjectInPath.Type getType()
            {
                return ObjectInPath.Type.EnergyOrbNormal;
            }

            public override ObjectInPath getObjectInPath()
            {
                return this.energyOrbNormal.v.data;
            }

        }

        #endregion

        #region Refresh

        private LevelGenerator levelGenerator = null;

        public GameObject orb;
        public ParticleSystem pickUpEffect;

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    EnergyOrbNormal energyOrbNormal = this.data.energyOrbNormal.v.data;
                    if (energyOrbNormal != null)
                    {
                        // set position
                        {
                            // find levelGenerator
                            if (levelGenerator == null)
                            {
                                BattleRush battleRush = energyOrbNormal.findDataInParent<BattleRush>();
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
                                /*Vector3 position = MakeObjectUpdate.GetPositionInLevelGenerator(levelGenerator, energyOrbNormal.P.v.z);
                                this.transform.position = new Vector3(position.x, position.y + 0.9f, position.z);*/
                            }
                        }
                        // collider enter
                        {
                            switch (energyOrbNormal.state.v)
                            {
                                case EnergyOrbNormal.State.Normal:
                                    {
                                        // check is picked up
                                        bool isPickedUp = false;
                                        {
                                            foreach (Collider collider in this.data.colliderEnters.vs)
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
                                                                BattleRush battleRush = energyOrbNormal.findDataInParent<BattleRush>();
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
                                            Logger.Log("energyOrbNormal pickup");
                                            energyOrbNormal.state.v = EnergyOrbNormal.State.PickUp;
                                        }
                                    }
                                    break;
                                case EnergyOrbNormal.State.PickUp:
                                    {

                                    }
                                    break;
                                case EnergyOrbNormal.State.PickedUpFinish:
                                    break;
                            }
                        }
                        // orb
                        {
                            if (orb != null)
                            {
                                switch (energyOrbNormal.state.v)
                                {
                                    case EnergyOrbNormal.State.Normal:
                                        {
                                            orb.SetActive(true);
                                        }
                                        break;
                                    case EnergyOrbNormal.State.PickUp:
                                    case EnergyOrbNormal.State.PickedUpFinish:
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
                                switch (energyOrbNormal.state.v)
                                {
                                    case EnergyOrbNormal.State.Normal:
                                        {
                                            pickUpEffect.gameObject.SetActive(false);
                                        }
                                        break;
                                    case EnergyOrbNormal.State.PickUp:
                                        {
                                            pickUpEffect.gameObject.SetActive(true);
                                            pickUpEffect.Play();
                                            Logger.Log("energyOrbNormal pickup effect");
                                            energyOrbNormal.state.v = EnergyOrbNormal.State.PickedUpFinish;
                                        }
                                        break;
                                    case EnergyOrbNormal.State.PickedUpFinish:
                                        {
                                            pickUpEffect.gameObject.SetActive(true);
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
                    EnergyOrbNormal energyOrbNormal = this.data.energyOrbNormal.v.data;
                    if (energyOrbNormal != null)
                    {
                        BattleRush battleRush = energyOrbNormal.findDataInParent<BattleRush>();
                        if (battleRush != null)
                        {
                            battleRush.laneObjects.remove(energyOrbNormal);
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
            if (data is UIData)
            {
                UIData uiData = data as UIData;
                // Child
                {
                    uiData.energyOrbNormal.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            {
                if (data is EnergyOrbNormal)
                {
                    EnergyOrbNormal energyOrbNormal = data as EnergyOrbNormal;
                    // Parent
                    {
                        DataUtils.addParentCallBack(energyOrbNormal, this, ref this.battleRush);
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
                    uiData.energyOrbNormal.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // Child
            {
                if (data is EnergyOrbNormal)
                {
                    EnergyOrbNormal energyOrbNormal = data as EnergyOrbNormal;
                    // Parent
                    {
                        DataUtils.removeParentCallBack(energyOrbNormal, this, ref this.battleRush);
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
                    case UIData.Property.energyOrbNormal:
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
                if (wrapProperty.p is EnergyOrbNormal)
                {
                    switch ((EnergyOrbNormal.Property)wrapProperty.n)
                    {
                        case EnergyOrbNormal.Property.state:
                            dirty = true;
                            break;
                        case EnergyOrbNormal.Property.P:
                            dirty = true;
                            break;
                        case EnergyOrbNormal.Property.R:
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
            Logger.Log("EnergyOrbNormal onTriggerEnter: " + collider);
            if (this.data != null)
            {
                this.data.colliderEnters.add(collider);
                refresh();
            }
        }

        private void OnTriggerExit(Collider collider)
        {
            Logger.Log("EnergyOrbNormal onTriggerEnter: " + collider);
            if (this.data != null)
            {
                this.data.colliderEnters.remove(collider);
                refresh();
            }
        }

    }
}