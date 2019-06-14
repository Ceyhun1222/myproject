using System;

namespace Aran.Temporality.Common.Entity.Util
{
    [Serializable]
    public class BusinessRuleUtil
    {
        public int Id;
        public string UID { get; set; }
        public int RuleEntityId { get; set; }
        public bool IsActive { get; set; }
        //
        public string ApplicableType { get; set; }
        public string ApplicableProperty { get; set; }
        public string Source { get; set; }
        public string Svbr { get; set; }
        public string Comments { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Level { get; set; }
    }
}
