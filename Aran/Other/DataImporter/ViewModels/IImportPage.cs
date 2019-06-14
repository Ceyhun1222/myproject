using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataImporter.Models;

namespace DataImporter.ViewModels
{
    public interface IImportPageVM
    {
        void Save();

        void Load(string fileName);

        IFeatType FeatType { get; }
    }
}
