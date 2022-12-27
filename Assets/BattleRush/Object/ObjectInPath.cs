using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectInPath : Data
{
    public enum Type
    {
        EnergyOrbNormal,
        /**energy_orb_power*/
        EnergyOrbPower,
        /**energy_orb_upgrade*/
        EnergyOrbUpgrade,
        OkgCoin,

        TroopCage,
        /**saw_blade*/
        SawBlade,
        /**blade*/
        Blade,
        /**fire_nozzle*/
        FireNozzle,
        /**pike*/
        Pike,
        /**upgrade_gate_free*/
        UpgradeGateFree,
        /**upgrade_gate_charge*/
        UpgradeGateCharge,
        /**hammer*/
        Hammer,
        /**grinder*/
        Grinder,

        /**cocoon_mantah*/
        CocoonMantah
    }

    public abstract Type getType();
}
