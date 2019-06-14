using Aran.Temporality.Common.Implementation;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Common.TossConverter.Abstract;
using Aran.Temporality.Common.TossConverter.Implementation;
using Aran.Temporality.Common.TossConverter.Interface;

namespace Aran.Temporality.Common.TossConverter
{
    public class NoDeleteMongoComparison : AbstractTossComparison
    {
        protected override ITossReadableRepository GetComparisonFirst(string repositoryName)
        {
            return new TossReadableNoDeleteRepository(repositoryName);
        }

        protected override ITossReadableRepository GetComparisonSecond(string repositoryName)
        {
            return new Implementation.MongoConverter(repositoryName);
        }
    }
}
