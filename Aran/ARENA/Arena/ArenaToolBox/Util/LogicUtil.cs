using System;
using PDM;
using System.Collections.Generic;
using System.Data;

namespace ARENA.Util
{
    public class LogicUtil
    {
 
        public static void RemoveFeature(PDMObject sellObj)
        {
            try
            {
                //ARENA_DataReader.DeleteObject(ARENA.MainForm.Instance.Environment.Data.TableDictionary[sellObj.GetType()], sellObj);
                var tp = sellObj.GetType().Name;
                switch (tp)
                {
                    case ("InstrunentApproachProcedure"):
                    case ("StandardInstrumentArrival"):
                    case ("StandardInstrumentDeparture"):
                        //ARENA_DataReader.DeleteObject(ARENA.MainForm.Instance.Environment.Data.TableDictionary[typeof(Procedure)], sellObj);

                        break;
                }

            }
            catch (Exception)
            {
                throw;
            }

             

        }

        public static void RemoveFeature(string tblName, string keyField, string keyValue)
        {
            try
            {
                //ARENA_DataReader.DeleteObject(tblName, keyField + " = '" +keyValue+"'");

            }
            catch (Exception)
            {
                throw;
            }



        }

    }
}
