using System.Windows.Forms;
using Aran.AranEnvironment;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.Geometry;

namespace Aran.Panda.RadarMA.Models
{
   public class MapToolItem : BaseTool 
	{
		public MapToolItem ( AranTool tsmItem )
		{
			_aranToolStripMenuItem = tsmItem;
			m_cursor = _aranToolStripMenuItem.Cursor;
		}

		public override void OnCreate ( object hook )
		{
		}

       public AranTool Tool {
           get { return _aranToolStripMenuItem; }
       }

		#region ITool Members

		public override bool Deactivate ( )
		{
			if ( _aranToolStripMenuItem.Deactivated != null )
			{
				_aranToolStripMenuItem.Deactivated ( );
			}
			return true;
		}

		public override bool OnContextMenu ( int x, int y )
		{
			//MessageBox.Show ( "OnContextMenu is called !" );
			return true;
		}

		public override void OnDblClick ( )
		{
            if (_aranToolStripMenuItem.MouseOnDblClickOnMap != null)
                _aranToolStripMenuItem.MouseOnDblClickOnMap();
		}

		public override void OnKeyDown ( int keyCode, int shift )
		{
			//MessageBox.Show ( "OnKeyDown is called !" );
		}

		public override void OnKeyUp ( int keyCode, int shift )
		{
			//MessageBox.Show ( "OnKeyUp is called !" );
		}

		public override void OnMouseDown ( int button, int shift, int x, int y )
		{
            if (_aranToolStripMenuItem.MouseDownOnMap != null)
            {
                MouseButtons mouseButton = System.Windows.Forms.MouseButtons.None;
                if (button == 1)
                    mouseButton = System.Windows.Forms.MouseButtons.Left;
                else if (button == 2)
                    mouseButton = System.Windows.Forms.MouseButtons.Right;
                else if (button == 4)
                    mouseButton = System.Windows.Forms.MouseButtons.Middle;

                IPoint pt = GlobalParams.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                MapMouseEventArg arg = new MapMouseEventArg(pt.X, pt.Y, mouseButton);
                _aranToolStripMenuItem.MouseDownOnMap(this, arg);
            }			
		}

		public override void OnMouseMove ( int button, int shift, int x, int y )
		{
			if ( _aranToolStripMenuItem.MouseMoveOnMap != null )
			{
				MouseButtons mouseButton = System.Windows.Forms.MouseButtons.None;
				if ( button == 1 )
					mouseButton = System.Windows.Forms.MouseButtons.Left;
				else if ( button == 2 )
					mouseButton = System.Windows.Forms.MouseButtons.Right;
				else if ( button == 4 )
					mouseButton = System.Windows.Forms.MouseButtons.Middle;

                IPoint pt = GlobalParams.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
				MapMouseEventArg arg = new MapMouseEventArg ( pt.X, pt.Y, mouseButton );
				_aranToolStripMenuItem.MouseMoveOnMap ( this, arg );
			}
		}

		public override void OnMouseUp ( int button, int shift, int x, int y )
		{
			if ( _aranToolStripMenuItem.MouseClickedOnMap != null )
			{
				MouseButtons mouseButton = System.Windows.Forms.MouseButtons.None;
				if ( button == 1 )
					mouseButton = System.Windows.Forms.MouseButtons.Left;
				else if ( button == 2 )
					mouseButton = System.Windows.Forms.MouseButtons.Right;
				else if ( button == 4 )
					mouseButton = System.Windows.Forms.MouseButtons.Middle;

                IPoint pt = GlobalParams.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
				MapMouseEventArg arg = new MapMouseEventArg ( pt.X, pt.Y, mouseButton );
				_aranToolStripMenuItem.MouseClickedOnMap ( this, arg );
			}			
		}

		public override void Refresh ( int hdc )
		{
		}

		#endregion

		private AranTool _aranToolStripMenuItem;
	}

}
