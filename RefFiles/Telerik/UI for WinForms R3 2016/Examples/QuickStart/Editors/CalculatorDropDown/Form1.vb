Imports Telerik.Examples.WinControls.Editors.ComboBox

Namespace Telerik.Examples.WinControls.Editors.CalculatorDropDown

    Partial Public Class Form1
        Inherits EditorExampleBaseForm
        ''' <summary>
        ''' 
        ''' </summary>
        Public Sub New()
            InitializeComponent()
            Me.SelectedControl = Me.radCalculatorDropDown1
        End Sub

        Protected Overrides Sub OnLoad(ByVal e As EventArgs)
            MyBase.OnLoad(e)

            Me.radCalculatorDropDown1.CalculatorElement.ShowPopup()
        End Sub
    End Class
End Namespace