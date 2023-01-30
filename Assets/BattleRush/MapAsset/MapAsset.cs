using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SegmentPathInfo
{
    public float angle = 0;// 45f;
    public float turnRate = 0;// 0f;
    public Vector3 turnAxis = Vector3.up;

    public void copy(SegmentPathInfo old)
    {
        this.angle = old.angle;
        this.turnRate = old.turnRate;
        this.turnAxis = old.turnAxis;
    }
}

[System.Serializable]
public class SegmentAsset
{
    public SegmentPathInfo pathInfo = new SegmentPathInfo();
    public uint repeat = 1;
    public Segment segment;
}

[CreateAssetMenu(fileName = "MapAsset", menuName = "BattleRush/SpawnMapAsset", order = 1)]
public class MapAsset : ScriptableObject
{
    public Segment defaultSegment;
    public List<SegmentAsset> segments = new List<SegmentAsset>();
}
