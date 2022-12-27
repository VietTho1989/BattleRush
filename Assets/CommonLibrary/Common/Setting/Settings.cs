using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : Data
{

    public static bool AlreadyInitLanguage = false;

    #region instance

    private static Settings instance;

    static Settings()
    {
        instance = new Settings();
    }

    public static Settings get()
    {
        return instance;
    }

    #endregion

    public VO<CommonLibrary.Language.Type> language;

    public VO<bool> useShortKey;

    public VO<string> countryCode;

    public VO<SystemLanguage> ipAddressCountry;

    #region already check ipAddressCountry

    public abstract class CheckIpAddressCountry : Data
    {

        public enum Type
        {
            Not,
            Already
        }

        public abstract Type getType();

    }

    public VD<CheckIpAddressCountry> checkIpAddressCountry;

    #endregion

    public VO<bool> canPause;

    #region Constructor

    public enum Property
    {
        language,
        useShortKey,
        countryCode,
        ipAddressCountry,
        checkIpAddressCountry,
        canPause
    }

    public Settings() : base()
    {
        this.language = new VO<CommonLibrary.Language.Type>(this, (byte)Property.language, CommonLibrary.Language.Type.vi);
        this.useShortKey = new VO<bool>(this, (byte)Property.useShortKey, true);
        this.countryCode = new VO<string>(this, (byte)Property.countryCode, "vi");
        this.ipAddressCountry = new VO<SystemLanguage>(this, (byte)Property.ipAddressCountry, SystemLanguage.Unknown);
        this.checkIpAddressCountry = new VD<CheckIpAddressCountry>(this, (byte)Property.checkIpAddressCountry, new CheckIpAddressS.Not());
        this.canPause = new VO<bool>(this, (byte)Property.canPause, false);
    }

    #endregion

}