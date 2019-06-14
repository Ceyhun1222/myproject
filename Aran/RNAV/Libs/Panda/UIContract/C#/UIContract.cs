using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using ARAN.Common;
using ARAN.Contracts.Registry;
using ARAN.Contracts.GeometryOperators;
using ARAN.GeometryClasses;
using System.Windows.Forms;

namespace ARAN.Contracts.UI
{
	public enum eUICommand
	{
		uiGetEnvInfo = 0,		// AppName, AppVersion, MainWindowHandle
		uiGetRelatedFileName,

		uiGetViewProjection = 10,

		uiSetViewProjection,

		uiGetExtent,
		uiSetExtent,
		uiGetDocumentLayerList,
		uiGetDocumentMap,

		uiDisplayMessage = 20,
		uiDrawPoint,
		uiDrawPointWithText,
		uiDrawPolyline,
		uiDrawPolygon,
		uiSetVisible,
		uiDeleteGraphic,
		uiShowAnimation,


		uiCreateToolBar = 30,
		uiCreateMenuBar,
		uiCreateToolButton,
		uiCreateCommandButton,
		uiDeleteControl,

		uiControlSetStyle = 40,
		uiControlSetCategory,
		uiControlSetCaption,

		uiControlSetChecked = 50,
		uiControlSetEnabled,
		uiToolButtonSetDeactivate,
		uiToolButtonSetDown,

		uiControlSetBitmap = 60,
		uiControlSetCursor,

		uiControlSetTooltip = 70,
		uiControlSetMessage,
		uiControlSetHelpID,
		uiControlSetHelpFile,

		uiControlSetOnClickHandler = 80,
		uiControlSetOnMouseMoveHandler,
		uiControlSetOnMouseDownHandler,
		uiControlSetOnMouseUpHandler,
		uiControlSetOnDblClickHandler,

		uiToolBarAddToolButton = 90,
		uiToolBarAddCommandButton,
		uiToolBarGetVisible,
		uiToolBarSetVisible,

		uiControlOnClick = 1000,
		uiControlOnMouseMove,
		uiControlOnMouseDown,
		uiControlOnMouseUp,
		uiControlOnDblClick,
		uiControlOnKeyDown,
		uiControlOnKeyUp,

		uiControlOnDeactivate,
		uiControlOnContextMenu,

        uiCreateNewToolItem = 1200
	}

	public enum eControlStyle
	{
		csTextOnly,		// Display text only.
		csIconOnly,		// Display icon only.
		csIconAndText,	// Display icon and text.
		csMenuBar		// Display bar as main menu.
	}

	[Flags]
	public enum ToolDeactivate
	{
		tdFalse = 0,
		tdTrue = 1,
		tdAsk = 2
	}

//----------------------------------------------------------------------------
/*
	class ToolBar;
	class MenuBar;
	class ToolButton;
	class CommandButton;
	class UIControl;
 * 
	TNotifyHandler = procedure (handlerData: Pointer; sender: TObject);
	TMouseHandler = procedure (handlerData: Pointer; sender: TObject; button: TMouseButton; shift: TShiftState; x,y: integer);
*/
	public class UIMouseEventArgs: EventArgs
	{
		int	_x, _y;
		double _mx, _my;
		MouseButtons _button;

		// Summary:
		//     Initializes a new instance of the System.Windows.Forms.MouseEventArgs class.
		//
		// Parameters:
		//   button:
		//     One of the System.Windows.Forms.MouseButtons values indicating which mouse
		//     button was pressed.
		//
		//   x:
		//     The x-coordinate of a mouse click, in pixels.
		//
		//   y:
		//     The y-coordinate of a mouse click, in pixels.
		//
		//   mx:
		//     The x-coordinate of a mouse click, in map units.
		//
		//   my:
		//     The y-coordinate of a mouse click, in map units.
		public UIMouseEventArgs(MouseButtons button, int x, int y, double mx, double my)
			: base()
		{
			_button = button;
			_x = x;			_y = y;
			_mx = mx;		_my = my;
		}

		// Summary:
		//     Gets which mouse button was pressed.
		//
		// Returns:
		//     One of the System.Windows.Forms.MouseButtons values.
		public MouseButtons Button { get { return _button; } }
		//
		// Summary:
		//     Gets the location of the mouse during the generating mouse event.
		//
		// Returns:
		//     A System.Drawing.Point containing the x- and y- coordinate of the mouse,
		//     in pixels.
		public System.Drawing.Point Location { get { return new System.Drawing.Point(_x, _y); } }
		//
		// Summary:
		//     Gets the x-coordinate of the mouse during the generating mouse event.
		//
		// Returns:
		//     The x-coordinate of the mouse, in pixels.
		public int X { get { return _x; } }
		//
		// Summary:
		//     Gets the y-coordinate of the mouse during the generating mouse event.
		//
		// Returns:
		//     The y-coordinate of the mouse, in pixels.
		public int Y { get { return _y; } }
		//
		// Summary:
		//     Gets the location of the mouse during the generating mouse event.
		//
		// Returns:
		//     A System.Drawing.Point containing the x- and y- coordinate of the mouse,
		//     in pixels.
		public ARAN.GeometryClasses.Point mLocation { get { return new ARAN.GeometryClasses.Point(_mx, _my); } }
		//
		// Summary:
		//     Gets the x-coordinate of the mouse during the generating mouse event.
		//
		// Returns:
		//     The x-coordinate of the mouse, in map units.
		public double mX { get { return _mx; } }
		//
		// Summary:
		//     Gets the y-coordinate of the mouse during the generating mouse event.
		//
		// Returns:
		//     The y-coordinate of the mouse, in map units.
		public double mY { get { return _my; } }
	}

