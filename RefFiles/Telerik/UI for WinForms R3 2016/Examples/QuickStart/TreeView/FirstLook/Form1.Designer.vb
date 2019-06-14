Imports Telerik.WinControls.UI

Namespace Telerik.Examples.WinControls.TreeView.FirstLook
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
			Me.components = New System.ComponentModel.Container()
			Dim resources As New System.ComponentModel.ComponentResourceManager(GetType(Form1))
			Me.imageList1 = New ImageList(Me.components)
			Me.radPanel2 = New Telerik.WinControls.UI.RadPanel()
			Me.radTreeView1 = New Telerik.WinControls.UI.RadTreeView()
			Me.radPanel3 = New Telerik.WinControls.UI.RadPanel()
			Me.radDropDownButton1 = New Telerik.WinControls.UI.RadDropDownButton()
			Me.radLabel1 = New Telerik.WinControls.UI.RadLabel()
			Me.radTextBox1 = New Telerik.WinControls.UI.RadTextBox()
			Me.radPanel1 = New Telerik.Examples.WinControls.TreeView.TreeExampleHeaderPanel()
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radPanel2, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radPanel2.SuspendLayout()
			CType(Me.radTreeView1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radPanel3, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radPanel3.SuspendLayout()
			CType(Me.radDropDownButton1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radTextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' settingsPanel
			' 
			Me.settingsPanel.Location = New Point(982, 1)
			Me.settingsPanel.Size = New Size(200, 747)
			' 
			' imageList1
			' 
			Me.imageList1.ImageStream = (CType(resources.GetObject("imageList1.ImageStream"), ImageListStreamer))
			Me.imageList1.TransparentColor = Color.Transparent
			Me.imageList1.Images.SetKeyName(0, "folder_feed.png")
			Me.imageList1.Images.SetKeyName(1, "feed.png")
			' 
			' radPanel2
			' 
			Me.radPanel2.Controls.Add(Me.radTreeView1)
			Me.radPanel2.Controls.Add(Me.radPanel3)
			Me.radPanel2.Controls.Add(Me.radPanel1)
			Me.radPanel2.Dock = DockStyle.Left
			Me.radPanel2.Location = New Point(0, 0)
			Me.radPanel2.Name = "radPanel2"
			Me.radPanel2.Size = New Size(450, 526)
			Me.radPanel2.TabIndex = 2
			' 
			' radTreeView1
			' 
			Me.radTreeView1.Anchor = (CType((((AnchorStyles.Top Or AnchorStyles.Bottom) Or AnchorStyles.Left) Or AnchorStyles.Right), AnchorStyles))
			Me.radTreeView1.ImageIndex = 0
			Me.radTreeView1.ImageList = Me.imageList1
			Me.radTreeView1.Location = New Point(7, 81)
			Me.radTreeView1.Name = "radTreeView1"
			Me.radTreeView1.Size = New Size(436, 438)
			Me.radTreeView1.SpacingBetweenNodes = -1
			Me.radTreeView1.TabIndex = 3
			Me.radTreeView1.Text = "radTreeView1"
			' 
			' radPanel3
			' 
			Me.radPanel3.Anchor = (CType(((AnchorStyles.Top Or AnchorStyles.Left) Or AnchorStyles.Right), AnchorStyles))
			Me.radPanel3.Controls.Add(Me.radDropDownButton1)
			Me.radPanel3.Controls.Add(Me.radLabel1)
			Me.radPanel3.Controls.Add(Me.radTextBox1)
			Me.radPanel3.Location = New Point(7, 7)
			Me.radPanel3.Name = "radPanel3"
			Me.radPanel3.Size = New Size(436, 40)
			Me.radPanel3.TabIndex = 2
			' 
			' radDropDownButton1
			' 
			Me.radDropDownButton1.Location = New Point(288, 8)
			Me.radDropDownButton1.Name = "radDropDownButton1"
			Me.radDropDownButton1.Size = New Size(140, 24)
			Me.radDropDownButton1.TabIndex = 4
			Me.radDropDownButton1.Text = "None"
			' 
			' radLabel1
			' 
			Me.radLabel1.Location = New Point(253, 11)
			Me.radLabel1.Name = "radLabel1"
			Me.radLabel1.Size = New Size(29, 18)
			Me.radLabel1.TabIndex = 2
			Me.radLabel1.Text = "Sort:"
			' 
			' radTextBox1
			' 
			Me.radTextBox1.Location = New Point(6, 10)
			Me.radTextBox1.Name = "radTextBox1"
			Me.radTextBox1.NullText = "Type here to filter"
			Me.radTextBox1.Size = New Size(241, 20)
			Me.radTextBox1.TabIndex = 0
			Me.radTextBox1.TabStop = False
			' 
			' radPanel1
			' 
			Me.radPanel1.Anchor = (CType(((AnchorStyles.Top Or AnchorStyles.Left) Or AnchorStyles.Right), AnchorStyles))
			Me.radPanel1.Location = New Point(7, 51)
			Me.radPanel1.Name = "radPanel1"
			Me.radPanel1.Size = New Size(436, 30)
			Me.radPanel1.TabIndex = 1
			' 
			' Form1
			' 
			Me.AutoScaleDimensions = New SizeF(6F, 13F)
			Me.AutoScaleMode = AutoScaleMode.Font
			Me.Controls.Add(Me.radPanel2)
			Me.Name = "Form1"
			Me.Size = New Size(1223, 536)
			Me.Controls.SetChildIndex(Me.radPanel2, 0)
			Me.Controls.SetChildIndex(Me.settingsPanel, 0)
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radPanel2, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radPanel2.ResumeLayout(False)
			CType(Me.radTreeView1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radPanel3, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radPanel3.ResumeLayout(False)
			Me.radPanel3.PerformLayout()
			CType(Me.radDropDownButton1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLabel1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radTextBox1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radPanel1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private imageList1 As ImageList
		Private radPanel1 As TreeExampleHeaderPanel
		Private radPanel2 As RadPanel
		Private radTreeView1 As RadTreeView
		Private radPanel3 As RadPanel
		Private radLabel1 As RadLabel
		Private radTextBox1 As RadTextBox
		Private radDropDownButton1 As RadDropDownButton
	End Class
End Namespace
