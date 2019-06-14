
using DataImporter.Enums;

namespace DataImporter.Models
{
    public interface IFeatType
    {
        string Header { get; set; }
        FeatureType FeatType { get; set; }
    }
}