	//[SerializableAttribute]
	public delegate void UINotifyEventHandler(Object _this, Object sender);
	//[SerializableAttribute]
	public delegate void UIMouseEventHandlerE(object sender, UIMouseEventArgs e);
	public delegate void UIMouseEventHandler(Object _this, Object sender, MouseButtons button, Int32 shift, Int32 x, Int32 y, double fx, double fy);

	//----------------------------------------------------------------------------

	public class UIControl
	{
		public Int32 m_id;

		protected EventHandler m_onClick;
		protected UINotifyEventHandler m_onClickHandler;
		protected Object m_onClickHandlerData;

		protected UIContract m_uiContract;

		public Int32 ID { get { return m_id; } }
		public EventHandler OnClick { get { return m_onClick; } }
		public UINotifyEventHandler OnClickHandler { get { return m_onClickHandler; } }
		public Object OnClickHandlerData { get { return m_onClickHandlerData; } }

		public UIContract uiContract { get { return m_uiContract; } }

		protected UIControl(UIContract uiContract, Int32 id)
		{
			m_uiContract = uiContract;
			m_id = id;
			m_onClick = null;
			m_onClickHandler = null;
			m_onClickHandlerData = null;
			m_uiContract.AddControl(this);
		}

		~UIControl()
		{
			m_uiContract.RemoveControl(this);
			Int32 handle = m_uiContract.Handle;
			Registry_Contract.BeginMessage(handle, (Int32)eUICommand.uiDeleteControl);
			Registry_Contract.PutInt32(handle, m_id);
			Registry_Contract.EndMessage(handle);
		}

		public void SetStyle(eControlStyle style)
		{
			Int32 handle = m_uiContract.Handle;
			Registry_Contract.BeginMessage(handle, (Int32)eUICommand.uiControlSetStyle);
			Registry_Contract.PutInt32(handle, m_id);
			Registry_Contract.PutInt32(handle, (Int32)style);
			Registry_Contract.EndMessage(handle);
		}

		public void SetCaption(String value)
		{
			Int32 handle = m_uiContract.Handle;
			Registry_Contract.BeginMessage(handle, (Int32)eUICommand.uiControlSetCaption);
			Registry_Contract.PutInt32(handle, m_id);
			Registry_Contract.PutString(handle, value);
			Registry_Contract.EndMessage(handle);
		}

		public void SetBitmap(UIBitmap value)
		{
			Int32 handle = m_uiContract.Handle;
			Registry_Contract.BeginMessage(handle, (Int32)eUICommand.uiControlSetBitmap);
			Registry_Contract.PutInt32(handle, m_id);
			value.Pack(handle);
			Registry_Contract.EndMessage(handle);
		}

		public void SetCursor(UICursor value)
		{
			Int32 handle = m_uiContract.Handle;
			Registry_Contract.BeginMessage(handle, (Int32)eUICommand.uiControlSetCursor);
			Registry_Contract.PutInt32(handle, m_id);
			value.Pack(handle);
			Registry_Contract.EndMessage(handle);
		}

		public void SetToolTip(String value)
		{
			Int32 handle = m_uiContract.Handle;
			Registry_Contract.BeginMessage(handle, (Int32)eUICommand.uiControlSetTooltip);
			Registry_Contract.PutInt32(handle, m_id);
			Registry_Contract.PutString(handle, value);
			Registry_Contract.EndMessage(handle);
		}

		public void SetCategory(String value)
		{
			Int32 handle = m_uiContract.Handle;
			Registry_Contract.BeginMessage(handle, (Int32)eUICommand.uiControlSetCategory);
			Registry_Contract.PutInt32(handle, m_id);
			Registry_Contract.PutString(handle, value);
			Registry_Contract.EndMessage(handle);
		}

		public void SetChecked(Boolean value)
		{
			Int32 handle = m_uiContract.Handle;
			Registry_Contract.BeginMessage(handle, (Int32)eUICommand.uiControlSetChecked);
			Registry_Contract.PutInt32(handle, m_id);
			Registry_Contract.PutBool(handle, value);
			Registry_Contract.EndMessage(handle);
		}

