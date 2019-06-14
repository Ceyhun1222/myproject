using Aran.Temporality.Common.TossConverter.Abstract;
using Aran.Temporality.Common.TossConverter.Implementation;
using Aran.Temporality.Common.TossConverter.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aran.Temporality.Common.TossConverter
{
    public class MongoGeometryCleaner : AbstractTossGeometryCleaner
    {
        protected override ITossGeometryCleaner GetGeometryCleaner(string repositoryName)
        {
            return new Implementation.MongoConverter(repositoryName);
        }
    }
}
