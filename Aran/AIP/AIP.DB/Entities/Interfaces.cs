
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIP.DB
{
    // Interfaces
    public interface IRootEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Browsable(false), Display(AutoGenerateField = false)]
        int id { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        int? eAIPID { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        eAIP eAIP { get; set; }
    }

    public interface ISection
    {

        [DisplayName("Section Status"), Description("Section Status")]
        [Browsable(true), Editable(true), Category("Edit")]
        SectionStatusEnum SectionStatus { get; set; }

        [DisplayName("Section Name"), Description("Section Name")]
        [Browsable(false), Editable(false)]
        SectionName SectionName { get; set; }

        [DisplayName("Section Title"), Description("Section Title")]
        [Browsable(true), Editable(true), Category("Edit")]
        string Title { get; set; }

        [Browsable(true), Editable(true), Category("Edit")]
        NILReason NIL { get; set; }

    }

    public interface ILinkToSection
    {
        [Browsable(false), Display(AutoGenerateField = false)]
        int? AIPSectionID { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        AIPSection AIPSection { get; set; }
    }

    public interface IUserChangesEntity
    {
        // User Created
        [Category("Edit"), DisplayName("Created by")]
        [DataEntryOption(Visible = false, ReadOnly = true)]
        [GridViewOption(Visible = true, ReadOnly = true)]
        [PropertyOrder(90)]
        User CreatedUser { get; set; }

        [Category("Edit"), DisplayName("Create date"), DataType("DateTime2")]
        [DataEntryOption(Visible = false, ReadOnly = true)]
        [GridViewOption(Visible = true, ReadOnly = true)]
        [PropertyOrder(95)]
        DateTime CreatedDate { get; set; }

        [Category("Edit"), DisplayName("Created by"), Description("Created by")]
        [DataEntryOption(Visible = true, ReadOnly = true)]
        [GridViewOption(Visible = false, ReadOnly = true)]
        [PropertyOrder(100)]
        // User Id
        int? CreatedUserId { get; set; }

        // User Changed object
        [Category("Edit"), DisplayName("Changed by")]
        [DataEntryOption(Visible = false, ReadOnly = true)]
        [GridViewOption(Visible = true, ReadOnly = true)]
        [PropertyOrder(110)]
        User ChangedUser { get; set; }

        [Category("Edit"), DisplayName("Change date"), DataType("DateTime2")]
        [DataEntryOption(Visible = false, ReadOnly = true)]
        [GridViewOption(Visible = true, ReadOnly = true)]
        [PropertyOrder(120)]
        DateTime ChangedDate { get; set; }

        [Category("Edit"), DisplayName("Changed by"), Description("User")]
        [DataEntryOption(Visible = true, ReadOnly = true)]
        [GridViewOption(Visible = false, ReadOnly = true)]
        [PropertyOrder(130)]
        // User Id
        int? ChangedUserId { get; set; }
    }
}
