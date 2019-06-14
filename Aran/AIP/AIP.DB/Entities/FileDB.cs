using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using Telerik.WinControls.Enumerations;
using Aran.Aim.Enums;
using System.Collections.ObjectModel;
using Telerik.WinControls.UI;

namespace AIP.DB
{
    //[Table("FileDB")]
    //public class FileDB
    //{
    //    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    [Browsable(false), Display(AutoGenerateField = false)]
    //    public int id { get; set; }

    //    [Category("Main properties"), DisplayName("Path"), Description("Path")]
    //    public string Path { get; set; }

    //    [Category("Main properties"), DisplayName("Effective date"), Description("Effective date"), DataType("DateTime2")]
    //    public DateTime Effectivedate { get; set; }

    //    [Category("Main properties"), DisplayName("Language"), Description("Enter language")]
    //    public string lang { get; set; }

    //    [Browsable(false), Display(AutoGenerateField = false)]
    //    public string Version { get; set; }

    //    [Category("Main properties"), DisplayName("Cancel"), Description("Cancel")]
    //    [DataEntryOption(Visible = true, ReadOnly = true)]
    //    public bool Cancel { get; set; }


    //    [Browsable(false), Display(AutoGenerateField = false)]
    //    public int? FileDataId { get; set; }

    //    [Browsable(false), Display(AutoGenerateField = false)]
    //    public FileDBData FileData { get; set; }
    //}
    

    //[Table("FileDBData")]
    //public class FileDBData
    //{
    //    public FileDBData()
    //    {
    //    }

    //    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    [Browsable(false), Display(AutoGenerateField = false)]
    //    public int id { get; set; }

    //    [Browsable(false), Display(AutoGenerateField = false)]
    //    public byte[] Data { get; set; }
        
    //    [Browsable(false), Display(AutoGenerateField = false)]
    //    public string Hash { get; set; }
    //}
    
}