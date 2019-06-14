namespace AIP.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialPostgreSQL : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.Abbreviation",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        IdKey = c.String(maxLength: 41),
                        Ident = c.String(),
                        Details = c.String(),
                        Identifier = c.Guid(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Version = c.Int(nullable: false),
                        EffectivedateFrom = c.DateTime(nullable: false),
                        EffectivedateTo = c.DateTime(),
                        LanguageReferenceId = c.Int(),
                        UserId = c.Int(),
                        IsCanceled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("public.LanguageReference", t => t.LanguageReferenceId)
                .ForeignKey("public.User", t => t.UserId)
                .Index(t => t.IdKey)
                .Index(t => t.LanguageReferenceId)
                .Index(t => t.UserId);
            
            CreateTable(
                "public.LanguageReference",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        eAIPOptionsId = c.Int(),
                        Name = c.String(),
                        Value = c.String(),
                        AIXMValue = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("public.eAIPOptions", t => t.eAIPOptionsId)
                .Index(t => t.eAIPOptionsId);
            
            CreateTable(
                "public.eAIPOptions",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "public.LanguageText",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        LanguageReferenceId = c.Int(),
                        LanguageCategory = c.Int(nullable: false),
                        Name = c.String(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("public.LanguageReference", t => t.LanguageReferenceId)
                .Index(t => t.LanguageReferenceId);
            
            CreateTable(
                "public.User",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        TossId = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "public.AIPFile",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        SectionName = c.Int(nullable: false),
                        ChartNumberId = c.Int(),
                        AirportHeliport = c.String(),
                        Title = c.String(),
                        Description = c.String(),
                        FileName = c.String(),
                        Order = c.Int(nullable: false),
                        AIPFileDataId = c.Int(),
                        Identifier = c.Guid(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Version = c.Int(nullable: false),
                        EffectivedateFrom = c.DateTime(nullable: false),
                        EffectivedateTo = c.DateTime(),
                        LanguageReferenceId = c.Int(),
                        UserId = c.Int(),
                        IsCanceled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("public.FileData", t => t.AIPFileDataId)
                .ForeignKey("public.ChartNumber", t => t.ChartNumberId)
                .ForeignKey("public.LanguageReference", t => t.LanguageReferenceId)
                .ForeignKey("public.User", t => t.UserId)
                .Index(t => t.ChartNumberId)
                .Index(t => t.AIPFileDataId)
                .Index(t => t.LanguageReferenceId)
                .Index(t => t.UserId);
            
            CreateTable(
                "public.FileData",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Data = c.Binary(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "public.FileDataHash",
                c => new
                    {
                        id = c.Int(nullable: false),
                        Hash = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("public.FileData", t => t.id)
                .Index(t => t.id);
            
            CreateTable(
                "public.ChartNumber",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "public.AIPPage",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        eAIPID = c.Int(),
                        PageType = c.Int(nullable: false),
                        DocType = c.Int(nullable: false),
                        AIPPageDataId = c.Int(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedUserId = c.Int(),
                        ChangedDate = c.DateTime(nullable: false),
                        ChangedUserId = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("public.AIPPageData", t => t.AIPPageDataId)
                .ForeignKey("public.User", t => t.ChangedUserId)
                .ForeignKey("public.User", t => t.CreatedUserId)
                .ForeignKey("public.AIP", t => t.eAIPID)
                .Index(t => t.eAIPID)
                .Index(t => t.AIPPageDataId)
                .Index(t => t.CreatedUserId)
                .Index(t => t.ChangedUserId);
            
            CreateTable(
                "public.AIPPageData",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Data = c.Binary(),
                        Hash = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "public.AIP",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        ICAOcountrycode = c.String(),
                        State = c.String(),
                        Publishingstate = c.String(),
                        Publishingorganisation = c.String(),
                        Edition = c.String(),
                        Publicationdate = c.DateTime(),
                        Effectivedate = c.DateTime(nullable: false),
                        Toc = c.Int(nullable: false),
                        Remarks = c.String(),
                        _class = c.String(name: "class"),
                        _base = c.String(name: "base"),
                        title = c.String(),
                        lang = c.String(),
                        Version = c.String(),
                        AIPStatus = c.Int(nullable: false),
                        Amendment_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("public.AIPAmendment", t => t.Amendment_id)
                .Index(t => t.Amendment_id);
            
            CreateTable(
                "public.SubClasses",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        eAIPID = c.Int(),
                        AIPSectionID = c.Int(),
                        SubClassType = c.Int(nullable: false),
                        SubClassID = c.Int(),
                        OrderNumber = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("public.Section", t => t.AIPSectionID)
                .ForeignKey("public.SubClasses", t => t.SubClassID)
                .ForeignKey("public.AIP", t => t.eAIPID)
                .Index(t => t.eAIPID)
                .Index(t => t.AIPSectionID)
                .Index(t => t.SubClassID);
            
            CreateTable(
                "public.Section",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        eAIPID = c.Int(),
                        SectionStatus = c.Int(nullable: false),
                        SectionName = c.Int(nullable: false),
                        Title = c.String(),
                        NIL = c.Int(nullable: false),
                        AirportHeliportID = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("public.AirportHeliport", t => t.AirportHeliportID)
                .ForeignKey("public.AIP", t => t.eAIPID)
                .Index(t => t.eAIPID)
                .Index(t => t.AirportHeliportID);
            
            CreateTable(
                "public.AirportHeliport",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        eAIPID = c.Int(),
                        Type = c.Int(),
                        Name = c.String(),
                        LocationIndicatorICAO = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("public.AIP", t => t.eAIPID)
                .Index(t => t.eAIPID);
            
            CreateTable(
                "public.LocationDefinition",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        LocationTableid = c.Int(),
                        LocationIdent = c.String(),
                        LocationName = c.String(),
                        LocationDefinitionAFS = c.Int(nullable: false),
                        LocationDefinitionType = c.Int(nullable: false),
                        eAIPID = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("public.AIP", t => t.eAIPID)
                .ForeignKey("public.LocationTable", t => t.LocationTableid)
                .Index(t => t.LocationTableid)
                .Index(t => t.eAIPID);
            
            CreateTable(
                "public.Routesegmentusagereference",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Routesegmentid = c.Int(),
                        Routesegmentusagedirection = c.String(),
                        Routesegmentusageleveltype = c.String(),
                        Routesegmentusagemaximallevel = c.String(),
                        Routesegmentusageminimallevel = c.String(),
                        Ref = c.String(),
                        eAIPID = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("public.AIP", t => t.eAIPID)
                .ForeignKey("public.RouteSegment", t => t.Routesegmentid)
                .Index(t => t.Routesegmentid)
                .Index(t => t.eAIPID);
            
            CreateTable(
                "public.Sec36Table2",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Sec36Tableid = c.Int(),
                        inboundCourse = c.String(),
                        turnDirection = c.String(),
                        speedLimit = c.String(),
                        upperLimit = c.String(),
                        lowerLimit = c.String(),
                        duration = c.String(),
                        length = c.String(),
                        eAIPID = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("public.AIP", t => t.eAIPID)
                .ForeignKey("public.Sec36Table", t => t.Sec36Tableid)
                .Index(t => t.Sec36Tableid)
                .Index(t => t.eAIPID);
            
            CreateTable(
                "public.Navaidindication",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Significantpointreferenceid = c.Int(),
                        Navaidindicationradial = c.String(),
                        Navaidindicationdistance = c.String(),
                        eAIPID = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("public.AIP", t => t.eAIPID)
                .ForeignKey("public.Significantpointreference", t => t.Significantpointreferenceid)
                .Index(t => t.Significantpointreferenceid)
                .Index(t => t.eAIPID);
            
            CreateTable(
                "public.Description",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        eAIPID = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("public.AIP", t => t.eAIPID)
                .Index(t => t.eAIPID);
            
            CreateTable(
                "public.Group",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Amendmentid = c.Int(),
                        Type = c.Int(nullable: false),
                        eAIPID = c.Int(),
                        Description_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("public.AIPAmendment", t => t.Amendmentid)
                .ForeignKey("public.Description", t => t.Description_id)
                .ForeignKey("public.AIP", t => t.eAIPID)
                .Index(t => t.Amendmentid)
                .Index(t => t.eAIPID)
                .Index(t => t.Description_id);
            
            CreateTable(
                "public.Circular",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Number = c.String(),
                        Year = c.String(),
                        Series = c.String(),
                        Remarks = c.String(),
                        EffectivedateTo = c.DateTime(),
                        Publicationdate = c.DateTime(nullable: false),
                        Identifier = c.Guid(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Version = c.Int(nullable: false),
                        EffectivedateFrom = c.DateTime(nullable: false),
                        LanguageReferenceId = c.Int(),
                        UserId = c.Int(),
                        IsCanceled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("public.LanguageReference", t => t.LanguageReferenceId)
                .ForeignKey("public.User", t => t.UserId)
                .Index(t => t.LanguageReferenceId)
                .Index(t => t.UserId);
            
            CreateTable(
                "public.DBConfig",
                c => new
                    {
                        Key = c.Int(nullable: false),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Key);
            
            CreateTable(
                "public.eAIPpackage",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        packagename = c.String(),
                        eAIP_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("public.AIP", t => t.eAIP_id)
                .Index(t => t.eAIP_id);
            
            CreateTable(
                "public.eAISpackage",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        ICAOcountrycode = c.String(),
                        State = c.String(),
                        Publishingstate = c.String(),
                        Publishingorganisation = c.String(),
                        Publicationdate = c.DateTime(),
                        Effectivedate = c.DateTime(nullable: false),
                        lang = c.String(),
                        Version = c.String(),
                        Status = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedUserId = c.Int(),
                        ChangedDate = c.DateTime(nullable: false),
                        ChangedUserId = c.Int(),
                        eAIPpackage_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("public.User", t => t.ChangedUserId)
                .ForeignKey("public.User", t => t.CreatedUserId)
                .ForeignKey("public.eAIPpackage", t => t.eAIPpackage_id)
                .Index(t => t.CreatedUserId)
                .Index(t => t.ChangedUserId)
                .Index(t => t.eAIPpackage_id);
            
            CreateTable(
                "public.PDFPages",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        eAIPID = c.Int(),
                        Section = c.Int(nullable: false),
                        AirportHeliportID = c.Int(),
                        Page = c.Int(nullable: false),
                        PageeAIPID = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("public.AirportHeliport", t => t.AirportHeliportID)
                .ForeignKey("public.AIP", t => t.eAIPID)
                .ForeignKey("public.AIP", t => t.PageeAIPID)
                .Index(t => t.eAIPID)
                .Index(t => t.AirportHeliportID)
                .Index(t => t.PageeAIPID);
            
            CreateTable(
                "public.Supplement",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Number = c.String(),
                        Year = c.String(),
                        Affects = c.String(),
                        EffectivedateTo = c.DateTime(),
                        Publicationdate = c.DateTime(nullable: false),
                        Identifier = c.Guid(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Version = c.Int(nullable: false),
                        EffectivedateFrom = c.DateTime(nullable: false),
                        LanguageReferenceId = c.Int(),
                        UserId = c.Int(),
                        IsCanceled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("public.LanguageReference", t => t.LanguageReferenceId)
                .ForeignKey("public.User", t => t.UserId)
                .Index(t => t.LanguageReferenceId)
                .Index(t => t.UserId);
            
            CreateTable(
                "public.AIPAmendment",
                c => new
                    {
                        id = c.Int(nullable: false),
                        Description_id = c.Int(),
                        Type = c.Int(nullable: false),
                        AmendmentStatus = c.Int(nullable: false),
                        Number = c.String(),
                        Year = c.String(),
                        Publicationdate = c.String(),
                        Effectivedate = c.String(),
                        Remarks = c.String(),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("public.SubClasses", t => t.id)
                .ForeignKey("public.Description", t => t.Description_id)
                .Index(t => t.id)
                .Index(t => t.Description_id);
            
            CreateTable(
                "public.Designatedpoint",
                c => new
                    {
                        id = c.Int(nullable: false),
                        Designatedpointident = c.String(),
                        Latitude = c.String(),
                        Longitude = c.String(),
                        SIDSTAR = c.String(),
                        DesignatedpointListed = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("public.SubClasses", t => t.id)
                .Index(t => t.id);
            
            CreateTable(
                "public.LocationTable",
                c => new
                    {
                        id = c.Int(nullable: false),
                        Caption = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("public.SubClasses", t => t.id)
                .Index(t => t.id);
            
            CreateTable(
                "public.Navaid",
                c => new
                    {
                        id = c.Int(nullable: false),
                        Navaidtype = c.String(),
                        Navaidname = c.String(),
                        Navaidident = c.String(),
                        Navaidmagneticvariation = c.String(),
                        Navaiddeclination = c.String(),
                        Navaidfrequency = c.String(),
                        Navaidhours = c.String(),
                        Latitude = c.String(),
                        Longitude = c.String(),
                        Navaidelevation = c.String(),
                        NavaidListed = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("public.SubClasses", t => t.id)
                .Index(t => t.id);
            
            CreateTable(
                "public.Route",
                c => new
                    {
                        id = c.Int(nullable: false),
                        Routedesignator = c.String(),
                        RouteRNP = c.String(),
                        Routesegmentusage = c.String(),
                        Significantpointremark = c.String(),
                        Routeremark = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("public.SubClasses", t => t.id)
                .Index(t => t.id);
            
            CreateTable(
                "public.RouteSegment",
                c => new
                    {
                        id = c.Int(nullable: false),
                        RoutesegmentRNP = c.String(),
                        Routesegmenttruetrack = c.String(),
                        Routesegmentreversetruetrack = c.String(),
                        Routesegmentmagtrack = c.String(),
                        Routesegmentreversemagtrack = c.String(),
                        Routesegmentlength = c.String(),
                        Routesegmentwidth = c.String(),
                        Routesegmentupper = c.String(),
                        Routesegmentlower = c.String(),
                        Routesegmentminimum = c.String(),
                        Routesegmentloweroverride = c.String(),
                        RoutesegmentATC = c.String(),
                        Routesegmentairspaceclass = c.String(),
                        RoutesegmentCOP = c.String(),
                        Routesegmentremarkreference = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("public.SubClasses", t => t.id)
                .Index(t => t.id);
            
            CreateTable(
                "public.Sec36Table",
                c => new
                    {
                        id = c.Int(nullable: false),
                        Name = c.String(),
                        Ident = c.String(),
                        X = c.String(),
                        Y = c.String(),
                        rowspan = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("public.SubClasses", t => t.id)
                .Index(t => t.id);
            
            CreateTable(
                "public.Significantpointreference",
                c => new
                    {
                        id = c.Int(nullable: false),
                        SignificantpointATC = c.String(),
                        Significantpointdescription = c.String(),
                        Ref = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("public.SubClasses", t => t.id)
                .Index(t => t.id);
            
            CreateTable(
                "public.Subsection",
                c => new
                    {
                        id = c.Int(nullable: false),
                        Title = c.String(),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("public.SubClasses", t => t.id)
                .Index(t => t.id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("public.Subsection", "id", "public.SubClasses");
            DropForeignKey("public.Significantpointreference", "id", "public.SubClasses");
            DropForeignKey("public.Sec36Table", "id", "public.SubClasses");
            DropForeignKey("public.RouteSegment", "id", "public.SubClasses");
            DropForeignKey("public.Route", "id", "public.SubClasses");
            DropForeignKey("public.Navaid", "id", "public.SubClasses");
            DropForeignKey("public.LocationTable", "id", "public.SubClasses");
            DropForeignKey("public.Designatedpoint", "id", "public.SubClasses");
            DropForeignKey("public.AIPAmendment", "Description_id", "public.Description");
            DropForeignKey("public.AIPAmendment", "id", "public.SubClasses");
            DropForeignKey("public.Supplement", "UserId", "public.User");
            DropForeignKey("public.Supplement", "LanguageReferenceId", "public.LanguageReference");
            DropForeignKey("public.PDFPages", "PageeAIPID", "public.AIP");
            DropForeignKey("public.PDFPages", "eAIPID", "public.AIP");
            DropForeignKey("public.PDFPages", "AirportHeliportID", "public.AirportHeliport");
            DropForeignKey("public.eAISpackage", "eAIPpackage_id", "public.eAIPpackage");
            DropForeignKey("public.eAISpackage", "CreatedUserId", "public.User");
            DropForeignKey("public.eAISpackage", "ChangedUserId", "public.User");
            DropForeignKey("public.eAIPpackage", "eAIP_id", "public.AIP");
            DropForeignKey("public.Circular", "UserId", "public.User");
            DropForeignKey("public.Circular", "LanguageReferenceId", "public.LanguageReference");
            DropForeignKey("public.AIPPage", "eAIPID", "public.AIP");
            DropForeignKey("public.AIP", "Amendment_id", "public.AIPAmendment");
            DropForeignKey("public.Group", "eAIPID", "public.AIP");
            DropForeignKey("public.Group", "Description_id", "public.Description");
            DropForeignKey("public.Group", "Amendmentid", "public.AIPAmendment");
            DropForeignKey("public.Description", "eAIPID", "public.AIP");
            DropForeignKey("public.Section", "eAIPID", "public.AIP");
            DropForeignKey("public.Navaidindication", "Significantpointreferenceid", "public.Significantpointreference");
            DropForeignKey("public.Navaidindication", "eAIPID", "public.AIP");
            DropForeignKey("public.Sec36Table2", "Sec36Tableid", "public.Sec36Table");
            DropForeignKey("public.Sec36Table2", "eAIPID", "public.AIP");
            DropForeignKey("public.Routesegmentusagereference", "Routesegmentid", "public.RouteSegment");
            DropForeignKey("public.Routesegmentusagereference", "eAIPID", "public.AIP");
            DropForeignKey("public.LocationDefinition", "LocationTableid", "public.LocationTable");
            DropForeignKey("public.LocationDefinition", "eAIPID", "public.AIP");
            DropForeignKey("public.SubClasses", "eAIPID", "public.AIP");
            DropForeignKey("public.SubClasses", "SubClassID", "public.SubClasses");
            DropForeignKey("public.SubClasses", "AIPSectionID", "public.Section");
            DropForeignKey("public.AirportHeliport", "eAIPID", "public.AIP");
            DropForeignKey("public.Section", "AirportHeliportID", "public.AirportHeliport");
            DropForeignKey("public.AIPPage", "CreatedUserId", "public.User");
            DropForeignKey("public.AIPPage", "ChangedUserId", "public.User");
            DropForeignKey("public.AIPPage", "AIPPageDataId", "public.AIPPageData");
            DropForeignKey("public.AIPFile", "UserId", "public.User");
            DropForeignKey("public.AIPFile", "LanguageReferenceId", "public.LanguageReference");
            DropForeignKey("public.AIPFile", "ChartNumberId", "public.ChartNumber");
            DropForeignKey("public.FileDataHash", "id", "public.FileData");
            DropForeignKey("public.AIPFile", "AIPFileDataId", "public.FileData");
            DropForeignKey("public.Abbreviation", "UserId", "public.User");
            DropForeignKey("public.Abbreviation", "LanguageReferenceId", "public.LanguageReference");
            DropForeignKey("public.LanguageText", "LanguageReferenceId", "public.LanguageReference");
            DropForeignKey("public.LanguageReference", "eAIPOptionsId", "public.eAIPOptions");
            DropIndex("public.Subsection", new[] { "id" });
            DropIndex("public.Significantpointreference", new[] { "id" });
            DropIndex("public.Sec36Table", new[] { "id" });
            DropIndex("public.RouteSegment", new[] { "id" });
            DropIndex("public.Route", new[] { "id" });
            DropIndex("public.Navaid", new[] { "id" });
            DropIndex("public.LocationTable", new[] { "id" });
            DropIndex("public.Designatedpoint", new[] { "id" });
            DropIndex("public.AIPAmendment", new[] { "Description_id" });
            DropIndex("public.AIPAmendment", new[] { "id" });
            DropIndex("public.Supplement", new[] { "UserId" });
            DropIndex("public.Supplement", new[] { "LanguageReferenceId" });
            DropIndex("public.PDFPages", new[] { "PageeAIPID" });
            DropIndex("public.PDFPages", new[] { "AirportHeliportID" });
            DropIndex("public.PDFPages", new[] { "eAIPID" });
            DropIndex("public.eAISpackage", new[] { "eAIPpackage_id" });
            DropIndex("public.eAISpackage", new[] { "ChangedUserId" });
            DropIndex("public.eAISpackage", new[] { "CreatedUserId" });
            DropIndex("public.eAIPpackage", new[] { "eAIP_id" });
            DropIndex("public.Circular", new[] { "UserId" });
            DropIndex("public.Circular", new[] { "LanguageReferenceId" });
            DropIndex("public.Group", new[] { "Description_id" });
            DropIndex("public.Group", new[] { "eAIPID" });
            DropIndex("public.Group", new[] { "Amendmentid" });
            DropIndex("public.Description", new[] { "eAIPID" });
            DropIndex("public.Navaidindication", new[] { "eAIPID" });
            DropIndex("public.Navaidindication", new[] { "Significantpointreferenceid" });
            DropIndex("public.Sec36Table2", new[] { "eAIPID" });
            DropIndex("public.Sec36Table2", new[] { "Sec36Tableid" });
            DropIndex("public.Routesegmentusagereference", new[] { "eAIPID" });
            DropIndex("public.Routesegmentusagereference", new[] { "Routesegmentid" });
            DropIndex("public.LocationDefinition", new[] { "eAIPID" });
            DropIndex("public.LocationDefinition", new[] { "LocationTableid" });
            DropIndex("public.AirportHeliport", new[] { "eAIPID" });
            DropIndex("public.Section", new[] { "AirportHeliportID" });
            DropIndex("public.Section", new[] { "eAIPID" });
            DropIndex("public.SubClasses", new[] { "SubClassID" });
            DropIndex("public.SubClasses", new[] { "AIPSectionID" });
            DropIndex("public.SubClasses", new[] { "eAIPID" });
            DropIndex("public.AIP", new[] { "Amendment_id" });
            DropIndex("public.AIPPage", new[] { "ChangedUserId" });
            DropIndex("public.AIPPage", new[] { "CreatedUserId" });
            DropIndex("public.AIPPage", new[] { "AIPPageDataId" });
            DropIndex("public.AIPPage", new[] { "eAIPID" });
            DropIndex("public.ChartNumber", new[] { "Name" });
            DropIndex("public.FileDataHash", new[] { "id" });
            DropIndex("public.AIPFile", new[] { "UserId" });
            DropIndex("public.AIPFile", new[] { "LanguageReferenceId" });
            DropIndex("public.AIPFile", new[] { "AIPFileDataId" });
            DropIndex("public.AIPFile", new[] { "ChartNumberId" });
            DropIndex("public.LanguageText", new[] { "LanguageReferenceId" });
            DropIndex("public.LanguageReference", new[] { "eAIPOptionsId" });
            DropIndex("public.Abbreviation", new[] { "UserId" });
            DropIndex("public.Abbreviation", new[] { "LanguageReferenceId" });
            DropIndex("public.Abbreviation", new[] { "IdKey" });
            DropTable("public.Subsection");
            DropTable("public.Significantpointreference");
            DropTable("public.Sec36Table");
            DropTable("public.RouteSegment");
            DropTable("public.Route");
            DropTable("public.Navaid");
            DropTable("public.LocationTable");
            DropTable("public.Designatedpoint");
            DropTable("public.AIPAmendment");
            DropTable("public.Supplement");
            DropTable("public.PDFPages");
            DropTable("public.eAISpackage");
            DropTable("public.eAIPpackage");
            DropTable("public.DBConfig");
            DropTable("public.Circular");
            DropTable("public.Group");
            DropTable("public.Description");
            DropTable("public.Navaidindication");
            DropTable("public.Sec36Table2");
            DropTable("public.Routesegmentusagereference");
            DropTable("public.LocationDefinition");
            DropTable("public.AirportHeliport");
            DropTable("public.Section");
            DropTable("public.SubClasses");
            DropTable("public.AIP");
            DropTable("public.AIPPageData");
            DropTable("public.AIPPage");
            DropTable("public.ChartNumber");
            DropTable("public.FileDataHash");
            DropTable("public.FileData");
            DropTable("public.AIPFile");
            DropTable("public.User");
            DropTable("public.LanguageText");
            DropTable("public.eAIPOptions");
            DropTable("public.LanguageReference");
            DropTable("public.Abbreviation");
        }
    }
}
