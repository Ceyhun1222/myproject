Namespace Telerik.Examples.WinControls.TreeView.DragDrop
	Partial Public Class Form1
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary>
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>


		#Region "Windows Form Designer generated code"

		''' <summary>
		''' Required method for Designer support - do not modify
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
			Me.components = New System.ComponentModel.Container()
			Dim radTreeNode1 As New Telerik.WinControls.UI.RadTreeNode()
			Dim radTreeNode2 As New Telerik.WinControls.UI.RadTreeNode()
			Dim radTreeNode3 As New Telerik.WinControls.UI.RadTreeNode()
			Dim radTreeNode4 As New Telerik.WinControls.UI.RadTreeNode()
			Dim radTreeNode5 As New Telerik.WinControls.UI.RadTreeNode()
			Dim radTreeNode6 As New Telerik.WinControls.UI.RadTreeNode()
			Dim radTreeNode7 As New Telerik.WinControls.UI.RadTreeNode()
			Dim radTreeNode8 As New Telerik.WinControls.UI.RadTreeNode()
			Dim radTreeNode9 As New Telerik.WinControls.UI.RadTreeNode()
			Dim radTreeNode10 As New Telerik.WinControls.UI.RadTreeNode()
			Dim resources As New System.ComponentModel.ComponentResourceManager(GetType(Form1))
			Dim radTreeNode11 As New Telerik.WinControls.UI.RadTreeNode()
			Dim radTreeNode12 As New Telerik.WinControls.UI.RadTreeNode()
			Dim radTreeNode13 As New Telerik.WinControls.UI.RadTreeNode()
			Dim radTreeNode14 As New Telerik.WinControls.UI.RadTreeNode()
			Dim radTreeNode15 As New Telerik.WinControls.UI.RadTreeNode()
			Dim radTreeNode16 As New Telerik.WinControls.UI.RadTreeNode()
			Dim radTreeNode17 As New Telerik.WinControls.UI.RadTreeNode()
			Dim radTreeNode18 As New Telerik.WinControls.UI.RadTreeNode()
			Dim radTreeNode19 As New Telerik.WinControls.UI.RadTreeNode()
			Dim radTreeNode20 As New Telerik.WinControls.UI.RadTreeNode()
			Me.radTreeView2 = New Telerik.WinControls.UI.RadTreeView()
			Me.imageList1 = New ImageList(Me.components)
			Me.radTreeView1 = New Telerik.WinControls.UI.RadTreeView()
			Me.radTextBox1 = New Telerik.WinControls.UI.RadTextBox()
			Me.radPanel1 = New Telerik.Examples.WinControls.TreeView.TreeExampleHeaderPanel()
			Me.radPanel2 = New Telerik.WinControls.UI.RadPanel()
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radTreeView2, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radTreeView1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radTextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radPanel2, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radPanel2.SuspendLayout()
			Me.SuspendLayout()
			' 
			' settingsPanel
			' 
			Me.settingsPanel.Location = New Point(969, 1)
			Me.settingsPanel.Size = New Size(794, 998)
			' 
			' radTreeView2
			' 
			Me.radTreeView2.AllowDragDrop = True
			Me.radTreeView2.AllowDrop = True
			Me.radTreeView2.ImageList = Me.imageList1
			Me.radTreeView2.Location = New Point(280, 43)
			Me.radTreeView2.Name = "radTreeView2"
			radTreeNode1.Expanded = True
			radTreeNode1.Image = (CType(resources.GetObject("radTreeNode1.Image"), Image))
			radTreeNode1.ImageIndex = 0
			radTreeNode2.Image = (CType(resources.GetObject("radTreeNode2.Image"), Image))
			radTreeNode2.ImageIndex = 1
			radTreeNode2.Text = "Deleted Items"
			radTreeNode3.Image = (CType(resources.GetObject("radTreeNode3.Image"), Image))
			radTreeNode3.ImageIndex = 2
			radTreeNode3.Text = "Drafts"
			radTreeNode4.Image = (CType(resources.GetObject("radTreeNode4.Image"), Image))
			radTreeNode4.ImageIndex = 3
			radTreeNode4.Text = "Inbox"
			radTreeNode5.Image = (CType(resources.GetObject("radTreeNode5.Image"), Image))
			radTreeNode5.ImageIndex = 5
			radTreeNode5.Text = "OutBox"
			radTreeNode6.Image = (CType(resources.GetObject("radTreeNode6.Image"), Image))
			radTreeNode6.ImageIndex = 6
			radTreeNode6.Text = "Rss Feeds"
			radTreeNode7.Image = (CType(resources.GetObject("radTreeNode7.Image"), Image))
			radTreeNode7.ImageIndex = 8
			radTreeNode7.Text = "Sent Items"
			radTreeNode8.Expanded = True
			radTreeNode8.Image = (CType(resources.GetObject("radTreeNode8.Image"), Image))
			radTreeNode8.ImageIndex = 9
			radTreeNode9.Image = (CType(resources.GetObject("radTreeNode9.Image"), Image))
			radTreeNode9.ImageIndex = 8
			radTreeNode9.Text = "Large Mail"
			radTreeNode10.Image = (CType(resources.GetObject("radTreeNode10.Image"), Image))
			radTreeNode10.ImageIndex = 8
			radTreeNode10.Text = "Unread Mail"
			radTreeNode8.Nodes.AddRange(New Telerik.WinControls.UI.RadTreeNode() { radTreeNode9, radTreeNode10})
			radTreeNode8.NumberOfColors = 2
			radTreeNode8.Text = "Search Folders"
			radTreeNode1.Nodes.AddRange(New Telerik.WinControls.UI.RadTreeNode() { radTreeNode2, radTreeNode3, radTreeNode4, radTreeNode5, radTreeNode6, radTreeNode7, radTreeNode8})
			radTreeNode1.Text = "Personal Folders"
			Me.radTreeView2.Nodes.AddRange(New Telerik.WinControls.UI.RadTreeNode() { radTreeNode1})
			Me.radTreeView2.Size = New Size(260, 408)
			Me.radTreeView2.SpacingBetweenNodes = -1
			Me.radTreeView2.TabIndex = 5
			Me.radTreeView2.Text = "radTreeView2"
			' 
			' imageList1
			' 
			Me.imageList1.ImageStream = (CType(resources.GetObject("imageList1.ImageStream"), ImageListStreamer))
			Me.imageList1.TransparentColor = Color.Fuchsia
			Me.imageList1.Images.SetKeyName(0, "0_Personal_Folders.gif")
			Me.imageList1.Images.SetKeyName(1, "1_Deleted_Items.gif")
			Me.imageList1.Images.SetKeyName(2, "2_Drafts.gif")
			Me.imageList1.Images.SetKeyName(3, "3_Inbox.gif")
			Me.imageList1.Images.SetKeyName(4, "4_Junk_E-mail.gif")
			Me.imageList1.Images.SetKeyName(5, "5_Outbox.gif")
			Me.imageList1.Images.SetKeyName(6, "6_Rss_Feeds.gif")
			Me.imageList1.Images.SetKeyName(7, "7_Sent_Items.gif")
			Me.imageList1.Images.SetKeyName(8, "8.1_Search.gif")
			Me.imageList1.Images.SetKeyName(9, "8_Search.gif")
			' 
			' radTreeView1
			' 
			Me.radTreeView1.AllowDragDrop = True
			Me.radTreeView1.AllowDrop = True
			Me.radTreeView1.ImageList = Me.imageList1
			Me.radTreeView1.Location = New Point(12, 43)
			Me.radTreeView1.Name = "radTreeView1"
			radTreeNode11.Expanded = True
			radTreeNode11.Image = (CType(resources.GetObject("radTreeNode11.Image"), Image))
			radTreeNode11.ImageIndex = 0
			radTreeNode12.Image = (CType(resources.GetObject("radTreeNode12.Image"), Image))
			radTreeNode12.ImageIndex = 1
			radTreeNode12.Text = "Deleted Items"
			radTreeNode13.Image = (CType(resources.GetObject("radTreeNode13.Image"), Image))
			radTreeNode13.ImageIndex = 2
			radTreeNode13.Text = "Drafts"
			radTreeNode14.Image = (CType(resources.GetObject("radTreeNode14.Image"), Image))
			radTreeNode14.ImageIndex = 3
			radTreeNode14.Text = "Inbox"
			radTreeNode15.Image = (CType(resources.GetObject("radTreeNode15.Image"), Image))
			radTreeNode15.ImageIndex = 5
			radTreeNode15.Text = "OutBox"
			radTreeNode16.Image = (CType(resources.GetObject("radTreeNode16.Image"), Image))
			radTreeNode16.ImageIndex = 6
			radTreeNode16.Text = "Rss Feeds"
			radTreeNode17.Image = (CType(resources.GetObject("radTreeNode17.Image"), Image))
			radTreeNode17.ImageIndex = 8
			radTreeNode17.Text = "Sent Items"
			radTreeNode18.Expanded = True
			radTreeNode18.Image = (CType(resources.GetObject("radTreeNode18.Image"), Image))
			radTreeNode18.ImageIndex = 9
			radTreeNode19.Image = (CType(resources.GetObject("radTreeNode19.Image"), Image))
			radTreeNode19.ImageIndex = 8
			radTreeNode19.Text = "Large Mail"
			radTreeNode20.Image = (CType(resources.GetObject("radTreeNode20.Image"), Image))
			radTreeNode20.ImageIndex = 8
			radTreeNode20.Text = "Unread Mail"
			radTreeNode18.Nodes.AddRange(New Telerik.WinControls.UI.RadTreeNode() { radTreeNode19, radTreeNode20})
			radTreeNode18.NumberOfColors = 2
			radTreeNode18.Text = "Search Folders"
			radTreeNode11.Nodes.AddRange(New Telerik.WinControls.UI.RadTreeNode() { radTreeNode12, radTreeNode13, radTreeNode14, radTreeNode15, radTreeNode16, radTreeNode17, radTreeNode18})
			radTreeNode11.Text = "Personal Folders"
			Me.radTreeView1.Nodes.AddRange(New Telerik.WinControls.UI.RadTreeNode() { radTreeNode11})
			Me.radTreeView1.Size = New Size(260, 408)
			Me.radTreeView1.SpacingBetweenNodes = -1
			Me.radTreeView1.TabIndex = 4
			Me.radTreeView1.Text = "radTreeView1"
			' 
			' radTextBox1
			' 
			Me.radTextBox1.AllowDrop = True
			Me.radTextBox1.Anchor = (CType(((AnchorStyles.Top Or AnchorStyles.Left) Or AnchorStyles.Right), AnchorStyles))
			Me.radTextBox1.BackgroundImageLayout = ImageLayout.Stretch
			Me.radTextBox1.Location = New Point(12, 12)
			Me.radTextBox1.Name = "radTextBox1"
			Me.radTextBox1.ReadOnly = True
			Me.radTextBox1.Size = New Size(528, 20)
			Me.radTextBox1.TabIndex = 2
			Me.radTextBox1.TabStop = False
			' 
			' radPanel1
            ' 
			Me.radPanel1.Location = New Point(0, 0)
            Me.radPanel1.Name = "radPanel1"
            Me.radPanel1.PanelElement.ForeColor = System.Drawing.Color.Black
			Me.radPanel1.Size = New Size(550, 31)
			Me.radPanel1.TabIndex = 8
			Me.radPanel1.Text = "Drag && drop between controls"
			' 
			' radPanel2
			' 
			Me.radPanel2.Controls.Add(Me.radTreeView2)
			Me.radPanel2.Controls.Add(Me.radTextBox1)
			Me.radPanel2.Controls.Add(Me.radTreeView1)
			Me.radPanel2.Location = New Point(0, 31)
			Me.radPanel2.Name = "radPanel2"
			Me.radPanel2.Size = New Size(550, 459)
			Me.radPanel2.TabIndex = 7
			' 
			' Form1
			' 
			Me.BackColor = Color.Transparent
			Me.Controls.Add(Me.radPanel2)
			Me.Controls.Add(Me.radPanel1)
			Me.Name = "Form1"
			Me.Size = New Size(1223, 536)
			Me.Controls.SetChildIndex(Me.radPanel1, 0)
			Me.Controls.SetChildIndex(Me.radPanel2, 0)
			Me.Controls.SetChildIndex(Me.settingsPanel, 0)
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radTreeView2, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radTreeView1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radTextBox1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radPanel1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radPanel2, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radPanel2.ResumeLayout(False)
			Me.radPanel2.PerformLayout()
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private radTextBox1 As Telerik.WinControls.UI.RadTextBox
		Private radTreeView1 As Telerik.WinControls.UI.RadTreeView
		Private imageList1 As ImageList
		Private radTreeView2 As Telerik.WinControls.UI.RadTreeView
		Private radPanel2 As Telerik.WinControls.UI.RadPanel
		Private radPanel1 As TreeExampleHeaderPanel

	End Class
End Namespace