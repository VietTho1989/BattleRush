using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
// using KidPedia.Utils;

public class SettingsUpdate : UpdateBehavior<Settings>
{

    #region lifeCycle

    private System.Action<object> callback;

    private void Awake()
    {
        // language
        /*{
            callback = (myObject) =>
            {
                setLanguageByLanguageTool();
            };
            EventDispatcher.Instance.RegisterListener(EventID.OnChangeLanguage, callback);
            // init
            setLanguageByLanguageTool();
        }*/
        this.setData(Settings.get());
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        /*if (callback != null && EventDispatcher.Instance != null)
        {
            EventDispatcher.Instance.RemoveListener(EventID.OnChangeLanguage, callback);
            callback = null;
        }*/
    }

    private void setLanguageByLanguageTool()
    {
        // en de fr pt es it vi
        /*switch (Language.LanguageToolExtensions.LANGUAGE)
        {
            case Language.TYPE_LANGUAGE.ENGLISH:
                Settings.get().language.v = CommonLibrary.Language.Type.en;
                break;
            case Language.TYPE_LANGUAGE.GERMAN:
                Settings.get().language.v = CommonLibrary.Language.Type.de;
                break;
            case Language.TYPE_LANGUAGE.FRENCH:
                Settings.get().language.v = CommonLibrary.Language.Type.fr;
                break;
            case Language.TYPE_LANGUAGE.PORTUGAL:
                Settings.get().language.v = CommonLibrary.Language.Type.pt;
                break;
            case Language.TYPE_LANGUAGE.SPAIN:
                Settings.get().language.v = CommonLibrary.Language.Type.es;
                break;
            case Language.TYPE_LANGUAGE.ITALY:
                Settings.get().language.v = CommonLibrary.Language.Type.it;
                break;
            case Language.TYPE_LANGUAGE.VIETNAMESE:
                Settings.get().language.v = CommonLibrary.Language.Type.vi;
                break;
            case Language.TYPE_LANGUAGE.NOT_DEFINE:
                Settings.get().language.v = CommonLibrary.Language.Type.vi;
                break;
            default:
                Logger.LogError("unknown language: " + Language.LanguageToolExtensions.LANGUAGE);
                break;
        }*/
    }

    #endregion

    #region Update

    private bool needSaveLanguage = false;

    public override void update()
    {
        if (dirty)
        {
            dirty = false;
            if (this.data != null)
            {

            }
            else
            {
                Logger.LogError("data null");
            }
        }
    }

    public override bool isShouldDisableUpdate()
    {
        return true;
    }

    #endregion

    #region implement callBacks

    public override void onAddCallBack<T>(T data)
    {
        if(data is Settings)
        {
            Settings settings = data as Settings;
            // Child
            {
                settings.checkIpAddressCountry.allAddCallBack(this);
            }
            dirty = true;
            return;
        }
        // Child
        if (data is Settings.CheckIpAddressCountry)
        {
            Settings.CheckIpAddressCountry checkIpAddressCountry = data as Settings.CheckIpAddressCountry;
            // Update
            {
                switch (checkIpAddressCountry.getType())
                {
                    case Settings.CheckIpAddressCountry.Type.Not:
                        {
                            CheckIpAddressS.Not not = checkIpAddressCountry as CheckIpAddressS.Not;
                            UpdateUtils.makeUpdate<CheckIpAddressS.NotUpdate, CheckIpAddressS.Not>(not, this.transform);
                        }
                        break;
                    case Settings.CheckIpAddressCountry.Type.Already:
                        {
                            CheckIpAddressS.Already already = checkIpAddressCountry as CheckIpAddressS.Already;
                            UpdateUtils.makeUpdate<CheckIpAddressS.AlreadyUpdate, CheckIpAddressS.Already>(already, this.transform);
                        }
                        break;
                    default:
                        Logger.LogError("unknown type: " + checkIpAddressCountry.getType());
                        break;
                }
            }
            dirty = true;
            return;
        }
        Logger.LogError("Don't process: " + data + "; " + this);
    }

    public override void onRemoveCallBack<T>(T data, bool isHide)
    {
        if(data is Settings)
        {
            Settings settings = data as Settings;
            // Child
            {
                settings.checkIpAddressCountry.allRemoveCallBack(this);
            }
            this.setDataNull(settings);
            return;
        }
        // Child
        if (data is Settings.CheckIpAddressCountry)
        {
            Settings.CheckIpAddressCountry checkIpAddressCountry = data as Settings.CheckIpAddressCountry;
            // Update
            {
                switch (checkIpAddressCountry.getType())
                {
                    case Settings.CheckIpAddressCountry.Type.Not:
                        {
                            CheckIpAddressS.Not not = checkIpAddressCountry as CheckIpAddressS.Not;
                            not.removeCallBackAndDestroy(typeof(CheckIpAddressS.NotUpdate));
                        }
                        break;
                    case Settings.CheckIpAddressCountry.Type.Already:
                        {
                            CheckIpAddressS.Already already = checkIpAddressCountry as CheckIpAddressS.Already;
                            already.removeCallBackAndDestroy(typeof(CheckIpAddressS.AlreadyUpdate));
                        }
                        break;
                    default:
                        Logger.LogError("unknown type: " + checkIpAddressCountry.getType());
                        break;
                }
            }
            return;
        }
        Logger.LogError("Don't process: " + data + "; " + this);
    }

    public override void onUpdateSync<T>(WrapProperty wrapProperty, List<Sync<T>> syncs)
    {
        if (WrapProperty.checkError(wrapProperty))
        {
            return;
        }
        if(wrapProperty.p is Settings)
        {
            switch ((Settings.Property)wrapProperty.n)
            {
                case Settings.Property.language:
                    {
                        needSaveLanguage = true;
                        dirty = true;
                    }
                    break;
                case Settings.Property.checkIpAddressCountry:
                    {
                        ValueChangeUtils.replaceCallBack(this, syncs);
                        dirty = true;
                    }
                    break;
                default:
                    Logger.LogError("Don't process: " + wrapProperty + "; " + this);
                    break;
            }
            return;
        }
        // Child
        if (wrapProperty.p is Settings.CheckIpAddressCountry)
        {
            return;
        }
        Logger.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
    }

    #endregion

}