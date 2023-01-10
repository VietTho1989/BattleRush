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

[System.Serializable]
public struct Position
{

    public static Position Zero = new Position(0, 0);

    public const float DefaultSegmentWidth = 8.0f;// -4 -> 4
    public const float DefaultSegmentHeight = 48.0f;

    public Position(float x, float z)
    {
        this.x = x;
        this.z = z;
    }

    /**
     * tu 0 den 1: vi tri phan tram chieu ngang segment
     * */
    public float x;

    /**
     * vi tri chieu doc, phan du 1 la % thuoc chieu doc segment
     * */
    public float z;
}