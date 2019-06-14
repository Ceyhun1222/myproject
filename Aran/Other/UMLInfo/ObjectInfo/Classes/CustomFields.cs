using System.Collections.Generic;

namespace UMLInfo.Classes
{
    public class CustomEnumFields
    {
        public string Name { get; set; }
        public List<Fields> Fields { get; set; }
    }

    public class Fields
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
