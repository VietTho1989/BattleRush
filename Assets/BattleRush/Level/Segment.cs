using System.Collections;
using System.Collections.Generic;
using Dreamteck.Forever;
using UnityEngine;

public class Segment : MonoBehaviour
{
    // Start is called before the first frame update
    public LevelSegment segment;

    #region type

    public enum SegmentType
    {
        Run,
        Arena
    }

    public SegmentType segmentType;

    #endregion

    public float length = 48;

    void Start()
    {
        if (segment != null)
            this.name = "Segment " + segment.index + "; " + segment.EvaluatePosition(0) + "; " + segment.EvaluatePosition(100) + ", " + segment.GetBounds().size;
    }

    public float segmentPos = 0;

    #region for editor

    public SegmentAsset segmentAsset = new SegmentAsset();
    public bool isDeleted = false;

    #endregion

}