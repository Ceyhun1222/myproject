using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aerodrome.Metadata
{
    public class ExtensionDataContext
    {
        private ExtensionDataProvider _extensionDataProvider;

        public string fileName = "extensionData.obj";

        public ExtensionData ProjectExtensionData;

        public ExtensionDataContext(string path)
        {
            _extensionDataProvider = new ExtensionDataProvider(path, fileName);
            ProjectExtensionData = new ExtensionData();
        }

        public void LoadExtensionData()
        {
            ProjectExtensionData = _extensionDataProvider.GetExtensionData();
        }

        public void CommitExtensionData()
        {
            _extensionDataProvider.CommitExtensionData(ProjectExtensionData);
        }

       
    }
}
