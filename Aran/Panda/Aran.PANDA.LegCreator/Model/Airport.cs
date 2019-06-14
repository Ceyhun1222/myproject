using System;
using GalaSoft.MvvmLight;

namespace Aran.PANDA.LegCreator.Model
{
	public class Airport : ObservableObject
	{
		private string _name;

		private Guid _id;

		public Guid Id
		{
			get
			{
				return _id;
			}
			set
			{
				Set ( ( ) => Id, ref _id, value );
			}
		}


		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				Set ( ( ) => Name, ref _name, value );
			}
		}		
	}
}