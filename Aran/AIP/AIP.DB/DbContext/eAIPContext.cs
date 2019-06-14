using System;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
//using EntityFramework.Audit;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using AIP.BaseLib.Class;
using AIP.DB.Migrations;
using Npgsql;

namespace AIP.DB
{
    [DbConfigurationType(typeof(DbContextConfiguration))]
    public class eAIPContext : DbContext
    {
        public static DefaultConnectionFactory ConnectionFactory = DefaultConnectionFactory.PGSQL;
        public static readonly string DefaultConnection = (ConnectionFactory == DefaultConnectionFactory.MSSQL)
            ? "name=DbConnection"
            : "name=PGDbConnection";
        public static readonly string DefaultConnectionName = (ConnectionFactory == DefaultConnectionFactory.MSSQL)
            ? "DbConnection"
            : "PGDbConnection";

        // For MSSQL or initial
        //public eAIPContext() : base(DefaultConnection)
        //{
        //    //Database.SetInitializer(new DropCreateDatabaseAlways<eAIPContext>());
        //    //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<eAIPContext>());
        //    Database.SetInitializer(new CreateDatabaseIfNotExists<eAIPContext>());
        //    //Database.SetInitializer(new MigrateDatabaseToLatestVersion<eAIPContext, AIP.DB.Migrations.Configuration>());
        //}