		public void SetEnabled(Boolean value)
		{
			Int32 handle = m_uiContract.Handle;
			Registry_Contract.BeginMessage(handle, (Int32)eUICommand.uiControlSetEnabled);
			Registry_Contract.PutInt32(handle, m_id);
			Registry_Contract.PutBool(handle, value);
			Registry_Contract.EndMessage(handle);
		}

		public void SetMessage(String value)
		{
			Int32 handle = m_uiContract.Handle;
			Registry_Contract.BeginMessage(handle, (Int32)eUICommand.uiControlSetMessage);
			Registry_Contract.PutInt32(handle, m_id);
			Registry_Contract.PutString(handle, value);
			Registry_Contract.EndMessage(handle);
		}

		public void SetHelpID(Int32 value)
		{
			Int32 handle = m_uiContract.Handle;
			Registry_Contract.BeginMessage(handle, (Int32)eUICommand.uiControlSetHelpID);
			Registry_Contract.PutInt32(handle, m_id);
			Registry_Contract.PutInt32(handle, value);
			Registry_Contract.EndMessage(handle);
		}

		public void SetHelpFile(String value)
		{
			Int32 handle = m_uiContract.Handle;
			Registry_Contract.BeginMessage(handle, (Int32)eUICommand.uiControlSetHelpFile);
			Registry_Contract.PutInt32(handle, m_id);
			Registry_Contract.PutString(handle, value);
			Registry_Contract.EndMessage(handle);
		}

		public void SetOnClickHandler(EventHandler handler)
		{
			m_onClick = handler;
			Int32 handle = m_uiContract.Handle;

			Registry_Contract.BeginMessage(handle, (Int32)eUICommand.uiControlSetOnClickHandler);
			Registry_Contract.PutInt32(handle, m_id);
			Registry_Contract.PutBool(handle, handler != null);
			Registry_Contract.EndMessage(handle);
		}

		public void SetOnClickHandler(Object handlerData, UINotifyEventHandler handler)
		{
			m_onClickHandlerData = handlerData;
			m_onClickHandler = handler;
			Int32 handle = m_uiContract.Handle;

			Registry_Contract.BeginMessage(handle, (Int32)eUICommand.uiControlSetOnClickHandler);
			Registry_Contract.PutInt32(handle, m_id);
			Registry_Contract.PutBool(handle, handler != null);
			Registry_Contract.EndMessage(handle);
		}
	}

	public class ToolButton : UIControl
	{
		protected UIMouseEventHandlerE m_onMouseMove;
		protected UIMouseEventHandler m_onMouseMoveHandler;
		protected Object m_onMouseMoveHandlerData;

		protected UIMouseEventHandlerE m_onMouseDown;
		protected UIMouseEventHandler m_onMouseDownHandler;
		protected Object m_onMouseDownHandlerData;

		protected UIMouseEventHandlerE m_onMouseUp;
		protected UIMouseEventHandler m_onMouseUpHandler;
		protected Object m_onMouseUpHandlerData;

		protected EventHandler m_onDblClick;
		protected UINotifyEventHandler m_onDblClickHandler;
		protected Object m_onDblClickHandlerData;

		public UIMouseEventHandlerE OnMouseMove { get { return m_onMouseMove; } }
		public UIMouseEventHandler OnMouseMoveHandler { get { return m_onMouseMoveHandler; } }
		public Object OnMouseMoveHandlerData { get { return m_onMouseMoveHandlerData; } }

		public UIMouseEventHandlerE OnMouseDown { get { return m_onMouseDown; } }
		public UIMouseEventHandler OnMouseDownHandler { get { return m_onMouseDownHandler; } }
		public Object OnMouseDownHandlerData { get { return m_onMouseDownHandlerData; } }

		public UIMouseEventHandlerE OnMouseUp { get { return m_onMouseUp; } }
		public UIMouseEventHandler OnMouseUpHandler { get { return m_onMouseUpHandler; } }
		public Object OnMouseUpHandlerData { get { return m_onMouseUpHandlerData; } }

		public EventHandler OnDblClick { get { return m_onDblClick; } }
		public UINotifyEventHandler OnDblClickHandler { get { return m_onDblClickHandler; } }
		public Object OnDblClickHandlerData { get { return m_onDblClickHandlerData; } }

		public ToolButton(UIContract uiContract, Int32 id)
			: base(uiContract, id)
		{
			m_onMouseMove = null;
			m_onMouseMoveHandler = null;
			m_onMouseMoveHandlerData = null;

			m_onMouseDown = null;
			m_onMouseDownHandler = null;
			m_onMouseDownHandlerData = null;

			m_onMouseUp = null;
			m_onMouseUpHandler = null;
			m_onMouseUpHandlerData = null;

			m_onDblClick = null;
			m_onDblClickHandler = null;
			m_onDblClickHandlerData = null;
		}

