namespace Aran.Temporality.Common.Abstract.Query
{
    //possibly not needed
    internal class AbstractFeatureByExampleQuery<T> : AbstractQuery
    {
        public T Example { get; set; }
    }
}