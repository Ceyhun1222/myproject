﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Aran.Aim.InputFormLib
{
    public class NDateTimePicker : System.Windows.Forms.DateTimePicker
    {
        private DateTimePickerFormat oldFormat = DateTimePickerFormat.Long;
        private string oldCustomFormat = null;
        private bool bIsNull = false;

        public NDateTimePicker ()
            : base ()
        {
        }

        public new DateTime Value
        {
            get
            {
                if (bIsNull)
                    return DateTime.MinValue;
                else
                    return base.Value;
            }
            set
            {
                if (value == DateTime.MinValue)
                {
                    if (bIsNull == false)
                    {
                        oldFormat = this.Format;
                        oldCustomFormat = this.CustomFormat;
                        bIsNull = true;
                    }

                    this.Format = DateTimePickerFormat.Custom;
                    this.CustomFormat = " ";
                }
                else
                {
                    if (bIsNull)
                    {
                        this.Format = oldFormat;
                        this.CustomFormat = oldCustomFormat;
                        bIsNull = false;
                    }
                    base.Value = value;
                }
            }
        }

        protected override void OnCloseUp (EventArgs eventargs)
        {
            if (Control.MouseButtons == MouseButtons.None)
            {
                if (bIsNull)
                {
                    this.Format = oldFormat;
                    this.CustomFormat = oldCustomFormat;
                    bIsNull = false;
                }
            }
            base.OnCloseUp (eventargs);
        }

        protected override void OnKeyDown (KeyEventArgs e)
        {
            base.OnKeyDown (e);

            if (e.KeyCode == Keys.Delete)
                this.Value = DateTime.MinValue;
        }
    }
}