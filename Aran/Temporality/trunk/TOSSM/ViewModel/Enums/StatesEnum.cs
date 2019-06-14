using System.ComponentModel;

namespace TOSSM.ViewModel.Enums
{
    public enum StatesEnum
    {
        [Description("Snapshot")]
        Snapshot,
        [Description("BaseLine")]
        BaseLine,
        [Description("Temporary")]
        Temporary
    }
}