using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANCOR.MapCore
{
    public class IElementInfo
    {
        private string _GeneralInfo;
        public string GeneralInfo
        {
            get { return _GeneralInfo; }
            set { _GeneralInfo = value; }
        }

        private string _AcntElementType;
        public string AcntElementType
        {
            get { return _AcntElementType; }
            set { _AcntElementType = value; }
        }

        private string _AcntElementId;
        public string AcntElementId
        {
            get { return _AcntElementId; }
            set { _AcntElementId = value; }
        }

        public IElementInfo(string IElementName)
        {
            string[] words = IElementName.Split(':');
            if (words.Length == 3)
            {
                this.GeneralInfo = words[0];
                this.AcntElementType = words[1];
                this.AcntElementId = words[2];

            }
            else
            {
                this.GeneralInfo = "";
                this.AcntElementType = "";
                this.AcntElementId = "";
            }

        }
    }
}
