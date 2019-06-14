using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDM.PropertyExtension
{
    /// <summary>
    /// Атрибут для задания признака поля обязательного к заполнению
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class Mandatory: Attribute
    {
        private bool _MandatoryFlag;

        public bool MandatoryFlag
        {
            get { return _MandatoryFlag; }
        }

        public Mandatory(bool flag)
        {
            _MandatoryFlag = flag;
        }

       
    }
}
