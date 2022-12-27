using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIUtils
{

    public const string MainTex = "_MainTex";


    /*public static void OnClickTweenButton(Button btn)
    {
        if (btn != null)
        {
            Vector3 tempBtn = btn.transform.localScale;
            if (tempBtn.x > 0)
            {
                tempBtn = new Vector3(0.85f, 0.85f, 0);
            }
            iTween.ScaleTo(btn.gameObject, iTween.Hash("scale", tempBtn, "time", 0.1f, "easetype", iTween.EaseType.linear));
        }
        else
        {
            Logger.LogError("btn null");
        }
    }*/

    #region Button

    public static void SetButtonOnClick(Button btn, UnityEngine.Events.UnityAction action)
    {
        if (btn != null)
        {
            // btn.onClick.RemoveListener(action);
            // btn.onClick.AddListener(action);
        }
        else
        {
            // Debug.LogError("btn null");
        }
    }

    #endregion

    public static UIBehavior<T> Instantiate<T>(T data, UIBehavior<T> prefab, Transform transform) where T : Data
    {
        if (data != null)
        {
            if (transform != null && prefab != null)
            {
                UIBehavior<T> old = data.findCallBack<UIBehavior<T>>(prefab.GetType());
                if (old == null)
                {
                    UIBehavior<T> newUI = TrashMan.normalSpawn(prefab, transform);
                    newUI.setData(data);
                    data.uiGameObject = newUI.gameObject;
                    // haveTransformData
                    {
                        if(newUI is HaveTransformData)
                        {
                            data.haveTransformData = (HaveTransformData)newUI;
                        }
                    }
                    // if (needRefresh)
                    newUI.refresh();
                    return newUI;
                }
                else
                {
                    Logger.LogError("already have");
                    return old;
                }
            }
            else
            {
                Logger.LogError("transform or prefab null");
                return null;
            }
        }
        else
        {
            Logger.LogError("error, Instantiate: why data null");
            return null;
        }
    }

    public static UIBehavior<T> Instantiate<T>(T data, UIBehavior<T> prefab, Transform transform, UIRectTransform uiRectTransform) where T : Data
    {
        if (data != null)
        {
            if (transform != null && prefab != null)
            {
                UIBehavior<T> old = data.findCallBack<UIBehavior<T>>(prefab.GetType());
                if (old == null)
                {
                    /*UIBehavior<T> newUI = TrashMan.normalSpawnUI (prefab, transform, data);
                    return newUI;*/
                    UIBehavior<T> newUI = TrashMan.normalSpawn(prefab, transform);
                    newUI.setData(data);
                    uiRectTransform.set((RectTransform)newUI.transform);
                    data.uiGameObject = newUI.gameObject;
                    // haveTransformData
                    {
                        if (newUI is HaveTransformData)
                        {
                            data.haveTransformData = (HaveTransformData)newUI;
                        }
                    }
                    // if (needRefresh)
                    newUI.refresh();
                    return newUI;
                }
                else
                {
                    Logger.LogError("already have");
                    return old;
                }
            }
            else
            {
                Logger.LogError("transform or prefab null");
                return null;
            }
        }
        else
        {
            Logger.LogError("error, Instantiate: why data null");
            return null;
        }
    }

    public static UIBehavior<T> Instantiate<T>(T data, UIBehavior<T> prefab, Transform transform, int siblingIndex) where T : Data
    {
        if (data != null)
        {
            if (transform != null && prefab != null)
            {
                UIBehavior<T> old = data.findCallBack<UIBehavior<T>>(prefab.GetType());
                if (old == null)
                {
                    UIBehavior<T> newUI = TrashMan.normalSpawn(prefab, transform);
                    newUI.setData(data);
                    newUI.transform.SetSiblingIndex(siblingIndex);
                    data.uiGameObject = newUI.gameObject;
                    // haveTransformData
                    {
                        if (newUI is HaveTransformData)
                        {
                            data.haveTransformData = (HaveTransformData)newUI;
                        }
                    }
                    // if (needRefresh)
                    newUI.refresh();
                    return newUI;
                }
                else
                {
                    Logger.LogError("already have");
                    return old;
                }
            }
            else
            {
                Logger.LogError("transform null");
                return null;
            }
        }
        else
        {
            Logger.LogError("error, Instantiate: why data null");
            return null;
        }
    }

    public static UIBehavior<T> Instantiate<T>(T data, UIBehavior<T> prefab, Transform transform, Vector3 globalScale) where T : Data
    {
        if (data != null)
        {
            if (transform != null && prefab != null)
            {
                UIBehavior<T> old = data.findCallBack<UIBehavior<T>>(prefab.GetType());
                if (old == null)
                {
                    UIBehavior<T> newUI = TrashMan.normalSpawn(prefab, transform, globalScale);
                    newUI.setData(data);
                    data.uiGameObject = newUI.gameObject;
                    // haveTransformData
                    {
                        if (newUI is HaveTransformData)
                        {
                            data.haveTransformData = (HaveTransformData)newUI;
                        }
                    }
                    // if (needRefresh)
                    newUI.refresh();
                    return newUI;
                }
                else
                {
                    Logger.LogError("already have");
                    return old;
                }
            }
            else
            {
                Logger.LogError("transform or prefab null");
                return null;
            }
        }
        else
        {
            Logger.LogError("error, Instantiate: why data null");
            return null;
        }
    }

    #region instantiate root

    public static UIBehavior<T> Instantiate<T>(T data, UIBehavior<T> prefab) where T : Data
    {
        if (data != null)
        {
            if (prefab != null)
            {
                UIBehavior<T> old = data.findCallBack<UIBehavior<T>>(prefab.GetType());
                if (old == null)
                {
                    UIBehavior<T> newUI = TrashMan.normalSpawn(prefab, null);
                    newUI.setData(data);
                    data.uiGameObject = newUI.gameObject;
                    // haveTransformData
                    {
                        if (newUI is HaveTransformData)
                        {
                            data.haveTransformData = (HaveTransformData)newUI;
                        }
                    }
                    // if (needRefresh)
                    newUI.refresh();
                    return newUI;
                }
                else
                {
                    Logger.LogError("already have");
                    return old;
                }
            }
            else
            {
                Logger.LogError("transform or prefab null");
                return null;
            }
        }
        else
        {
            Logger.LogError("error, Instantiate: why data null");
            return null;
        }
    }

    #endregion

    #region dropDown

    public static void RefreshDropDownOptions(Dropdown drValue, string[] options)
    {
        if (drValue.options.Count != options.Length)
        {
            drValue.options.Clear();
            for (int i = 0; i < options.Length; i++)
            {
                Dropdown.OptionData optionData = new Dropdown.OptionData();
                {
                    optionData.text = options[i];
                }
                drValue.options.Add(optionData);
            }
        }
        else
        {
            // options
            for (int i = 0; i < options.Length; i++)
            {
                drValue.options[i].text = options[i];
            }
        }
    }

    public static void RefreshDropDownOptions(Dropdown drValue, List<string> options)
    {
        if (drValue.options.Count != options.Count)
        {
            drValue.options.Clear();
            for (int i = 0; i < options.Count; i++)
            {
                Dropdown.OptionData optionData = new Dropdown.OptionData();
                {
                    optionData.text = options[i];
                }
                drValue.options.Add(optionData);
            }
        }
        else
        {
            // options
            for (int i = 0; i < options.Count; i++)
            {
                drValue.options[i].text = options[i];
            }
        }
    }

    #endregion

    #region scale

    public static readonly Vector3 Vector1 = new Vector3(1, 1, 1);

    public static void SetGlobalScale(Transform transform)
    {
        SetGlobalScale(transform, Vector1);
    }

    public static void SetGlobalScale(Transform transform, Vector3 globalScale)
    {
        if (transform.parent != null)
        {
            Vector3 localScale = transform.parent.InverseTransformVector(globalScale);
            transform.localScale = localScale;
        }
        else
        {
            // Debug.LogError("transform parent null");
            transform.localScale = globalScale;
        }
    }

    #endregion

    public static void UpdateTransformData(Data data)
    {
        if (data != null)
        {
            // find
            HaveTransformData ui = data.findTransformData();
            // process
            if (ui != null)
            {
                ui.setDirtyForTransformData();
                // updateTransform
                TransformData transformData = ui.getTransformData();
                RectTransform transform = (RectTransform)ui.getUITransform();
                if (transformData != null && transform != null)
                {
                    transformData.update(transform);
                    // Debug.LogError("haveUI set dirty: " + ui + ", " + transformData + ", " + transform);
                }
                else
                {
                    Logger.LogError("transformData, transform null");
                }
            }
            else
            {
                Logger.LogError("ui null");
            }
        }
        else
        {
            Logger.LogError("data null");
        }
    }

    #region UI

    public static void SetHeaderPosition(Text lbTitle, UIRectTransform.ShowType showType, ref float deltaY)
    {
        /*switch (showType)
        {
            case UIRectTransform.ShowType.Normal:
                {
                    float headerHeight = Setting.get().getButtonSize();
                    if (lbTitle != null)
                    {
                        lbTitle.gameObject.SetActive(true);
                        UIRectTransform.SetHeight(lbTitle.rectTransform, headerHeight);
                    }
                    else
                    {
                        Debug.LogError("lbTitle null");
                    }
                    deltaY += headerHeight;
                }
                break;
            case UIRectTransform.ShowType.HeadLess:
                {
                    if (lbTitle != null)
                    {
                        lbTitle.gameObject.SetActive(false);
                    }
                    else
                    {
                        Debug.LogError("lbTitle null");
                    }
                }
                break;
            case UIRectTransform.ShowType.OnlyHead:
                break;
            default:
                Debug.LogError("unknown type: " + showType);
                break;
        }*/
    }

    #region setLabelContentPosition

    public static void SetLabelContentPosition(Text label, RequestChangeBoolUI.UIData requestChangeBoolUIData, ref float deltaY)
    {
        /*if (requestChangeBoolUIData != null)
        {
            float itemHeight = Setting.get().getItemSize();
            if (label != null)
            {
                label.gameObject.SetActive(true);
                UIRectTransform.SetPosY(label.rectTransform, deltaY);
                UIRectTransform.SetHeight(label.rectTransform, itemHeight);
            }
            else
            {
                Debug.LogError("label null");
            }
            UIRectTransform.SetPosY(requestChangeBoolUIData, deltaY + (itemHeight - UIConstants.RequestBoolDim) / 2.0f);
            deltaY += itemHeight;
        }
        else
        {
            if (label != null)
            {
                label.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("label null");
            }
        }*/
    }

    public static void SetLabelContentPosition(Text label, RequestChangeEnumUI.UIData requestChangeEnumUIData, ref float deltaY)
    {
        /*if (requestChangeEnumUIData != null)
        {
            float itemHeight = Setting.get().getItemSize();
            if (label != null)
            {
                label.gameObject.SetActive(true);
                UIRectTransform.SetPosY(label.rectTransform, deltaY);
                UIRectTransform.SetHeight(label.rectTransform, itemHeight);
            }
            else
            {
                Debug.LogError("label null");
            }
            UIRectTransform.SetPosY(requestChangeEnumUIData, deltaY + (itemHeight - UIConstants.RequestEnumHeight) / 2.0f);
            deltaY += itemHeight;
        }
        else
        {
            if (label != null)
            {
                label.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("label null");
            }
        }*/
    }

    public static void SetLabelContentPosition(Text label, RequestChangeFloatUI.UIData requestChangeFloatUIData, ref float deltaY)
    {
        /*if (requestChangeFloatUIData != null)
        {
            float itemHeight = Setting.get().getItemSize();
            if (label != null)
            {
                label.gameObject.SetActive(true);
                UIRectTransform.SetPosY(label.rectTransform, deltaY);
                UIRectTransform.SetHeight(label.rectTransform, itemHeight);
            }
            else
            {
                Debug.LogError("label null");
            }
            UIRectTransform.SetPosY(requestChangeFloatUIData, deltaY + (itemHeight - UIConstants.RequestHeight) / 2.0f);
            deltaY += itemHeight;
        }
        else
        {
            if (label != null)
            {
                label.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("label null");
            }
        }*/
    }

    public static void SetLabelContentPosition(Text label, RequestChangeIntUI.UIData requestChangeIntUIData, ref float deltaY)
    {
        /*if (requestChangeIntUIData != null)
        {
            float itemHeight = Setting.get().getItemSize();
            if (label != null)
            {
                label.gameObject.SetActive(true);
                UIRectTransform.SetPosY(label.rectTransform, deltaY);
                UIRectTransform.SetHeight(label.rectTransform, itemHeight);
            }
            else
            {
                Debug.LogError("label null");
            }
            UIRectTransform.SetPosY(requestChangeIntUIData, deltaY + (itemHeight - UIConstants.RequestHeight) / 2.0f);
            deltaY += itemHeight;
        }
        else
        {
            if (label != null)
            {
                label.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("label null");
            }
        }*/
    }

    public static void SetLabelContentPosition(Text label, RequestChangeLongUI.UIData requestChangeLongUIData, ref float deltaY)
    {
        /*if (requestChangeLongUIData != null)
        {
            float itemHeight = Setting.get().getItemSize();
            if (label != null)
            {
                label.gameObject.SetActive(true);
                UIRectTransform.SetPosY(label.rectTransform, deltaY);
                UIRectTransform.SetHeight(label.rectTransform, itemHeight);
            }
            else
            {
                Debug.LogError("label null");
            }
            UIRectTransform.SetPosY(requestChangeLongUIData, deltaY + (itemHeight - UIConstants.RequestHeight) / 2.0f);
            deltaY += itemHeight;
        }
        else
        {
            if (label != null)
            {
                label.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("label null");
            }
        }*/
    }

    public static void SetLabelContentPosition(Text label, RequestChangeStringUI.UIData requestChangeStringUIData, ref float deltaY)
    {
        /*if (requestChangeStringUIData != null)
        {
            float itemHeight = Setting.get().getItemSize();
            if (label != null)
            {
                label.gameObject.SetActive(true);
                UIRectTransform.SetPosY(label.rectTransform, deltaY);
                UIRectTransform.SetHeight(label.rectTransform, itemHeight);
            }
            else
            {
                Debug.LogError("label null");
            }
            UIRectTransform.SetPosY(requestChangeStringUIData, deltaY + (itemHeight - UIConstants.RequestEnumHeight) / 2.0f);
            deltaY += itemHeight;
        }
        else
        {
            if (label != null)
            {
                label.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("label null");
            }
        }*/
    }

    #endregion

    #region setLabelPositionWithBg

    public static void SetLabelContentPositionBg(Text label, RequestChangeBoolUI.UIData requestChangeBoolUIData, ref float deltaY, ref float bgHeight)
    {
        /*if (requestChangeBoolUIData != null)
        {
            float itemHeight = Setting.get().getItemSize();
            if (label != null)
            {
                label.gameObject.SetActive(true);
                UIRectTransform.SetPosY(label.rectTransform, deltaY);
                UIRectTransform.SetHeight(label.rectTransform, itemHeight);
            }
            else
            {
                Debug.LogError("label null");
            }
            UIRectTransform.SetPosY(requestChangeBoolUIData, deltaY + (itemHeight - UIConstants.RequestBoolDim) / 2.0f);
            bgHeight += itemHeight;
            deltaY += itemHeight;
        }
        else
        {
            if (label != null)
            {
                label.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("label null");
            }
        }*/
    }

    public static void SetLabelContentPositionBg(Text label, RequestChangeEnumUI.UIData requestChangeEnumUIData, ref float deltaY, ref float bgHeight)
    {
        /*if (requestChangeEnumUIData != null)
        {
            float itemHeight = Setting.get().getItemSize();
            if (label != null)
            {
                label.gameObject.SetActive(true);
                UIRectTransform.SetPosY(label.rectTransform, deltaY);
                UIRectTransform.SetHeight(label.rectTransform, itemHeight);
            }
            else
            {
                Debug.LogError("label null");
            }
            UIRectTransform.SetPosY(requestChangeEnumUIData, deltaY + (itemHeight - UIConstants.RequestEnumHeight) / 2.0f);
            bgHeight += itemHeight;
            deltaY += itemHeight;
        }
        else
        {
            if (label != null)
            {
                label.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("label null");
            }
        }*/
    }

    public static void SetLabelContentPositionBg(Text label, RequestChangeFloatUI.UIData requestChangeFloatUIData, ref float deltaY, ref float bgHeight)
    {
        /*if (requestChangeFloatUIData != null)
        {
            float itemHeight = Setting.get().getItemSize();
            if (label != null)
            {
                label.gameObject.SetActive(true);
                UIRectTransform.SetPosY(label.rectTransform, deltaY);
                UIRectTransform.SetHeight(label.rectTransform, itemHeight);
            }
            else
            {
                Debug.LogError("label null");
            }
            UIRectTransform.SetPosY(requestChangeFloatUIData, deltaY + (itemHeight - UIConstants.RequestHeight) / 2.0f);
            bgHeight += itemHeight;
            deltaY += itemHeight;
        }
        else
        {
            if (label != null)
            {
                label.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("label null");
            }
        }*/
    }

    public static void SetLabelContentPositionBg(Text label, RequestChangeIntUI.UIData requestChangeIntUIData, ref float deltaY, ref float bgHeight)
    {
        /*if (requestChangeIntUIData != null)
        {
            float itemHeight = Setting.get().getItemSize();
            if (label != null)
            {
                label.gameObject.SetActive(true);
                UIRectTransform.SetPosY(label.rectTransform, deltaY);
                UIRectTransform.SetHeight(label.rectTransform, itemHeight);
            }
            else
            {
                Debug.LogError("label null");
            }
            UIRectTransform.SetPosY(requestChangeIntUIData, deltaY + (itemHeight - UIConstants.RequestHeight) / 2.0f);
            bgHeight += itemHeight;
            deltaY += itemHeight;
        }
        else
        {
            if (label != null)
            {
                label.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("label null");
            }
        }*/
    }

    public static void SetLabelContentPositionBg(Text label, RequestChangeLongUI.UIData requestChangeLongUIData, ref float deltaY, ref float bgHeight)
    {
        /*if (requestChangeLongUIData != null)
        {
            float itemHeight = Setting.get().getItemSize();
            if (label != null)
            {
                label.gameObject.SetActive(true);
                UIRectTransform.SetPosY(label.rectTransform, deltaY);
                UIRectTransform.SetHeight(label.rectTransform, itemHeight);
            }
            else
            {
                Debug.LogError("label null");
            }
            UIRectTransform.SetPosY(requestChangeLongUIData, deltaY + (itemHeight - UIConstants.RequestHeight) / 2.0f);
            bgHeight += itemHeight;
            deltaY += itemHeight;
        }
        else
        {
            if (label != null)
            {
                label.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("label null");
            }
        }*/
    }

    public static void SetLabelContentPositionBg(Text label, RequestChangeStringUI.UIData requestChangeStringUIData, ref float deltaY, ref float bgHeight)
    {
        /*if (requestChangeStringUIData != null)
        {
            float itemHeight = Setting.get().getItemSize();
            if (label != null)
            {
                label.gameObject.SetActive(true);
                UIRectTransform.SetPosY(label.rectTransform, deltaY);
                UIRectTransform.SetHeight(label.rectTransform, itemHeight);
            }
            else
            {
                Debug.LogError("label null");
            }
            UIRectTransform.SetPosY(requestChangeStringUIData, deltaY + (itemHeight - UIConstants.RequestEnumHeight) / 2.0f);
            bgHeight += itemHeight;
            deltaY += itemHeight;
        }
        else
        {
            if (label != null)
            {
                label.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("label null");
            }
        }*/
    }

    #endregion

    #endregion

}