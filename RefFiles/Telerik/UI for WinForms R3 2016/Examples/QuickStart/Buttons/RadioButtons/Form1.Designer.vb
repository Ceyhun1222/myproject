Namespace Telerik.Examples.WinControls.Buttons.RadioButtons
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
			Me.radTextBoxEvents = New Telerik.WinControls.UI.RadTextBox()
			Me.radGroupBox1 = New Telerik.WinControls.UI.RadGroupBox()
			Me.radRadioDonut = New Telerik.WinControls.UI.RadRadioButton()
			Me.radRadioSquare = New Telerik.WinControls.UI.RadRadioButton()
			Me.radRadioOffice = New Telerik.WinControls.UI.RadRadioButton()
			Me.radRadioRound = New Telerik.WinControls.UI.RadRadioButton()
			Me.radRadioRegular = New Telerik.WinControls.UI.RadRadioButton()
			Me.radRadioCustShape = New Telerik.WinControls.UI.RadRadioButton()
			CType(Me.radPanelDemoHolder, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radPanelDemoHolder.SuspendLayout()
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.settingsPanel.SuspendLayout()
			CType(Me.radTextBoxEvents, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radGroupBox1.SuspendLayout()
			CType(Me.radRadioDonut, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radRadioSquare, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radRadioOffice, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radRadioRound, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radRadioRegular, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radRadioCustShape, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' radPanelDemoHolder
			' 
			Me.radPanelDemoHolder.Controls.Add(Me.radRadioDonut)
			Me.radPanelDemoHolder.Controls.Add(Me.radRadioRegular)
			Me.radPanelDemoHolder.Controls.Add(Me.radRadioSquare)
			Me.radPanelDemoHolder.Controls.Add(Me.radRadioCustShape)
			Me.radPanelDemoHolder.Controls.Add(Me.radRadioRound)
			Me.radPanelDemoHolder.Controls.Add(Me.radRadioOffice)
			Me.radPanelDemoHolder.Location = New Point(0, 0)
			Me.radPanelDemoHolder.Size = New Size(175, 182)
			' 
			' settingsPanel
			' 
			Me.settingsPanel.Controls.Add(Me.radGroupBox1)
			Me.settingsPanel.Location = New Point(973, 1)
			Me.settingsPanel.Size = New Size(250, 534)
			Me.settingsPanel.Controls.SetChildIndex(Me.radGroupBox1, 0)
			' 
			' radTextBoxEvents
			' 
			Me.radTextBoxEvents.AutoSize = False
			Me.radTextBoxEvents.Location = New Point(12, 21)
			Me.radTextBoxEvents.Multiline = True
			Me.radTextBoxEvents.Name = "radTextBoxEvents"
			Me.radTextBoxEvents.Size = New Size(139, 170)
			Me.radTextBoxEvents.TabIndex = 0
			Me.radTextBoxEvents.TabStop = False
			' 
			' radGroupBox1
			' 
			Me.radGroupBox1.AccessibleRole = AccessibleRole.Grouping
			Me.radGroupBox1.Controls.Add(Me.radTextBoxEvents)
			Me.radGroupBox1.FooterText = ""
			Me.radGroupBox1.HeaderText = " Events "
			Me.radGroupBox1.Location = New Point(15, 6)
			Me.radGroupBox1.Name = "radGroupBox1"
			' 
			' 
			' 
			Me.radGroupBox1.RootElement.Padding = New Padding(2, 18, 2, 2)
			Me.radGroupBox1.Size = New Size(162, 203)
			Me.radGroupBox1.TabIndex = 1
			Me.radGroupBox1.Text = " Events "
			' 
			' radRadioDonut
			' 
			Me.radRadioDonut.ForeColor = Color.Black
			Me.radRadioDonut.Location = New Point(0, 81)
			Me.radRadioDonut.Name = "radRadioDonut"
			Me.radRadioDonut.Size = New Size(51, 18)
			Me.radRadioDonut.TabIndex = 0
			Me.radRadioDonut.Text = "Donut"

			' 
			' radRadioSquare
			' 
			Me.radRadioSquare.ForeColor = Color.Black
			Me.radRadioSquare.Location = New Point(0, 61)
			Me.radRadioSquare.Name = "radRadioSquare"
			Me.radRadioSquare.Size = New Size(55, 18)
			Me.radRadioSquare.TabIndex = 0
			Me.radRadioSquare.Text = "Square"

			' 
			' radRadioOffice
			' 
			Me.radRadioOffice.ForeColor = Color.Black
			Me.radRadioOffice.Location = New Point(0, 101)
			Me.radRadioOffice.Name = "radRadioOffice"
			Me.radRadioOffice.Size = New Size(71, 18)
			Me.radRadioOffice.TabIndex = 0
			Me.radRadioOffice.Text = "Office Tab"

			' 
			' radRadioRound
			' 
			Me.radRadioRound.ForeColor = Color.Black
			Me.radRadioRound.Location = New Point(0, 40)
			Me.radRadioRound.Name = "radRadioRound"
			Me.radRadioRound.Size = New Size(105, 18)
			Me.radRadioRound.TabIndex = 0
			Me.radRadioRound.Text = "Round Rectangle"

			' 
			' radRadioRegular
			' 
			Me.radRadioRegular.ForeColor = Color.Black
			Me.radRadioRegular.Location = New Point(0, -2)
			Me.radRadioRegular.Name = "radRadioRegular"
			Me.radRadioRegular.Size = New Size(58, 18)
			Me.radRadioRegular.TabIndex = 0
			Me.radRadioRegular.Text = "Regular"

			' 
			' radRadioCustShape
			' 
			Me.radRadioCustShape.ForeColor = Color.Black
			Me.radRadioCustShape.Location = New Point(0, 20)
			Me.radRadioCustShape.Name = "radRadioCustShape"
			Me.radRadioCustShape.Size = New Size(93, 18)
			Me.radRadioCustShape.TabIndex = 0
			Me.radRadioCustShape.Text = "Custom Shape"

			' 
			' Form1
			' 
			Me.Name = "Form1"
			Me.Size = New Size(1170, 671)
			CType(Me.radPanelDemoHolder, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radPanelDemoHolder.ResumeLayout(False)
			Me.radPanelDemoHolder.PerformLayout()
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
			Me.settingsPanel.ResumeLayout(False)
			Me.settingsPanel.PerformLayout()
			CType(Me.radTextBoxEvents, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radGroupBox1.ResumeLayout(False)
			CType(Me.radRadioDonut, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radRadioSquare, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radRadioOffice, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radRadioRound, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radRadioRegular, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radRadioCustShape, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private radGroupBox1 As Telerik.WinControls.UI.RadGroupBox
		Private radTextBoxEvents As Telerik.WinControls.UI.RadTextBox
		Private radRadioDonut As Telerik.WinControls.UI.RadRadioButton
		Private radRadioSquare As Telerik.WinControls.UI.RadRadioButton
		Private radRadioOffice As Telerik.WinControls.UI.RadRadioButton
		Private radRadioRound As Telerik.WinControls.UI.RadRadioButton
		Private radRadioCustShape As Telerik.WinControls.UI.RadRadioButton
		Private radRadioRegular As Telerik.WinControls.UI.RadRadioButton
	End Class
End Namespace
