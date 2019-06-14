Imports System.ComponentModel
Namespace Telerik.Examples.WinControls.Buttons.CheckBoxes
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
			Me.radCheckBox3 = New Telerik.WinControls.UI.RadCheckBox()
			Me.radCheckBox2 = New Telerik.WinControls.UI.RadCheckBox()
			Me.radCheckBox1 = New Telerik.WinControls.UI.RadCheckBox()
			Me.radGroupEvents = New Telerik.WinControls.UI.RadGroupBox()
			Me.radTextBoxEvents = New Telerik.WinControls.UI.RadTextBox()
			CType(Me.radPanelDemoHolder, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radPanelDemoHolder.SuspendLayout()
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.settingsPanel.SuspendLayout()
			CType(Me.radCheckBox3, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radCheckBox2, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radCheckBox1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radGroupEvents, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radGroupEvents.SuspendLayout()
			CType(Me.radTextBoxEvents, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' radPanelDemoHolder
			' 
			Me.radPanelDemoHolder.Controls.Add(Me.radCheckBox1)
			Me.radPanelDemoHolder.Controls.Add(Me.radCheckBox3)
			Me.radPanelDemoHolder.Controls.Add(Me.radCheckBox2)
			Me.radPanelDemoHolder.Size = New Size(255, 173)
			' 
			' settingsPanel
			' 
			Me.settingsPanel.Controls.Add(Me.radGroupEvents)
			Me.settingsPanel.Location = New Point(973, 1)
			Me.settingsPanel.Size = New Size(250, 534)
			Me.settingsPanel.Controls.SetChildIndex(Me.radGroupEvents, 0)
			' 
			' radCheckBox3
			' 
			Me.radCheckBox3.Font = New Font("Arial", 14F, FontStyle.Regular, GraphicsUnit.Point, (CByte(0)))
			Me.radCheckBox3.Location = New Point(-1, 49)
			Me.radCheckBox3.Name = "radCheckBox3"
			Me.radCheckBox3.Size = New Size(109, 25)
			Me.radCheckBox3.TabIndex = 2
			Me.radCheckBox3.Text = "Arial, 14pt"
			' 
			' radCheckBox2
			' 
			Me.radCheckBox2.Font = New Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, (CByte(0)))
			Me.radCheckBox2.Location = New Point(-1, 24)
			Me.radCheckBox2.Name = "radCheckBox2"
			Me.radCheckBox2.Size = New Size(96, 22)
			Me.radCheckBox2.TabIndex = 1
			Me.radCheckBox2.Text = "Arial, 12pt"
			' 
			' radCheckBox1
			' 
			Me.radCheckBox1.Font = New Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point, (CByte(204)))
			Me.radCheckBox1.Location = New Point(-1, -1)
			Me.radCheckBox1.Name = "radCheckBox1"
			Me.radCheckBox1.Size = New Size(83, 19)
			Me.radCheckBox1.TabIndex = 0
			Me.radCheckBox1.Text = "Arial, 10pt"
			' 
			' radGroupEvents
			' 
			Me.radGroupEvents.AccessibleRole = AccessibleRole.Grouping
			Me.radGroupEvents.Controls.Add(Me.radTextBoxEvents)
			Me.radGroupEvents.FooterText = ""
			Me.radGroupEvents.HeaderText = " Events "
			Me.radGroupEvents.Location = New Point(15, 6)
			Me.radGroupEvents.Name = "radGroupEvents"
			' 
			' 
			' 
			Me.radGroupEvents.RootElement.Padding = New Padding(2, 18, 2, 2)
			Me.radGroupEvents.Size = New Size(162, 188)
			Me.radGroupEvents.TabIndex = 0
			Me.radGroupEvents.Text = " Events "
			' 
			' radTextBoxEvents
			' 
			Me.radTextBoxEvents.AutoSize = False
			Me.radTextBoxEvents.Location = New Point(13, 23)
			Me.radTextBoxEvents.Multiline = True
			Me.radTextBoxEvents.Name = "radTextBoxEvents"
			Me.radTextBoxEvents.Size = New Size(138, 154)
			Me.radTextBoxEvents.TabIndex = 0
			Me.radTextBoxEvents.TabStop = False
			' 
			' Form1
			' 
			Me.Name = "Form1"
			Me.Padding = New Padding(2, 35, 2, 4)
			CType(Me.radPanelDemoHolder, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radPanelDemoHolder.ResumeLayout(False)
			Me.radPanelDemoHolder.PerformLayout()
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
			Me.settingsPanel.ResumeLayout(False)
			Me.settingsPanel.PerformLayout()
			CType(Me.radCheckBox3, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radCheckBox2, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radCheckBox1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radGroupEvents, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radGroupEvents.ResumeLayout(False)
			CType(Me.radTextBoxEvents, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private radCheckBox1 As Telerik.WinControls.UI.RadCheckBox
		Private radCheckBox2 As Telerik.WinControls.UI.RadCheckBox
		Private radCheckBox3 As Telerik.WinControls.UI.RadCheckBox
		Private radGroupEvents As Telerik.WinControls.UI.RadGroupBox
		Private radTextBoxEvents As Telerik.WinControls.UI.RadTextBox
	End Class
End Namespace