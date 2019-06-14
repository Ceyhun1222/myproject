Imports System.Text
Imports Telerik.WinControls.UI

Namespace Telerik.Examples.WinControls.TreeView
	Public Class TreeExampleHeaderPanel
		Inherits RadPanel
		Protected Overrides Sub OnLoad(ByVal desiredSize As Size)
			MyBase.OnLoad(desiredSize)

			'this.PanelElement.PanelFill.BackColor = Color.FromArgb(220, 238, 253);
			'this.PanelElement.PanelFill.BackColor2 = Color.FromArgb(170, 207, 253);
			'this.PanelElement.PanelFill.NumberOfColors = 2;
			'this.PanelElement.PanelFill.GradientStyle = Telerik.WinControls.GradientStyles.Linear;
			'this.PanelElement.PanelBorder.BoxStyle = Telerik.WinControls.BorderBoxStyle.FourBorders;
			'this.PanelElement.PanelBorder.TopColor = Color.FromArgb(61, 107, 192);
			'this.PanelElement.PanelBorder.LeftColor = Color.FromArgb(61, 107, 192);
			'this.PanelElement.PanelBorder.RightColor = Color.FromArgb(61, 107, 192);
			'this.PanelElement.PanelBorder.BottomColor = Color.FromArgb(187, 217, 253);
			'this.PanelElement.PanelBorder.TopShadowColor = Color.FromArgb(228,242,253);
			'this.PanelElement.PanelBorder.LeftShadowColor = Color.FromArgb(211,232,253);
			'this.PanelElement.PanelBorder.RightShadowColor = Color.FromArgb(211,232,253);
			'this.PanelElement.PanelBorder.LeftWidth = 1;
			'this.PanelElement.PanelBorder.TopWidth = 1;
			'this.PanelElement.PanelBorder.RightWidth = 1;
			'this.PanelElement.PanelBorder.BottomWidth = 1;
			Me.PanelElement.Font = New Font("Segoe UI", 10, FontStyle.Bold)
			Me.PanelElement.Padding = New Padding(8, 4, 2, 2)
			'this.PanelElement.PanelBorder.BorderDrawMode = Telerik.WinControls.BorderDrawModes.VerticalOverHorizontal;
		End Sub

        Public Overrides Property ThemeClassName() As String
            Get
                Return GetType(RadPanel).FullName
            End Get

            Set(value As String)

            End Set
        End Property

	End Class
End Namespace
