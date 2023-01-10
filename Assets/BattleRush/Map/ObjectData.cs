using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using LitJson;
using UnityEngine;

/**{"I":"troop_cage_1","P":"0.3,0,52.9","R":"0,1,0,0"},*/
namespace BattleRushS
{
    public class ObjectData : Data
    {

        public VO<ObjectInPath.Type> I;

        public VO<Position> position;

        #region Constructor

        public enum Property
        {
            I,
            position,
        }

        public ObjectData() : base()
        {
            this.I = new VO<ObjectInPath.Type>(this, (byte)Property.I, ObjectInPath.Type.OkgCoin);
            this.position = new VO<Position>(this, (byte)Property.position, Position.Zero);
        }

        #endregion

        public void parseJson(JsonData jsonData)
        {
            // I
            {
                string value = "";
                {
                    try
                    {
                        value = (string)jsonData["I"];
                    }
                    catch (System.Exception e)
                    {
                        Logger.LogWarning("status: " + e);
                    }
                }
                switch (value)
                {
                    case "okg_coin":
                        {
                            this.I.v = ObjectInPath.Type.OkgCoin;                           
                        }
                        break;
                    case "cocoon_mantah":// "cocoon_mantah":
                        {
                            this.I.v = ObjectInPath.Type.CocoonMantah;
                        }
                        break;
                    case "energy_orb_normal":
                        {
                            this.I.v = ObjectInPath.Type.EnergyOrbNormal;
                        }
                        break;

                    case "troop_cage_1":
                    case "troop_cage_2":
                    case "troop_cage_3":
                        {
                            this.I.v = ObjectInPath.Type.TroopCage;
                        }
                        break;
                    case "saw_blade":
                        {
                            this.I.v = ObjectInPath.Type.SawBlade;
                        }
                        break;
                    case "blade":
                        {
                            this.I.v = ObjectInPath.Type.Blade;
                        }
                        break;
                    case "fire_nozzle":
                        {
                            this.I.v = ObjectInPath.Type.FireNozzle;
                        }
                        break;
                    case "pike":
                        {
                            this.I.v = ObjectInPath.Type.Pike;
                        }
                        break;
                    case "energy_orb_upgrade":
                        {
                            this.I.v = ObjectInPath.Type.EnergyOrbUpgrade;
                        }
                        break;
                    case "upgrade_gate_free":
                        {
                            this.I.v = ObjectInPath.Type.UpgradeGateFree;
                        }
                        break;
                    case "upgrade_gate_charge":
                        {
                            this.I.v = ObjectInPath.Type.UpgradeGateCharge;
                        }
                        break;
                    case "hammer":
                        {
                            this.I.v = ObjectInPath.Type.Hammer;
                        }
                        break;
                    case "grinder":
                        {
                            this.I.v = ObjectInPath.Type.Grinder;
                        }
                        break;
                    case "energy_orb_power":
                        {
                            this.I.v = ObjectInPath.Type.EnergyOrbPower;
                        }
                        break;
                    default:
                        Logger.LogError("unknown object data: " + value);
                        break;
                }              
            }
            // position
            {
                // P
                Vector3 P = Vector3.zero;
                {
                    string value = "";
                    {
                        try
                        {
                            value = (string)jsonData["P"];
                        }
                        catch (System.Exception e)
                        {
                            Logger.LogWarning("status: " + e);
                        }
                    }
                    // parse string to vector 3
                    {
                        value = string.Join("", value.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
                        // Logger.Log("troop remove space: " + value);
                        string[] numbers = value.Split(",");
                        if (numbers != null && numbers.Length == 3)
                        {
                            float x = 0;
                            float y = 0;
                            float z = 0;
                            {
                                try
                                {
                                    x = float.Parse(numbers[0], CultureInfo.InvariantCulture);
                                    y = float.Parse(numbers[1], CultureInfo.InvariantCulture);
                                    z = float.Parse(numbers[2], CultureInfo.InvariantCulture);
                                }
                                catch (System.Exception e)
                                {
                                    Logger.LogError(e);
                                }
                            }
                            P = new Vector3(x, y, z);
                        }
                    }
                }
                // R
                Vector3 R = Vector3.zero;
                {
                    string value = "";
                    {
                        try
                        {
                            value = (string)jsonData["R"];
                        }
                        catch (System.Exception e)
                        {
                            Logger.LogWarning("status: " + e);
                        }
                    }
                    // parse string to vector 3
                    {
                        value = string.Join("", value.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
                        string[] numbers = value.Split(",");
                        if (numbers != null && numbers.Length == 3)
                        {
                            float x = 0;
                            float y = 0;
                            float z = 0;
                            {
                                try
                                {
                                    x = float.Parse(numbers[0], CultureInfo.InvariantCulture);
                                    y = float.Parse(numbers[1], CultureInfo.InvariantCulture);
                                    z = float.Parse(numbers[2], CultureInfo.InvariantCulture);
                                }
                                catch (System.Exception e)
                                {
                                    Logger.LogError(e);
                                }
                            }
                            R = new Vector3(x, y, z);
                        }
                    }
                }
                this.position.v = new Position(UnityEngine.Random.Range(0.0f, 1.0f), P.z / Position.DefaultSegmentHeight);
            }            
            // Logger.Log("ObjectData : " + this.I.v + ", " + this.P.v + ", " + this.R.v);

        }

    }

}