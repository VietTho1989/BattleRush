using System.Collections;
using System.Collections.Generic;
using BattleRushS.ArenaS.TroopS;
using BattleRushS.HeroS;
using UnityEngine;

namespace BattleRushS.ArenaS
{
    public class TakeDamageUpdate : UpdateBehavior<TakeDamage>
    {

        #region Update

        public override void update()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    Live live = this.data.findDataInParent<Live>();
                    if (live != null)
                    {
                        if (this.data.damages.vs.Count > 0)
                        {
                            // get
                            List<Damage> damages = new List<Damage>();
                            {
                                damages.AddRange(this.data.damages.vs);
                            }
                            // process
                            foreach (Damage damage in damages)
                            {
                                // reduce hitpoint
                                {
                                    if (live.hitpoint.v > 0)
                                    {
                                        // find percent
                                        float percent = 0.1f;
                                        {
                                            // find hp
                                            float hp = 100;
                                            {
                                                Troop troop = this.data.findDataInParent<Troop>();
                                                if (troop != null)
                                                {
                                                    if (troop.troopType.v != null)
                                                    {
                                                        switch (troop.troopType.v.getType())
                                                        {
                                                            case TroopType.Type.Hero:
                                                                {
                                                                    // TODO can hoan thien
                                                                }
                                                                break;
                                                            case TroopType.Type.Normal:
                                                                break;
                                                            case TroopType.Type.Monster:
                                                                {
                                                                    TroopInformation troopInformation = troop.troopType.v as TroopInformation;
                                                                    TroopInformation.Level level = troopInformation.levels.Find(check => check.level == troop.level.v);
                                                                    if (level != null)
                                                                    {
                                                                        hp = level.hp;
                                                                    }
                                                                    else
                                                                    {
                                                                        Logger.LogError("level null");
                                                                    }
                                                                }
                                                                break;
                                                            default:
                                                                Logger.LogError("unknown troop type: " + troop.troopType.v.getType());
                                                                break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Logger.LogError("troopType null");
                                                    }
                                                }
                                                else
                                                {
                                                    Logger.LogError("troop null");
                                                }
                                                // prevent = 0
                                                hp = Mathf.Max(hp, 1.0f);
                                            }
                                            percent = damage.damage.v / hp;
                                        }
                                        // process
                                        live.hitpoint.v = Mathf.Clamp(live.hitpoint.v - percent, 0, 1);
                                    }
                                }
                                // remove
                                {
                                    // find
                                    bool needRemove = true;
                                    {
                                        // TODO sau nay co the cai tien poison damange khong remove ngay
                                    }
                                    // process
                                    if (needRemove)
                                    {
                                        this.data.damages.remove(damage);
                                    }
                                }
                                // change state to die
                                if (live.hitpoint.v <= 0)
                                {
                                    Troop troop = this.data.findDataInParent<Troop>();
                                    if (troop != null)
                                    {
                                        Die die = troop.state.newOrOld<Die>();
                                        {

                                        }
                                        troop.state.v = die;
                                    }
                                    else
                                    {
                                        Logger.LogError("troop null");
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        Logger.LogError("troop null");
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
            if(data is TakeDamage)
            {
                TakeDamage takeDamage = data as TakeDamage;
                // Child
                {
                    takeDamage.damages.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            if(data is Damage)
            {
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if(data is TakeDamage)
            {
                TakeDamage takeDamage = data as TakeDamage;
                // Child
                {
                    takeDamage.damages.allRemoveCallBack(this);
                }
                this.setDataNull(takeDamage);
                return;
            }
            // Child
            if(data is Damage)
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
            if(wrapProperty.p is TakeDamage)
            {
                switch ((TakeDamage.Property)wrapProperty.n)
                {
                    case TakeDamage.Property.damages:
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
            // Child
            if(wrapProperty.p is Damage)
            {
                switch ((Damage.Property)wrapProperty.n)
                {
                    case Damage.Property.damage:
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