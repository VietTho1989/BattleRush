using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api.Mediation.AppLovin;

namespace SkymareGamePuzzle
{
    public class ApplovinInitializer : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            AppLovin.Initialize();
            AppLovin.SetHasUserConsent(true);
            AppLovin.SetIsAgeRestrictedUser(true);
        }

    }
}