		public void SetDeactivate(ToolDeactivate value)
		{
			Int32 handle = m_uiContract.Handle;
			Registry_Contract.BeginMessage(handle, (Int32)eUICommand.uiToolButtonSetDeactivate);
			Registry_Contract.PutInt32(handle, m_id);
			Registry_Contract.PutInt32(handle, (Int32)value);
			Registry_Contract.EndMessage(handle);
		}

		public void SetDownState(bool DownState)
		{
			Int32 handle = m_uiContract.Handle;
			Registry_Contract.BeginMessage(handle, (Int32)eUICommand.uiToolButtonSetDown);
			Registry_Contract.PutInt32(handle, m_id);
			Registry_Contract.PutBool(handle, DownState);
			Registry_Contract.EndMessage(handle);
		}

		public void SetOnMouseMoveHandler(Object handlerData, UIMouseEventHandler handler)
		{
			m_onMouseMoveHandlerData = handlerData;
			m_onMouseMoveHandler = handler;
			Int32 handle = m_uiContract.Handle;
			Registry_Contract.BeginMessage(handle, (Int32)eUICommand.uiControlSetOnMouseMoveHandler);
			Registry_Contract.PutInt32(handle, m_id);
			Registry_Contract.PutBool(handle, handler != null);
			Registry_Contract.EndMessage(handle);
		}

		public void SetOnMouseMoveHandler(UIMouseEventHandlerE handler)
		{
			m_onMouseMove = handler;
			Int32 handle = m_uiContract.Handle;
			Registry_Contract.BeginMessage(handle, (Int32)eUICommand.uiControlSetOnMouseMoveHandler);
			Registry_Contract.PutInt32(handle, m_id);
			Registry_Contract.PutBool(handle, handler != null);
			Registry_Contract.EndMessage(handle);
		}
		///
		public void SetOnMouseDownHandler(Object handlerData, UIMouseEventHandler handler)
		{
			m_onMouseDownHandlerData = handlerData;
			m_onMouseDownHandler = handler;
			Int32 handle = m_uiContract.Handle;
			Registry_Contract.BeginMessage(handle, (Int32)eUICommand.uiControlSetOnMouseDownHandler);
			Registry_Contract.PutInt32(handle, m_id);
			Registry_Contract.PutBool(handle, handler != null);
			Registry_Contract.EndMessage(handle);
		}

		public void SetOnMouseDownHandler(UIMouseEventHandlerE handler)
		{
			m_onMouseDown = handler;
			Int32 handle = m_uiContract.Handle;
			Registry_Contract.BeginMessage(handle, (Int32)eUICommand.uiControlSetOnMouseDownHandler);
			Registry_Contract.PutInt32(handle, m_id);
			Registry_Contract.PutBool(handle, handler != null);
			Registry_Contract.EndMessage(handle);
		}
		///
		public void SetOnMouseUpHandler(Object handlerData, UIMouseEventHandler handler)
		{
			m_onMouseUpHandlerData = handlerData;
			m_onMouseUpHandler = handler;
			Int32 handle = m_uiContract.Handle;
			Registry_Contract.BeginMessage(handle, (Int32)eUICommand.uiControlSetOnMouseUpHandler);
			Registry_Contract.PutInt32(handle, m_id);
			Registry_Contract.PutBool(handle, handler != null);
			Registry_Contract.EndMessage(handle);
		}

		public void SetOnMouseUpHandler(UIMouseEventHandlerE handler)
		{
			m_onMouseUp = handler;
			Int32 handle = m_uiContract.Handle;
			Registry_Contract.BeginMessage(handle, (Int32)eUICommand.uiControlSetOnMouseUpHandler);
			Registry_Contract.PutInt32(handle, m_id);
			Registry_Contract.PutBool(handle, handler != null);
			Registry_Contract.EndMessage(handle);
		}
		///
		public void SetOnDblClickHandler(Object handlerData, UINotifyEventHandler handler)
		{
			m_onDblClickHandlerData = handlerData;
			m_onDblClickHandler = handler;
			Int32 handle = m_uiContract.Handle;

			Registry_Contract.BeginMessage(handle, (Int32)eUICommand.uiControlSetOnDblClickHandler);
			Registry_Contract.PutInt32(handle, m_id);
			Registry_Contract.PutBool(handle, handler != null);
			Registry_Contract.EndMessage(handle);
		}

		public void SetOnDblClickHandler(EventHandler handler)
		{
			m_onDblClick = handler;
			Int32 handle = m_uiContract.Handle;

			Registry_Contract.BeginMessage(handle, (Int32)eUICommand.uiControlSetOnDblClickHandler);
			Registry_Contract.PutInt32(handle, m_id);
			Registry_Contract.PutBool(handle, handler != null);
			Registry_Contract.EndMessage(handle);
		}
	}

	public class CommandButton : UIControl
	{
		public CommandButton(UIContract uiContract, Int32 id)
			: base(uiContract, id)
		{
		}
	}

	public class ToolBar : UIControl
	{
		public ToolBar(UIContract uiContract, Int32 id)
			: base(uiContract, id)
		{
		}

