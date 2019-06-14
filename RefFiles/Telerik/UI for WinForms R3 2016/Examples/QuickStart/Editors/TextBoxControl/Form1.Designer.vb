Namespace Telerik.Examples.WinControls.Editors.TextBoxControl
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
			Me.radBtnSetBackgroundImage = New Telerik.WinControls.UI.RadButton()
			Me.radTextBoxControl1 = New Telerik.WinControls.UI.RadTextBoxControl()
			Me.radPanel1 = New Telerik.WinControls.UI.RadPanel()
			Me.radButtonSearch = New Telerik.WinControls.UI.RadButton()
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.settingsPanel.SuspendLayout()
			CType(Me.radBtnSetBackgroundImage, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radTextBoxControl1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radPanel1.SuspendLayout()
			CType(Me.radButtonSearch, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' settingsPanel
			' 
			Me.settingsPanel.Controls.Add(Me.radBtnSetBackgroundImage)
			Me.settingsPanel.Location = New Point(1073, 19)
			Me.settingsPanel.Size = New Size(69, 624)
			Me.settingsPanel.Controls.SetChildIndex(Me.radBtnSetBackgroundImage, 0)
			' 
			' radBtnSetBackgroundImage
			' 
			Me.radBtnSetBackgroundImage.Location = New Point(16, 20)
			Me.radBtnSetBackgroundImage.Name = "radBtnSetBackgroundImage"
			Me.radBtnSetBackgroundImage.Size = New Size(155, 24)
			Me.radBtnSetBackgroundImage.TabIndex = 1
			Me.radBtnSetBackgroundImage.Text = "Set Background Image"

			' 
			' radTextBoxControl1
			' 
			Me.radTextBoxControl1.Location = New Point(103, 137)
			Me.radTextBoxControl1.Name = "radTextBoxControl1"
			Me.radTextBoxControl1.NullText = "Search in Bing"
			Me.radTextBoxControl1.Size = New Size(372, 28)
			Me.radTextBoxControl1.TabIndex = 2
			CType(Me.radTextBoxControl1.GetChildAt(0).GetChildAt(0), Telerik.WinControls.UI.TextBoxViewElement).BackgroundImage = My.Resources.textbox_bg
			CType(Me.radTextBoxControl1.GetChildAt(0).GetChildAt(0), Telerik.WinControls.UI.TextBoxViewElement).Padding = New Padding(25, 6, 2, 0)
			CType(Me.radTextBoxControl1.GetChildAt(0).GetChildAt(1), Telerik.WinControls.UI.TextBoxWrapPanel).Padding = New Padding(25, 6, 2, 0)
			' 
			' radPanel1
			' 
			Me.radPanel1.BackgroundImage = My.Resources.ballons_image
			Me.radPanel1.Controls.Add(Me.radButtonSearch)
			Me.radPanel1.Controls.Add(Me.radTextBoxControl1)
			Me.radPanel1.Location = New Point(0, 0)
			Me.radPanel1.Name = "radPanel1"
			Me.radPanel1.Size = New Size(602, 420)
			Me.radPanel1.TabIndex = 3
			' 
			' radButtonSearch
			' 
			Me.radButtonSearch.Image = My.Resources.search
			Me.radButtonSearch.ImageAlignment = ContentAlignment.MiddleCenter
			Me.radButtonSearch.Location = New Point(481, 137)
			Me.radButtonSearch.Name = "radButtonSearch"
			Me.radButtonSearch.Size = New Size(32, 28)
			Me.radButtonSearch.TabIndex = 3

			' 
			' Form1
			' 
			Me.AutoScaleDimensions = New SizeF(6F, 13F)
			Me.AutoScaleMode = AutoScaleMode.Font
			Me.Controls.Add(Me.radPanel1)
			Me.Name = "Form1"
			Me.Size = New Size(1144, 572)
			Me.Controls.SetChildIndex(Me.settingsPanel, 0)
			Me.Controls.SetChildIndex(Me.radPanel1, 0)
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
			Me.settingsPanel.ResumeLayout(False)
			Me.settingsPanel.PerformLayout()
			CType(Me.radBtnSetBackgroundImage, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radTextBoxControl1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radPanel1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radPanel1.ResumeLayout(False)
			CType(Me.radButtonSearch, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private radBtnSetBackgroundImage As Telerik.WinControls.UI.RadButton
		Private radTextBoxControl1 As Telerik.WinControls.UI.RadTextBoxControl
		Private radPanel1 As Telerik.WinControls.UI.RadPanel
		Private radButtonSearch As Telerik.WinControls.UI.RadButton
	End Class
End Namespace