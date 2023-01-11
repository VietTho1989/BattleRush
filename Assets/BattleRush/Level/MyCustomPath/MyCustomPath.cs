using System.Collections;
using System.Collections.Generic;
using Dreamteck.Forever;
using Dreamteck.Splines;
using UnityEngine;

namespace BattleRushS
{
    public class MyCustomPath : HighLevelPathGenerator
    {

        #region battleRush

        public BattleRush battleRush = null;

        #endregion

        private float currentAngle = 0f;
        private bool positive = true;
        Vector3 continueOrientation = Vector3.zero;


        public override void Continue(LevelPathGenerator previousGenerator)
        {
            base.Continue(previousGenerator);
            continueOrientation = orientation;
        }

        protected override void GeneratePoint(ref Point point, int pointIndex)
        {
            base.GeneratePoint(ref point, pointIndex);
            if (isFirstPoint)
            {
                return;
            }
            Logger.Log("myCustomPath generate point: " + pointIndex);
            // find segment path info
            SegmentPathInfo segmentPathInfo = null;
            {
                // get from battle rush
                if (battleRush != null && battleRush.makeSegmentManager.v.mapAsset.v!=null)
                {
                    // increase index: 4 is last index
                    if (pointIndex == 4)
                    {
                        SegmentAsset segmentAsset = battleRush.makeSegmentManager.v.nextMakePath();
                        if (segmentAsset != null)
                        {
                            segmentPathInfo = segmentAsset.pathInfo;
                        }
                        else
                        {
                            Logger.LogError("segmentAsset null");
                        }
                        Logger.Log("myCustomPath next: " + battleRush.makeSegmentManager.v.index.v);
                    }
                    else
                    // make segment path info
                    {
                        // find segment asset
                        SegmentAsset segmentAsset = null;
                        {
                            if (battleRush.makeSegmentManager.v.index.v >= 0 && battleRush.makeSegmentManager.v.index.v < battleRush.makeSegmentManager.v.mapAsset.v.segments.Count)
                            {
                                segmentAsset = battleRush.makeSegmentManager.v.mapAsset.v.segments[battleRush.makeSegmentManager.v.index.v];
                            }
                            else
                            {
                                Logger.LogError("myCustomPath: why index error");
                            }
                        }
                        // process
                        if (segmentAsset != null)
                        {
                            Logger.Log("myCustomPath: found segment asset: " + segmentAsset);
                            segmentPathInfo = segmentAsset.pathInfo;
                        }
                        else
                        {
                            Logger.LogError("myCustomPath: segmentAsset null");
                        }
                    }
                }
                else
                {
                    Logger.LogError("myCustomPath: battleRush null");
                }
                // prevent null
                if (segmentPathInfo == null)
                {
                    Logger.LogError("myCustomPath: why segmentPathInfo null");
                    segmentPathInfo = new SegmentPathInfo();
                    /*{
                        segmentPathInfo.angle = 45f;
                        segmentPathInfo.turnRate = 10f;
                        segmentPathInfo.turnAxis = Vector3.up;
                    }*/
                }
            }
            // process
            {
                Logger.Log("myCustomPath: path: " + segmentPathInfo.angle + ", " + segmentPathInfo.turnRate + ", " + segmentPathInfo.turnAxis);
                /*if (positive && currentAngle == segmentPathInfo.angle)
                {
                    positive = false;
                }
                else if (currentAngle == -segmentPathInfo.angle)
                {
                    positive = true;
                }*/
                currentAngle = MoveAngle(currentAngle, segmentPathInfo.angle, segmentPathInfo.turnRate);
                SetOrientation(continueOrientation + currentAngle * segmentPathInfo.turnAxis.normalized);
                point.position = GetPointPosition();
                point.autoRotation = true;// false;
                point.rotation = Vector3.Lerp(orientation, orientation + MoveAngle(currentAngle, segmentPathInfo.angle, segmentPathInfo.turnRate) * segmentPathInfo.turnAxis.normalized, 0.5f);
            }          
        }

        float MoveAngle(float current, float target, float turnRate)
        {
            if (positive)
            {
                return Mathf.MoveTowards(current, target, turnRate);
            }
            else
            {
                return Mathf.MoveTowards(current, -target, turnRate);
            }
        }


        float MoveTargetAngle(float input, float minStep, float maxStep, bool restrict, float minTarget, float maxTarget)
        {
            float direction = Random.Range(0, 100) > 50f ? 1f : -1f;
            float move = Random.Range(minStep, maxStep) * direction;
            input += move;
            if (restrict) input = Mathf.Clamp(input, minTarget, maxTarget);
            else
            {
                if (input > 360f) input -= Mathf.FloorToInt(input / 360f) * 360f;
                else if (input < 0f) input += Mathf.FloorToInt(-input / 360f) * 360f;
            }
            return input;
        }

        public override void Initialize(LevelGenerator input)
        {
            base.Initialize(input);
            currentAngle = 0f;
            continueOrientation = orientation;
        }

        protected void OffsetPoints(SplinePoint[] points, Vector3 offset, Space space)
        {
            if (offset != Vector3.zero) segment.stitch = false;
            SplineSample result = new SplineSample();
            if (space == Space.Self && segment.previous != null) segment.previous.Evaluate(1.0, result);
            for (int i = 0; i < points.Length; i++) points[i].SetPosition(points[i].position + offset);
        }

    }
}