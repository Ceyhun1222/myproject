using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PDM;

namespace ARENA.Util
{
    public static class PdmObjectExtension
    {
        public static void Save(this PDMObject pdmObject)
        {
            pdmObject.StoreToDB(MainForm.Instance.Environment.Data.TableDictionary);
            MainForm.Instance.Environment.Data.PdmObjectList.Add(pdmObject);
        }
    }
}
