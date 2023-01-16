using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using GoogleMobileAds.Api.Mediation.UnityAds;

namespace TribalMonsterMerge
{
    public class UnityAdsInitializer : MonoBehaviour
    {

        void Start()
        {
            UnityAds.SetConsentMetaData("gdpr.consent", true);
        }

    }
}