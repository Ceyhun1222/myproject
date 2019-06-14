using System;
using System.Collections;
using System.Collections.Generic;
using Aran.Aim.Enums;
using MvvmCore;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace TOSSM.ViewModel.Control.AnnotationLanguage
{

    public class AnnotationLanguageViewModel : ViewModelBase
    {
        private List<string> _languageList;
        public List<string> LanguageList
        {
            get
            {
                if (_languageList == null)
                {
                    _languageList = Enum.GetNames(typeof(language)).ToList();
                    _languageList.Add(""); // empty value for any lang
                    _languageList = _languageList.OrderBy(t => t?.ToString()).ToList();
                }
                return _languageList;
            }
        }

        public Action<AnnotationLanguageViewModel> OnChanged { get; set; }

        private string _selectedLanguage;
        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                _selectedLanguage = value;
                OnPropertyChanged("SelectedLanguage");
                OnChanged?.Invoke(this);
            }
        }
    }
}
