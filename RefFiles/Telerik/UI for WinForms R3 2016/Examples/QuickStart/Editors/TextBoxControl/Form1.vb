Imports Telerik.QuickStart.WinControls

Namespace Telerik.Examples.WinControls.Editors.TextBoxControl
	Partial Public Class Form1
		Inherits ExamplesForm
		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub radBtnSetBackgroundImage_Click(ByVal sender As Object, ByVal e As EventArgs)
			Using fileDialog As New OpenFileDialog()
				fileDialog.Multiselect = False
				fileDialog.Filter = "Images (*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF;*.PNG;"

				If fileDialog.ShowDialog() = DialogResult.OK Then
					Dim image As Image = Image.FromStream(fileDialog.OpenFile())
					Me.radTextBoxControl1.TextBoxElement.ViewElement.BackgroundImage = image
				End If
			End Using
		End Sub

		'protected override void OnSizeChanged(System.EventArgs e)
		'{
		'    base.OnSizeChanged(e);

		'    if (this.radPanel1 != null)
		'    {
		'        float xCoord = ((float)(this.Width - this.radPanel1.Width)) / 2;
		'        float yCoord = ((float)(this.Height - this.radPanel1.Height)) / 2;
		'        this.radPanel1.Location = Point.Round(new PointF(xCoord, yCoord));
		'    }
		'}

		Private Sub radButtonSearch_Click(ByVal sender As Object, ByVal e As EventArgs)
			Dim text As String = Me.radTextBoxControl1.Text

			If Not String.IsNullOrEmpty(text) Then
				Dim query As String = String.Format("http://www.bing.com/search?q={0}", text)
				Process.Start(query)
			End If
		End Sub

		Protected Overrides Sub WireEvents()
			AddHandler radBtnSetBackgroundImage.Click, AddressOf radBtnSetBackgroundImage_Click
			AddHandler radButtonSearch.Click, AddressOf radButtonSearch_Click
		End Sub
	End Class
End Namespace
