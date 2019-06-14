using System;
using Aran.Temporality.Internal.Interface;

namespace Aran.Temporality.Common.Entity
{
    public enum ConfigurationType
    {
        PrecisionConfiguration
    }

    public class ConfigurationName
    {
        public static string PublicationResolutionConfiguration = "Publication Resolution";
        public static string ChartingResolutionConfiguration = "Charting Resolution";
    }


    [Serializable]
    public class Configuration : INHibernateEntity
    {
        public virtual int Id { get; set; }

        public virtual int Type { get; set; }//Configurationtype

        public virtual string Name { get; set; }//ConfigurationName

        public virtual byte[] Data { get; set; }
    }
}
