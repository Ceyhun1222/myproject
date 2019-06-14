using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Queries.Viewer;

namespace Aran.Queries.Common
{
    public class ViewFeature
    {
        private Aran.Aim.Features.Feature _featere;
        public ViewFeature(Aran.Aim.Features.Feature feature)
        {
            _featere = feature;
        }

        public DialogResult View() 
        {
            var viewer = new FeatureViewerForm();
            viewer.ApplyAllIsVisible();
            //viewer.DefaultEffectiveDate = DbProvider.DefaultEffectiveDate;
          //  viewer.GetFeature += new GetFeatureHandler(viewer_GetFeature);
            //viewer.StartOfValidChanged += Viewer_StartOfValidChanged;
            //viewer.ValidTimeSelected += Viewer_ValidTimeSelected;
            //viewer.ShowValidTimePanel();
            viewer.SetFeature(_featere);

            return  viewer.ShowDialog();
        }

    }
}
