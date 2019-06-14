using System;
using MvvmCore;
using TOSSM.ViewModel.Control.Airac;

namespace Aran.Temporality.CommonUtil.ViewModel.Airac
{
    public class AiracViewModel:ViewModelBase
    {
        public bool IsSeparator { get; set; }

        public int Index { get; set; }

        public override string DisplayName
        {
            get
            {
                if (Index > 0) return Index + " : " +  String.Format("{0:yyyy/MM/dd HH:mm}", AiracSelectorViewModel.GetAiracCycleByIndex(Index))+" UTC";
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
