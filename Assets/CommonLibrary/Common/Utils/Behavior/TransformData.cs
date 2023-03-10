using UnityEngine;
using System.Collections;

public class TransformData : Data
{

    public VO<Vector3> anchoredPosition;

    public VO<Vector2> anchorMin;// Vector2(0.5f, 0.5f);
    public VO<Vector2> anchorMax;// Vector2(0.5f, 0.5f);
    public VO<Vector2> pivot;// Vector2(0.5f, 0.5f);

    public VO<Vector2> offsetMin;// Vector2.zero;
    public VO<Vector2> offsetMax;// Vector2.zero;
    public VO<Vector2> sizeDelta;

    public VO<Quaternion> rotation;
    public VO<Vector3> scale;// Vector3(1, 1, 1);

    public VO<Vector2> size;

    #region Constructor

    public enum Property
    {
        anchoredPosition,
        anchorMin,
        anchorMax,
        pivot,
        offsetMin,
        offsetMax,
        sizeDelta,
        rotation,
        scale,
        size
    }

    public TransformData() : base()
    {
        this.anchoredPosition = new VO<Vector3>(this, (byte)Property.anchoredPosition, Vector3.zero);
        this.anchorMin = new VO<Vector2>(this, (byte)Property.anchorMin, new Vector2(0.5f, 0.5f));
        this.anchorMax = new VO<Vector2>(this, (byte)Property.anchorMax, new Vector2(0.5f, 0.5f));
        this.pivot = new VO<Vector2>(this, (byte)Property.pivot, new Vector2(0.5f, 0.5f));
        this.offsetMin = new VO<Vector2>(this, (byte)Property.offsetMin, Vector2.zero);
        this.offsetMax = new VO<Vector2>(this, (byte)Property.offsetMax, Vector2.zero);
        this.sizeDelta = new VO<Vector2>(this, (byte)Property.sizeDelta, Vector2.zero);
        this.rotation = new VO<Quaternion>(this, (byte)Property.rotation, new Quaternion());
        this.scale = new VO<Vector3>(this, (byte)Property.scale, new Vector3(1.0f, 1.0f, 1.0f));

        this.size = new VO<Vector2>(this, (byte)Property.size, new Vector2());
    }

    public void reset()
    {
        this.anchoredPosition.v = Vector3.zero;
        this.anchorMin.v = new Vector2(0.5f, 0.5f);
        this.anchorMax.v = new Vector2(0.5f, 0.5f);
        this.pivot.v = new Vector2(0.5f, 0.5f);
        this.offsetMin.v = Vector2.zero;
        this.offsetMax.v = Vector2.zero;
        this.sizeDelta.v = Vector2.zero;
        this.rotation.v = new Quaternion();
        this.scale.v = new Vector3(1.0f, 1.0f, 1.0f);

        this.size.v = new Vector2();
    }

    public override string ToString()
    {
        return ""+ this.size.v;
    }

    #endregion

    public void update(Transform transform)
    {
        if (transform is RectTransform)
        {
            RectTransform rectTransform = transform as RectTransform;
            {
                this.anchoredPosition.v = rectTransform.anchoredPosition;
                this.anchorMin.v = rectTransform.anchorMin;
                this.anchorMax.v = rectTransform.anchorMax;
                this.pivot.v = rectTransform.pivot;
                this.offsetMin.v = rectTransform.offsetMin;
                this.offsetMax.v = rectTransform.offsetMax;
                this.sizeDelta.v = rectTransform.sizeDelta;
                this.rotation.v = rectTransform.localRotation;
                this.scale.v = rectTransform.localScale;
            }
            // size
            this.size.v = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
        }
        else
        {
            Logger.LogError("why not rectTransform: " + this);
            this.size.v = new Vector2(0, 0);
        }
    }

    public static void AddCallBack(Data data, ValueChangeCallBack callBack)
    {
        if (data != null)
        {
            HaveTransformData haveTransformData = data.findTransformData();
            if (haveTransformData != null)
            {
                haveTransformData.getTransformData().addCallBack(callBack);
            }
            else
            {
                Logger.LogWarning("haveTransformData null: " + data);
            }
        }
        else
        {
            Logger.LogError("data null");
        }
    }

    public static void RemoveCallBack(Data data, ValueChangeCallBack callBack)
    {
        if (data != null)
        {
            HaveTransformData haveTransformData = data.findTransformData();
            if (haveTransformData != null)
            {
                haveTransformData.getTransformData().removeCallBack(callBack);
            }
            else
            {
                Logger.LogWarning("haveTransformData null: " + data);
            }
        }
        else
        {
            Logger.LogError("data null");
        }
    }

}