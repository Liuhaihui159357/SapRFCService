using Newtonsoft.Json;
using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SapRFCService.Models
{
    public class RFC
    {
        public static RfcConfigParameters GetSettingParms()
        {
            RfcConfigParameters parms = new RfcConfigParameters();
            parms.Add(RfcConfigParameters.Name, "ESQ");
            parms.Add(RfcConfigParameters.AppServerHost, "192.168.82.8");
            parms.Add(RfcConfigParameters.SystemNumber, "00");
            parms.Add(RfcConfigParameters.User, "WDSRFC");
            parms.Add(RfcConfigParameters.Password, "sJ@hsyg8");
            parms.Add(RfcConfigParameters.Client, "800");
            parms.Add(RfcConfigParameters.Language, "zf");
            parms.Add(RfcConfigParameters.PoolSize, "5");
            parms.Add(RfcConfigParameters.MaxPoolSize, "10");
            parms.Add(RfcConfigParameters.IdleTimeout, "600");
            return parms;
        }
    }
}