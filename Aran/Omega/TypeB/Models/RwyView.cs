using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Aran.Panda.Constants;

namespace Aran.Omega.TypeB.Models
{
    public class RwyView:ViewModels.ViewModel
    {
        private EnumName<RunwayClassificationType> _selClassification;
        public RwyView()
        {
            IsTrackChange = false;
            IsCodeLetterF = false;
            Classification = CreateClassification();
            CategoryList = CreateCategory();
            ChangeChecked(true);
            SelectedClassification = Classification[2];
            CatNumber = CategoryNumber.One;
        }
        private ObservableCollection<EnumName<RunwayClassificationType>> CreateClassification()
        {
            var result = new ObservableCollection<EnumName<RunwayClassificationType>>
                {
                    new EnumName<RunwayClassificationType>
                    {
                        Name = "Non-instrument",
                        Enum = RunwayClassificationType.NonInstrument
                    },
                    new EnumName<RunwayClassificationType>
                    {
                        Name = "Non-precision approach",
                        Enum = RunwayClassificationType.NonPrecisionApproach
                    },
                    new EnumName<RunwayClassificationType>
                    {
                        Name = "Precision approach",
                        Enum = RunwayClassificationType.PrecisionApproach
                    }
                };
            return result;
        }

        private ObservableCollection<EnumName<CategoryNumber>> CreateCategory()
        {
            var _categoryList = new ObservableCollection<EnumName<CategoryNumber>>
                {
                    new EnumName<CategoryNumber> {Name = "I", Enum = CategoryNumber.One},
                    new EnumName<CategoryNumber> {Name = "II,III", Enum = CategoryNumber.Two}
                };
            return _categoryList;
        }
        public string Name { get; set; }
        public RwyClass Rwy { get; set; }
        public RwyDirClass RWYDirection { get; set; }
        public List<RwyDirClass> RWYDirectionList { get; set; }
        public CategoryNumber CatNumber { get; set; }
        public int CodeNumber { get; set; }
        public bool IsTrackChange { get; set; }
        public bool IsCodeLetterF { get; set; }
        public ObservableCollection<EnumName<RunwayClassificationType>> Classification { get; set; }

        public EnumName<RunwayClassificationType> SelectedClassification
        {
            get { return _selClassification; }
            set
            {
                if (_selClassification == value)
                    return;

                _selClassification = value;
                if (value.Enum == RunwayClassificationType.PrecisionApproach)
                {
                    CategoryList.Clear();
                    foreach (var cat in CategoryList)
                        CategoryList.Add(cat);
                }
                else
                    CategoryList.Clear();
                NotifyPropertyChanged("SelectedClassification");
            }
        } 

        public ObservableCollection<EnumName<CategoryNumber>> CategoryList { get; set; }

        private bool _checked;
        public bool Checked
        {
            get { return _checked; }
            set
            {
                if (_checked == value)
                    return;
                _checked = value;
                if (RwyCheckedIsChanged != null)
                    RwyCheckedIsChanged(this, new EventArgs());
                NotifyPropertyChanged("Checked");
            }
        }

        public void ChangeChecked(bool isChecked)
        {
            Checked = true;
            NotifyPropertyChanged("Checked");
        }

        public event EventHandler RwyCheckedIsChanged;
    }
}
