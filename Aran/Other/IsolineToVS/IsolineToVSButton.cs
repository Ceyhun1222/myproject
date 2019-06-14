using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.ArcMapUI;

namespace IsolineToVS
{
    public class IsolineToVSButton : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        private IsolineToAixm _isolineToAixm;

        public IsolineToVSButton()
        {
        }

        protected override void OnClick()
        {
            var mapDoc = ArcMap.Application.Document as IMxDocument;

            if (mapDoc == null)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Invalid ArcMap Document", 
                    "Isoline To VS", 
                    System.Windows.Forms.MessageBoxButtons.OK, 
                    System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
            
            if (_isolineToAixm == null)
                _isolineToAixm = new IsolineToAixm();

            _isolineToAixm.Open();
        }
        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }

}
