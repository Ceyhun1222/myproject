namespace Aran.Temporality.Internal.Interface.Util
{
    internal interface IHasOffset<TOffsetType>
    {
        TOffsetType Offset { get; set; }
    }
}