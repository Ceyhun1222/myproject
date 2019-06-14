using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Holding
{
    public class CategoryClass
    {
        public categories Category { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
