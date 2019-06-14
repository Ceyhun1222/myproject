using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AControls
{
	public delegate void SelectedRowChanged(int index, string value);
	public delegate void RowCheckedChanged(string value, bool isChecked);
}
