using System.Collections;
using System.Collections.Generic;
using Dreamteck.Forever;
using UnityEngine;

public class SegmentEvent : MonoBehaviour
{

    public GameObject coinPrefab;

    private void Awake()
    {
        LevelSegment.onSegmentEntered += SegmentEnterHandler;
    }

    private void OnDestroy()
    {
        LevelSegment.onSegmentEntered -= SegmentEnterHandler;
    }

    void SegmentEnterHandler(LevelSegment segment)
    {
        // Debug.Log("Segment create: " + this);
        if (coinPrefab != null)
        {
            for (int i = 0; i < 8; i++)
            {
                GameObject coin = Instantiate(coinPrefab, new Vector3(4, -8 + 2 * i), Quaternion.identity, segment.transform);
                coin.transform.localPosition = new Vector3(4, -8 + 2 * i);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
