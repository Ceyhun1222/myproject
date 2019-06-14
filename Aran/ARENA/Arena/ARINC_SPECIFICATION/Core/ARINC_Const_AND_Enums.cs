using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARINC_Types
{
    public enum Customer_Area_code
    {
        // Summary:
        //     Europe.
        EUR = 0,
        //
        // Summary:
        //     East Europe.
        EEU = 1,
        //
        // Summary:
        //     Canada.
        CAN = 2,
        //
        // Summary:
        //     Pacific.
        PAC = 3,
        //
        // Summary:
        //     Latin America.
        LAM = 4,
        //
        // Summary:
        //     USA.
        USA = 5,
        //
        // Summary:
        //     Asia.
        MES = 5,
        //
        // Summary:
        //   South Pacific.
        SPA = 5,
        //
        // Summary:
        //     South America.
        SAM = 5,
        //
        // Summary:
        //     Africa.
        AFR = 5,
    }

    public struct ConstantValue
    {
        public const int AreaCodeColumn_Position = 2;
        public const int AreaCodeColumn_Length = 3;

        public const int SectionCodeColumn_Position = 5;
        public const int SectionCodeColumn_Length = 1;

        public const int SubSectionCodeColumn_Position = 6;
        public const int SubSectionCodeColumn_Length = 1;

        public const int SubSectionCodeColumn_P_Position = 13;
        public const int SubSectionCodeColumn_P_Length = 1;

        public const int Continuation_Record_No = 22;
        public const int Proc_Continuation_Record_No = 39;
        public const int FIR_Continuation_Record_No = 20;
        public const int Restrictive_Controlled_Continuation_Record_No = 25;
        public const int APPLICATION_TYPE = 23;
        public const int Proc_APPLICATION_TYPE = 40;


        public const int CustomArea = 2;
        public const int CustomArea_length = 3;
    }
}
