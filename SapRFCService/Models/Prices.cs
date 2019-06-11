﻿using Newtonsoft.Json;
using SAP.Middleware.Connector;
using SapRFCService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SapRFCService.Models
{
    public class Prices
    {
        public static string GetAllPrice(string P_DATE)
        {
            string result = "";

            try {

                IRfcTable IRetTable = null;
                List<Z_MM_QUBE_VKP0> Z_MM_QUBE_VKP0_List = new List<Z_MM_QUBE_VKP0>();
                RfcConfigParameters parms = RFC.GetSettingParms();
                RfcDestination rfcDest = RfcDestinationManager.GetDestination(parms);
                RfcRepository rfcRep = rfcDest.Repository;
                IRfcFunction IReader = rfcRep.CreateFunction("Z_MM_QUBE_VKP0");
                IReader.SetValue("P_DATE", P_DATE);

                IReader.Invoke(rfcDest);
                IRetTable = IReader.GetTable("ET_VKP0");
                Z_MM_QUBE_VKP0_List = IRetTable.AsQueryable().Select(x => new Z_MM_QUBE_VKP0
                {
                    MATNR = x.GetString("MATNR") ?? "",
                    WAERS = x.GetString("WAERS") ?? "",
                    BRTWR = x.GetString("BRTWR") ?? "",
                    DATAB = x.GetString("DATAB") ?? "",
                    DATBI = x.GetString("DATBI") ?? ""
                }
                 ).ToList();

                using (var db = new CubeRFCEntities())
                {
                    StringBuilder SQLcommand = new StringBuilder();
                    foreach (var item in Z_MM_QUBE_VKP0_List)
                    {
                        string InsertCommand = string.Format("Insert Into Price (MATNR,WAERS,BRTWR,DATAB,DATBI,CreateDate) values('{0}','{1}','{2}','{3}','{4}',GetDate()) ;", item.MATNR, item.WAERS, item.BRTWR, item.DATAB, item.DATBI);
                        SQLcommand.AppendLine(InsertCommand);
                    }
                    db.Database.ExecuteSqlCommand(SQLcommand.ToString());
                }

                result = JsonConvert.SerializeObject(Z_MM_QUBE_VKP0_List);
               
            } catch(Exception e)
            {
                result = e.ToString();
            }

            return result;
        }

    }

    public class Z_MM_QUBE_VKP0
    {
        public string MATNR = "";
        public string WAERS = "";
        public string BRTWR = "";
        public string DATAB = "";
        public string DATBI = "";
    }
}