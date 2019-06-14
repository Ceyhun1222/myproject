using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim;
using Aran.Aim.Utilities;

namespace MapEnv
{
    public partial class PropertySelectorControl : UserControl
    {
        private AimPropInfo [] _value;
        private AimClassInfo _classInfo;

        public event SelectorAddedPropInfoEventHandler SelectorAddedPropInfo;
        public event PropertySelectedEventHandler AfterSelect;
        public event EventHandler ValueChanged;

        public PropertySelectorControl ()
        {
            InitializeComponent ();
        }

        public AimPropInfo [] Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;

                if (value != null)
                {
                    string s = "";
                    
                    if (value.Length > 0)
                    {
                        s = value [0].Name;
                        for (int i = 1; i < value.Length; i++)
                        {
                            s += "/" + value [i].Name;
                        }
                    }

                    ui_propNameTB.Text = s;
                }

                if (ValueChanged != null)
                    ValueChanged (this, new EventArgs ());
            }
        }

        public AimClassInfo ClassInfo
        {
            get { return _classInfo; }
            set
            {
                _classInfo = value;
                ui_propNameSelectorButton.Enabled = (value != null);
            }
        }

        public new string Text
        {
            get { return ui_propNameTB.Text; }
            set
            {

                if (string.IsNullOrWhiteSpace (value))
                    return;

                Value = AimMetadataUtility.GetInnerProps ((int) _classInfo.Index, value);
            }
        }


        private void uiEvents_propNameSelectorButton_Click (object sender, EventArgs e)
        {
            if (_classInfo == null)
                return;

            Control control = (Control) sender;
            PropertySelectorForm propSelForm = new PropertySelectorForm ();

            propSelForm.SelectorAddedPropInfo += SelectorAddedPropInfo;
            propSelForm.PropertySelected += new PropertySelectedEventHandler (PropSelForm_PropertySelected);

            propSelForm.ClassInfo = _classInfo;
            propSelForm.StartPosition = FormStartPosition.Manual;

            propSelForm.Width = control.Right - ui_propNameTB.Left;
            propSelForm.Location = control.PointToScreen (new Point (control.Width - propSelForm.Width, control.Height));
            propSelForm.Show (this);

            propSelForm.SetSelected (Value);
        }

        private void PropSelForm_PropertySelected (object sender, PropertySelectedEventArgs e)
        {
            if (e.SelectedProp.Length == 0)
            {
                e.Cancel = true;
                return;
            }

            if (AfterSelect != null)
            {
                AfterSelect (this, e);

                if (e.Cancel)
                    return;
            }

            Value = e.SelectedProp;
        }
    }
}
