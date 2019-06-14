using Aran.Temporality.Common.Implementation;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Common.TossConverter.Abstract;
using Aran.Temporality.Common.TossConverter.Implementation;
using Aran.Temporality.Common.TossConverter.Interface;

namespace Aran.Temporality.Common.TossConverter
{
    public class NoDeleteMongoConverter : AbstractTossConverter
    {
        protected override ITossReadableRepository GetConverterFrom(string repositoryNameFrom)
        {
            return new TossReadableNoDeleteRepository(repositoryNameFrom);
        }

        protected override ITossConverterTo GetConverterTo(string repositoryNameTo)
        {
            return new MongoConverter(repositoryNameTo);
        }
    }
}
