using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ObjectS.KeyUnlockS
{
    public class KeyUnlockCollide : MonoBehaviour
    {

        private void OnTriggerEnter(Collider collider)
        {
            //Logger.Log("onTriggerEnter: " + collider);
            KeyUnlockUI keyUnlockUI = this.GetComponentInParent<KeyUnlockUI>();
            if (keyUnlockUI != null)
            {
                if (keyUnlockUI.data != null)
                {
                    keyUnlockUI.data.colliderEnters.add(collider);
                    keyUnlockUI.refresh();
                }
            }
            else
            {
                Logger.LogError("keyUnlockUI null");
            }           
        }

        private void OnTriggerExit(Collider collider)
        {
            //Logger.Log("onTriggerEnter: " + collider);
            KeyUnlockUI keyUnlockUI = this.GetComponentInParent<KeyUnlockUI>();
            if (keyUnlockUI != null)
            {
                if (keyUnlockUI.data != null)
                {
                    keyUnlockUI.data.colliderEnters.remove(collider);
                    keyUnlockUI.refresh();
                }
            }            
        }

    }
}