using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANCOR.MapCore
{
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SkipAttribute : Attribute
    {
        private bool _SkipFlag;

        public bool SkipFlag
        {
            get { return _SkipFlag; }
        }

        public SkipAttribute(bool flag)
        {
            _SkipFlag = flag;
        }


    }
}
