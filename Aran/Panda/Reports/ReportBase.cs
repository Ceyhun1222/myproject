using System.Globalization;
using System.IO;

namespace Aran.PANDA.Reports
{
	public class ReportBase
	{
		protected const string ReportFileExt = ".htm";
		protected const string ReportXLFileExt = ".xlsx";

		public System.Windows.Forms.ListView lListView;
		public bool IsFinished { get; protected set; }

		protected ExcellExporter _excellExporter;
		protected string pFileName;
		protected StreamWriter _stwr = null;

		protected bool isOpen = false;
		protected int length = 0;
		public byte[] Report { get; protected set; }

		public void OpenFile(string Name, string ReportTitle)
		{
			if (Name.Equals("")) return;
			if (isOpen) return;
			if (_stwr != null) return;

			pFileName = Name;
			Open(ReportTitle);
		}

		public virtual void Open(string ReportTitle)
		{
			_excellExporter = ExcellExporter.Get(ReportTitle);
			_excellExporter.CreateSheet("Info");

			_stwr = new StreamWriter(new FileStream(pFileName + ReportFileExt, FileMode.Create));

			_stwr.WriteLine("<html>");
			_stwr.WriteLine("<head>");
			_stwr.WriteLine("<title>PANDA - " + ReportTitle + "</title>");
			_stwr.WriteLine("<meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\" />");
			_stwr.WriteLine("<style>");
			_stwr.WriteLine("body {font-family: Arial, Sans-Serif; font-size:12;}");
			_stwr.WriteLine("table {font-family: Arial, Sans-Serif; font-size:10;}");

			_stwr.WriteLine("</style>");
			_stwr.WriteLine("</head>");
			_stwr.WriteLine("<body>");

			//H1("PANDA - " + ReportTitle);

			isOpen = true;
		}

		public void CloseFile()
		{
			if (!isOpen) return;
			if (_stwr == null) return;

			_stwr.WriteLine("</body>");
			_stwr.WriteLine("</html>");

			_stwr.Flush();
			_stwr.Dispose();
			_stwr = null;

			_excellExporter.WriteToFile(pFileName + ReportXLFileExt);
			Report = _excellExporter.GetBytes();

			isOpen = false;
			IsFinished = true;
		}

		public void Page(string name)
		{
			if (name != null && !name.Equals(""))
				_excellExporter.CreateSheet(name);
			else
				_excellExporter.CreateSheet();
		}

		public void ExH1(string text, bool encoded = false, bool bold = false)
		{
			_excellExporter.H1(text);
		}

		public void ExH2(string text, bool encoded = false, bool bold = false)
		{
			_excellExporter.H2(text);
		}

		public void H1(string text, bool encoded = false, bool bold = false)
		{
			_excellExporter.H1(text);
			HTMLString(text, encoded, bold);
		}

		public void H2(string text, bool encoded = false, bool bold = false)
		{
			_excellExporter.H2(text);
			HTMLString(text, encoded, bold);
		}

		public void ExString(string text, bool bold = false, bool italic = false, int size = 12)
		{
			_excellExporter.Text(text, bold, italic, size);
		}

		public void ExLine(string text, bool bold = false, bool italic = false, int size = 12)
		{
			_excellExporter.CreateRow().Text(text, bold, italic, size);
		}

		public void ExLine()
		{
			_excellExporter.CreateRow();
		}

	    public void Param(string name, double value, string unit = "")
	    {
	        Param(name, value.ToString(CultureInfo.InvariantCulture), unit);
	    }

        public void Param(string name, string value, string unit = "")
		{
			if (name == null || value == null) return;
			if (name == "" || value == "") return;

			string temp = "<b>" + System.Net.WebUtility.HtmlEncode(name) + "</b> " + System.Net.WebUtility.HtmlEncode(value);
			if (unit != "")
				temp = temp + " " + System.Net.WebUtility.HtmlEncode(unit);

			_stwr.WriteLine(temp + " <br>");

			//System.Net.WebUtility.HtmlEncode(name + " ", _stwr);
			//System.Net.WebUtility.HtmlEncode(value, _stwr);

			//if (unit != "")
			//{
			//	System.Net.WebUtility.HtmlEncode(" " + unit, _stwr);
			//}
			//_stwr.WriteLine("</br>");

			_excellExporter.CreateRow().Text(name + ":", true).Text(value);
			if (unit != null && !unit.Equals(""))
				_excellExporter.Text(unit);
		}

		public void Paragragh(string message, bool encoded = false, bool bold = false)
		{
			HTMLParagraph(message, encoded, bold);
			ExLine();
			ExLine(message, bold);
		}

		public void HTMLText(string text)
		{
			_stwr.Write(text);
		}

		public void HTMLParagraph(string message, bool encoded, bool bold)
		{
			string html = message;
			if (encoded)
				html = System.Net.WebUtility.HtmlEncode(html);

			if (bold)
				html = "<b>" + html + "</b>";
			html = "<p>" + html + "</p>";

			_stwr.WriteLine(html + "</br>");
		}

		public void WriteString(string message, bool encoded = false, bool bold = false)
		{
			HTMLString(message, encoded, bold);
			ExLine(message, bold);
		}

		public void HTMLString(string message, bool encoded = false, bool bold = false)
		{
			string html = message;
			if (encoded)
				html = System.Net.WebUtility.HtmlEncode(html);

			if (bold)
				html = "<b>" + html + "</b>";

			_stwr.WriteLine(html + "</br>");
		}

		public void WriteString()
		{
			HTMLString();
			ExLine();
		}

		public void HTMLString()
		{
			_stwr.WriteLine("</br>");
		}

		public void WriteMessage(string message)
		{
			HTMLMessage(message);
			ExLine(message);
		}

		public void HTMLMessage(string message)
		{
			_stwr.WriteLine(System.Net.WebUtility.HtmlEncode(message) + "<br>");
		}

		public void WriteMessage()
		{
			HTMLMessage();
			ExLine();
		}

		public void HTMLMessage()
		{
			_stwr.WriteLine("</br>");
		}
	}
}
