
namespace Aran.Aim.Data
{
    internal class ChoiceRef
    {
        public long Id { get; set; }

        public int PropType { get; set; }

        public bool IsFeature { get; set; }

        public AimObject AimObj { get; set; }

        public int ValueType { get; set; }
    }
}