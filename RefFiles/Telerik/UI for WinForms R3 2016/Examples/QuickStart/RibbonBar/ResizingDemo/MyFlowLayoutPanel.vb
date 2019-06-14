Imports System.Text

Namespace Telerik.Examples.WinControls.RibbonBar.ResizingDemo
	Friend Class MyFlowLayoutPanel
		Inherits FlowLayoutPanel
		Public Sub New()
			Me.SetStyle(ControlStyles.DoubleBuffer, True)
			Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
			Me.SetStyle(ControlStyles.UserPaint, True)
			Me.SetStyle(ControlStyles.SupportsTransparentBackColor Or ControlStyles.OptimizedDoubleBuffer, True)
		End Sub
	End Class
End Namespace
