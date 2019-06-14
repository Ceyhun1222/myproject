Namespace Telerik.Examples.WinControls.Buttons.RepeatButton
	Partial Public Class Form1
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary>
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		Protected Overrides Sub Dispose(ByVal disposing As Boolean)
			If disposing AndAlso (components IsNot Nothing) Then
				components.Dispose()
			End If
			MyBase.Dispose(disposing)
		End Sub

		#Region "Windows Form Designer generated code"

		''' <summary>
		''' Required method for Designer support - do not modify
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
			Me.radRepeatButton3 = New Telerik.WinControls.UI.RadRepeatButton()
			Me.radProgressBar1 = New Telerik.WinControls.UI.RadProgressBar()
			CType(Me.radPanelDemoHolder, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radPanelDemoHolder.SuspendLayout()
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radRepeatButton3, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radProgressBar1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' radPanelDemoHolder
			' 
			Me.radPanelDemoHolder.Controls.Add(Me.radRepeatButton3)
			Me.radPanelDemoHolder.Controls.Add(Me.radProgressBar1)
			Me.radPanelDemoHolder.Location = New Point(0, 0)
			Me.radPanelDemoHolder.Size = New Size(219, 124)
			' 
			' settingsPanel
			' 
			Me.settingsPanel.Location = New Point(1023, 1)
			Me.settingsPanel.Size = New Size(200, 735)
			' 
			' radRepeatButton3
			' 
			Me.radRepeatButton3.Font = New Font("Microsoft Sans Serif", 8.25F)
			Me.radRepeatButton3.Location = New Point(0, 0)
			Me.radRepeatButton3.Name = "radRepeatButton3"
			Me.radRepeatButton3.Size = New Size(140, 23)
			Me.radRepeatButton3.TabIndex = 6
			Me.radRepeatButton3.Text = "Press and Hold"

			' 
			' radProgressBar1
			' 
			Me.radProgressBar1.Location = New Point(0, 41)
			Me.radProgressBar1.Name = "radProgressBar1"
			Me.radProgressBar1.SeparatorColor1 = Color.FromArgb((CInt(Fix((CByte(239))))), (CInt(Fix((CByte(239))))), (CInt(Fix((CByte(239))))))
			Me.radProgressBar1.SeparatorWidth = 4
			Me.radProgressBar1.Size = New Size(140, 23)
			Me.radProgressBar1.StepWidth = 13
			Me.radProgressBar1.TabIndex = 7
			Me.radProgressBar1.Text = "radProgressBar1"
			' 
			' Form1
			' 
			Me.Name = "Form1"
			Me.Padding = New Padding(2, 35, 2, 4)
			Me.Size = New Size(1170, 671)
			CType(Me.radPanelDemoHolder, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radPanelDemoHolder.ResumeLayout(False)
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radRepeatButton3, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radProgressBar1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private radRepeatButton3 As Telerik.WinControls.UI.RadRepeatButton
		Private radProgressBar1 As Telerik.WinControls.UI.RadProgressBar

	End Class
End Namespace