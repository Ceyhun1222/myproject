using System;
using System.Configuration;

namespace Aran.Aim.Data.WebServices.AIXMRESTService.Config
{
    public class DatabaseConfig : ConfigurationSection
    {

        private static DatabaseConfig settings = ConfigurationManager.GetSection("DatabaseConfig") as DatabaseConfig;

        public static DatabaseConfig Settings
        {
            get
            {
                return settings;
            }
        }

        [ConfigurationProperty("Server",
            DefaultValue = "127.0.0.1",
            IsRequired = false)]
        public String Server
        {
            get
            {
                return (String)this["Server"];
            }
            set
            {
                this["Server"] = value;
            }
        }

        [ConfigurationProperty("Port",
            DefaultValue = 5432,
            IsRequired = false)]
        [IntegerValidator(MinValue = 1, MaxValue = 65535)]
        public int Port
        {
            get
            {
                return (int)this["Port"];
            }
            set
            {
                this["Port"] = value;
            }
        }

        [ConfigurationProperty("UserId",
         DefaultValue = "postgres",
         IsRequired = false)]
        public String UserId
        {
            get
            {
                return (String)this["UserId"];
            }
            set
            {
                this["UserId"] = value;
            }
        }

        [ConfigurationProperty("ServerPassword",
        DefaultValue = "airnav2012",
        IsRequired = false)]
        public String ServerPassword
        {
            get
            {
                return (String)this["ServerPassword"];
            }
            set
            {
                this["ServerPassword"] = value;
            }
        }

        [ConfigurationProperty("Database",
        DefaultValue = "postgres",
        IsRequired = false)]
        public String Database
        {
            get
            {
                return (String)this["Database"];
            }
            set
            {
                this["Database"] = value;
            }
        }

        [ConfigurationProperty("Timeout",
           DefaultValue = 340,
           IsRequired = false)]
        [IntegerValidator(MinValue = 1)]
        public int Timeout
        {
            get
            {
                return (int)this["Timeout"];
            }
            set
            {
                this["Timeout"] = value;
            }
        }

        [ConfigurationProperty("Username",
        DefaultValue = "administrator",
        IsRequired = false)]
        public String Username
        {
            get
            {
                return (String)this["Username"];
            }
            set
            {
                this["Username"] = value;
            }
        }

        [ConfigurationProperty("Password",
        DefaultValue = "administrator",
        IsRequired = false)]
        public String Password
        {
            get
            {
                return (String)this["Password"];
            }
            set
            {
                this["Password"] = value;
            }
        }
    }
}