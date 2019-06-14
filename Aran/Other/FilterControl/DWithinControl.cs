using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim.Data.Filters;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;

namespace Aran.Controls
{
    public partial class DWithinControl : UserControl
    {
        public DWithinControl()
        {
            InitializeComponent();

            var values = Enum.GetValues(typeof(UomDistance));
            foreach (UomDistance item in values)
                ui_distUomCB.Items.Add(item);
        }

        public DWithin GetValue(string propertyName)
        {
            double x, y;
            bool b = GetCoordinate(ui_coordTB.Text, out x, out y);
            if (!b || ui_distUomCB.SelectedItem == null)
                return null;

            Aran.Geometries.Point point = new Geometries.Point();
            point.SetCoords(x, y);

            var dw = new DWithin();
            dw.PropertyName = propertyName;
            dw.Point = point; ;
            dw.Distance = new ValDistance();
            dw.Distance.Value = Convert.ToDouble(ui_distNud.Value);
            dw.Distance.Uom = (UomDistance)ui_distUomCB.SelectedItem;
            return dw;
        }

        public void SetValue(DWithin value)
        {
            var point = value.Point as Aran.Geometries.Point;
            ui_coordTB.Text = point.Y + " " + point.X;

            ui_distNud.Value = (decimal)value.Distance.Value;
            ui_distUomCB.SelectedItem = value.Distance.Uom;
        }

        private bool GetCoordinate(string text, out double x, out double y)
        {
            x = double.NaN;
            y = double.NaN;
            var sa = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (sa.Length > 1) {
                try {
                    x = double.Parse(sa[1]);
                    y = double.Parse(sa[0]);
                    return true;
                }
                catch { }
            }

            return false;
        }
    }
}