		public void AddToolButton(ToolButton toolButton)
		{
			Int32 handle = m_uiContract.Handle;
			Registry_Contract.BeginMessage(handle, (Int32)eUICommand.uiToolBarAddToolButton);
			Registry_Contract.PutInt32(handle, m_id);
			Registry_Contract.PutInt32(handle, toolButton.ID);
			Registry_Contract.EndMessage(handle);
		}

		public void AddCommandButton(CommandButton commandButton)
		{
			Int32 handle = m_uiContract.Handle;
			Registry_Contract.BeginMessage(handle, (Int32)eUICommand.uiToolBarAddCommandButton);
			Registry_Contract.PutInt32(handle, m_id);
			Registry_Contract.PutInt32(handle, commandButton.ID);
			Registry_Contract.EndMessage(handle);
		}

		public Boolean Visible
		{
			get
			{
				Int32	handle = m_uiContract.Handle;
				Registry_Contract.BeginMessage(handle, (Int32)eUICommand.uiToolBarGetVisible);
				Registry_Contract.PutInt32(handle, m_id);
				Registry_Contract.EndMessage(handle);
				return Registry_Contract.GetBool(handle);
			}
			set
			{
				Int32	handle = m_uiContract.Handle;
				Registry_Contract.BeginMessage(handle, (Int32)eUICommand.uiToolBarSetVisible);
				Registry_Contract.PutInt32(handle, m_id);
				Registry_Contract.PutBool(handle, value);
				Registry_Contract.EndMessage(handle);
			}
		}
	}

	public class MenuBar : UIControl
	{
		public MenuBar(UIContract uiContract, Int32 id)
			: base(uiContract, id)
		{
		}
	}

//====================================================================???????????????

	//[SerializableAttribute]
	public delegate Int32 EventDelegate(UIContract privateData, Int32 command, Int32 inout);

	public class UIContract
	{
		private GCHandle		m_gcHandle;
		private Int32			FHandle;
		private List<UIControl>	FControls;

		private UIControl FindControl (Int32 id)
		{
			return FControls.Find(delegate(UIControl ctrl) { return ctrl.m_id == id; });
		}

        #region

        Registry_Contract.Method onEvent = delegate(Int32 sender, Int32 command, Int32 inout)
		{
			GCHandle handle = GCHandle.FromIntPtr(new IntPtr(sender));
			Object _this = (Object)handle.Target;

			Int32 result, id, shift, x, y;
			double fx, fy;
			Int32 sh = 0;
			MouseButtons button;

			try
			{
				UIContract uiContract;

				result = (Int32)Registry_Contract.rcOK;
				uiContract = (UIContract)_this;

				switch ((eUICommand)command)
				{
					case eUICommand.uiControlOnClick:
						id = Registry_Contract.GetInt32(inout);
						UIControl control = uiContract.FindControl(id);
						if (control == null) return result;
						if (control.OnClick != null)
							control.OnClick(control, new EventArgs());
						if (control.OnClickHandler != null)
							control.OnClickHandler(control.OnClickHandlerData, control);
						break;
					case eUICommand.uiControlOnMouseMove:
						id = Registry_Contract.GetInt32(inout);
						button = (MouseButtons)Registry_Contract.GetInt32(inout);
						shift = Registry_Contract.GetInt32(inout);
						x = Registry_Contract.GetInt32(inout);
						y = Registry_Contract.GetInt32(inout);
						fx = Registry_Contract.GetDouble(inout);
						fy = Registry_Contract.GetDouble(inout);
						ToolButton toolButton = (ToolButton)uiContract.FindControl(id);
						if (toolButton == null) return result;
						//		  ShiftState set of 
						//			(ssShift, ssAlt, ssCtrl, ssLeft, ssRight, ssMiddle, ssDouble);

						if ((shift & 1) != 0) sh = sh + 1;
						if ((shift & 2) != 0) sh = sh + 2;
						if ((shift & 4) != 0) sh = sh + 4;

						if (toolButton.OnMouseMove != null)
							toolButton.OnMouseMove(toolButton, new UIMouseEventArgs(button, x, y, fx, fy)); //button, sh, x, y
						if (toolButton.OnMouseMoveHandler != null)
							toolButton.OnMouseMoveHandler(toolButton.OnMouseMoveHandlerData, toolButton, button, sh, x, y, fx, fy);
						break;
					case eUICommand.uiControlOnMouseDown:
						id = Registry_Contract.GetInt32(inout);
						button = (MouseButtons)Registry_Contract.GetInt32(inout);
						shift = Registry_Contract.GetInt32(inout);
						x = Registry_Contract.GetInt32(inout);
						y = Registry_Contract.GetInt32(inout);
						fx = Registry_Contract.GetDouble(inout);
						fy = Registry_Contract.GetDouble(inout);
						toolButton = (ToolButton)uiContract.FindControl(id);
						if (toolButton == null) return result;
						//		  ShiftState set of 
						//			(ssShift, ssAlt, ssCtrl, ssLeft, ssRight, ssMiddle, ssDouble);

						sh = shift & 0xf;

						//if ((shift & 1) != 0) sh = sh + 1;
						//if ((shift & 2) != 0) sh = sh + 2;
						//if ((shift & 4) != 0) sh = sh + 4;

						if (toolButton.OnMouseDown != null)
							toolButton.OnMouseDown(toolButton, new UIMouseEventArgs(button, x, y, fx, fy)); //button, sh, x, y
						if (toolButton.OnMouseDownHandler != null)
							toolButton.OnMouseDownHandler(toolButton.OnMouseDownHandlerData, toolButton, button, sh, x, y, fx, fy);
						break;
					case eUICommand.uiControlOnMouseUp:
						id = Registry_Contract.GetInt32(inout);
						button = (MouseButtons)Registry_Contract.GetInt32(inout);
						shift = Registry_Contract.GetInt32(inout);
						x = Registry_Contract.GetInt32(inout);
						y = Registry_Contract.GetInt32(inout);
						fx = Registry_Contract.GetDouble(inout);
						fy = Registry_Contract.GetDouble(inout);
						toolButton = (ToolButton)uiContract.FindControl(id);
						if (toolButton == null) return result;
						//		  ShiftState set of 
						//			(ssShift, ssAlt, ssCtrl, ssLeft, ssRight, ssMiddle, ssDouble);

						if ((shift & 1) != 0) sh = sh + 1;
						if ((shift & 2) != 0) sh = sh + 2;
						if ((shift & 4) != 0) sh = sh + 4;

						if (toolButton.OnMouseUp != null)
							toolButton.OnMouseUp(toolButton, new UIMouseEventArgs( button, x, y, fx, fy)); //button, sh, x, y
						if (toolButton.OnMouseUpHandler != null)
							toolButton.OnMouseUpHandler(toolButton.OnMouseUpHandlerData, toolButton, button, sh, x, y, fx, fy);
						break;
					case eUICommand.uiControlOnDblClick:
						id = Registry_Contract.GetInt32(inout);
						toolButton = (ToolButton)uiContract.FindControl(id);
						if (toolButton == null) return result;
						if (toolButton.OnDblClick != null)
							toolButton.OnDblClick(toolButton, new EventArgs());
						if (toolButton.OnDblClickHandler != null)
							toolButton.OnDblClickHandler(toolButton.OnDblClickHandlerData, toolButton);
						break;
				}
			}
			catch
			{
				return (Int32)Registry_Contract.rcException;
			}
			return result;
		};

