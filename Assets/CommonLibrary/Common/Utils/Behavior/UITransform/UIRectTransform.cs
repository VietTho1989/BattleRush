using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class UIRectTransform
{

    public Vector3 anchoredPosition;

    public Vector2 anchorMin = new Vector2(0.5f, 0.5f);
    public Vector2 anchorMax = new Vector2(0.5f, 0.5f);
    public Vector2 pivot = new Vector2(0.5f, 0.5f);

    public Vector2 offsetMin = Vector2.zero;
    public Vector2 offsetMax = Vector2.zero;
    public Vector2 sizeDelta;

    public Quaternion rotation;
    public Vector3 scale = new Vector3(1, 1, 1);

    #region Constructor

    public UIRectTransform()
    {

    }

    public UIRectTransform(UIRectTransform rectTransform)
    {
        this.anchoredPosition = rectTransform.anchoredPosition;
        this.anchorMin = rectTransform.anchorMin;
        this.anchorMax = rectTransform.anchorMax;
        this.pivot = rectTransform.pivot;
        this.offsetMin = rectTransform.offsetMin;
        this.offsetMax = rectTransform.offsetMax;
        this.sizeDelta = rectTransform.sizeDelta;
        this.rotation = rectTransform.rotation;
        this.scale = rectTransform.scale;
    }

    public UIRectTransform(UIRectTransform rectTransform, float posY)
    {
        this.anchoredPosition = rectTransform.anchoredPosition;
        this.anchorMin = rectTransform.anchorMin;
        this.anchorMax = rectTransform.anchorMax;
        this.pivot = rectTransform.pivot;
        this.offsetMin = rectTransform.offsetMin;
        this.offsetMax = rectTransform.offsetMax;
        this.sizeDelta = rectTransform.sizeDelta;
        this.rotation = rectTransform.rotation;
        this.scale = rectTransform.scale;
        // posY
        this.setPosY(posY);
    }

    public override string ToString()
    {
        return "anchoredPosition: " + this.anchoredPosition
            + "; anchorMin: " + this.anchorMin
            + "; anchorMax: " + this.anchorMax
            + "; pivot: " + this.pivot
             + "; offsetMin: " + this.offsetMin
             + "; offsetMax: " + this.offsetMax
            + "; sizeDelta: " + this.sizeDelta
            + "; localRotation: " + this.rotation
            + "; localScale: " + this.scale;
    }

    public static string PrintRectTransform(RectTransform rectTransform)
    {
        return "anchoredPosition: " + rectTransform.anchoredPosition
            + "; anchorMin: " + rectTransform.anchorMin
            + "; anchorMax: " + rectTransform.anchorMax
            + "; pivot: " + rectTransform.pivot
             + "; offsetMin: " + rectTransform.offsetMin
             + "; offsetMax: " + rectTransform.offsetMax
            + "; sizeDelta: " + rectTransform.sizeDelta
            + "; localRotation: " + rectTransform.localRotation
            + "; localScale: " + rectTransform.localScale;
    }

    #endregion

    public void set(RectTransform rectTransform)
    {
        if (rectTransform != null)
        {
            // find is different
            bool isDifferent = true;
            {

            }
            // set
            if (isDifferent)
            {
                rectTransform.anchoredPosition = anchoredPosition;
                rectTransform.anchorMin = anchorMin;
                rectTransform.anchorMax = anchorMax;
                rectTransform.pivot = pivot;
                rectTransform.sizeDelta = sizeDelta;
                rectTransform.offsetMin = offsetMin;
                rectTransform.offsetMax = offsetMax;
                rectTransform.localRotation = rotation;
                rectTransform.localScale = scale;
            }
            else
            {
                Logger.LogError("why the same");
            }
        }
        else
        {
            Logger.LogError("rectTransform null");
        }
    }

    public static bool Set(Data data, UIRectTransform uiRectTransform)
    {
        bool ret = false;
        if (data != null)
        {
            RectTransform rectTransform = (RectTransform)Data.FindTransform(data);
            if (rectTransform != null)
            {
                if (uiRectTransform != null)
                {
                    uiRectTransform.set(rectTransform);
                    ret = true;
                }
                else
                {
                    Logger.LogError("uiRectTransform null");
                }
            }
            else
            {
                // Debug.LogError("rectTransform null");
            }
        }
        else
        {
            // Debug.LogError("data null");
        }
        return ret;
    }

    #region setPosX

    public static float SetPosX(RectTransform rectTransform, float posX)
    {
        float ret = 0;
        {
            if (rectTransform.anchorMin.x == 0.0f
            && rectTransform.anchorMax.x == 0.0f
            && rectTransform.pivot.x == 0.0f)
            {
                rectTransform.anchoredPosition = new Vector2(posX, rectTransform.anchoredPosition.y);
                rectTransform.offsetMin = new Vector2(posX, rectTransform.offsetMin.y);
                rectTransform.offsetMax = new Vector2(posX + rectTransform.sizeDelta.x, rectTransform.offsetMax.y);
                ret = rectTransform.rect.width;
            }
        }
        return ret;
    }

    public static float SetPosX(Data data, float posX)
    {
        float ret = 0;
        if (data != null)
        {
            RectTransform rectTransform = (RectTransform)Data.FindTransform(data);
            if (rectTransform != null)
            {
                SetPosX(rectTransform, posX);
                ret = rectTransform.rect.width;
            }
            else
            {
                Logger.LogError("rectTransform null");
            }
        }
        else
        {
            // Debug.LogError("data null");
        }
        return ret;
    }

    public static void SetWidth(RectTransform rectTransform, float size)
    {
        // Debug.LogError("setHeight: " + size);
        if (rectTransform.anchorMin.x == 0.0f
            && rectTransform.anchorMax.x == 0.0f
            && rectTransform.pivot.x == 0.0f)
        {
            rectTransform.offsetMin = new Vector2(rectTransform.anchoredPosition.x - size, rectTransform.offsetMin.y);
            rectTransform.sizeDelta = new Vector2(size, rectTransform.sizeDelta.y);
        }
        else
        {
            Logger.LogError("unknown rect type: " + UIRectTransform.PrintRectTransform(rectTransform));
        }
    }

    #endregion

    #region setPosY

    public void setPosY(float posY)
    {
        // RequestIntLongFloatRect, RequestEnumRect
        if (this.anchorMin.y == 1.0f
            && this.anchorMax.y == 1.0f
            && this.pivot.y == 1.0f)
        {
            this.anchoredPosition.y = -posY;
            this.offsetMin.y = -this.sizeDelta.y - posY;
            this.offsetMax.y = -posY;
        }
        else if (this.anchorMin.y == 0.5f
            && this.anchorMax.y == 0.5f
            && this.pivot.y == 0.5f)
        {
            this.anchoredPosition.y = -posY;
            this.offsetMin.y = -posY - this.sizeDelta.y / 2.0f;
            this.offsetMax.y = -posY + this.sizeDelta.y / 2.0f;
        }
        else
        {
            Logger.LogError("unknown rect type: " + this);
        }
    }

    public static void SetPosY(RectTransform rectTransform, float posY)
    {
        if (rectTransform.anchorMin.y == 1.0f
            && rectTransform.anchorMax.y == 1.0f
            && rectTransform.pivot.y == 1.0f)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -posY);
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, -rectTransform.sizeDelta.y - posY);
            rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, -posY);
        }
        else if (rectTransform.anchorMin.y == 0.0f
          && rectTransform.anchorMax.y == 0.0f
          && rectTransform.pivot.y == 0.0f)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, posY);
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, posY);
            rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, rectTransform.sizeDelta.y + posY);
        }
        else
        {
            Logger.LogError("unknown rect type: " + UIRectTransform.PrintRectTransform(rectTransform) + ", " + rectTransform.gameObject);
        }
    }

    /**
     * get height
     * */
    public static float SetPosY(Data data, UIRectTransform originRect, float posY)
    {
        float ret = 0;
        if (data != null)
        {
            RectTransform rectTransform = (RectTransform)Data.FindTransform(data);
            if (rectTransform != null)
            {
                if (originRect != null)
                {
                    originRect.set(rectTransform);
                    SetPosY(rectTransform, posY);
                    ret = rectTransform.rect.height;
                }
                else
                {
                    Logger.LogError("originRect null");
                }
            }
            else
            {
                Logger.LogError("rectTransform null");
            }
        }
        else
        {
            Logger.LogError("data null");
        }
        return ret;
    }

    /**
     * get height
     * */
    public static float SetPosY(Data data, float posY)
    {
        float ret = 0;
        if (data != null)
        {
            RectTransform rectTransform = (RectTransform)Data.FindTransform(data);
            if (rectTransform != null)
            {
                SetPosY(rectTransform, posY);
                ret = rectTransform.rect.height;
            }
            else
            {
                Logger.LogError("rectTransform null");
            }
        }
        else
        {
            // Logger.LogError("data null");
        }
        return ret;
    }

    public static float GetHeight(Data data)
    {
        float ret = 0;
        if (data != null)
        {
            RectTransform rectTransform = (RectTransform)Data.FindTransform(data);
            if (rectTransform != null)
            {
                ret = rectTransform.rect.height;
            }
            else
            {
                Logger.LogError("rectTransform null");
            }
        }
        else
        {
            Logger.LogError("data null");
        }
        return ret;
    }

    /*public static void SetSize(RectTransform rectTransform, Vector2 size)
    {
        Vector2 oldSize = rectTransform.rect.size;
        Vector2 deltaSize = size - oldSize;

        rectTransform.offsetMin = rectTransform.offsetMin - new Vector2(
            deltaSize.x * rectTransform.pivot.x,
            deltaSize.y * rectTransform.pivot.y);
        rectTransform.offsetMax = rectTransform.offsetMax + new Vector2(
            deltaSize.x * (1f - rectTransform.pivot.x),
            deltaSize.y * (1f - rectTransform.pivot.y));
    }

    public static void SetWidth(RectTransform rectTransform, float size)
    {
        SetSize(rectTransform, new Vector2(size, rectTransform.rect.size.y));
    }

    public static void SetHeight(RectTransform rectTransform, float size)
    {
        Debug.LogError("setHeight: " + size);
        SetSize(rectTransform, new Vector2(rectTransform.rect.size.x, size));
    }*/

    public static void SetHeight(RectTransform rectTransform, float size)
    {
        // Debug.LogError("setHeight: " + size);
        if (rectTransform.anchorMin.y == 1.0f
            && rectTransform.anchorMax.y == 1.0f
            && rectTransform.pivot.y == 1.0f)
        {
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, rectTransform.anchoredPosition.y - size);
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, size);
        }
        else if (rectTransform.anchorMin.y == 0.5f
          && rectTransform.anchorMax.y == 0.5f
          && rectTransform.pivot.y == 0.5f)
        {
            // offsetMin: (-180.0, -240.0); offsetMax: (180.0, 240.0); sizeDelta: (360.0, 480.0)
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, -size / 2.0f);
            rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, size / 2.0f);
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, size);
        }
        else if (rectTransform.anchorMin.y == 0.0f
           && rectTransform.anchorMax.y == 0.0f
           && rectTransform.pivot.y == 0.0f)
        {
            rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, rectTransform.anchoredPosition.y - size);
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, size);
        }
        else
        {
            Logger.LogError("unknown rect type: " + UIRectTransform.PrintRectTransform(rectTransform));
        }
    }

    #endregion

    public static UIRectTransform CreateTopBottomRect(float height)
    {
        UIRectTransform rect = new UIRectTransform();
        {
            // anchoredPosition: (0.0, 0.0); anchorMin: (0.0, 1.0); anchorMax: (1.0, 1.0); pivot: (0.5, 1.0);
            // offsetMin: (0.0, -300.0); offsetMax: (0.0, 0.0); sizeDelta: (0.0, 300.0);
            rect.anchoredPosition = new Vector3(0.0f, 0.0f, 0.0f);
            rect.anchorMin = new Vector2(0.0f, 1.0f);
            rect.anchorMax = new Vector2(1.0f, 1.0f);
            rect.pivot = new Vector2(0.5f, 1.0f);
            rect.offsetMin = new Vector2(0.0f, -height);
            rect.offsetMax = new Vector2(0.0f, 0.0f);
            rect.sizeDelta = new Vector2(0.0f, height);
        }
        return rect;
    }

    public static UIRectTransform CreateFullRect(float left, float right, float top, float bottom)
    {
        UIRectTransform ret = new UIRectTransform(UIConstants.FullParent);
        {
            // left 5, right 10, top 30, bottom 60
            // anchoredPosition: (-2.5, 15.0); offsetMin: (5.0, 60.0); 
            // offsetMax: (-10.0, -30.0); sizeDelta: (-15.0, -90.0);
            ret.anchoredPosition = new Vector3((left - right) / 2.0f, (bottom - top) / 2.0f, 0);
            ret.offsetMin = new Vector2(left, bottom);
            ret.offsetMax = new Vector2(-right, -top);
            ret.sizeDelta = new Vector2(-(left + right), -(top + bottom));
        }
        return ret;
    }

    public static UIRectTransform CreateCenterRect(float width, float height)
    {
        UIRectTransform ret = new UIRectTransform();
        {
            ret.anchoredPosition = new Vector3(0.0f, 0.0f, 0.0f);
            ret.anchorMin = new Vector2(0.5f, 0.5f);
            ret.anchorMax = new Vector2(0.5f, 0.5f);
            ret.pivot = new Vector2(0.5f, 0.5f);
            ret.offsetMin = new Vector2(-width/2, -height/2);
            ret.offsetMax = new Vector2(width/2, height/2);
            ret.sizeDelta = new Vector2(width, height);
        }
        return ret;
    }

    public static UIRectTransform CreateCenterRect(float width, float height, float x, float y)
    {
        UIRectTransform ret = new UIRectTransform();
        {
            ret.anchoredPosition = new Vector3(x, y, 0.0f);
            ret.anchorMin = new Vector2(0.5f, 0.5f);
            ret.anchorMax = new Vector2(0.5f, 0.5f);
            ret.pivot = new Vector2(0.5f, 0.5f);
            ret.offsetMin = new Vector2(x - width / 2, y - height / 2);
            ret.offsetMax = new Vector2(x + width / 2, y + height / 2);
            ret.sizeDelta = new Vector2(width, height);
        }
        return ret;
    }

    #region set center posY

    public static void SetCenterPosY(RectTransform rectTransform, float posY)
    {
        if (rectTransform != null)
        {
            // -100
            // anchoredPosition: (0.0, 100.0); offsetMin: (-80.0, 85.0); offsetMax: (80.0, 115.0); sizeDelta: (160.0, 30.0);
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -posY);
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, -posY - rectTransform.sizeDelta.y / 2.0f);
            rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, -posY + rectTransform.sizeDelta.y / 2.0f);
        }
        else
        {
            Logger.LogError("rectTransform null");
        }
    }

    #endregion

    #region siblingIndex

    public static bool SetSiblingIndex(Data data, int siblingIndex)
    {
        bool ret = false;
        if (data != null)
        {
            Transform transform = Data.FindTransform(data);
            if (transform != null)
            {
                transform.SetSiblingIndex(siblingIndex);
                ret = true;
            }
            else
            {
                Logger.LogError("transform null");
            }
        }
        else
        {
            // Logger.LogError("data null");
        }
        return ret;
    }

    public static bool SetSiblingIndexLast(Data data)
    {
        bool ret = false;
        if (data != null)
        {
            Transform transform = Data.FindTransform(data);
            if (transform != null)
            {
                transform.SetAsLastSibling();
                ret = true;
            }
            else
            {
                Logger.LogError("transform null");
            }
        }
        else
        {
            // Debug.LogError("data null");
        }
        return ret;
    }

    public static bool SetActive(Data data, bool isActive)
    {
        bool ret = false;
        if (data != null)
        {
            Transform transform = Data.FindTransform(data);
            if (transform != null)
            {
                transform.gameObject.SetActive(isActive);
                ret = true;
            }
            else
            {
                Logger.LogError("transform null");
            }
        }
        else
        {
            Logger.LogError("data null");
        }
        return ret;
    }

    #endregion

    public static void GetMargin(RectTransform rectTransform, out float left, out float right, out float top, out float bottom)
    {
        if(rectTransform.anchorMin == new Vector2(0.5f, 0.5f) && rectTransform.anchorMax == new Vector2(0.5f, 0.5f) && rectTransform.pivot == new Vector2(0.5f, 0.5f))
        {
            left = rectTransform.rect.xMin + rectTransform.anchoredPosition.x;
            right = rectTransform.rect.xMax + rectTransform.anchoredPosition.x;
            top = rectTransform.rect.yMin + rectTransform.anchoredPosition.y;
            bottom = rectTransform.rect.yMax + rectTransform.anchoredPosition.y;
        }
        else
        {
            left = rectTransform.rect.xMin;
            right = rectTransform.rect.xMax;
            top = rectTransform.rect.yMin;
            bottom = rectTransform.rect.yMax;
        }
    }

    public enum ShowType
    {
        Normal,
        HeadLess,
        OnlyHead
    }

    #region create request rect

    public const float CommonToggleHeight = 40;
    public const float CommonDropDownHeight = 30;
    public const float CommonTextHeight = 30;

    public static UIRectTransform createRequestRect(float paddingLeft, float paddingRight, float height)
    {
        UIRectTransform rect = new UIRectTransform();
        {
            rect.anchoredPosition = new Vector3((paddingLeft - paddingRight) / 2, 0f, 0f);
            rect.anchorMin = new Vector2(0.0f, 1.0f);
            rect.anchorMax = new Vector2(1.0f, 1.0f);
            rect.pivot = new Vector2(0.5f, 1f);
            rect.offsetMin = new Vector2(paddingLeft, -height);
            rect.offsetMax = new Vector2(-paddingRight, 0);
            rect.sizeDelta = new Vector2(-paddingLeft - paddingRight, height);
        }
        return rect;
    }

    #endregion

    #region headerHeight

    #region setButtonTopLeft

    public static void SetButtonTopLeftTransform(Data data, float x = 0)
    {
        if (data != null)
        {
            RectTransform rectTransform = (RectTransform)Data.FindTransform(data);
            if (rectTransform != null)
            {
                SetButtonTopLeftTransform(rectTransform, x);
            }
            else
            {
                Logger.LogError("rectTransform null");
            }
        }
        else
        {
            Logger.LogError("data null");
        }
    }

    public static void SetButtonTopLeftTransform(MonoBehaviour button, float x = 0)
    {
        if (button != null)
        {
            SetButtonTopLeftTransform((RectTransform)button.transform, x);
        }
        else
        {
            Logger.LogError("button null");
        }
    }

    private static void SetButtonTopLeftTransform(RectTransform rectTransform, float x = 0)
    {
        /*UIRectTransform rect = new UIRectTransform();
        {
            // anchoredPosition: (60.0, 0.0); anchorMin: (0.0, 1.0); anchorMax: (0.0, 1.0); pivot: (0.0, 1.0);
            //  offsetMin: (60.0, -30.0); offsetMax: (90.0, 0.0); sizeDelta: (30.0, 30.0);
            float buttonSize = Setting.get().buttonSize.v;
            rect.anchoredPosition = new Vector3(x, 0.0f, 0.0f);
            rect.anchorMin = new Vector2(0.0f, 1.0f);
            rect.anchorMax = new Vector2(0.0f, 1.0f);
            rect.pivot = new Vector2(0.0f, 1.0f);
            rect.offsetMin = new Vector2(x, -buttonSize);
            rect.offsetMax = new Vector2(x + buttonSize, 0.0f);
            rect.sizeDelta = new Vector2(buttonSize, buttonSize);
        }
        rect.set(rectTransform);*/
    }

    #endregion

    public static void SetButtonTopRightTransform(MonoBehaviour button)
    {
        /*if (button != null)
        {
            UIRectTransform rect = new UIRectTransform();
            {
                // anchoredPosition: (0.0, 0.0); anchorMin: (0.0, 1.0); anchorMax: (0.0, 1.0); pivot: (0.0, 1.0);
                // offsetMin: (0.0, -30.0); offsetMax: (30.0, 0.0); sizeDelta: (30.0, 30.0);
                float buttonSize = Setting.get().buttonSize.v;
                rect.anchoredPosition = new Vector3(0.0f, 0.0f, 0.0f);
                rect.anchorMin = new Vector2(1.0f, 1.0f);
                rect.anchorMax = new Vector2(1.0f, 1.0f);
                rect.pivot = new Vector2(1.0f, 1.0f);
                rect.offsetMin = new Vector2(-buttonSize, -buttonSize);
                rect.offsetMax = new Vector2(0.0f, 0.0f);
                rect.sizeDelta = new Vector2(buttonSize, buttonSize);
            }
            rect.set((RectTransform)button.transform);
        }
        else
        {
            Debug.LogError("button null");
        }*/
    }

    public static void SetTitleTransform(Text lbTitle)
    {
        /*if (lbTitle != null)
        {
            UIRectTransform.SetHeight((RectTransform)lbTitle.rectTransform, Setting.get().getButtonSize());
        }
        else
        {
            Debug.LogError("lbTitle null");
        }*/
    }

    public static void SetButtonTopRightTransformWidthHeight(MonoBehaviour button, float width, float height)
    {
        /*if (button != null)
        {
            UIRectTransform rect = new UIRectTransform();
            {
                // anchoredPosition: (0.0, 0.0); anchorMin: (0.0, 1.0); anchorMax: (0.0, 1.0); pivot: (0.0, 1.0);
                // offsetMin: (0.0, -30.0); offsetMax: (30.0, 0.0); sizeDelta: (30.0, 30.0);
                float buttonSize = Setting.get().buttonSize.v;
                rect.anchoredPosition = new Vector3(0.0f, 0.0f, 0.0f);
                rect.anchorMin = new Vector2(1.0f, 1.0f);
                rect.anchorMax = new Vector2(1.0f, 1.0f);
                rect.pivot = new Vector2(1.0f, 1.0f);
                rect.offsetMin = new Vector2(-width, -height);
                rect.offsetMax = new Vector2(0.0f, 0.0f);
                rect.sizeDelta = new Vector2(width, height);
            }
            rect.set((RectTransform)button.transform);
        }
        else
        {
            Debug.LogError("button null");
        }*/
    }

    #endregion

}