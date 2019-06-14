Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms

Namespace Telerik.Examples.WinControls.Tools.VisualStyleBuilder
    Partial Public Class LaunchForm
        Inherits Telerik.QuickStart.WinControls.CustomThemeExamplesLauncherForm
        Public Sub New()
            InitializeComponent()

            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(LaunchForm))
            Me.pictureBoxLaunchExample.ButtonImage = CType(resources.GetObject("pictureBoxLaunchExample.Image"), System.Drawing.Image)
        End Sub
    End Class
End Namespace