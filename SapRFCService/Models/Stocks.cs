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
    public class Stocks
    {
        public static string GetAllStock(string P_DATE)
        {
            string result = "";

            try {

                IRfcTable IRetTable = null;
                List<Z_MM_QUBE_MENGE> Z_MM_QUBE_MENGE_List = new List<Z_MM_QUBE_MENGE>();
                RfcConfigParameters parms = RFC.GetSettingParms();
                RfcDestination rfcDest = RfcDestinationManager.GetDestination(parms);
                RfcRepository rfcRep = rfcDest.Repository;
                IRfcFunction IReader = rfcRep.CreateFunction("Z_MM_QUBE_MENGE");
                IReader.SetValue("P_DATE", P_DATE);

                IReader.Invoke(rfcDest);
                IRetTable = IReader.GetTable("ET_MENGE");
                Z_MM_QUBE_MENGE_List = IRetTable.AsQueryable().Select(x => new Z_MM_QUBE_MENGE
                {
                    MATNR = x.GetString("MATNR") ?? "",
                    MENGE = x.GetString("MENGE") ?? "",
                    WERKS = x.GetString("WERKS") ?? ""
                }
                 ).ToList();

                using (var db = new CubeRFCEntities())
                {
                    StringBuilder SQLcommand = new StringBuilder();
                    foreach (var item in Z_MM_QUBE_MENGE_List)
                    {
                        string InsertCommand = string.Format("Insert Into Stock (MATNR,MENGE,WERKS,CreateDate) values('{0}','{1}','{2}',GetDate()) ;", item.MATNR, item.MENGE, item.WERKS);
                        SQLcommand.AppendLine(InsertCommand);
                    }
                    db.Database.ExecuteSqlCommand(SQLcommand.ToString());
                }

                result = JsonConvert.SerializeObject(Z_MM_QUBE_MENGE_List);

            } catch(Exception e)
            {
                result = e.ToString();
            }         
            return result;
        }
    }

    public class Z_MM_QUBE_MENGE
    {
        public string MATNR = "";
        public string MENGE = "";
        public string WERKS = "";

    }
}