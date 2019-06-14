using Framework.Stasy.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aerodrome.Metadata
{
    public class ExtensionData
    {
        public ExtensionData()
        {
            Id = Guid.Empty;
        }

        public bool IsInitialized => (Id != Guid.Empty);

        public Guid Id
        {
            get;
            set;
        }

        public string ADHP { get; set; }

        public string Name { get; set; }

        public string Organization { get; set; }

        public int AmdbVersion { get; set; }

        public bool IsReadOnly { get; set; }

        public bool ProjectIsOpened
        {
            get
            {
                if (AerodromeDataCash.ProjectEnvironment is null)
                    return false;
                else
                    return true;
            }
        }

        public void Clear()
        {
            Initialize();
            Id = Guid.Empty;
        }

        public void Initialize()
        {
            Id = Guid.NewGuid();
            AmdbVersion = 0;
            IsReadOnly = false;
        }

    }
}
