using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Panda.Vss;
using Aran.PANDA.Common;

namespace Aran.PANDA.Vss
{
    public partial class SecondPageControl :  PageControl
    {
        private FirstPageControl _firstPage;

        public SecondPageControl()
        {
            InitializeComponent();
        }


        #region Override

        public override void LoadPage()
        {
            base.LoadPage();

            IsLast = true;

            SetOffsetAngle();
        }

        public override void SetAllPageControls(IEnumerable<PageControl> allPageControls)
        {
            base.SetAllPageControls(allPageControls);

            _firstPage = allPageControls.Where(pg => pg is FirstPageControl).First() as FirstPageControl;
        }

        public override void NextClicked()
        {
            base.NextClicked();
        }

        public override void BackClicked()
        {
            base.BackClicked();

            DoPageChanged(_firstPage, false);
        }

        #endregion

        private void SetOffsetAngle()
        {
            ui_OffsetAngleTB.Text = Globals.GetDoubleText(ARANMath.RadToDeg(_firstPage.OffsetAngle));
            ui_distanceToIntersTB.Text = Globals.DistanceFormat(_firstPage.IntersectThrDistance);
            ui_abeamDistFrom1400mTB.Text = Globals.DistanceFormat(_firstPage.AbeamDistanceFrom1400M);
        }
    }
}
