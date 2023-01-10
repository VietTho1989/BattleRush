using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS
{
    public class AddObjectToBattleRushUI : MonoBehaviour
    {
        public uint objectDataId = uint.MaxValue;

        private void Awake()
        {
            
        }

        private void Start()
        {
            // TODO fix position: chua hieu tai sao phai can them vao
            {
                this.transform.localPosition = new Vector3(this.transform.localPosition.x, 1, this.transform.localPosition.z);
            }
            // UI
            BattleRushUI.UIData.ObjectInPathUIInterface uiInterface = this.GetComponent<BattleRushUI.UIData.ObjectInPathUIInterface>();
            //Logger.Log("uiInterface: " + uiInterface);
            if (uiInterface != null)
            {
                BattleRushUI battleRushUI = this.GetComponentInParent<BattleRushUI>();
                if (battleRushUI != null)
                {
                    BattleRushUI.UIData battleRushUIData = battleRushUI.data;
                    if (battleRushUIData != null)
                    {
                        // find
                        BattleRushUI.UIData.ObjectInPathUI objectInPathUI = null;
                        {
                            foreach (BattleRushUI.UIData.ObjectInPathUI check in battleRushUIData.objectInPaths.vs)
                            {
                                ObjectInPath objectInPath = check.getObjectInPath();
                                if (objectInPath != null)
                                {
                                    if (objectInPath.uid == objectDataId)
                                    {
                                        objectInPathUI = check;
                                        break;
                                    }
                                }
                                else
                                {
                                    Logger.LogError("objectInPath null");
                                }
                            }
                        }
                        uiInterface.setObjectInPathUIData(objectInPathUI);
                    }
                    else
                    {
                        Logger.LogError("battleRushUIData null");
                    }
                }
                else
                {
                    Logger.LogError("battleRushUI null");
                }
            }
            else
            {
                Logger.LogError("uiInterface null");
            }
        }

    }
}
