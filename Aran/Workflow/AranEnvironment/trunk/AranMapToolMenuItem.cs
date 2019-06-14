using System.Windows.Forms;

namespace Aran.AranEnvironment
{
	public class AranTool : ToolStripMenuItem
	{
		public AranTool ( )
		{
		}

		public Cursor Cursor
		{
			get;
			set;
		}

		public MouseClickedOnMapEventHandler MouseClickedOnMap;
        public MouseClickedOnMapEventHandler MouseDownOnMap;
		public MouseMoveOnMapEventHandler MouseMoveOnMap;
        public MouseDblClickOnMapEventHandler MouseOnDblClickOnMap;
        public MouseRightlClickOnMapEventHandler MouseOnRightClickOnMap;
		public DeactivateEventHandler Deactivated;
	}

	public class MapMouseEventArg
	{
		public MapMouseEventArg ( )
		{

		}

		public MapMouseEventArg ( double x, double y )
		{
			X = x;
			Y = y;
		}

		public MapMouseEventArg ( double x, double y, MouseButtons mouseButton )
			: this ( x, y )
		{
			Button = mouseButton;
		}

		public MouseButtons Button
		{
			get;
			set;
		}

		public double X
		{
			get;
			set;
		}

		public double Y
		{
			get;
			set;
		}
	}

	public delegate void MouseClickedOnMapEventHandler ( object sender, MapMouseEventArg arg );
	public delegate void MouseMoveOnMapEventHandler ( object sender, MapMouseEventArg arg );
    public delegate void MouseDblClickOnMapEventHandler();
    public delegate void MouseRightlClickOnMapEventHandler();
	public delegate void DeactivateEventHandler ( );
}