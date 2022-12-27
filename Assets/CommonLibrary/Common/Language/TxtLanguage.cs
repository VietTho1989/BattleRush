using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CommonLibrary
{
    public class TxtLanguage : Data
    {

        public VO<string> defaultTxt;

        public LD<Locale> locales;

        public Locale findLocale(Language.Type type)
        {
            return this.locales.vs.Find(local => local.type.v == type);
        }

        #region Constructor

        public enum Property
        {
            defaultTxt,
            locales
        }

        public TxtLanguage(string defaultTxt) : base()
        {
            this.defaultTxt = new VO<string>(this, (byte)Property.defaultTxt, defaultTxt);
            this.locales = new LD<Locale>(this, (byte)Property.locales);
        }

        #endregion

        public void add(Language.Type type, string txt)
        {
            Locale locale = this.findLocale(type);
            if (locale != null)
            {
                // Debug.LogError ("locale not null: " + this);
                locale.txt.v = txt;
            }
            else
            {
                locale = new Locale();
                {
                    locale.uid = this.locales.makeId();
                    locale.type.v = type;
                    locale.txt.v = txt;
                }
                this.locales.add(locale);
            }
        }

        #region get

        public string get(Language.Force force = null)
        {
            if (force == null)
            {
                return get(Settings.get().language.v, this.defaultTxt.v);
            }
            else
            {
                switch (force.getType())
                {
                    case Language.Force.Type.Have:
                        {
                            Language.Force.Have have = force as Language.Force.Have;
                            return get(have.language.v, this.defaultTxt.v);
                        }
                    case Language.Force.Type.None:
                        return get(Settings.get().language.v, this.defaultTxt.v);
                    default:
                        Logger.LogError("unknown type: " + force.getType());
                        return get(Settings.get().language.v, this.defaultTxt.v);
                }
            }
        }

        private string get(string defaultTxt)
        {
            return get(Settings.get().language.v, defaultTxt);
        }

        private string get(Language.Type type, string defaultTxt)
        {
            Locale locale = this.findLocale(type);
            if (locale != null)
            {
                if (!string.IsNullOrEmpty(locale.txt.v))
                {
                    return locale.txt.v;
                }
                else
                {
                    Logger.LogError("why locale text null");
                    return defaultTxt;
                }
            }
            else
            {
                // Debug.LogError ("type null: " + this);
                return defaultTxt;
            }
        }

        #endregion

    }
}