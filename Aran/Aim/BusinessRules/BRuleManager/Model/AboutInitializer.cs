using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRuleManager.Model
{
    class AboutInitializer : BaseInitializer
    {
        public AboutInitializer(object model) : base(model)
        {
            DefaultSize = new System.Windows.Size(250, 200);
            WindowTitle = "About - Business Rules Manager";
            ShowInTaskbar = false;

            _model.ApplicationName = "Business Rules Manager";
            _model.Version = "Version: 1.0.0";
            _model._isModal = true;
            _model._resizeMode = System.Windows.ResizeMode.NoResize;
        }
    }
}
