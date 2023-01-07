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

        public VO<string> I;

        public VO<Position> position;

        #region Constructor

        public enum Property
        {
            I,
            position,
        }

        public ObjectData() : base()
        {
            this.I = new VO<string>(this, (byte)Property.I, "");
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
                this.I.v = value;
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