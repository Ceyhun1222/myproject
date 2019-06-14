using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARINC_Types;

namespace ARINC_DECODER_CORE.AIRTRACK_Objects
{
    public class AIRTRACK_Airspace_Segment : Object_AIRTRACK
    {

        private string _aisrpaceARINC_Type;

        public string AisrpaceARINC_Type
        {
            get { return _aisrpaceARINC_Type; }
            set { _aisrpaceARINC_Type = value; }
        }

        private string _areaCode;

        public string AreaCode
        {
            get { return _areaCode; }
            set { _areaCode = value; }
        }

        private string _MultipleCode;

        public string MultipleCode
        {
            get { return _MultipleCode; }
            set { _MultipleCode = value; }
        }


        public AIRTRACK_Airspace_Segment()
        {
        }

        public AIRTRACK_Airspace_Segment(ARINC_OBJECT arincObj)
        {
            this.ARINC_OBJ = arincObj;
            this.ID_AIRTRACK = Guid.NewGuid().ToString();
            if (arincObj is ARINC_FIR_UIR_Primary_Records)
            {
                this.INFO_AIRTRACK = (arincObj as ARINC_FIR_UIR_Primary_Records).FIR_UIR_Identifier;
                this.AreaCode = (arincObj as ARINC_FIR_UIR_Primary_Records).FIR_UIR_Identifier.Substring(0, 2);
                this.MultipleCode = "A";
  
            }
            if (arincObj is ARINC_Restrictive_Airspace_Primary_Records)
            {
                this.INFO_AIRTRACK = (arincObj as ARINC_Restrictive_Airspace_Primary_Records).Restrictive_Airspace_Designation;
                this.AreaCode = (arincObj as ARINC_Restrictive_Airspace_Primary_Records).ICAO_Code;
                this.MultipleCode = (arincObj as ARINC_Restrictive_Airspace_Primary_Records).Multiple_Code;

            }
            if (arincObj is ARINC_Controlled_Airspace_Primary_Records)
            {
                this.INFO_AIRTRACK = (arincObj as ARINC_Controlled_Airspace_Primary_Records).Controlled_Airspace_Name;
                this.AreaCode = (arincObj as ARINC_Controlled_Airspace_Primary_Records).ICAO_Code;
                this.MultipleCode = (arincObj as ARINC_Controlled_Airspace_Primary_Records).Multiple_Code;
            }

            this.AisrpaceARINC_Type = arincObj.Object_Type;

        }
        
    }
}
