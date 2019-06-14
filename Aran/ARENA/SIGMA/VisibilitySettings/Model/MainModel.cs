using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using ESRI.ArcGIS.esriSystem;

namespace VisibilityTool.Model
{
	public class FeatModel : INotifyPropertyChanged
	{
		private int _handle;
		private bool _isSelected;

		public FeatModel ()
		{
			_isSelected = false;
		}

		public string Identifier { get; set; }
	    public int Id { get; set; }  

		public string CodeType { get; set; }
		public string Description { get; set; }
		public IGeometry Shape { get; set; }

		public bool IsVisible { get; set; }

		public bool IsSelected
		{
			get => _isSelected;
		    set
			{
				_isSelected = value;
				Draw (_isSelected);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsSelected"));
            }
		}

		private void Draw (bool show)
		{
			GlobalParams.Graphics.SafeDeleteGraphic (_handle);
			GlobalParams.HookHelper.ActiveView.Refresh (); 
			if (!show)
				return;
		    if (Shape != null)
		    {
		        IClone clone = Shape as IClone;
		        var cl = clone.Clone();

				if ( Shape.GeometryType == esriGeometryType.esriGeometryPolygon )
				{
					var newGeo = cl as IPolygon;
					var prjGeo = GlobalParams.SpatialOperation.ToProject ( newGeo );
					if ( prjGeo != null && !prjGeo.IsEmpty )
						_handle = GlobalParams.Graphics.DrawEsriDefaultMultiPolygon ( ( IPolygon ) prjGeo, IsVisible );
				}
				else if ( Shape.GeometryType == esriGeometryType.esriGeometryPolyline )
				{
					var newGeo = cl as IPolyline;
					var prjGeo = GlobalParams.SpatialOperation.ToProject ( newGeo );
					if ( prjGeo != null && !prjGeo.IsEmpty )
						_handle = GlobalParams.Graphics.DrawMultiLineString ( ( IPolyline ) prjGeo, IsVisible );
				}
				else if ( Shape.GeometryType == esriGeometryType.esriGeometryPoint )
				{
					var newGeo = cl as IPoint;
					var prjGeo = GlobalParams.SpatialOperation.ToProject ( newGeo );
					if ( prjGeo != null && !prjGeo.IsEmpty )
						_handle = GlobalParams.Graphics.DrawEsriPoint ( ( IPoint ) prjGeo, IsVisible );
				}
		        GlobalParams.HookHelper.ActiveView.Refresh();
		    }
		}

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion
	}	
}