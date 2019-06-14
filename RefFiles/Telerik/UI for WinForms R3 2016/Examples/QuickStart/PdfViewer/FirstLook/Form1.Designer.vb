Imports Telerik.QuickStart.WinControls

namespace Telerik.Examples.WinControls.PdfViewer.FirstLook
    Partial Class Form1
        Inherits ExamplesForm

        'Form overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()> _
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Me.radPdfViewer1 = New Telerik.WinControls.UI.RadPdfViewer()
            Me.radPDFCommandBar1 = New Telerik.WinControls.UI.RadPdfViewerNavigator()
            DirectCast(Me.radPdfViewer1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radPDFCommandBar1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            ' 
            ' radPdfViewer1
            ' 
            Me.radPdfViewer1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.radPdfViewer1.Location = New System.Drawing.Point(0, 68)
            Me.radPdfViewer1.Name = "radPdfViewer1"
            Me.radPdfViewer1.Size = New System.Drawing.Size(767, 441)
            Me.radPdfViewer1.TabIndex = 0
            Me.radPdfViewer1.Text = "radPdfViewer1"
            Me.radPdfViewer1.ReadingMode = Telerik.WinControls.UI.ReadingMode.OnDemand
            Me.radPdfViewer1.ViewerMode = Telerik.WinControls.UI.FixedDocumentViewerMode.Pan
            ' 
            ' radPDFCommandBar1
            ' 
            Me.radPDFCommandBar1.Dock = System.Windows.Forms.DockStyle.Top
            Me.radPDFCommandBar1.Location = New System.Drawing.Point(0, 0)
            Me.radPDFCommandBar1.Name = "radPDFCommandBar1"
            Me.radPDFCommandBar1.AssociatedViewer = Me.radPdfViewer1
            Me.radPDFCommandBar1.Size = New System.Drawing.Size(767, 68)
            Me.radPDFCommandBar1.TabIndex = 1
            Me.radPDFCommandBar1.Text = "radPDFCommandBar1"
            ' 
            ' Form1
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 13.0F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(767, 509)
            Me.Controls.Add(Me.radPdfViewer1)
            Me.Controls.Add(Me.radPDFCommandBar1)
            Me.Name = "Form1"
            Me.Text = "Form1"
            DirectCast(Me.radPdfViewer1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radPDFCommandBar1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

        Private radPdfViewer1 As Telerik.WinControls.UI.RadPdfViewer
        Private radPDFCommandBar1 As Telerik.WinControls.UI.RadPdfViewerNavigator

    End Class
End Namespace
