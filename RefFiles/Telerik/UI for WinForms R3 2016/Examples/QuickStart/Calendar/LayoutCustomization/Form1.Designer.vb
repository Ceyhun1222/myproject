Namespace Telerik.Examples.WinControls.Calendar.LayoutCustomization
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
			Me.radCalendar1 = New Telerik.WinControls.UI.RadCalendar()
			Me.radGroupDisplay = New Telerik.WinControls.UI.RadGroupBox()
			Me.radRadio7Cols = New Telerik.WinControls.UI.RadRadioButton()
			Me.radRadio21Cols = New Telerik.WinControls.UI.RadRadioButton()
			Me.radRadio14Cols = New Telerik.WinControls.UI.RadRadioButton()
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.settingsPanel.SuspendLayout()
			CType(Me.radCalendar1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radGroupDisplay, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radGroupDisplay.SuspendLayout()
			CType(Me.radRadio7Cols, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radRadio21Cols, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radRadio14Cols, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' settingsPanel
			' 
			Me.settingsPanel.Controls.Add(Me.radGroupDisplay)
			Me.settingsPanel.Location = New Point(729, 1)
			Me.settingsPanel.Size = New Size(250, 867)
			Me.settingsPanel.Controls.SetChildIndex(Me.radGroupDisplay, 0)
			' 
			' radCalendar1
			' 
			Me.radCalendar1.BackColor = Color.FromArgb((CInt(Fix((CByte(248))))), (CInt(Fix((CByte(248))))), (CInt(Fix((CByte(248))))))
			Me.radCalendar1.FocusedDate = New Date(2013, 3, 13, 0, 0, 0, 0)
			Me.radCalendar1.ForeColor = Color.Black
			Me.radCalendar1.Location = New Point(0, 0)
			Me.radCalendar1.Name = "radCalendar1"
			Me.radCalendar1.Size = New Size(215, 196)
			Me.radCalendar1.TabIndex = 0
			Me.radCalendar1.Text = "radCalendar1"
			Me.radCalendar1.ZoomFactor = 1.2F
			' 
			' radGroupDisplay
			' 
			Me.radGroupDisplay.AccessibleRole = AccessibleRole.Grouping
			Me.radGroupDisplay.Controls.Add(Me.radRadio7Cols)
			Me.radGroupDisplay.Controls.Add(Me.radRadio21Cols)
			Me.radGroupDisplay.Controls.Add(Me.radRadio14Cols)
			Me.radGroupDisplay.HeaderText = "Display Options"
			Me.radGroupDisplay.Location = New Point(15, 6)
			Me.radGroupDisplay.Name = "radGroupDisplay"
			' 
			' 
			' 
			Me.radGroupDisplay.RootElement.Padding = New Padding(2, 18, 2, 2)
			Me.radGroupDisplay.Size = New Size(162, 116)
			Me.radGroupDisplay.TabIndex = 2
			Me.radGroupDisplay.Text = "Display Options"
			' 
			' radRadio7Cols
			' 
			Me.radRadio7Cols.Location = New Point(17, 34)
			Me.radRadio7Cols.Name = "radRadio7Cols"
			Me.radRadio7Cols.Size = New Size(123, 18)
			Me.radRadio7Cols.TabIndex = 3
			Me.radRadio7Cols.Text = "7 columns by 6 rows"

			' 
			' radRadio21Cols
			' 
			Me.radRadio21Cols.Location = New Point(17, 78)
			Me.radRadio21Cols.Name = "radRadio21Cols"
			Me.radRadio21Cols.Size = New Size(129, 18)
			Me.radRadio21Cols.TabIndex = 3
			Me.radRadio21Cols.Text = "21 coulmns by 2 rows"

			' 
			' radRadio14Cols
			' 
			Me.radRadio14Cols.Location = New Point(17, 56)
			Me.radRadio14Cols.Name = "radRadio14Cols"
			Me.radRadio14Cols.Size = New Size(129, 18)
			Me.radRadio14Cols.TabIndex = 3
			Me.radRadio14Cols.Text = "14 coulmns by 3 rows"

			' 
			' Form1
			' 
			Me.Controls.Add(Me.radCalendar1)
			Me.Name = "Form1"
			Me.Size = New Size(1170, 754)
			Me.Controls.SetChildIndex(Me.radCalendar1, 0)
			Me.Controls.SetChildIndex(Me.settingsPanel, 0)
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
			Me.settingsPanel.ResumeLayout(False)
			Me.settingsPanel.PerformLayout()
			CType(Me.radCalendar1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radGroupDisplay, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radGroupDisplay.ResumeLayout(False)
			Me.radGroupDisplay.PerformLayout()
			CType(Me.radRadio7Cols, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radRadio21Cols, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radRadio14Cols, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private radCalendar1 As Telerik.WinControls.UI.RadCalendar
		Private radGroupDisplay As Telerik.WinControls.UI.RadGroupBox
		Private radRadio7Cols As Telerik.WinControls.UI.RadRadioButton
		Private radRadio14Cols As Telerik.WinControls.UI.RadRadioButton
		Private radRadio21Cols As Telerik.WinControls.UI.RadRadioButton


	End Class
End Namespace