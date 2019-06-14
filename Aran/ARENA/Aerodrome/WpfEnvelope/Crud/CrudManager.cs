using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using WpfEnvelope.Crud.Framework;

namespace WpfEnvelope.Crud
{
	public class CrudManager
	{
		internal CrudManager()
		{
			NavigationManager = new NavigationManager();
		}

		private readonly List<MetaTypeRegistration> _registeredTyes
			= new List<MetaTypeRegistration>();
		public IEnumerable<MetaTypeRegistration> RegisteredTypes
		{
			get { return _registeredTyes; }
		}

		public NavigationManager NavigationManager { get; private set; }

		internal IEnumerable TryGetDataSource(Type t)
		{
			// TODO: evtl. abgeleitete Typen?
			var ret = RegisteredTypes.FirstOrDefault(tr => tr.Type == t);
			return ret == null
				? null : 
				ret.DataSourceManager.All;
		}

		public TypeConfiguration<T> RegisterType<T>(Func<IEnumerable<T>> dataSourceGetMethod)
		{
			// TODO: check already registered

			MetaTypeRegistration mtr = new MetaTypeRegistration(typeof(T));
			mtr.DataSourceManager.SetDataSourceGetMethod(() => dataSourceGetMethod());
			_registeredTyes.Add(mtr);

			return new TypeConfiguration<T>(mtr);
		}
	}
}
