using System.Collections;
using System.Collections.Generic;
using Dreamteck.Forever;
using UnityEngine;

public class TestSegmentObject : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        LevelSegment levelSegment = this.GetComponentInParent<LevelSegment>();
        if (levelSegment != null)
        {
            /**ham UpdateReferences co the xet**/
            levelSegment.UpdateReferences();
        }
        else
        {
            Logger.LogError("levelSegment null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
