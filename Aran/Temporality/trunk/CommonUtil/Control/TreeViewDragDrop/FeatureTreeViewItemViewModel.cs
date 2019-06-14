using System;
using System.Collections.Generic;
using System.Windows;
using Aran.Aim;
using Aran.Temporality.Common.Entity.Util;
using Aran.Temporality.CommonUtil.Util;

namespace Aran.Temporality.CommonUtil.Control.TreeViewDragDrop
{
    public class FeatureTreeViewItemViewModel : TreeViewItemViewModel
    {
        private LightFeature _lightFeature;
        public LightFeature LightFeature
        {
            get { return _lightFeature; }
            set
            {
                _lightFeature = value;
                HasFieldErrors = LightFeature != null && LightFeature.HasFieldProblems;
            }
        }

        public HashSet<string> MandatoryLinks;
        public HashSet<string> OptionalLinks;

        public HashSet<string> MandatoryProperties;
        public HashSet<string> OptionalProperties;

        private Visibility _editControlVisibility;
        public Visibility EditControlVisibility
        {
            get { return _editControlVisibility; }
            set
            {
                _editControlVisibility = value;
                OnPropertyChanged("EditControlVisibility");
            }
        }


        private bool _hasFieldErrors;
        public bool HasFieldErrors
        {
            get { return _hasFieldErrors; }
            set
            {
                _hasFieldErrors = value;
                if (HasFieldErrors)
                {
                    FieldProblemVisibility=Visibility.Visible;
                }
                else
                {
                    FieldProblemVisibility = Visibility.Collapsed;
                }
            }
        }

        private Visibility _fieldProblemVisibility=Visibility.Collapsed;
        public Visibility FieldProblemVisibility
        {
            get { return _fieldProblemVisibility; }
            set { _fieldProblemVisibility = value; }
        }


        private int _flag;
        public int Flag
        {
            get { return _flag; }
            set
            {
                _flag = value;

                if ((Flag & LightData.Missing) == 0)
                {
                    MissingOptionalVisibility = Visibility.Collapsed;
                    MissingMandatoryVisibility = Visibility.Collapsed;
                    OkVisibility = Visibility.Visible;
                }
                else if ((Flag & LightData.Missing) != 0 && (Flag & LightData.IsMandatory) != 0)
                {
                    MissingOptionalVisibility = Visibility.Collapsed;
                    MissingMandatoryVisibility = Visibility.Visible;
                    OkVisibility = Visibility.Collapsed;
                }
                else if ((Flag & LightData.Missing) != 0 && (Flag & LightData.IsOptional) != 0)
                {
                    MissingOptionalVisibility = Visibility.Visible;
                    MissingMandatoryVisibility = Visibility.Collapsed;
                    OkVisibility = Visibility.Collapsed;
                }
            }
        }


        private Visibility _okVisibility = Visibility.Visible;
        public Visibility OkVisibility
        {
            get { return _okVisibility; }
            set { _okVisibility = value; }
        }


        private Visibility _missingMandatoryVisibility = Visibility.Collapsed;
        public Visibility MissingMandatoryVisibility
        {
            get { return _missingMandatoryVisibility; }
            set { _missingMandatoryVisibility = value; }
        }

        private Visibility _missingOptionalVisibility = Visibility.Collapsed;
        public Visibility MissingOptionalVisibility
        {
            get { return _missingOptionalVisibility; }
            set { _missingOptionalVisibility = value; }
        }


        public Uri IconSource
        {
            get {
                return IsDirect ? new Uri("pack://application:,,,/Resources/Images/in.png", UriKind.RelativeOrAbsolute) : 
                    new Uri("pack://application:,,,/Resources/Images/out.png", UriKind.RelativeOrAbsolute);
            }
        }

        private bool _isDirect;
        public bool IsDirect
        {
            get { return _isDirect; }
            set
            {
                _isDirect = value;
                OnPropertyChanged("IsDirect");
                OnPropertyChanged("IconSource");
            }
        }
        

        public Action<FeatureTreeViewItemViewModel> CheckedChanged { get; set; }

        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked==value) return;
                _isChecked = value;
                OnPropertyChanged("IsChecked");
                if (CheckedChanged!=null)
                {
                    CheckedChanged(this);
                }
            }
        }

        private FeatureType? _featureType;
        public FeatureType? FeatureType
        {
            get { return _featureType; }
            set
            {
                _featureType = value;
                if (_featureType!=null)
                {
                    Name = _featureType.ToString();
                }
                OnPropertyChanged("FeatureType");
            }
        }

       
        public override string ToString()
        {
            return Name;
        }
    }
}
