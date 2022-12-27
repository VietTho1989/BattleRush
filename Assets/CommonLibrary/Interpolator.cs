using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interpolator
{

    #region animation curve

    public static float AnimationCurvePolator(float start, float end, float t)
    {
        t = Mathf.Clamp(t, 0, 1);
        AnimationCurve anim = AnimationCurve.EaseInOut(0, start, 1, end);
        return anim.Evaluate(t);
    }

    #endregion

    /**
     * bound time: tien roi lui tra lai
     * */
    public static float GetBoundTime(float t)
    {
        t = Mathf.Clamp(t, 0, 1);
        if (t <= 0.5f)
        {
            // tu 0 den 1 trong thoi gian tu 0 den 0.5
            return 2 * t;
        }
        else
        {
            // tu 1 den 0 trong thoi gian tu 0.5 den 1
            return 2 - 2 * t;
        }
    }

    public static float GetQuadBoundTime(float t)
    {
        t = Mathf.Clamp(t, 0, 1);
        if (t <= 0.25f)
        {
            // tu 0 den 1 trong thoi gian tu 0 den 0.5
            return 2 * t;
        }
        else
        {
            // tu 1 den 0 trong thoi gian tu 0.5 den 1
            return 2 - 2 * t;
        }
    }

    /**
     * qua di qua lai 4 lan
     */
    public static float BounceLinearPolator(float start, float end, float t)
    {
        t = Mathf.Clamp(t, 0, 1);
        if (t <= 0.5f)
        {
            return start + (end - start) * 2 * t;
        }
        else
        {
            return end + (start - end) * 2 * (t - 0.5f);
        }
    }

    public static float LinearPolator(float start, float end, float t)
    {
        t = Mathf.Clamp(t, 0, 1);
        return start + (end - start) * t;
    }

    public static float AccelerateInterpolator(float start, float end, float t, float factor)
    {
        t = Mathf.Clamp(t, 0, 1);
        return start + (end - start) * Mathf.Pow(t, 2 * factor);
    }

    public static float DecelerateInterpolator(float start, float end, float t, float factor)
    {
        t = Mathf.Clamp(t, 0, 1);
        return start + (end - start) * (1 - Mathf.Pow(1 - t, 2 * factor));
    }

    public static float AccelerateDecelerateInterpolator(float start, float end, float t)
    {
        t = Mathf.Clamp(t, 0, 1);
        return (float)(start + (end - start) * (Mathf.Cos((t + 1) * Mathf.PI) / 2 + 0.5));
    }

    public static float AnticipateInterpolator(float start, float end, float t, float T)
    {
        t = Mathf.Clamp(t, 0, 1);
        return start + (end - start) * ((T + 1) * Mathf.Pow(t, 3) - T * Mathf.Pow(t, 2));
    }

    public static float OvershootInterpolator(float start, float end, float t, float T)
    {
        t = Mathf.Clamp(t, 0, 1);
        return start + (end - start) * ((T + 1) * Mathf.Pow(t - 1, 3) + T * Mathf.Pow(t - 1, 2) + 1);
    }

    public static float AnticipateOvershootInterpolator(float start, float end, float t, float tension, float extraTension)
    {
        t = Mathf.Clamp(t, 0, 1);
        if (t < 0.5f)
        {
            return start + (end - start) * (0.5f * ((tension + 1) * Mathf.Pow(2 * t, 3) - tension * Mathf.Pow(2 * t, 2)));
        }
        else
        {
            return start + (end - start) * (0.5f * ((extraTension + 1) * Mathf.Pow(2 * t - 2, 3) + extraTension * Mathf.Pow(2 * t - 2, 2)) + 1);
        }
    }

    public static float BounceInterpolator(float start, float end, float t)
    {
        t = Mathf.Clamp(t, 0, 1);
        if (t < 0.31489f)
        {
            return start + (end - start) * (8 * Mathf.Pow(1.1226f * t, 2));
        }
        else if (t < 0.65990f)
        {
            return start + (end - start) * (8 * Mathf.Pow(1.1226f * t - 0.54719f, 2) + 0.7f);
        }else if(t< 0.85908f)
        {
            return start + (end - start) * (8 * Mathf.Pow(1.1226f * t - 0.8526f, 2) + 0.9f);
        }
        else
        {
            return start + (end - start) * (8 * Mathf.Pow(1.1226f * t - 1.0435f, 2) + 0.95f);
        }
    }

    public static float CycleInterpolator(float start, float end, float t, int cycle)
    {
        t = Mathf.Clamp(t, 0, 1);
        return start + (end - start) * (Mathf.Sin(2 * Mathf.PI * cycle * t));
    }

    public static float HesitateInterpolator(float start, float end, float t)
    {
        t = Mathf.Clamp(t, 0, 1);
        return start + (end - start) * (0.5f * (Mathf.Pow(2 * t - 1, 3) + 1));
    }

}