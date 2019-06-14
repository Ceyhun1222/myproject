using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;

using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace MedicalAppCS
{
    public class NullableSpinEditor : RadSpinEditor
    {
        public event EventHandler NullableValueChanged;

        public decimal? NullableValue
        {
            get
            {
                return (this.SpinElement as NullableSpinEditorElement).NullableValue;
            }
            set
            {
                (this.SpinElement as NullableSpinEditorElement).NullableValue = value;
            }
        }

        public NullableSpinEditor()
        {
            this.AutoSize = true;
            this.TabStop = false;
            base.SetStyle(ControlStyles.Selectable, true);
        }

        protected override void CreateChildItems(RadElement parent)
        {
            Type baseType = typeof(RadSpinEditor);
            NullableSpinEditorElement element = new NullableSpinEditorElement();
            element.RightToLeft = this.RightToLeft == System.Windows.Forms.RightToLeft.Yes;
            this.RootElement.Children.Add(element);

            element.ValueChanging += spinElement_ValueChanging;
            element.ValueChanged += spinElement_ValueChanged;
            element.TextChanging += spinElement_TextChanging;
            element.NullableValueChanged += element_NullableValueChanged;

            element.KeyDown += OnSpinElementKeyDown;
            element.KeyPress += OnSpinElementKeyPress;
            element.KeyUp += OnSpinElementKeyUp;

            baseType.GetField("spinElement", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(this, element);
        }

        void element_NullableValueChanged(object sender, EventArgs e)
        {
            if (this.NullableValueChanged != null)
            {
                this.NullableValueChanged(this, EventArgs.Empty);
            }
        }

        private Dictionary<string, MethodInfo> cache = new Dictionary<string, MethodInfo>();
        private void InvokeBaseMethod(string name, params object[] parameters)
        {
            if (!cache.ContainsKey(name))
            {
                cache[name] = typeof(RadSpinEditor).GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic);
            }

            cache[name].Invoke(this, parameters);
        }

        private void OnSpinElementKeyUp(object sender, KeyEventArgs e)
        {
            this.InvokeBaseMethod("OnSpinElementKeyUp", sender, e);
        }

        private void OnSpinElementKeyPress(object sender, KeyPressEventArgs e)
        {
            this.InvokeBaseMethod("OnSpinElementKeyPress", sender, e);
        }

        private void OnSpinElementKeyDown(object sender, KeyEventArgs e)
        {
            this.InvokeBaseMethod("OnSpinElementKeyDown", sender, e);
        }

        private void spinElement_TextChanging(object sender, TextChangingEventArgs e)
        {
            this.InvokeBaseMethod("spinElement_TextChanging", sender, e);
        }

        private void spinElement_ValueChanged(object sender, EventArgs e)
        {
            this.InvokeBaseMethod("spinElement_ValueChanged", sender, e);
            this.NullableValue = this.Value;
        }

        private void spinElement_ValueChanging(object sender, ValueChangingEventArgs e)
        {
            this.InvokeBaseMethod("spinElement_ValueChanging", sender, e);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Tab)
            {
                (this.SpinElement as NullableSpinEditorElement).CommitText();
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            (this.SpinElement as NullableSpinEditorElement).CommitText();
            base.OnLostFocus(e);
        }

        protected override Size DefaultSize
        {
            get
            {
                return GetDpiScaledSize(new Size(100, 20));
            }
        }
    }

    public class NullableSpinEditorElement : RadSpinElement
    {
        private decimal? nullableValue;

        public decimal? NullableValue
        {
            get
            {
                return this.nullableValue;
            }
            set
            {
                this.nullableValue = value;
                if (value.HasValue)
                {
                    this.internalValue = value.Value;
                }
                else
                {
                    this.internalValue = this.MinValue;
                }

                this.Validate();
                this.OnNullableValueChanged();
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.CommitText();
                e.Handled = true;
                return;
            }

            base.OnKeyDown(e);
        }

        public virtual void CommitText()
        {
            if (this.TextBoxItem.Text == string.Empty)
            {
                this.NullableValue = null;
            }
            else
            {
                this.NullableValue = this.GetValueFromText();
            }
        }

        public override bool Validate()
        {
            if (!this.NullableValue.HasValue)
            {
                this.TextBoxItem.Text = string.Empty;
                return true;
            }
            this.TextBoxItem.Text = this.internalValue.ToString((this.ThousandsSeparator ? "N" : "F") + this.DecimalPlaces.ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);

            return true;
        }

        public override void PerformStep(decimal step)
        {
            decimal value = this.GetValueFromText();

            try
            {
                decimal incValue = value + step;
                value = incValue;
            }
            catch (OverflowException)
            {
            }

            this.NullableValue = this.Constrain(value);
            this.Validate();
        }

        protected override Type ThemeEffectiveType
        {
            get
            {
                return typeof(RadSpinElement);
            }
        }

        public event EventHandler NullableValueChanged;

        protected virtual void OnNullableValueChanged()
        {
            if (this.NullableValueChanged != null)
            {
                this.NullableValueChanged(this, EventArgs.Empty);
            }
        }
    }
}
