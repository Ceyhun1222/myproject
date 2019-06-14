using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapEnv.Layers
{
    public class CategoriesSymbol : ICloneable
    {
        public CategoriesSymbol()
        {
            PropertyName = "";
            Symbols = new Dictionary<string, ISymbol>();
        }

        public string PropertyName { get; set; }
        public ISymbol DefaultSymbol { get; set; }
        public Dictionary<string, ISymbol> Symbols { get; private set; }

        public ISymbol GetSymbol(string value)
        {
            if (value != null) {
                ISymbol symbol;
                if (Symbols.TryGetValue(value, out symbol))
                    return symbol;
            }
            return DefaultSymbol;
        }

        public void Assign(CategoriesSymbol source)
        {
            PropertyName = source.PropertyName;
            var clone = source.DefaultSymbol as IClone;
            if (clone != null)
                DefaultSymbol = clone.Clone() as ISymbol;

            Symbols.Clear();
            foreach (string key in source.Symbols.Keys) {
                clone = source.Symbols[key] as IClone;
                if (clone != null)
                    Symbols.Add(key, clone.Clone() as ISymbol);
            }
        }

        public object Clone()
        {
            var result = new CategoriesSymbol();
            result.Assign(this);
            return result;
        }
    }
}
