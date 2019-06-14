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
    public interface IRunwayLoaderFactory
    {
        IRunwayFileLoader CreateFileLoader(RunwayFileFormatType fileFormatType, string fileName);
    }
}
