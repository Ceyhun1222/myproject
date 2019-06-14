using Aran.AranEnvironment;
using Aran.PANDA.Common;
using DataImporter.Enums;

namespace DataImporter.Factories.ObstacleLoader
{
    class ObstacleLoaderFactory : IObstacleLoaderFactory
    {
        private readonly SpatialReferenceOperation _spOperation;
        private readonly ILogger _logger;

        public ObstacleLoaderFactory(ILogger logger, SpatialReferenceOperation spOperation)
        {
            _logger = logger;
            _spOperation = spOperation;
        }
        public IObstacleFileLoader CreateFileLoader(ObstacleFileFormatType fileFormatType, string fileName)
        {
            switch (fileFormatType)
            {
                case ObstacleFileFormatType.ExcelStandard:
                    return new ExcelObstacleLoader(fileName, _logger, _spOperation);
            }

            return new ExcelObstacleLoader(fileName, _logger, _spOperation);
        }
    }
}

