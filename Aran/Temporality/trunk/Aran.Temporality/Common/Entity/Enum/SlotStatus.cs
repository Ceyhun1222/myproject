using System.ComponentModel;

namespace Aran.Temporality.Common.Entity.Enum
{
    public enum SlotStatus
    {
        [Description("Empty")]
        Empty,

        [Description("Opened")]
        Opened,

        [Description("To Be Checked")]
        ToBeChecked,

        //

        [Description("Checking")]
        Checking,

        [Description("Checked, Incorrect")]
        CheckFailed,

        [Description("Can not be published due to error")]
        PublishingFailed,

        [Description("Validated, Ok")]
        ValidatedOk,

        [Description("Checked, Ok")]
        CheckOk,

        [Description("Expired")]
        Expired,

        [Description("To Be Published")]
        ToBePublished,

        [Description("Publishing")]
        Publishing,

        [Description("Published")]
        Published,

        [Description("Checking was cancelled")]
        CheckCancelled
    }
}