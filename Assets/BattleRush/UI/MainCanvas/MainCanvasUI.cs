using System.Collections;
using System.Collections.Generic;
using BattleRushS.ArenaS;
using UnityEngine;

namespace BattleRushS
{
    public class MainCanvasUI : UIBehavior<MainCanvasUI.UIData>
    {

        #region UIData

        public class UIData : Data
        {

            #region Constructor

            public enum Property
            {

            }

            public UIData() : base()
            {

            }

            #endregion

        }

        #endregion

        #region Refresh

        public GameObject btnSetting;

        public GameObject btnRemoveAds;

        // start: bottom left
        public GameObject btnHeroes;
        public GameObject btnEvolution;
        public GameObject btnTroops;
        // start: bottom right
        public GameObject btnMission;
        public GameObject btnUpgrade;

        // play information
        public TMPro.TextMeshProUGUI tvCoin;
        public TMPro.TextMeshProUGUI tvNormalOrb;
        public TMPro.TextMeshProUGUI tvEnergyOrb;
        public TMPro.TextMeshProUGUI tvTroop;

        public TMPro.TextMeshProUGUI tvLevel;
        public TMPro.TextMeshProUGUI tvTotalCoin;

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    BattleRushUI.UIData battleRushUIData = this.data.findDataInParent<BattleRushUI.UIData>();
                    if (battleRushUIData != null)
                    {
                        BattleRush battleRush = battleRushUIData.battleRush.v.data;
                        if (battleRush != null)
                        {
                            // btnSetting
                            {
                                if (btnSetting != null)
                                {
                                    switch (battleRush.state.v.getType())
                                    {
                                        case BattleRush.State.Type.Load:
                                        case BattleRush.State.Type.Edit:
                                            {
                                                btnSetting.transform.parent.gameObject.SetActive(false);
                                            }
                                            break;
                                        case BattleRush.State.Type.Start:
                                        case BattleRush.State.Type.Play:
                                        case BattleRush.State.Type.End:
                                            {
                                                btnSetting.transform.parent.gameObject.SetActive(true);
                                            }
                                            break;
                                        default:
                                            Logger.LogError("unknown type: " + battleRush.state.v.getType());
                                            break;
                                    }
                                }
                                else
                                {
                                    Logger.LogError("btnSetting null");
                                }
                            }
                            // btnRemoveAds
                            {
                                if (btnRemoveAds != null)
                                {
                                    btnRemoveAds.SetActive(!battleRush.adsBanner.v.removeAds.v && battleRush.state.v.getType() == BattleRush.State.Type.Start);
                                }
                                else
                                {
                                    Logger.LogError("btnRemoveAds null");
                                }
                            }
                            // tvLevel
                            {
                                if (tvLevel != null)
                                {
                                    tvLevel.text = "" + battleRush.level.v;
                                }
                                else
                                {
                                    Logger.LogError("tvLevel null");
                                }
                            }
                            // tvTotalCoin
                            {
                                if (tvTotalCoin != null)
                                {
                                    switch (battleRush.state.v.getType())
                                    {
                                        case BattleRush.State.Type.Load:
                                        case BattleRush.State.Type.Edit:
                                            {
                                                tvTotalCoin.transform.parent.gameObject.SetActive(false);
                                            }
                                            break;
                                        case BattleRush.State.Type.Start:                                            
                                        case BattleRush.State.Type.Play:
                                        case BattleRush.State.Type.End:
                                            {
                                                tvTotalCoin.transform.parent.gameObject.SetActive(true);
                                                tvTotalCoin.text = "" + battleRush.totalCoin.v;
                                            }
                                            break;
                                        default:
                                            Logger.LogError("unknown type: " + battleRush.state.v.getType());
                                            break;
                                    }
                                }
                                else
                                {
                                    Logger.LogError("tvTotalCoin null");
                                }
                            }

                            // start main left
                            {
                                if (btnHeroes != null)
                                {
                                    btnHeroes.SetActive(battleRush.state.v.getType() == BattleRush.State.Type.Start);
                                }
                                else
                                {
                                    Logger.LogError("btnHeroes null");
                                }
                                if (btnEvolution != null)
                                {
                                    btnEvolution.SetActive(battleRush.state.v.getType() == BattleRush.State.Type.Start);
                                }
                                else
                                {
                                    Logger.LogError("btnEvolution null");
                                }
                                if (btnTroops != null)
                                {
                                    btnTroops.SetActive(battleRush.state.v.getType() == BattleRush.State.Type.Start);
                                }
                                else
                                {
                                    Logger.LogError("btnTroops null");
                                }
                                if (btnMission != null)
                                {
                                    btnMission.SetActive(battleRush.state.v.getType() == BattleRush.State.Type.Start);
                                }
                                else
                                {
                                    Logger.LogError("btnMission null");
                                }
                                if (btnUpgrade != null)
                                {
                                    btnUpgrade.SetActive(battleRush.state.v.getType() == BattleRush.State.Type.Start);
                                }
                                else
                                {
                                    Logger.LogError("btnUpgrade null");
                                }
                            }
                            // play information
                            {
                                if (tvCoin != null)
                                {
                                    switch (battleRush.state.v.getType())
                                    {
                                        case BattleRush.State.Type.Load:
                                        case BattleRush.State.Type.Edit:
                                        case BattleRush.State.Type.Start:
                                            {
                                                tvCoin.transform.parent.gameObject.SetActive(false);
                                            }
                                            break;
                                        case BattleRush.State.Type.Play:
                                        case BattleRush.State.Type.End:
                                            {
                                                tvCoin.transform.parent.gameObject.SetActive(true);
                                                tvCoin.text = "" + battleRush.levelCoin.v;
                                            }
                                            break;
                                        default:
                                            Logger.LogError("unknown type: " + battleRush.state.v.getType());
                                            break;
                                    }
                                }
                                else
                                {
                                    Logger.LogError("tvCoin null");
                                }
                                if (tvNormalOrb != null)
                                {
                                    switch (battleRush.state.v.getType())
                                    {
                                        case BattleRush.State.Type.Load:
                                        case BattleRush.State.Type.Edit:
                                        case BattleRush.State.Type.Start:
                                            {
                                                tvNormalOrb.transform.parent.gameObject.SetActive(false);
                                            }
                                            break;
                                        case BattleRush.State.Type.Play:
                                        case BattleRush.State.Type.End:
                                            {
                                                tvNormalOrb.transform.parent.gameObject.SetActive(true);
                                                tvNormalOrb.text = "" + battleRush.levelNormalOrb.v;
                                            }
                                            break;
                                        default:
                                            Logger.LogError("unknown type: " + battleRush.state.v.getType());
                                            break;
                                    }
                                }
                                else
                                {
                                    Logger.LogError("tvNormalOrb null");
                                }
                                if (tvEnergyOrb != null)
                                {
                                    switch (battleRush.state.v.getType())
                                    {
                                        case BattleRush.State.Type.Load:
                                        case BattleRush.State.Type.Edit:
                                        case BattleRush.State.Type.Start:
                                            {
                                                tvEnergyOrb.transform.parent.gameObject.SetActive(false);
                                            }
                                            break;
                                        case BattleRush.State.Type.Play:
                                        case BattleRush.State.Type.End:
                                            {
                                                tvEnergyOrb.transform.parent.gameObject.SetActive(true);
                                                tvEnergyOrb.text = "" + battleRush.levelEnergyOrb.v;
                                            }
                                            break;
                                        default:
                                            Logger.LogError("unknown type: " + battleRush.state.v.getType());
                                            break;
                                    }
                                }
                                else
                                {
                                    Logger.LogError("tvEnergyOrb null");
                                }
                                if (tvTroop != null)
                                {
                                    switch (battleRush.state.v.getType())
                                    {
                                        case BattleRush.State.Type.Load:
                                        case BattleRush.State.Type.Edit:
                                        case BattleRush.State.Type.Start:
                                            {
                                                tvTroop.transform.parent.gameObject.SetActive(false);
                                            }
                                            break;
                                        case BattleRush.State.Type.Play:
                                        case BattleRush.State.Type.End:
                                            {
                                                tvTroop.transform.parent.gameObject.SetActive(true);
                                                // find count
                                                int count = 0;
                                                {
                                                    switch (battleRush.hero.v.heroMove.v.sub.v.getType())
                                                    {
                                                        case HeroS.HeroMove.Sub.Type.Run:
                                                            {
                                                                count = battleRush.hero.v.troopFollows.vs.Count;
                                                            }
                                                            break;
                                                        case HeroS.HeroMove.Sub.Type.Arena:
                                                            {
                                                                // find
                                                                Arena arenaFightCurrentOn = null;
                                                                {
                                                                    foreach (Arena check in battleRush.arenas.vs)
                                                                    {
                                                                        ArenaUI arenaUI = check.findCallBack<ArenaUI>();
                                                                        if (arenaUI != null)
                                                                        {
                                                                            Segment segment = arenaUI.GetComponentInParent<Segment>();
                                                                            if (segment != null)
                                                                            {
                                                                                if (segment == battleRush.hero.v.heroMove.v.currentSegment.v)
                                                                                {
                                                                                    arenaFightCurrentOn = check;
                                                                                    break;
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                Logger.LogError("levelSegment null");
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            Logger.LogError("arenaUI null");
                                                                        }
                                                                    }
                                                                }
                                                                // process
                                                                if (arenaFightCurrentOn != null)
                                                                {
                                                                    // find
                                                                    int troopCount = 0;
                                                                    {
                                                                        foreach(Troop troop in arenaFightCurrentOn.troops.vs)
                                                                        {
                                                                            if(troop.teamId.v==0 && troop.troopType.v.getType()== TroopType.Type.Normal && troop.state.v.getType()== Troop.State.Type.Live)
                                                                            {
                                                                                troopCount++;
                                                                            }
                                                                        }
                                                                    }
                                                                    // set
                                                                    count = troopCount;
                                                                }
                                                                else
                                                                {
                                                                    Logger.LogError("arenaFightCurrentOn null");
                                                                    count = battleRush.hero.v.troopFollows.vs.Count;
                                                                }
                                                            }
                                                            break;
                                                        default:
                                                            break;
                                                    }
                                                }
                                                // process
                                                tvTroop.text = "" + count;
                                            }
                                            break;
                                        default:
                                            Logger.LogError("unknown type: " + battleRush.state.v.getType());
                                            break;
                                    }
                                }
                                else
                                {
                                    Logger.LogError("tvTroop null");
                                }
                            }
                        }
                        else
                        {
                            Logger.LogError("battleRush null");
                        }
                    }
                    else
                    {
                        Logger.LogError("battleRushUIData null");
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

        private BattleRushUI.UIData battleRushUIData = null;

        public override void onAddCallBack<T>(T data)
        {
            if(data is UIData)
            {
                UIData uiData = data as UIData;
                // Parent
                {
                    DataUtils.addParentCallBack(uiData, this, ref this.battleRushUIData);
                }
                dirty = true;
                return;
            }
            // Parent
            {
                if(data is BattleRushUI.UIData)
                {
                    BattleRushUI.UIData battleRushUIData = data as BattleRushUI.UIData;
                    // Child
                    {
                        battleRushUIData.battleRush.allAddCallBack(this);
                    }
                    dirty = true;
                    return;
                }
                // Child
                {
                    if (data is BattleRush)
                    {
                        BattleRush battleRush = data as BattleRush;
                        // Child
                        {
                            battleRush.adsBanner.allAddCallBack(this);
                            battleRush.hero.allAddCallBack(this);
                            battleRush.arenas.allAddCallBack(this);
                        }
                        dirty = true;
                        return;
                    }
                    // Child
                    {
                        if (data is AdsBannerController)
                        {
                            dirty = true;
                            return;
                        }
                        if(data is Hero)
                        {
                            dirty = true;
                            return;
                        }
                        // arena
                        {
                            if(data is Arena)
                            {
                                Arena arena = data as Arena;
                                // Child
                                {
                                    arena.troops.allAddCallBack(this);
                                }
                                dirty = true;
                                return;
                            }
                            // Child
                            if(data is Troop)
                            {
                                dirty = true;
                                return;
                            }
                        }
                    }
                }               
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if(data is UIData)
            {
                UIData uiData = data as UIData;
                // Parent
                {
                    DataUtils.removeParentCallBack(uiData, this, ref this.battleRushUIData);
                }
                this.setDataNull(uiData);
                return;
            }
            // Parent
            {
                if (data is BattleRushUI.UIData)
                {
                    BattleRushUI.UIData battleRushUIData = data as BattleRushUI.UIData;
                    // Child
                    {
                        battleRushUIData.battleRush.allRemoveCallBack(this);
                    }
                    return;
                }
                // Child
                {
                    if (data is BattleRush)
                    {
                        BattleRush battleRush = data as BattleRush;
                        // Child
                        {
                            battleRush.adsBanner.allRemoveCallBack(this);
                            battleRush.hero.allRemoveCallBack(this);
                            battleRush.arenas.allRemoveCallBack(this);
                        }
                        return;
                    }
                    // Child
                    {
                        if (data is AdsBannerController)
                        {
                            return;
                        }
                        if(data is Hero)
                        {
                            return;
                        }
                        // arena
                        {
                            if (data is Arena)
                            {
                                Arena arena = data as Arena;
                                // Child
                                {
                                    arena.troops.allRemoveCallBack(this);
                                }
                                return;
                            }
                            // Child
                            if (data is Troop)
                            {
                                return;
                            }
                        }
                    }
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
                return;
            }
            // Parent
            {
                if (wrapProperty.p is BattleRushUI.UIData)
                {
                    switch ((BattleRushUI.UIData.Property)wrapProperty.n)
                    {
                        case BattleRushUI.UIData.Property.battleRush:
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
                    if (wrapProperty.p is BattleRush)
                    {
                        switch ((BattleRush.Property)wrapProperty.n)
                        {
                            case BattleRush.Property.state:
                                dirty = true;
                                break;
                            case BattleRush.Property.adsBanner:
                                {
                                    ValueChangeUtils.replaceCallBack(this, syncs);
                                    dirty = true;
                                }
                                break;
                            case BattleRush.Property.level:
                                dirty = true;
                                break;
                            case BattleRush.Property.levelCoin:
                                dirty = true;
                                break;
                            case BattleRush.Property.levelNormalOrb:
                                dirty = true;
                                break;
                            case BattleRush.Property.levelEnergyOrb:
                                dirty = true;
                                break;
                            case BattleRush.Property.totalCoin:
                                dirty = true;
                                break;
                            case BattleRush.Property.hero:
                                {
                                    ValueChangeUtils.replaceCallBack(this, syncs);
                                    dirty = true;
                                }
                                break;
                            case BattleRush.Property.arenas:
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
                        if (wrapProperty.p is AdsBannerController)
                        {
                            switch ((AdsBannerController.Property)wrapProperty.n)
                            {
                                case AdsBannerController.Property.removeAds:
                                    dirty = true;
                                    break;
                                default:
                                    break;
                            }
                            return;
                        }
                        if(wrapProperty.p is Hero)
                        {
                            switch ((Hero.Property)wrapProperty.n)
                            {
                                case Hero.Property.troopFollows:
                                    dirty = true;
                                    break;
                                default:
                                    break;
                            }
                            return;
                        }
                        // arena
                        {
                            if (wrapProperty.p is Arena)
                            {
                                switch ((Arena.Property)wrapProperty.n)
                                {
                                    case Arena.Property.troops:
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
                            if (wrapProperty.p is Troop)
                            {
                                switch ((Troop.Property)wrapProperty.n)
                                {
                                    case Troop.Property.teamId:
                                        dirty = true;
                                        break;
                                    case Troop.Property.troopType:
                                        dirty = true;
                                        break;
                                    case Troop.Property.state:
                                        dirty = true;
                                        break;
                                    default:
                                        break;
                                }
                                return;
                            }
                        }
                    }                    
                }               
            }
            Logger.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion
        
        public void onClickBtnRemoveAds()
        {
            if (this.data != null)
            {
                BattleRushUI.UIData battleRushUIData = this.data.findDataInParent<BattleRushUI.UIData>();
                if (battleRushUIData != null)
                {
                    BattleRush battleRush = battleRushUIData.battleRush.v.data;
                    if (battleRush != null)
                    {
                        battleRush.adsBanner.v.removeAds.v = true;
                    }
                    else
                    {
                        Logger.LogError("battleRush null");
                    }
                }
                else
                {
                    Logger.LogError("battleRushUIData null");
                }
            }
            else
            {
                Logger.LogError("data null");
            }
        }

        public void onClickBtnUpgrade()
        {
            if (this.data != null)
            {
                BattleRushUI.UIData battleRushUIData = this.data.findDataInParent<BattleRushUI.UIData>();
                if (battleRushUIData != null)
                {
                    BattleRush battleRush = battleRushUIData.battleRush.v.data;
                    if (battleRush != null)
                    {
                        switch (battleRush.state.v.getType())
                        {
                            case BattleRush.State.Type.Start:
                                {
                                    // find cost
                                    int cost = 500;
                                    {
                                        // TODO can hoan thien
                                    }
                                    // process
                                    if (battleRush.totalCoin.v >= cost)
                                    {
                                        battleRush.hero.v.level.v++;
                                        battleRush.totalCoin.v -= cost;
                                    }
                                }
                                break;
                            default:
                                Logger.LogError("error state: " + battleRush.state.v.getType());
                                break;
                        }
                    }
                    else
                    {
                        Logger.LogError("battleRush null");
                    }
                }
                else
                {
                    Logger.LogError("battleRushUIData null");
                }
            }
            else
            {
                Logger.LogError("data null");
            }
        }
    }
}