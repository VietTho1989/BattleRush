using System.Collections;
using System.Collections.Generic;
using BattleRushS.HeroS;
using UnityEngine;

namespace BattleRushS.ObjectS
{
    public class ObstructionUI : UIBehavior<ObstructionUI.UIData>
    {

        #region UIData

        public class UIData : Data
        {

            public VO<ReferenceData<Obstruction>> obstruction;

            public LO<Collider> colliderEnters;

            #region Constructor

            public enum Property
            {
                obstruction,
                colliderEnters
            }

            public UIData() : base()
            {
                this.obstruction = new VO<ReferenceData<Obstruction>>(this, (byte)Property.obstruction, new ReferenceData<Obstruction>(null));
                this.colliderEnters = new LO<Collider>(this, (byte)Property.colliderEnters);
            }

            #endregion

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
                    Obstruction obstruction = this.data.obstruction.v.data;
                    if (obstruction != null)
                    {
                        foreach(Collider collider in this.data.colliderEnters.vs)
                        {
                            if (!collider)
                            {
                                // collider already destroyed
                                continue;
                            }
                            // hero
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
                                            if(obstruction.alreadyTakeDamageHeroIds.vs.Contains(hero.uid) || obstruction.takingDamageHeroIds.vs.Contains(hero.uid))
                                            {
                                                // already add
                                                Logger.Log("obstruction already add hero");
                                            }
                                            else
                                            {
                                                obstruction.takingDamageHeroIds.add(hero.uid);
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
                                else
                                {
                                    Logger.LogError("heroUI null");
                                }
                                // troops
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
                                                    if (obstruction.alreadyTakeDamageTroopIds.vs.Contains(troopFollow.uid) || obstruction.takingDamageTroopIds.vs.Contains(troopFollow.uid))
                                                    {
                                                        // already add
                                                        Logger.Log("obstruction already add troop");
                                                    }
                                                    else
                                                    {
                                                        Logger.Log("troop follow meet obstruction");
                                                        obstruction.takingDamageTroopIds.add(troopFollow.uid);
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
                                            Logger.Log("troopFollowUI null");
                                        }
                                    }
                                }
                            }
                        }                        
                    }
                    else
                    {
                        Logger.LogError("obstruction null");
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

        #region implement callBacks

        public override void onAddCallBack<T>(T data)
        {
            if(data is UIData)
            {
                UIData uiData = data as UIData;
                // Child
                {
                    uiData.obstruction.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            if(data is Obstruction)
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
                    uiData.obstruction.allRemoveCallBack(this);
                }
                return;
            }
            // Child
            if (data is Obstruction)
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
                    case UIData.Property.obstruction:
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
            if (wrapProperty.p is Obstruction)
            {
                switch ((Obstruction.Property)wrapProperty.n)
                {
                    case Obstruction.Property.damageType:
                        dirty = true;
                        break;
                    case Obstruction.Property.takingDamageHeroIds:
                        dirty = true;
                        break;
                    case Obstruction.Property.takingDamageTroopIds:
                        dirty = true;
                        break;
                    case Obstruction.Property.alreadyTakeDamageHeroIds:
                        dirty = true;
                        break;
                    case Obstruction.Property.alreadyTakeDamageTroopIds:
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

        private void OnTriggerEnter(Collider collider)
        {
            Logger.Log("ObstructionUI onTriggerEnter: " + collider + ", " + this.gameObject);
            if (this.data != null)
            {
                this.data.colliderEnters.add(collider);
                refresh();
            }
            else
            {
                Logger.LogError("data null");
            }
        }

        private void OnTriggerExit(Collider collider)
        {
            Logger.Log("ObstructionUI onTriggerExit: " + collider + ", " + this.gameObject);
            if (this.data != null)
            {
                this.data.colliderEnters.remove(collider);
                refresh();
            }
            else
            {
                Logger.LogError("data null");
            }
        }

    }
}