        #endregion

		private static int ControlsCompareFunction(UIControl control1, UIControl control2)
		{
			if(control1.m_id < control2.m_id)	return -1;
			if(control1.m_id > control2.m_id)	return 1;
			return 0;
		}
		//------------------------------------------------------------------------------

		private Int32 CreateControl (String name, eUICommand command)
		{
			Registry_Contract.BeginMessage (FHandle, (Int32)command);
			Registry_Contract.PutString (FHandle, name);
			Registry_Contract.EndMessage (FHandle);
			return Registry_Contract.GetInt32(FHandle);
		}

		public void AddControl (UIControl control)
		{
			FControls.Add(control);
			FControls.Sort(ControlsCompareFunction);
		}

		public void RemoveControl (UIControl control)
		{
			FControls.Remove(control);
		}

		private SpatialReference GetViewProjection()
		{
			Registry_Contract.BeginMessage(FHandle, (Int32)eUICommand.uiGetViewProjection);
			Registry_Contract.EndMessage(FHandle);

            bool notNull = Registry_Contract.GetBool (FHandle);
            if (notNull)
            {
                SpatialReference result = new SpatialReference ();
                result.UnPack (FHandle);
                return result;
            }
            return null;
		}

		private void SetViewProjection (SpatialReference spatialReference)
		{
			Registry_Contract.BeginMessage(FHandle, (Int32)eUICommand.uiSetViewProjection);
			spatialReference.Pack(FHandle);
			Registry_Contract.EndMessage(FHandle);
		}

		private UInt32 GetDocumentMap()
		{
			Registry_Contract.BeginMessage(FHandle, (Int32)eUICommand.uiGetDocumentMap);
			Registry_Contract.EndMessage (FHandle);
			return (UInt32)(Registry_Contract.GetInt32(FHandle));
		}

		public UIContract()
		{
			FHandle = 0;
			FControls = new List <UIControl>();

			FHandle = Registry_Contract.GetInstance("UIService");
			m_gcHandle = Registry_Contract.SetHandler(FHandle, this, onEvent);
		}

		~UIContract()
		{
			Registry_Contract.FreeInstance(FHandle);
			m_gcHandle.Free();
		}

		public Boolean IsValid()
		{
			return	FHandle != 0;
		}

