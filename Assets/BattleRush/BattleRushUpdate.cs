using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleRushS.StateS;
using BattleRushS.ArenaS;
using BattleRushS.ObjectS;

namespace BattleRushS
{
    public class BattleRushUpdate : UpdateBehavior<BattleRush>
    {

        public override void update()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    
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

        #region implement callBacks

        public override void onAddCallBack<T>(T data)
        {
            if (data is BattleRush)
            {
                BattleRush battleRush = data as BattleRush;
                // Child
                {
                    battleRush.state.allAddCallBack(this);
                    battleRush.hero.allAddCallBack(this);
                    battleRush.mapData.allAddCallBack(this);
                    battleRush.arenas.allAddCallBack(this);
                    battleRush.laneObjects.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Child
            {
                if (data is BattleRush.State)
                {
                    BattleRush.State state = data as BattleRush.State;
                    // Update
                    {
                        switch (state.getType())
                        {
                            case BattleRush.State.Type.Load:
                                {
                                    Load load = state as Load;
                                    UpdateUtils.makeUpdate<LoadUpdate, Load>(load, this.transform);
                                }
                                break;
                            case BattleRush.State.Type.Start:
                                {
                                    Start start = state as Start;
                                    UpdateUtils.makeUpdate<StartUpdate, Start>(start, this.transform);
                                }
                                break;
                            case BattleRush.State.Type.Play:
                                {
                                    Play play = state as Play;
                                    UpdateUtils.makeUpdate<PlayUpdate, Play>(play, this.transform);
                                }
                                break;
                            case BattleRush.State.Type.End:
                                {
                                    End end = state as End;
                                    UpdateUtils.makeUpdate<EndUpdate, End>(end, this.transform);
                                }
                                break;
                            default:
                                Logger.LogError("unknown type: " + state.getType());
                                break;
                        }
                    }
                    dirty = true;
                    return;
                }
                if(data is Hero)
                {
                    Hero hero = data as Hero;
                    // Update
                    {
                        UpdateUtils.makeUpdate<HeroUpdate, Hero>(hero, this.transform);
                    }
                    dirty = true;
                    return;
                }
                if(data is MapData)
                {
                    dirty = true;
                    return;
                }
                if(data is Arena)
                {
                    Arena arena = data as Arena;
                    // Update
                    {
                        UpdateUtils.makeUpdate<ArenaUpdate, Arena>(arena, this.transform);
                    }
                    dirty = true;
                    return;
                }
                if(data is ObjectInPath)
                {
                    ObjectInPath objectInPath = data as ObjectInPath;
                    // Update
                    {
                        switch (objectInPath.getType())
                        {
                            case ObjectInPath.Type.TroopCage:
                                {
                                    TroopCage troopCage = objectInPath as TroopCage;
                                    UpdateUtils.makeUpdate<TroopCageUpdate, TroopCage>(troopCage, this.transform);
                                }
                                break;
                            case ObjectInPath.Type.EnergyOrbNormal:
                                {
                                    EnergyOrbNormal energyOrbNormal = objectInPath as EnergyOrbNormal;
                                    UpdateUtils.makeUpdate<EnergyOrbNormalUpdate, EnergyOrbNormal>(energyOrbNormal, this.transform);
                                }
                                break;
                            case ObjectInPath.Type.EnergyOrbPower:
                                {
                                    EnergyOrbPower energyOrbPower = objectInPath as EnergyOrbPower;
                                    UpdateUtils.makeUpdate<EnergyOrbPowerUpdate, EnergyOrbPower>(energyOrbPower, this.transform);
                                }
                                break;
                            case ObjectInPath.Type.EnergyOrbUpgrade:
                                {
                                    EnergyOrbUpgrade energyOrbUpgrade = objectInPath as EnergyOrbUpgrade;
                                    UpdateUtils.makeUpdate<EnergyOrbUpgradeUpdate, EnergyOrbUpgrade>(energyOrbUpgrade, this.transform);
                                }
                                break;
                            case ObjectInPath.Type.OkgCoin:
                                {
                                    Coin coin = objectInPath as Coin;
                                    UpdateUtils.makeUpdate<CoinUpdate, Coin>(coin, this.transform);
                                }
                                break;
                            case ObjectInPath.Type.SawBlade:
                                {
                                    SawBlade sawBlade = objectInPath as SawBlade;
                                    UpdateUtils.makeUpdate<SawBladeUpdate, SawBlade>(sawBlade, this.transform);
                                }
                                break;
                            case ObjectInPath.Type.Blade:
                                {
                                    Blade blade = objectInPath as Blade;
                                    UpdateUtils.makeUpdate<BladeUpdate, Blade>(blade, this.transform);
                                }
                                break;
                            case ObjectInPath.Type.FireNozzle:
                                {
                                    FireNozzle fireNozzle = objectInPath as FireNozzle;
                                    UpdateUtils.makeUpdate<FireNozzleUpdate, FireNozzle>(fireNozzle, this.transform);
                                }
                                break;
                            case ObjectInPath.Type.Pike:
                                {
                                    Pike pike = objectInPath as Pike;
                                    UpdateUtils.makeUpdate<PikeUpdate, Pike>(pike, this.transform);
                                }
                                break;
                            case ObjectInPath.Type.UpgradeGateFree:
                                {
                                    UpgradeGateFree upgradeGateFree = objectInPath as UpgradeGateFree;
                                    UpdateUtils.makeUpdate<UpgradeGateFreeUpdate, UpgradeGateFree>(upgradeGateFree, this.transform);
                                }
                                break;
                            case ObjectInPath.Type.UpgradeGateCharge:
                                {
                                    UpgradeGateCharge upgradeGateCharge = objectInPath as UpgradeGateCharge;
                                    UpdateUtils.makeUpdate<UpgradeGateChargeUpdate, UpgradeGateCharge>(upgradeGateCharge, this.transform);
                                }
                                break;
                            case ObjectInPath.Type.Hammer:
                                {
                                    Hammer hammer = objectInPath as Hammer;
                                    UpdateUtils.makeUpdate<HammerUpdate, Hammer>(hammer, this.transform);
                                }
                                break;
                            case ObjectInPath.Type.Grinder:
                                {
                                    Grinder grinder = objectInPath as Grinder;
                                    UpdateUtils.makeUpdate<GrinderUpdate, Grinder>(grinder, this.transform);
                                }
                                break;
                            case ObjectInPath.Type.CocoonMantah:
                                {
                                    CocoonMantah cocoonMantah = objectInPath as CocoonMantah;
                                    UpdateUtils.makeUpdate<CocoonMantahUpdate, CocoonMantah>(cocoonMantah, this.transform);
                                }
                                break;
                        }
                    }
                    dirty = true;
                    return;
                }
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if (data is BattleRush)
            {
                BattleRush battleRush = data as BattleRush;
                // Child
                {
                    battleRush.state.allRemoveCallBack(this);
                    battleRush.hero.allRemoveCallBack(this);
                    battleRush.mapData.allRemoveCallBack(this);
                    battleRush.arenas.allRemoveCallBack(this);
                    battleRush.laneObjects.allRemoveCallBack(this);
                }
                this.setDataNull(battleRush);
                return;
            }
            // Child
            {
                if (data is BattleRush.State)
                {
                    BattleRush.State state = data as BattleRush.State;
                    // Update
                    {
                        switch (state.getType())
                        {
                            case BattleRush.State.Type.Load:
                                {
                                    Load load = state as Load;
                                    load.removeCallBackAndDestroy(typeof(LoadUpdate));
                                }
                                break;
                            case BattleRush.State.Type.Start:
                                {
                                    Start start = state as Start;
                                    start.removeCallBackAndDestroy(typeof(StartUpdate));
                                }
                                break;
                            case BattleRush.State.Type.Play:
                                {
                                    Play play = state as Play;
                                    play.removeCallBackAndDestroy(typeof(PlayUpdate));
                                }
                                break;
                            case BattleRush.State.Type.End:
                                {
                                    End end = state as End;
                                    end.removeCallBackAndDestroy(typeof(EndUpdate));
                                }
                                break;
                            default:
                                Logger.LogError("unknown type: " + state.getType());
                                break;
                        }
                    }
                    return;
                }
                if (data is Hero)
                {
                    Hero hero = data as Hero;
                    // Update
                    {
                        hero.removeCallBackAndDestroy(typeof(HeroUpdate));
                    }
                    return;
                }
                if (data is MapData)
                {
                    return;
                }
                if (data is Arena)
                {
                    Arena arena = data as Arena;
                    // Update
                    {
                        arena.removeCallBackAndDestroy(typeof(ArenaUpdate));
                    }
                    return;
                }
                if (data is ObjectInPath)
                {
                    ObjectInPath objectInPath = data as ObjectInPath;
                    // Update
                    {
                        switch (objectInPath.getType())
                        {
                            case ObjectInPath.Type.TroopCage:
                                {
                                    TroopCage troopCage = objectInPath as TroopCage;
                                    troopCage.removeCallBackAndDestroy(typeof(TroopCageUpdate));
                                }
                                break;
                            case ObjectInPath.Type.EnergyOrbNormal:
                                {
                                    EnergyOrbNormal energyOrbNormal = objectInPath as EnergyOrbNormal;
                                    energyOrbNormal.removeCallBackAndDestroy(typeof(EnergyOrbNormal));
                                }
                                break;
                            case ObjectInPath.Type.EnergyOrbPower:
                                {
                                    EnergyOrbPower energyOrbPower = objectInPath as EnergyOrbPower;
                                    energyOrbPower.removeCallBackAndDestroy(typeof(EnergyOrbPowerUpdate));
                                }
                                break;
                            case ObjectInPath.Type.EnergyOrbUpgrade:
                                {
                                    EnergyOrbUpgrade energyOrbUpgrade = objectInPath as EnergyOrbUpgrade;
                                    energyOrbUpgrade.removeCallBackAndDestroy(typeof(EnergyOrbUpgradeUpdate));
                                }
                                break;
                            case ObjectInPath.Type.OkgCoin:
                                {
                                    Coin coin = objectInPath as Coin;
                                    coin.removeCallBackAndDestroy(typeof(CoinUpdate));
                                }
                                break;
                            case ObjectInPath.Type.SawBlade:
                                {
                                    SawBlade sawBlade = objectInPath as SawBlade;
                                    sawBlade.removeCallBackAndDestroy(typeof(SawBladeUpdate));
                                }
                                break;
                            case ObjectInPath.Type.Blade:
                                {
                                    Blade blade = objectInPath as Blade;
                                    blade.removeCallBackAndDestroy(typeof(BladeUpdate));
                                }
                                break;
                            case ObjectInPath.Type.FireNozzle:
                                {
                                    FireNozzle fireNozzle = objectInPath as FireNozzle;
                                    fireNozzle.removeCallBackAndDestroy(typeof(FireNozzleUpdate));
                                }
                                break;
                            case ObjectInPath.Type.Pike:
                                {
                                    Pike pike = objectInPath as Pike;
                                    pike.removeCallBackAndDestroy(typeof(PikeUpdate));
                                }
                                break;
                            case ObjectInPath.Type.UpgradeGateFree:
                                {
                                    UpgradeGateFree upgradeGateFree = objectInPath as UpgradeGateFree;
                                    upgradeGateFree.removeCallBackAndDestroy(typeof(UpgradeGateFreeUpdate));
                                }
                                break;
                            case ObjectInPath.Type.UpgradeGateCharge:
                                {
                                    UpgradeGateCharge upgradeGateCharge = objectInPath as UpgradeGateCharge;
                                    upgradeGateCharge.removeCallBackAndDestroy(typeof(UpgradeGateChargeUpdate));
                                }
                                break;
                            case ObjectInPath.Type.Hammer:
                                {
                                    Hammer hammer = objectInPath as Hammer;
                                    hammer.removeCallBackAndDestroy(typeof(HammerUpdate));
                                }
                                break;
                            case ObjectInPath.Type.Grinder:
                                {
                                    Grinder grinder = objectInPath as Grinder;
                                    grinder.removeCallBackAndDestroy(typeof(GrinderUpdate));
                                }
                                break;
                            case ObjectInPath.Type.CocoonMantah:
                                {
                                    CocoonMantah cocoonMantah = objectInPath as CocoonMantah;
                                    cocoonMantah.removeCallBackAndDestroy(typeof(CocoonMantahUpdate));
                                }
                                break;
                        }
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
            if (wrapProperty.p is BattleRush)
            {
                switch ((BattleRush.Property)wrapProperty.n)
                {
                    case BattleRush.Property.state:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case BattleRush.Property.hero:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case BattleRush.Property.mapData:
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
                    case BattleRush.Property.laneObjects:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    default:
                        Logger.LogError("Don't process: " + wrapProperty + "; " + this);
                        break;
                }
                return;
            }
            // Child
            {
                if (wrapProperty.p is BattleRush.State)
                {
                    return;
                }
                if(wrapProperty.p is Hero)
                {
                    return;
                }
                if (wrapProperty.p is MapData)
                {
                    return;
                }
                if(wrapProperty.p is ObjectInPath)
                {
                    return;
                }
            }
            Logger.LogError("Don't process: " + data + "; " + syncs + "; " + this);
        }

        #endregion

    }
}