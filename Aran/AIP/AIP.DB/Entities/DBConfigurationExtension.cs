using System;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;

namespace AIP.DB.Entities
{
    public static class DBConfigurationExtension
    {
        public static T GetDBConfiguration<T>(this DbContext db, Cfg key)
        {
            var sc0 = db.Set<DBConfig>();
            var sc = sc0?.Where(x=>x.Key == key).FirstOrDefault();
            if (sc == null) return default(T);

            var value = sc.Value;
            var tc = TypeDescriptor.GetConverter(typeof(T));
            try
            {
                var convertedValue = (T)tc.ConvertFromString(value);
                return convertedValue;
            }
            catch (NotSupportedException)
            {
                return default(T);
            }
        }

        public static void SetDBConfiguration(this DbContext db, Cfg key, object value)
        {
            var sc0 = db.Set<DBConfig>();
            var sc = sc0?.Where(x => x.Key == key).FirstOrDefault();
            if (sc == null)
            {
                sc = new DBConfig { Key = key };
                db.Set<DBConfig>().Add(sc);
            }

            sc.Value = value?.ToString();
        }

        public static void SaveDBConfiguration(this DbContext db)
        {
            db.SaveChangesAsync();
        }
    }
}