		public void GetEnvInfo(out String ProgName, out Int32 info0, out Int32 info1, out Int32 info2, out Int32 info3)
		{
			Registry_Contract.BeginMessage(FHandle, (Int32)eUICommand.uiGetEnvInfo);
			Registry_Contract.EndMessage (FHandle);
			ProgName = Registry_Contract.GetString(FHandle);
			info0 = Registry_Contract.GetInt32(FHandle);
			info1 = Registry_Contract.GetInt32(FHandle);
			info2 = Registry_Contract.GetInt32(FHandle);
			info3 = Registry_Contract.GetInt32(FHandle);
		}

		public String GetRelatedFileName()
		{
			Registry_Contract.BeginMessage (FHandle, (Int32)eUICommand.uiGetRelatedFileName);
			Registry_Contract.EndMessage (FHandle);
			return Registry_Contract.GetString(FHandle);
		}

		public void DisplayMessage(Int32 pos, String msg)
		{
			Registry_Contract.BeginMessage(FHandle, (Int32)eUICommand.uiDisplayMessage);
			Registry_Contract.PutInt32(FHandle, pos);
			Registry_Contract.PutString(FHandle, msg);
			Registry_Contract.EndMessage(FHandle);
		}

		public void GetExtent (out double xmin, out double ymin, out double xmax, out double ymax)
		{
			Registry_Contract.BeginMessage(FHandle, (Int32)eUICommand.uiGetExtent);
			Registry_Contract.EndMessage(FHandle);
			xmin = Registry_Contract.GetDouble(FHandle);
			ymin = Registry_Contract.GetDouble(FHandle);
			xmax = Registry_Contract.GetDouble(FHandle);
			ymax = Registry_Contract.GetDouble(FHandle);
		}

		public void SetExtent(double xmin, double ymin, double xmax, double ymax)
		{
			Registry_Contract.BeginMessage(FHandle, (Int32)eUICommand.uiSetExtent);
			Registry_Contract.PutDouble(FHandle, xmin);
			Registry_Contract.PutDouble(FHandle, ymin);
			Registry_Contract.PutDouble(FHandle, xmax);
			Registry_Contract.PutDouble(FHandle, ymax);
			Registry_Contract.EndMessage(FHandle);
		}

		public Int32 DrawPoint(Point point, PointSymbol symbol)
		{
			Registry_Contract.BeginMessage(FHandle, (Int32)eUICommand.uiDrawPoint);
			point.Pack (FHandle);
			symbol.Pack (FHandle);
			Registry_Contract.EndMessage(FHandle);
			return Registry_Contract.GetInt32(FHandle);
		}

		public Int32 DrawPoint(Point point, Int32 color)
		{
			PointSymbol symbol = new PointSymbol(color, 8);

			Registry_Contract.BeginMessage(FHandle, (Int32)eUICommand.uiDrawPoint);
			point.Pack(FHandle);
			symbol.Pack(FHandle);
			Registry_Contract.EndMessage(FHandle);
			return Registry_Contract.GetInt32(FHandle);
		}

		public Int32 DrawPoint(Point point, Int32 color, ePointStyle style)
		{
			PointSymbol symbol = new PointSymbol(style, color, 8);

			Registry_Contract.BeginMessage(FHandle, (Int32)eUICommand.uiDrawPoint);
			point.Pack(FHandle);
			symbol.Pack(FHandle);
			Registry_Contract.EndMessage(FHandle);
			return Registry_Contract.GetInt32(FHandle);
		}

		public Int32 DrawPointWithText(Point point, PointSymbol symbol, String text)
		{
			Registry_Contract.BeginMessage(FHandle, (Int32)eUICommand.uiDrawPointWithText);
			point.Pack(FHandle);
			symbol.Pack(FHandle);
			Registry_Contract.PutString(FHandle, text);
			Registry_Contract.EndMessage(FHandle);
			return Registry_Contract.GetInt32(FHandle);
		}

		public Int32 DrawPointWithText(Point point, Int32 color, String text)
		{
			PointSymbol	symbol = new PointSymbol(color, 8);

			Registry_Contract.BeginMessage(FHandle, (Int32)eUICommand.uiDrawPointWithText);
			point.Pack(FHandle);
			symbol.Pack(FHandle);
			Registry_Contract.PutString(FHandle, text);
			Registry_Contract.EndMessage(FHandle);
			return Registry_Contract.GetInt32(FHandle);
		}

		public Int32 DrawPolyline(PolyLine polyline, LineSymbol symbol)
		{
			Registry_Contract.BeginMessage(FHandle, (Int32)eUICommand.uiDrawPolyline);
			polyline.Pack(FHandle);
			symbol.Pack(FHandle);
			Registry_Contract.EndMessage(FHandle);
			return Registry_Contract.GetInt32(FHandle);
		}

		public Int32 DrawPolyline(PolyLine polyline, Int32 color, Int32 Width)
		{
			LineSymbol symbol = new LineSymbol(eLineStyle.slsSolid, color, Width);

			Registry_Contract.BeginMessage(FHandle, (Int32)eUICommand.uiDrawPolyline);
			polyline.Pack(FHandle);
			symbol.Pack(FHandle);
			Registry_Contract.EndMessage(FHandle);
			return Registry_Contract.GetInt32(FHandle);
		}

