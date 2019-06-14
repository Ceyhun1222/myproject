using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Aim.Features
{
    public interface ICRCExtension
    {
        void SetCRCItems(string value);
        string GetCRCItems();

        void SetCRCValue(string value);
        string GetCRCValue();
    }
}
