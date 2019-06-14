using System;
using System.Collections.Generic;
using System.Text;

namespace ARAN.Common
{
    public interface  PandaItem:ICloneable
    {
        void Pack(int handle);
        void UnPack(int handle);
        void Assign(PandaItem source);
    }
}
