
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ArenaS
{
    public class ArenaUI : UIBehavior<ArenaUI.UIData>
    {

        #region UIData

        public class UIData : Data
        {

            public VO<ReferenceData<Arena>> arena;

            public LD<TroopUI.UIData> troops;

            #region Constructor

            public enum Property
            {
                arena,
                troops
            }

            public UIData() : base()
            {
                this.arena = new VO<ReferenceData<Arena>>(this, (byte)Property.arena, new ReferenceData<Arena>(null));
                this.troops = new LD<TroopUI.UIData>(this, (byte)Property.troops);
            }

            #endregion

        }

        #endregion

        public override void Awake()
        {
            base.Awake();          
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            var navMesh = UnityEngine.AI.NavMesh.CalculateTriangulation();
            if (this.data != null)
            {
                Arena arena = this.data.arena.v.data;
                if (arena != null)
                {
                    BattleRushUI.UIData battleRushUIData = this.data.findDataInParent<BattleRushUI.UIData>();
                    if (battleRushUIData != null)
                    {
                        // remove data
                        {
                            BattleRush battleRush = battleRushUIData.battleRush.v.data;
                            if (battleRush != null)
                            {
                                battleRush.arenas.remove(arena);
                            }
                            else
                            {
                                Logger.LogError("battleRush null");
                            }
                        }
                        // remove ui data
                        battleRushUIData.arenas.remove(this.data);
                    }
                    else
                    {
                        Logger.LogError("battleRushUIData null");
                    }
                }
                else
                {
                    Logger.LogError("arena null");
                }                
            }
            else
            {
                Logger.LogError("data null");
            }
        }

        #region Refresh

        public Transform center;

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    Arena arena = this.data.arena.v.data;
                    if (arena != null)
                    {
                        Logger.Log("ArenaUI: make troops: " + arena.troops.vs.Count);
                        // troops
                        {
                            // get old 
                            List<TroopUI.UIData> olds = new List<TroopUI.UIData>();
                            {
                                olds.AddRange(this.data.troops.vs);
                            }
                            // Update
                            {
                                foreach (Troop troop in arena.troops.vs)
                                {
                                    // get UIData
                                    TroopUI.UIData troopUIData = null;
                                    bool needAdd = false;
                                    {
                                        // get old
                                        foreach (TroopUI.UIData check in olds)
                                        {
                                            if (check.troop.v.data == troop)
                                            {
                                                troopUIData = check;
                                            }
                                        }
                                        // make new
                                        if (troopUIData == null)
                                        {
                                            troopUIData = new TroopUI.UIData();
                                            {
                                                troopUIData.uid = this.data.troops.makeId();
                                            }
                                            needAdd = true;
                                        }
                                        else
                                        {
                                            olds.Remove(troopUIData);
                                        }
                                    }
                                    // update
                                    {
                                        troopUIData.troop.v = new ReferenceData<Troop>(troop);
                                    }
                                    // add
                                    if (needAdd)
                                    {
                                        this.data.troops.add(troopUIData);
                                    }
                                }
                            }
                            // remove old
                            foreach(TroopUI.UIData old in olds)
                            {
                                this.data.troops.remove(old);
                            }
                        }
                    }
                    else
                    {
                        Logger.LogError("arena null");
                    }
                }
                else
                {
                    Logger.LogError("ArenaUI: data null");
                    BattleRushUI battleRushUI = this.GetComponentInParent<BattleRushUI>();
                    if (battleRushUI != null)
                    {
                        BattleRushUI.UIData battleRushUIData = battleRushUI.data;
                        if (battleRushUIData != null)
                        {
                            BattleRush battleRush = battleRushUIData.battleRush.v.data;
                            if (battleRush != null)
                            {
                                UIData arenaUIData = new UIData();
                                {
                                    arenaUIData.uid = battleRushUIData.arenas.makeId();
                                    // arena
                                    {
                                        Arena arena = new Arena();
                                        {
                                            arena.uid = battleRush.arenas.makeId();
                                            battleRush.arenas.add(arena);
                                        }
                                        arenaUIData.arena.v = new ReferenceData<Arena>(arena);
                                    }
                                }
                                // set
                                {
                                    battleRushUIData.arenas.add(arenaUIData);
                                    this.setData(arenaUIData);
                                }                                
                            }
                            else
                            {
                                Logger.LogError("ArenaUI: battleRush null");
                            }
                        }
                        else
                        {
                            Logger.LogError("ArenaUI: battleRushUIData null");
                        }
                    }
                    else
                    {
                        Logger.LogError("ArenaUI: battleRushUI null");
                        dirty = true;
                    }
                }
            }
        }

        public override bool isShouldDisableUpdate()
        {
            return true;
        }

        #endregion

        #region implement callBacks

        public TroopUI troopPrefab;

        public override void onAddCallBack<T>(T data)
        {
            if(data is UIData)
            {
                UIData uiData = data as UIData;
                // Child
                {
                    uiData.arena.allAddCallBack(this);
                    uiData.troops.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            {
                if(data is Arena)
                {
                    dirty = true;
                    return;
                }
                if(data is TroopUI.UIData)
                {
                    TroopUI.UIData troopUIData = data as TroopUI.UIData;
                    // UI
                    {
                        UIUtils.Instantiate(troopUIData, troopPrefab, this.transform);
                    }
                    dirty = true;
                    return;
                }
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if(data is UIData)
            {
                UIData uiData = data as UIData;
                // Child
                {
                    uiData.arena.allRemoveCallBack(this);
                    uiData.troops.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // Child
            {
                if (data is Arena)
                {
                    return;
                }
                if (data is TroopUI.UIData)
                {
                    TroopUI.UIData troopUIData = data as TroopUI.UIData;
                    // UI
                    {
                        troopUIData.removeCallBackAndDestroy(typeof(TroopUI));
                    }
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
            if(wrapProperty.p is UIData)
            {
                switch ((UIData.Property)wrapProperty.n)
                {
                    case UIData.Property.arena:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case UIData.Property.troops:
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
            {
                if (wrapProperty.p is Arena)
                {
                    switch ((Arena.Property)wrapProperty.n)
                    {
                        case Arena.Property.troops:
                            dirty = true;
                            break;
                        default:
                            break;
                    }
                    return;
                }
                if (wrapProperty.p is TroopUI.UIData)
                {
                    return;
                }
            }
            Logger.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

    }
}