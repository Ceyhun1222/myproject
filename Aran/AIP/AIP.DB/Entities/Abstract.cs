using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Telerik.WinControls.UI;

namespace AIP.DB
{
    //Abstract classes
    public abstract class SubClass : IRootEntity, ILinkToSection
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Browsable(false), Display(AutoGenerateField = false)]
        public int id { get; set; }
        // Parent Data
        [Browsable(false), Display(AutoGenerateField = false)]
        public int? eAIPID { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        public eAIP eAIP { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        public int? AIPSectionID { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        public AIPSection AIPSection { get; set; }

        [Browsable(false), ReadOnly(true)]
        public SubClassType SubClassType { get; set; }
        [Browsable(false), Display(AutoGenerateField = false)]
        public int? SubClassID { get; set; }
        [Browsable(false), Display(AutoGenerateField = false)]
        public virtual SubClass Parent { get; set; }
        [Browsable(false), Display(AutoGenerateField = false)]
        public virtual ICollection<SubClass> Children { get; set; }


        [Browsable(false), Display(AutoGenerateField = false)]
        public int? OrderNumber { get; set; }

    }

    public abstract class SimpleEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Browsable(false), Display(AutoGenerateField = false)]
        public int id { get; set; }

        // Parent Data
        [Browsable(false), Display(AutoGenerateField = false)]
        public int? eAIPID { get; set; }

        [Browsable(false), Display(AutoGenerateField = false)]
        public eAIP eAIP { get; set; }
    }

    public abstract class TemporalityEntity
    {
        public TemporalityEntity()
        {
            EffectivedateFrom = DateTime.UtcNow.Date;
            Version = 1;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [GridViewOption(Visible = false, ReadOnly = true)]
        [DataEntryOption(Visible = true, ReadOnly = true)]
        [PropertyOrder(10)]
        public int id { get; set; }
        
        [DataType("UniqueIdentifier")]
        [GridViewOption(Visible = false, ReadOnly = true)]
        [DataEntryOption(Visible = true, ReadOnly = true)]
        [PropertyOrder(20)]
        public Guid Identifier { get; set; }

        [DisplayName("Creation date"), Description("Creation date"), DataType("DateTime2")]
        [ReadOnly(true)]
        [GridViewOption(Visible = false, ReadOnly = true)]
        [DataEntryOption(Visible = true, ReadOnly = true)]
        [PropertyOrder(30)]
        public DateTime Created { get; set; }

        [ReadOnly(true)]
        [DataEntryOption(Visible = true, ReadOnly = true)]
        [GridViewOption(Visible = true, ReadOnly = true)]
        [PropertyOrder(40)]
        public int Version { get; set; }

        [Category("Edit"), DisplayName("Effective date from"), Description("Effective date from"), DataType("DateTime2")]
        [GridViewOption(Visible = true, ReadOnly = true, MaxWidth = 120)]
        [PropertyOrder(50)]
        public DateTime EffectivedateFrom { get; set; }

        [Category("Edit"), DisplayName("Effective date till"), Description("Effective date till"), DataType("DateTime2")]
        [ReadOnly(true)]
        [DataEntryOption(Visible = false, ReadOnly = true)]
        [GridViewOption(Visible = false, ReadOnly = true)]
        [PropertyOrder(60)]
        public virtual DateTime? EffectivedateTo { get; set; }

        // Language Id
        [Category("Edit"), DisplayName("Language"), Description("Select language, if required")]
        [DataEntryOption(Visible = false, ReadOnly = true)]
        [GridViewOption(Visible = true, ReadOnly = true)]
        [PropertyOrder(70)]
        public virtual LanguageReference LanguageReference { get; set; }

        [Category("Edit"), DisplayName("Select Language"), Description("Select language, if required")]
        [DataEntryOption(Visible = true, ReadOnly = false)]
        [GridViewOption(Visible = false, ReadOnly = true)]
        [PropertyOrder(80)]
        // Language Id
        public virtual int? LanguageReferenceId { get; set; }


        // User
        [Category("Edit"), DisplayName("User")]
        [DataEntryOption(Visible = false, ReadOnly = true)]
        [GridViewOption(Visible = true, ReadOnly = true)]
        [PropertyOrder(90)]
        public virtual User User { get; set; }

        [Category("Edit"), DisplayName("User"), Description("User")]
        [DataEntryOption(Visible = true, ReadOnly = true)]
        [GridViewOption(Visible = false, ReadOnly = true)]
        [PropertyOrder(100)]
        // User Id
        public int? UserId { get; set; }

        [Category("Edit")]
        [DataEntryOption(Visible = true, ReadOnly = true)]
        [GridViewOption(Visible = false, ReadOnly = true)]
        [PropertyOrder(110)]
        public bool IsCanceled { get; set; }
    }
}
