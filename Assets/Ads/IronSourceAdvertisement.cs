using GoogleMobileAds.Api.Mediation.IronSource;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkymareGamePuzzle
{
    public class IronSourceAdvertisement : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            IronSource.SetConsent(true);
            IronSource.SetMetaData("do_not_sell", "true");
        }
    }
}