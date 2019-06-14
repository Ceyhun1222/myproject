using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Temporality.CommonUtil.ViewModel;

namespace Aran.Temporality.CommonUtil.Report.UserReport
{
    partial class UserReportTemplate
    {
        public List<EasyUserViewModel> Users { get; set; }

        private DateTime _date = DateTime.Now;
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }
    }
}
