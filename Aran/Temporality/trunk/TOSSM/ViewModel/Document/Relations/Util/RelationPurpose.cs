using System.ComponentModel;

namespace TOSSM.ViewModel.Document.Relations.Util
{
    public enum RelationPurpose
    {
        [Description("To be deleted")]
        ToBeDeleted,
        [Description("Stays intact")]
        StaysIntact,
        [Description("Need to be marked")]
        NeedToBeMarked,
        [Description("Will be unlinked")]
        WillBeUnlinked,
    }
}