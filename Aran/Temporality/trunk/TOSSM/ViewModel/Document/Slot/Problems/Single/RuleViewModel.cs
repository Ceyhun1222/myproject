using System;
using System.Collections.Generic;
using Aran.Temporality.Common.Entity.Util;
using MvvmCore;

namespace TOSSM.ViewModel.Document.Slot.Problems.Single
{
    public class RuleViewModel : ViewModelBase
    {
        public readonly BusinessRuleUtil Util;
        public RuleViewModel(BusinessRuleUtil businessRuleUtil)
        {
            Util = businessRuleUtil;
            IsActive = Util.IsActive;
        }

		

        private int _count;
        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                OnPropertyChanged("Count");
            }
        }

        public string RuleUID => Util.UID;

        public int RuleId => Util?.Id ?? 0;

        public int RuleEntityId => Util?.RuleEntityId ?? 0;

        public bool HasChange()
        {
            if (Util != null)
            {
                return IsActive != Util.IsActive;
            }
            return false;
        }

        public void CancelChange()
        {
            if (Util!=null)
            {
                IsActive = Util.IsActive;
            }
        }

        public Action<RuleViewModel> OnChanged { get; set; }

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                OnPropertyChanged("IsActive");
                if (OnChanged!=null)
                {
                    OnChanged(this);
                }
            }
        }

        //
        public string ApplicableObject
        {
            get
            {
                if (Util == null) return null;

                 if (Util.ApplicableType!=null)
                 {
                     return Util.ApplicableType + 
                         (Util.ApplicableProperty==null?"":" "+Util.ApplicableProperty);
                 }
                return "[...] " + Util.ApplicableProperty;
            }
        }
      
        public string Source => Util == null ? null : Util.Source;

        public string Svbr => Util == null ? null : Util.Svbr;

        public string Comments => Util == null ? null : Util.Comments;

        public string Name => Util == null ? null : Util.Name;

        public string Category => Util == null ? null : "Category: " + Util.Category;
        public string Level => Util == null ? null : Util.Level;


        private List<string> _description;
        public List<string> GetDescriptions()
        {
            return _description ??
                   (_description =
                    new List<string>
                        {
                            Util.ApplicableProperty,
                            Util.ApplicableType,
                            Util.Category,
                            Util.Comments,
                            Util.Svbr,
                            Util.Level,
                            Util.Name,
                            Util.Id.ToString()
                        });
        }
    }
}