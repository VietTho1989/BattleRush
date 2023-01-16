using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api.Mediation.Vungle;

public class VungleInitializer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vungle.UpdateConsentStatus(VungleConsent.ACCEPTED);
    }
}
