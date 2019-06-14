using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using MvvmCore;

namespace TOSSM.ViewModel.Control.PropertyPrecision
{
    public class PropertyPrecisionButtonViewModel : ViewModelBase
    {
        public PropertyPrecisionControlViewModel ParentModel { get; set; }

        public string Name { get; set; }
        public int Position { get; set; }

        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (_isChecked==value) return;
                SetChecked(true);
                ParentModel.OnCheckedSet(this);
            }
        }

        public void SetChecked(bool value)
        {
            _isChecked = value;
            OnPropertyChanged("IsChecked");
        }

        public Thickness Margin { get; set; }
    }

    public class PropertyPrecisionControlViewModel : ViewModelBase
    {
        #region Buttons

        private const int BeforeCommaButtons = 3;
        private const int AfterCommaButtons = 6;

        private List<PropertyPrecisionButtonViewModel> _buttons;
       
        public List<PropertyPrecisionButtonViewModel> Buttons
        {
            get
            {
                if (_buttons == null)
                {
                    //before dot
                    _buttons=new List<PropertyPrecisionButtonViewModel>();
                    for (var i = 0; i < BeforeCommaButtons; i++)
                    {
                        _buttons.Add(new PropertyPrecisionButtonViewModel
                        {
                            ParentModel = this,
                            Name = (BeforeCommaButtons - i).ToString(),
                            Position = i - BeforeCommaButtons,
                        });
                    }

                    //dot
                    _buttons.Add(new PropertyPrecisionButtonViewModel
                    {
                        ParentModel = this,
                        Name = ".",
                        Position = 0,
                    });

                    //after dot
                    for (var i = 0; i < AfterCommaButtons; i++)
                    {
                        _buttons.Add(new PropertyPrecisionButtonViewModel
                        {
                            ParentModel = this,
                            Name = (i+1).ToString(),
                            Position = i + 1,
                        });
                    }

                    //no format
                    _buttons.Add(new PropertyPrecisionButtonViewModel
                    {
                        ParentModel = this,
                        Name = "*",
                        Position = 10,
                        Margin=new Thickness(5,0,2,0),
                    });

                }
                return _buttons;
            }
        }

        public void OnCheckedSet(PropertyPrecisionButtonViewModel buttonModel)
        {
            if (!buttonModel.IsChecked) return;
            UpdateFormatValueAccordingToPosition(buttonModel.Position);
        }

        #endregion

        #region Format and UI Processing

        private void UpdateFormatValueAccordingToPosition(int position)
        {
            if (position == 10)
            {
                FormatValue = 100;
                return;
            }
            if (position >= 0)
            {
                FormatValue = (FormatValue - FormatValue % 10 + position % 10)%100;
                return;
            }
            FormatValue = (FormatValue % 10 - position * 10)%100;
        }

        private void UpdateUiAccordingToPosition(int position)
        {
            if (position == 10)
            {
                foreach (var otherButton in Buttons.Where(t => t.Position != 10))
                {
                    otherButton.SetChecked(false);
                }

                Buttons.First(t => t.Position == 10).SetChecked(true);

                return;
            }

            Buttons.First(t => t.Position == 10).SetChecked(false);

            if (position >= 0)
            {
                for (var i = 0; i <= AfterCommaButtons; i++)
                {
                    Buttons.First(t => t.Position == i).SetChecked(i <= position);
                }
                return;
            }


            for (var i = 0; i < BeforeCommaButtons; i++)
            {
                Buttons.First(t => t.Position == i - BeforeCommaButtons).SetChecked(i - BeforeCommaButtons >= position);
            }
        }

        #endregion

        #region ToolTip
        private string _toolTip;
        public string ToolTip
        {
            get => _toolTip;
            set
            {
                _toolTip = value;
                OnPropertyChanged("ToolTip");
            }
        }

        private void UpdateToolTip()
        {
            if (_formatValue >= 100)
            {
                ToolTip = "No format is applied, value is shown as is.";
                return;
            }

            var beforeDot = _formatValue / 10;
            var afterDot = _formatValue % 10;

            ToolTip = "Value is formatted to show at least " + beforeDot + " digits before and exactly " +
                afterDot + " digits after comma.";
        }

        #endregion

        #region Format value

        public Action<int> UpdateValue { get; set; }

        private int _formatValue=100;
        public int FormatValue
        {
            get => _formatValue;
            set
            {
                _formatValue = value;
                if (UpdateValue != null)
                {
                    UpdateValue(FormatValue);
                }
            }
        }

        private Enum _enumValue;
        public Enum EnumValue
        {
            get => _enumValue;
            set
            {
                _enumValue = value;
                OnPropertyChanged("EnumValue");
            }
        }

        public void OnValueChanged(int newPropertyValue)
        {
            _formatValue = newPropertyValue;

            if (_formatValue >= 100)
            {
                UpdateUiAccordingToPosition(10);
            }
            else
            {
                UpdateUiAccordingToPosition(-(_formatValue / 10));
                UpdateUiAccordingToPosition(_formatValue % 10);
            }

            UpdateToolTip();
        }

        #endregion
    }
}
