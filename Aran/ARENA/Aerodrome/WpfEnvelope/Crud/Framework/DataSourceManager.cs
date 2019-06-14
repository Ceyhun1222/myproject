using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Collections.Specialized;

namespace WpfEnvelope.Crud.Framework
{
	internal class DataSourceManager
	{
		private Func<IEnumerable> _dataSourceGetMethod;

		public IEnumerable All
		{
			get
			{
				var originalCollection = _dataSourceGetMethod();
				if (CheckNeedsManualRefresh(originalCollection))
					return _dataSourceGetMethod().Cast<object>().ToArray();
				else
					return originalCollection;
			}
		}

		public bool NeedsManualRefresh
		{
			get { return CheckNeedsManualRefresh(_dataSourceGetMethod()); }
		}

		private bool CheckNeedsManualRefresh(IEnumerable collection)
		{
			if (collection is INotifyCollectionChanged)
				return false;
			else
				return true;
		}

		public void SetDataSourceGetMethod(Func<IEnumerable> dataSourceGetMethod)
		{
			_dataSourceGetMethod = dataSourceGetMethod;
		}
	}
}
