using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHand : MonoBehaviour
{

    private const float MaxPos = 0.27f;

    private const float HalfWidthDuration = 0.45f;
    private float LoopDuration = 4 * HalfWidthDuration;

    private RectTransform _rectTran;

    // Use this for initialization
    void Start()
    {
        _rectTran = GetComponent<RectTransform>();
    }

    private float time = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        if (time >= LoopDuration)
        {
            time -= LoopDuration;
        }
        // find anchor
        float anchorX = 0;
        {
            int halfIndex = (int)(time / HalfWidthDuration);
            float halfTime = (time % HalfWidthDuration) / HalfWidthDuration;
            switch (halfIndex)
            {
                case 0:
                    // in right and increase
                    anchorX = 0.5f + Interpolator.AccelerateInterpolator(0, MaxPos, halfTime, 2);// 0.5f + MaxPos * halfTime;
                    break;
                case 1:
                    // in right and decrease
                    anchorX = 0.5f + Interpolator.AccelerateInterpolator(0, MaxPos, 1 - halfTime, 2);// MaxPos * (1 - halfTime);
                    break;
                case 2:
                    // in left and decrease
                    anchorX = 0.5f - Interpolator.AccelerateInterpolator(0, MaxPos, halfTime, 2);// MaxPos * halfTime;
                    break;
                case 3:
                    // in left and increase
                    anchorX = 0.5f - Interpolator.AccelerateInterpolator(0, MaxPos, 1 - halfTime, 2);// MaxPos * (1 - halfTime);
                    break;
                default:
                    Debug.LogError("unknown halfIndex: " + halfIndex);
                    break;
            }
        }
        // set
        _rectTran.anchorMin = new Vector2(anchorX, 0.0f);
        _rectTran.anchorMax = new Vector2(anchorX, 0.0f);
    }

}