        public eAIPContext() : base(GetDbConnection(), true)
        {
            //Database.SetInitializer(new DropCreateDatabaseAlways<eAIPContext>());
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<eAIPContext>());
            Database.SetInitializer(new CreateDatabaseIfNotExists<eAIPContext>());
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<eAIPContext, Configuration>());
        }

        // For MSSQL: in the Xhtml module: using (eAIPContext db = new eAIPContext(AIP.DB.SingleConnection.String))
        public eAIPContext(string connectionString) : base(connectionString)
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<eAIPContext>());
        }


        public static string ServerName()
        {
            try
            {
                string connectString = ConfigurationManager.ConnectionStrings[DefaultConnectionName].ToString();
                System.Data.Common.DbConnectionStringBuilder builder = new System.Data.Common.DbConnectionStringBuilder();
                builder.ConnectionString = connectString;
                string DataSource = ConnectionFactory == DefaultConnectionFactory.MSSQL ? builder["Data Source"] as string : builder["Server"] as string;
                string Server = (DataSource?.Contains(@"\") == true) ? DataSource.GetBeforeOrEmpty(@"\") : DataSource;
                return Server;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static DbConnection GetDbConnection()
        {
            var providerName = ConnectionFactory == DefaultConnectionFactory.MSSQL ? "System.Data.SqlClient" : "Npgsql";
            var conn = DbProviderFactories.GetFactory(providerName).CreateConnection();
            conn.ConnectionString = AIP.DB.SingleConnection.String;
            return conn;
        }

        static eAIPContext()
        {
            //AuditManager.DefaultConfiguration.AutoSavePreAction = (context, audit) =>
            //   // ADD "Where(x => x.AuditEntryID == 0)" to allow multiple SaveChanges with same Audit
            //   (context as eAIPContext).AuditEntries.AddRange(audit.Entries);
        }

        public DbSet<DB.Route> Route { get; set; }
        public DbSet<AIPSection> AIPSection { get; set; }
        public DbSet<AirportHeliport> AirportHeliport { get; set; }
        public DbSet<Subsection> Subsection { get; set; }
        public DbSet<Designatedpoint> Designatedpoint { get; set; }
        public DbSet<Routesegment> Routesegment { get; set; }
        public DbSet<eAIP> eAIP { get; set; }
        public DbSet<eAISpackage> eAISpackage { get; set; }
        public DbSet<eAIPpackage> eAIPpackage { get; set; }
        public DbSet<Amendment> Amendment { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<Description> Description { get; set; }
        public DbSet<Significantpointreference> Significantpointreference { get; set; }
        public DbSet<Navaidindication> Navaidindication { get; set; }
        public DbSet<Routesegmentusagereference> Routesegmentusagereference { get; set; }
        public DbSet<Navaid> Navaid { get; set; }
        public DbSet<Sec36Table> Sec36Table { get; set; }
        public DbSet<Sec36Table2> Sec36Table2 { get; set; }
        public DbSet<LocationTable> LocationTable { get; set; }
        public DbSet<LocationDefinition> LocationDefinition { get; set; }
        public DbSet<LanguageText> LanguageTexts { get; set; }
        public DbSet<LanguageReference> LanguageReference { get; set; }
        public DbSet<eAIPOptions> eAIPOptions { get; set; }
        public DbSet<AIPFile> AIPFile { get; set; }
        public DbSet<AIPFileData> AIPFileData { get; set; }
        public DbSet<AIPFileDataHash> AIPFileDataHash { get; set; }
        public DbSet<ChartNumber> ChartNumber { get; set; }
        public DbSet<Abbreviation> Abbreviation { get; set; }
        public DbSet<DBConfig> DBConfiguration { get; set; }
        public DbSet<Supplement> Supplement { get; set; }
        public DbSet<Circular> Circular { get; set; }
        public DbSet<PDFPage> PDFPage { get; set; }

        public DbSet<AIPPage> AIPPage { get; set; }
        public DbSet<AIPPageData> AIPPageData { get; set; }
        public DbSet<User> User { get; set; }
        //public DbSet<AuditEntry> AuditEntries { get; set; }
        //public DbSet<AuditEntryProperty> AuditEntryProperties { get; set; }

        //public DbSet<FileDB> FileDB { get; set; }
        //public DbSet<FileDBData> FileDBData { get; set; }

        //public override int SaveChanges()
        //{
        //    var audit = new Audit();
        //    audit.CreatedBy = "";
        //    audit.PreSaveChanges(this);
        //    var rowAffecteds = base.SaveChanges();
        //    audit.PostSaveChanges();

        //    if (audit.Configuration.AutoSavePreAction != null)
        //    {
        //        audit.Configuration.AutoSavePreAction(this, audit);
        //        base.SaveChanges();
        //    }

        //    return rowAffecteds;
        //}


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (ConnectionFactory == DefaultConnectionFactory.MSSQL)
            {
                //var convention = new AttributeToColumnAnnotationConvention<DefaultValueAttribute, string>("SqlDefaultValue", (p, attributes) => attributes.SingleOrDefault()?.Value.ToString());
                //modelBuilder.Conventions.Add(convention);
            }
            else
            {
                modelBuilder.HasDefaultSchema("public");
            }

            base.OnModelCreating(modelBuilder);
        }


    }

    public class DbContextConfiguration : DbConfiguration
    {
        //public static string EFCachePath = Directory.GetCurrentDirectory() + @"\Cache\";
        public static string EFCachePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Cache\";

        public DbContextConfiguration() : base()
        {
            if (eAIPContext.ConnectionFactory == DefaultConnectionFactory.PGSQL)
            {
                var name = "Npgsql";

                SetProviderFactory(providerInvariantName: name,
                    providerFactory: NpgsqlFactory.Instance);

                SetProviderServices(providerInvariantName: name,
                    provider: NpgsqlServices.Instance);

                SetDefaultConnectionFactory(connectionFactory: new NpgsqlConnectionFactory());
            }

            if (!Directory.Exists(EFCachePath))
            {
                Directory.CreateDirectory(EFCachePath);
            }
            this.SetModelStore(new DefaultDbModelStore(EFCachePath));

        }

        public void LoadConfig()
        {
            if (eAIPContext.ConnectionFactory == DefaultConnectionFactory.PGSQL)
            {
                var name = "Npgsql";
                SetProviderFactory(name, NpgsqlFactory.Instance);
                SetProviderServices(name, NpgsqlServices.Instance);
                SetDefaultConnectionFactory(new NpgsqlConnectionFactory());
            }
        }
    }

    public enum DefaultConnectionFactory
    {
        MSSQL, // Stable, Connect to MicrosoftSQL. New database creation is requiring. Changes in the app.config is needed.
        PGSQL // Developing, COnnect to PostgreSQL. New database creation is requiring. Changes in the app.config is needed.
    }
}
