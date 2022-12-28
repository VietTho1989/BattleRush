using System.Collections;
using System.Collections.Generic;
using BattleRushS.HeroS;
using UnityEngine;

namespace BattleRushS.ArenaS
{
    public class PreBattleUpdate : UpdateBehavior<PreBattle>
    {

        #region Update

        public override void update()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    // make hero's troop
                    {
                        Arena arena = this.data.findDataInParent<Arena>();
                        if (arena != null)
                        {
                            BattleRush battleRush = this.data.findDataInParent<BattleRush>();
                            if (battleRush != null)
                            {
                                // hero
                                {
                                    Hero hero = battleRush.hero.v;
                                    // make hero troop
                                    {
                                        Troop troop = new Troop();
                                        {
                                            troop.uid = arena.troops.makeId();
                                            troop.teamId.v = 0;
                                            troop.troopType.v = hero.troopType.v;
                                            troop.hitpoint.v = 1.0f;
                                        }
                                        arena.troops.add(troop);
                                    }
                                }
                                // troop
                                foreach (TroopFollow troopFollow in battleRush.hero.v.troopFollows.vs)
                                {
                                    Troop troop = new Troop();
                                    {
                                        troop.uid = arena.troops.makeId();
                                        troop.teamId.v = 0;
                                        troop.troopType.v = troopFollow.troopType.v;
                                        troop.hitpoint.v = troopFollow.hitPoint.v;
                                    }
                                    arena.troops.add(troop);
                                }
                            }
                            else
                            {
                                Logger.LogError("battleRush null");
                            }
                        }
                        else
                        {
                            Logger.LogError("arena null");
                        }                        
                    }
                    // make enemy's troop
                    {
                        Arena arena = this.data.findDataInParent<Arena>();
                        if (arena != null)
                        {
                            // get
                            List<TroopFollow.TroopType> enemyTroopTypes = new List<TroopFollow.TroopType>();
                            {
                                foreach (TroopFollow.TroopType check in System.Enum.GetValues(typeof(TroopFollow.TroopType)))
                                {
                                    if (!TroopFollow.IsHero(check))
                                    {
                                        enemyTroopTypes.Add(check);
                                    }
                                }
                            }
                            // make
                            for (int i = 0; i < Random.Range(5, 12); i++)
                            {
                                Troop troop = new Troop();
                                {
                                    troop.uid = arena.troops.makeId();
                                    troop.teamId.v = 1;
                                    // troopType
                                    {
                                        if (enemyTroopTypes.Count > 0)
                                        {
                                            troop.troopType.v = enemyTroopTypes[Random.Range(0, enemyTroopTypes.Count)];
                                        }
                                        else
                                        {
                                            Logger.LogError("enemyTroopTypes null");
                                        }                                       
                                    }                                   
                                    troop.hitpoint.v = 1.0f;
                                }
                                arena.troops.add(troop);
                            }
                        }
                        else
                        {
                            Logger.LogError("arena null");
                        }                       
                    }
                    // change to next stage
                    {
                        Arena arena = this.data.findDataInParent<Arena>();
                        if (arena != null)
                        {
                            MoveTroopToFormation moveTroopToFormation = arena.stage.newOrOld<MoveTroopToFormation>();
                            {

                            }
                            arena.stage.v = moveTroopToFormation;
                        }
                        else
                        {
                            Logger.LogError("arena null");
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

        public override void onAddCallBack<T>(T data)
        {
            if(data is PreBattle)
            {
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if(data is PreBattle)
            {
                PreBattle preBattle = data as PreBattle;
                this.setDataNull(preBattle);
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
            if(wrapProperty.p is PreBattle)
            {
                switch ((PreBattle.Property)wrapProperty.n)
                {
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