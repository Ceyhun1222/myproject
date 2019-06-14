using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.AranEnvironment;
using Aran.PANDA.Common;
using DataImporter.Enums;

namespace DataImporter.Factories.RunwayLoader
{
    class RunwayLoaderFactory:IRunwayLoaderFactory
    {
        private ILogger _logger;
        private SpatialReferenceOperation _spOperation;

        public RunwayLoaderFactory(ILogger logger, SpatialReferenceOperation spOperation)
        {
            _logger = logger;
            _spOperation = spOperation;
        }
        public IRunwayFileLoader CreateFileLoader(RunwayFileFormatType fileFormatType, string fileName)
        {
            switch (fileFormatType)
            {
                case RunwayFileFormatType.ExcelStandard:
                    return new ExcelRunwayLoader(fileName, _logger, _spOperation);
                default:
                    return new ExcelRunwayLoader(fileName, _logger, _spOperation);
            }
        }
    }
}
