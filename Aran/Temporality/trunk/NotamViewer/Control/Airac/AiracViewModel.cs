using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvvmCore;
using NotamViewer.Converter;

namespace ADM.ViewModel.Control.Airac
{
    public class AiracViewModel:ViewModelBase
    {
        public bool IsSeparator { get; set; }

        public int Index { get; set; }

        public override string DisplayName
        {
            get
            {
                if (Index > 0) return Index.ToString() + " : " + 
                    HumanReadableConverter.ToHuman(AiracSelectorViewModel.GetAiracCycleByIndex(Index))+" UTC";
                if (Index == 0) return "";
                return "Custom";
            }
            protected set
            {
                base.DisplayName = value;
            }
        }
    }
}
