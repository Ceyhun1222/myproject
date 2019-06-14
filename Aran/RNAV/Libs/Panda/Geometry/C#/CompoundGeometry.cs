using System;
using System.Collections;
using System.Text;

namespace ARAN.GeometryClasses
{
    public abstract class CompoundGeometry : Geometry
    {
       
        public CompoundGeometry()
        {
            _geometryList = new ArrayList();
        }
        
        public void Clear()
        {
            _geometryList.Clear();
        }
        
               
        public override void Pack(int handle)
        {
           Registry.PutInt(handle, _geometryList.Count);
            for (int i = 0; i <= GetCount() - 1; i++)
                GetGeometry(i).Pack(handle);
        }
       
        public void Unpack(int handle)
        {
            int geomtype;
            Geometry geometry;
            int size = Registry.GetInt(handle);
            Clear();
            for (int i = 0; i < size - 1; i++)
            {
                geomtype = Registry.GetInt(handle);
                geometry = Geometry.GeometryCreate(geomtype);
                geometry.UnPack(handle);
                AddGeometry(geometry);
            }   
        }
       
        public void RemoveAt(int index)
        {
            _geometryList.Remove(index);
        }
        
        protected void SetGeometry(int index, Geometry val)
        {
            _geometryList[index] = val;
        }
        
        protected Geometry GetGeometry(int index)
        {
            if ((index >= 0) && (index <= _geometryList.Count))
            {
                return (Geometry)_geometryList[index];
            }
            else
                return null;
        }
        
        protected int GetCount()
        {
            return _geometryList.Count;
        }

        protected void AddGeometry(Geometry geometry)
        {
            _geometryList.Add(geometry.Clone());
        }
        
        protected void InsertGeometry(int index, Geometry geometry)
        {
            if ((index >= 0) && (index < _geometryList.Count))
                _geometryList.Insert(index, geometry.Clone());

        }

        private ArrayList _geometryList;
    }
}