		public Int32 DrawPart(Part part, Int32 color, Int32 width)
		{
			PolyLine polyLine = new PolyLine();
			polyLine.Add(part);
			return DrawPolyline(polyLine, color, width);
		}

		public Int32 DrawPart(Part part, LineSymbol symbol)
		{
			PolyLine polyLine = new PolyLine();
			polyLine.Add(part);
			return DrawPolyline(polyLine, symbol);
		}
				

		public Int32 DrawPolygon(Polygon polygon, FillSymbol symbol)
		{
			Registry_Contract.BeginMessage(FHandle, (Int32)eUICommand.uiDrawPolygon);
			polygon.Pack (FHandle);
			symbol.Pack (FHandle);
			Registry_Contract.EndMessage(FHandle);
			return Registry_Contract.GetInt32(FHandle);
		}

		public Int32 DrawPolygon(Polygon polygon, Int32 color, eFillStyle style)
		{
			FillSymbol symbol = new FillSymbol();
			symbol.Color = color;
			symbol.Style = style;
			symbol.Outline.Color = color;

			Registry_Contract.BeginMessage(FHandle, (Int32)eUICommand.uiDrawPolygon);
			polygon.Pack(FHandle);
			symbol.Pack(FHandle);
			Registry_Contract.EndMessage(FHandle);
			return Registry_Contract.GetInt32(FHandle);
		}

		public Int32 DrawRing(Ring ring, Int32 color, eFillStyle style)
		{
			Polygon polygon = new Polygon();
			polygon.Add(ring);
			return DrawPolygon(polygon, color, style);
		}

		public Int32 DrawRing(Ring ring, FillSymbol symbol)
		{
			Polygon polygon = new Polygon();
			polygon.Add(ring);
			return DrawPolygon(polygon, symbol);
		}

		public void SetVisible(Int32 graphicHandle, Boolean isVisible)
		{
			Registry_Contract.BeginMessage(FHandle, (Int32)eUICommand.uiSetVisible);
			Registry_Contract.PutInt32(FHandle, graphicHandle);
			Registry_Contract.EndMessage(FHandle);
		}

		public Int32 DeleteGraphic(Int32 Handle)
		{
			Registry_Contract.BeginMessage(FHandle, (Int32)eUICommand.uiDeleteGraphic);
			Registry_Contract.PutInt32(FHandle, Handle);
			Registry_Contract.EndMessage(FHandle);
			//	return  Registry_EndMessage (handle);
			return 0;
		}

		public void ShowAnimation(Boolean show)
		{
			Registry_Contract.BeginMessage(FHandle, (Int32)eUICommand.uiShowAnimation);
			Registry_Contract.PutBool(FHandle, show);
			Registry_Contract.EndMessage(FHandle);
		}

		public void HideAnimation()
		{
			ShowAnimation(false);
		}

		public void ShowAnimation()
		{
			ShowAnimation(true);
		}

		public Int32 SafeDeleteGraphic(ref Int32 handle)
		{
			if( handle != -1)
			{
				try
				{
					Registry_Contract.BeginMessage(FHandle, (Int32)eUICommand.uiDeleteGraphic);
					Registry_Contract.PutInt32(FHandle, handle);
					Registry_Contract.EndMessage(FHandle);
				}
				catch{}
			}
			handle = -1;
			return 0;
		}

		public ToolBar CreateToolBar(String name)
		{
			Int32 id = CreateControl(name, eUICommand.uiCreateToolBar);
			return new ToolBar(this, id);
		}

		public MenuBar CreateMenuBar(String name)
		{
			Int32 id = CreateControl(name, eUICommand.uiCreateMenuBar);
			return new MenuBar(this, id);
		}

		public ToolButton CreateToolButton(String name)
		{
			Int32 id = CreateControl(name, eUICommand.uiCreateToolButton);
			return new ToolButton(this, id);
		}

		public CommandButton CreateCommandButton(String name)
		{
			Int32 id = CreateControl(name, eUICommand.uiCreateCommandButton);
			return new CommandButton(this, id);
		}

		public List<String> GetDocumentLayerList()
		{
			Registry_Contract.BeginMessage(FHandle, (Int32)eUICommand.uiGetDocumentLayerList);
			Registry_Contract.EndMessage(FHandle);
			return Registry_Contract.GetStringArray(FHandle);
		}

		public static Int32 RGB(int r,  int g, int b)
		{
			return r | (g << 8 ) | (b << 16);
		}

		public SpatialReference ViewProjection
		{
			get	{	return GetViewProjection();	}
			set	{	SetViewProjection(value);	}
		}

		public UInt32 DocumentMap
		{
			get	{	return GetDocumentMap();	}
		}

		public Int32 Handle
		{
			get	{	return FHandle;	}
		}

	}

}
