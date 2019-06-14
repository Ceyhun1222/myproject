using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim.InputFormLib;
using Aran.Aim.Features;
using Aran.Aim.Metadata.UI;

namespace Aran.Aim.InputForm
{
    public partial class TestForm : Form
    {
        private AimEntityControl _entityControl;

        public TestForm ()
        {
            InitializeComponent ();

            _entityControl = new AimEntityControl ();
            _entityControl.Width = Width;
            _entityControl.Height = Height - 39;
            _entityControl.Anchor =
                AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;

            Controls.Add (_entityControl);
        }

        private void TestForm_Load (object sender, EventArgs e)
        {
            Feature feature = CreateAirportHeliport ();
            AimClassInfo classInfo = UIMetadata.Instance.GetClassInfo ((int) feature.FeatureType);

            _entityControl.LoadDbEntity (feature, classInfo);

            _entityControl.Expandable = true;
        }

        private AirportHeliport CreateAirportHeliport ()
        {
            AirportHeliport ah = new AirportHeliport ();

            ah.Name = "name - 1";
            ah.Designator = "DESIGNATOR - 1";
            ah.MagneticVariation = 5.2;

            ah.ARP = new ElevatedPoint ();
            ah.ARP.Geo.SetCoords (55.66, 23.45);

            return ah;
        }
    }
}
