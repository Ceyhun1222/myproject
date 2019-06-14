using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.PANDA.Common;
using DataImporter.Enums;
using NPOI.XSSF.UserModel;

namespace DataImporter.Factories.ObstacleLoader
{
    public interface IObstacleLoaderFactory
    {
        IObstacleFileLoader CreateFileLoader(ObstacleFileFormatType fileFormatType, string fileName);
    }
}
