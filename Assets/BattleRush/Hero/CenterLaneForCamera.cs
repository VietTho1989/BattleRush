using System.Collections;
using System.Collections.Generic;
using Dreamteck.Forever;
using Dreamteck.Splines;
using UnityEngine;

namespace BattleRushS.HeroS
{
    public class CenterLaneForCamera : MonoBehaviour
    {

        public LaneRunner runner;
        SplineSample evalResult = new SplineSample();
        Transform trs;
        public float cameraDistance = 0f;
        public float cameraHeight = 0f;
        public float cameraAngle = 0f;
        Quaternion rotationSmooth = Quaternion.identity;

        void Awake()
        {
            trs = transform;
        }

        private void LateUpdate()
        {
            if (!LevelGenerator.instance.ready)
            {
                this.transform.localPosition = Vector3.zero;
                runner.result.percent = 0;
                return;
            }
            // run logic
            {
                // Logger.Log("runner percent: " + runner.result.percent);
                int runnerSegmentIndex = 0;
                for (int i = 0; i < LevelGenerator.instance.segments.Count; i++)
                {
                    if (LevelGenerator.instance.segments[i] == runner.segment)
                    {
                        runnerSegmentIndex = i;
                        break;
                    }
                }
                double globalPercent = LevelGenerator.instance.LocalToGlobalPercent(runner.result.percent, runnerSegmentIndex);
                LevelGenerator.instance.Evaluate(LevelGenerator.instance.Travel(globalPercent, cameraDistance, Spline.Direction.Backward), evalResult);
                trs.position = evalResult.position + Vector3.up * cameraHeight;
                rotationSmooth = Quaternion.Slerp(rotationSmooth, Quaternion.Slerp(evalResult.rotation, runner.result.rotation, Time.deltaTime * 0.5f), Time.deltaTime * 2f);
                trs.rotation = rotationSmooth * Quaternion.AngleAxis(cameraAngle, Vector3.right);
            }
        }

    }
}