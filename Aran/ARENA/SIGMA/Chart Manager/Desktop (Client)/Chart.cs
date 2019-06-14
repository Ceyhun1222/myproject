using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartManager.ChartServices
{
    public partial class Chart
    {
        public int AiracNumber
        {
            get
            {
                return AiracUtil.AiracUtil.GetAiracCycleByDate(BeginEffectiveDate);   
            }
        }
    }
}
