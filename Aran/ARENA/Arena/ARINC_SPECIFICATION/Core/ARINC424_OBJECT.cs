using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ARINC_Types
{
    [XmlType]
    [Serializable()]
    public class ARINC_OBJECT
    {
        private string _CustomerArea;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string CustomerArea
        {
            get { return _CustomerArea; }
            set { _CustomerArea = value; }
        }

        private string _Name;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private string _Object_Type;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Object_Type
        {
            get { return _Object_Type; }
            set { _Object_Type = value; }
        }

        private string _ID;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public string GenerateIdentificationString()
        {

            string res = "";
            Type t = this.GetType();
            System.Reflection.PropertyInfo[] props = t.GetProperties();

            string[] Ignoredprop = { "Continuation_Record", "Simulation_Continuation_Record", "Flight_Planing_Continuation_Record", "Limitation_Continuation_Record", "File_Record_No", "Cycle_Date", "ID"};

            foreach (System.Reflection.PropertyInfo prp in props)
            {
                try
                {
                    //if (prp.Name.StartsWith("Geometry")) continue;

                    if (Ignoredprop.ToList().IndexOf(prp.Name) >= 0) continue;
                    object objPropVal = prp.GetValue(this, null);

                    if ((objPropVal != null) && (objPropVal.ToString().CompareTo("NaN") != 0))
                    {

                   // //System.Diagnostics.Debug.WriteLine(prp.Name+ " Value =" + objPropVal.ToString()) ;

                        res = res + objPropVal.ToString().Trim();
                    }
                }
                catch 
                {
                    //Log.Add("   Status: Error " + ex.Message + "(Propname = " + prp.Name);
                    continue;
                }
            }

            return res;
        }

        public ARINC_OBJECT()
        {
            this.ID = Guid.NewGuid().ToString();
        }

   
        
    }

}
