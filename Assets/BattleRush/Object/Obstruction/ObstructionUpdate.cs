using System.Collections;
using System.Collections.Generic;
using BattleRushS.HeroS;
using UnityEngine;

namespace BattleRushS.ObjectS
{
    public class ObstructionUpdate : UpdateBehavior<Obstruction>
    {

        #region Update

        public override void update()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    BattleRush battleRush = this.data.findDataInParent<BattleRush>();
                    if (battleRush != null)
                    {
                        // hero
                        {
                            foreach (uint heroId in this.data.takingDamageHeroIds.vs)
                            {
                                if (!this.data.alreadyTakeDamageHeroIds.vs.Contains(heroId))
                                {
                                    // find hero
                                    Hero hero = null;
                                    {
                                        if (battleRush.hero.v.uid == heroId)
                                        {
                                            hero = battleRush.hero.v;
                                        }
                                    }
                                    // process
                                    if (hero != null)
                                    {
                                        switch (this.data.damageType.v)
                                        {
                                            case Obstruction.DamageType.OneHitDie:
                                                {
                                                    hero.hitPoint.v = 0;
                                                }
                                                break;
                                            case Obstruction.DamageType.HalfHitPoint:
                                                {
                                                    hero.hitPoint.v = Mathf.Max(0.0f, hero.hitPoint.v - 0.5f);
                                                }
                                                break;
                                            default:
                                                Logger.LogError("unknown damage type: " + this.data.damageType.v);
                                                break;
                                        }
                                        this.data.alreadyTakeDamageHeroIds.vs.Add(heroId);
                                    }
                                    else
                                    {
                                        Logger.LogError("hero null");
                                    }
                                }
                                else
                                {
                                    Logger.Log("already make damage to hero");
                                }
                            }
                        }
                        // troop
                        {
                            foreach (uint troopId in this.data.takingDamageTroopIds.vs)
                            {
                                if (!this.data.alreadyTakeDamageTroopIds.vs.Contains(troopId))
                                {
                                    // find troop
                                    TroopFollow troopFollow = null;
                                    {
                                        troopFollow = battleRush.hero.v.troopFollows.vs.Find(check => check.uid == troopId);
                                    }
                                    // process
                                    if (troopFollow != null)
                                    {
                                        switch (this.data.damageType.v)
                                        {
                                            case Obstruction.DamageType.OneHitDie:
                                                {
                                                    troopFollow.hitPoint.v -= 1;
                                                }
                                                break;
                                            case Obstruction.DamageType.HalfHitPoint:
                                                {
                                                    troopFollow.hitPoint.v -= 0.5f;
                                                }
                                                break;
                                            default:
                                                Logger.LogError("unknown damage type: " + this.data.damageType.v);
                                                break;
                                        }
                                        this.data.alreadyTakeDamageTroopIds.vs.Add(troopId);
                                    }
                                    else
                                    {
                                        Logger.LogError("troop null");
                                    }
                                }
                                else
                                {
                                    Logger.Log("already make damage to troop");
                                }
                            }
                        }
                    }
                    else
                    {
                        Logger.LogError("hero null");
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
            if(data is Obstruction)
            {
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if(data is Obstruction)
            {
                Obstruction obstruction = data as Obstruction;
                this.setDataNull(obstruction);
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
            if(wrapProperty.p is Obstruction)
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
            Logger.LogError("Don't process: " + data + "; " + syncs + "; " + this);
        }

        #endregion

    }
}