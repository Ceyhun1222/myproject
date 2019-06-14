using Aran.Package;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapEnv.Layers
{
    public class TableShapeInfo : ICloneable, IPackable
    {
        public TableShapeInfo()
        {
            GeoProperty = "";
            TextProperty = "";
            CategorySymbol = new CategoriesSymbol();
        }

        public TableShapeInfo(string geoProp, ISymbol defSymbol)
        {
            GeoProperty = geoProp;
            TextProperty = "";
            CategorySymbol = new CategoriesSymbol();
            CategorySymbol.DefaultSymbol = defSymbol;
        }

        public string GeoProperty { get; set; }
        public string TextProperty { get; set; }
        public CategoriesSymbol CategorySymbol { get; private set; }
        public ITextSymbol TextSymbol { get; set; }

        public void Assign(TableShapeInfo source)
        {
            GeoProperty = source.GeoProperty;
            TextProperty = source.TextProperty;

            if (source.CategorySymbol != null)
                CategorySymbol = source.CategorySymbol.Clone() as CategoriesSymbol;

            var clone = source.TextSymbol as IClone;
            if (clone != null)
                TextSymbol = clone.Clone() as ITextSymbol;
        }

        public object Clone()
        {
            TableShapeInfo result = new TableShapeInfo();
            result.Assign(this);
            return result;
        }

        #region IPackable

        void IPackable.Pack(PackageWriter writer)
        {
            writer.PutString(GeoProperty);
            writer.PutString(TextProperty);
            writer.PutString(CategorySymbol.PropertyName);

            bool notNull = (CategorySymbol.DefaultSymbol != null);
            writer.PutBool(notNull);
            if (notNull)
                (CategorySymbol.DefaultSymbol as IPersistStream).Pack(writer);

            writer.PutInt32(CategorySymbol.Symbols.Count);
            foreach (string key in CategorySymbol.Symbols.Keys) {
                ISymbol symbol = CategorySymbol.Symbols[key];
                writer.PutString(key);
                (symbol as IPersistStream).Pack(writer);
            }

            notNull = (TextSymbol != null);
            writer.PutBool(notNull);
            if (notNull)
                (TextSymbol as IPersistStream).Pack(writer);
        }

        void IPackable.Unpack(PackageReader reader)
        {
            GeoProperty = reader.GetString();
            TextProperty = reader.GetString();
            CategorySymbol.PropertyName = reader.GetString();

            bool notNul = reader.GetBool();
            if (notNul)
                CategorySymbol.DefaultSymbol = LayerPackage.UnpackPersistStream(reader) as ISymbol;

            int count = reader.GetInt32();
            for (int i = 0; i < count; i++) {
                string key = reader.GetString();
                ISymbol symbol = LayerPackage.UnpackPersistStream(reader) as ISymbol;
                CategorySymbol.Symbols.Add(key, symbol);
            }

            notNul = reader.GetBool();
            if (notNul)
                TextSymbol = LayerPackage.UnpackPersistStream(reader) as ITextSymbol;
        }

        #endregion
    }
}
