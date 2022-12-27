using System.Collections;
using System.Collections.Generic;
using AdvancedCoroutines;
using LitJson;
using UnityEngine;
using UnityEngine.Networking;

namespace CheckIpAddressS.NotS
{
    public class RequestUpdate : UpdateBehavior<Request>
    {

        #region Update

        public override void update()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    startRoutine(ref this.requestRoutine, TaskRequest());
                }
                else
                {
                    Logger.LogError("data null");
                }
            }
        }

        public override bool isShouldDisableUpdate()
        {
            return false;
        }

        #endregion

        #region task

        private Routine requestRoutine;

        public override List<Routine> getRoutineList()
        {
            List<Routine> ret = new List<Routine>();
            {
                ret.Add(requestRoutine);
            }
            return ret;
        }

        public const string LocalizationIPAddress = "LocalizationIPAddress";

        public IEnumerator TaskRequest()
        {
            using (UnityWebRequest req = UnityWebRequest.Get("http://ip-api.com/json"))
            {
                yield return req.SendWebRequest();

                if (this.data != null)
                {
                    // find content
                    string content = "";
                    {
                        if (req.isNetworkError)
                        {
                            content = PlayerPrefs.GetString(LocalizationIPAddress, "");
                            Logger.Log("error: " + req.error + "; " + content);
                        }
                        else
                        {
                            Logger.Log("LocalizationByIPAddress: " + req.downloadHandler.text);
                            content = req.downloadHandler.text;
                            PlayerPrefs.SetString(LocalizationIPAddress, content);
                            PlayerPrefs.Save();
                        }
                    }
                    // process
                    {
                        try
                        {
                            JsonData jsonData = JsonMapper.ToObject(content);
                            string countryString = (string)jsonData["country"];
                            Settings.get().countryCode.v = (string)jsonData["countryCode"];
                            Logger.Log("LocalizationByIPAddress: content: " + content + "; " + countryString + "; " + Settings.get().countryCode);
                            switch (countryString)
                            {
                                case "Vietnam":
                                    {
                                        Settings.get().ipAddressCountry.v = SystemLanguage.Vietnamese;
                                    }
                                    break;
                                default:
                                    {
                                        Settings.get().ipAddressCountry.v = SystemLanguage.Unknown;
                                    }
                                    break;
                            }
                            // change to already
                            {
                                Settings settings = this.data.findDataInParent<Settings>();
                                if (settings != null)
                                {
                                    Already already = settings.checkIpAddressCountry.newOrOld<Already>();
                                    {

                                    }
                                    settings.checkIpAddressCountry.v = already;
                                }
                                else
                                {
                                    Logger.LogError("not null");
                                }
                            }
                        }
                        catch (System.Exception e)
                        {
                            Logger.LogError("LocalizationByIPAddress: " + e);
                            Settings.get().ipAddressCountry.v = SystemLanguage.Unknown;
                            // retry
                            {
                                Not not = this.data.findDataInParent<Not>();
                                if (not != null)
                                {
                                    Retry retry = not.sub.newOrOld<Retry>();
                                    {

                                    }
                                    not.sub.v = retry;
                                }
                                else
                                {
                                    Logger.LogError("not null");
                                }
                            }
                        }
                        Logger.Log("ipAddressCountry: " + Settings.get().ipAddressCountry.v);
                    }
                }
                else
                {
                    Logger.LogError("data null");
                }
            }
        }

        #endregion

        #region implement callBacks

        public override void onAddCallBack<T>(T data)
        {
            if(data is Request)
            {
                dirty = true;
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if(data is Request)
            {
                Request request = data as Request;
                this.setDataNull(request);
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
            if(wrapProperty.p is Request)
            {
                return;
            }
            Logger.LogError("Don't process: " + data + "; " + syncs + "; " + this);
        }

        #endregion

    }
}