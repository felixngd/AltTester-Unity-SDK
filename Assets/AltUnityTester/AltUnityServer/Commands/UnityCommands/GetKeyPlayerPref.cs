﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class GetKeyPlayerPref :  Command
    {
        PLayerPrefKeyType type;
        string value;

        public GetKeyPlayerPref(PLayerPrefKeyType type, string value)
        {
            this.type = type;
            this.value = value;
        }

        public override string Execute()
        {
            UnityEngine.Debug.Log("getKeyPlayerPref for: " + value);
            string response = AltUnityRunner._altUnityRunner.errorNotFoundMessage;
            if (UnityEngine.PlayerPrefs.HasKey(value))
            {
                switch (type)
                {
                    case PLayerPrefKeyType.String:
                        UnityEngine.Debug.Log("Option string " + UnityEngine.PlayerPrefs.GetString(value));
                        response = UnityEngine.PlayerPrefs.GetString(value);
                        break;
                    case PLayerPrefKeyType.Float:
                        UnityEngine.Debug.Log("Option Float " + UnityEngine.PlayerPrefs.GetFloat(value));
                        response = UnityEngine.PlayerPrefs.GetFloat(value) + "";
                        break;
                    case PLayerPrefKeyType.Int:
                        UnityEngine.Debug.Log("Option Int " + UnityEngine.PlayerPrefs.GetInt(value));
                        response = UnityEngine.PlayerPrefs.GetInt(value) + "";
                        break;
                }
            }
            return response;
        }
    }
}
