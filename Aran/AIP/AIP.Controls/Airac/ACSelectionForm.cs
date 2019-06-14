using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AIP.BaseLib.Airac
{
    public partial class ACSelectionForm : Form
    {
        private List<int> _yaerList;
        private int _yearIndex;
        private DateTime _value;
        private AiracSelectionMode _airacSelectionMode;


        public ACSelectionForm()
        {
            InitializeComponent();
            
            _yaerList = new List<int>();

            foreach (var item in AiracCycle.AiracCycleList) {
                if (!_yaerList.Contains(item.RadCutOff.Year))
                    _yaerList.Add(item.RadCutOff.Year);
            }

            YearIndex = 0;
            ui_dateItemsFLPanel.AutoSize = true;
            ui_dateItemsFLPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ui_dateItemsFLPanel.WrapContents = true;
        }


        public event EventHandler ValueChanged;


        public DateTime Value
        {
            get { return _value; }
            set
            {
                _value = value;

                YearIndex = _yaerList.IndexOf(value.Year);

                foreach (Button button in ui_dateItemsFLPanel.Controls) {
                    button.FlatAppearance.BorderSize = ((DateTime)button.Tag == value ? 1 : 0);
                }
            }
        }

        public AiracSelectionMode AiracSelectionMode
        {
            get { return _airacSelectionMode; }
            set
            {
                _airacSelectionMode = value;
                tabControl1.SelectedIndex = (value == AiracSelectionMode.Custom ? 1 : 0);
                if (tabControl1.SelectedIndex == 1)
                    ui_monthCalendar.SetDate(_value);
            }
        }


        protected override CreateParams CreateParams
        {
            get
            {
                const int CS_DROPSHADOW = 0x20000;
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        private int YearIndex
        {
            get { return _yearIndex; }
            set
            {
                if (value < 0 || value >= _yaerList.Count)
                    return;

                _yearIndex = value;
                ui_airacTitleLabel.Text = _yaerList[value].ToString();

                FillDates();
            }
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);

            Close();
        }

        private void Prev_Click(object sender, EventArgs e)
        {
            YearIndex--;
        }

        private void Next_Click(object sender, EventArgs e)
        {
            YearIndex++;
        }

        private void FillDates()
        {
            var year = _yaerList[_yearIndex];
            var list = AiracCycle.AiracCycleList.Where(ac => ac.RadCutOff.Year == year);
            ui_dateItemsFLPanel.Controls.Clear();

            foreach (var item in list) {
                var button = CreateAiracItemButton(item.RadCutOff);

                ui_dateItemsFLPanel.Controls.Add(button);
            }
        }

        private Button CreateAiracItemButton(DateTime dt)
        {
            var button = new Button();
            button.Text = dt.ToString("dd - MMM"); //dt.Day + "/" + dt.Month;
            button.Size = new Size(60, 22);
            button.Margin = new Padding(0);
            button.Tag = dt;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Click += AiracItemButton_Click;
            //button.FlatAppearance.MouseOverBackColor = Color.Transparent;
            //button.FlatAppearance.MouseDownBackColor = Color.Transparent;

            var ac = AiracCycle.AiracCycleList.FirstOrDefault(item => item.RadCutOff == dt);
            if (ac != null)
                ui_toolTip.SetToolTip(button, ac.GetInfo());

            return button;
        }

        private void AiracItemButton_Click(object sender, EventArgs e)
        {
            _value = (DateTime)(sender as System.Windows.Forms.Control).Tag;
            _airacSelectionMode = AiracSelectionMode.Airac;

            Close();

            if (ValueChanged != null)
                ValueChanged(this, e);
        }

        private void DateItemsFLPanel_SizeChanged(object sender, EventArgs e)
        {
            var x = (ui_dateItemsFLPanel.Parent.Width - ui_dateItemsFLPanel.Width) / 2;
            var y = (ui_dateItemsFLPanel.Parent.Height - ui_dateItemsFLPanel.Height) / 2;

            ui_dateItemsFLPanel.Location = new Point(x, y);
        }

        private void MonthCalendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            _value = new DateTime(e.End.Year, e.End.Month, e.End.Day, 0, 0, 0);
            
            _airacSelectionMode = AiracSelectionMode.Custom;

            if (ValueChanged != null)
                ValueChanged(this, e);

            Close();
        }
    }
}
