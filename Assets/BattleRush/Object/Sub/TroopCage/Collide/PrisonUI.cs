using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRushS.ObjectS.TroopCageS
{
    public class PrisonUI : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider collider)
        {
            Logger.Log("TroopCageUI: PrisonUI: onTriggerEnter: " + collider + ", " + this.gameObject);
        }

        private void OnTriggerExit(Collider collider)
        {
            Logger.Log("TroopCageUI: PrisonUI: onTriggerEnter: " + collider + ", " + this.gameObject);
        }

    }

}