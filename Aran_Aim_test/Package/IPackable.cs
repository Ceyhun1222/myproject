using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Package
{
    public interface IPackable
    {
        void Pack ( PackageWriter writer );
        void Unpack ( PackageReader reader );
    }
}
