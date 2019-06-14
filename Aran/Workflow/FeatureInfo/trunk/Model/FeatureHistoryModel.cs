using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim;
using Aran.Aim.Features;
using System.Collections.ObjectModel;

namespace Aran.Aim.FeatureInfo
{
	public class FeatureHistoryModel
	{
		public FeatureHistoryModel ()
		{
			Items = new ObservableCollection<FeatureHistoryInfo> ();
		}

		public void Open (List<Feature> features)
		{
			foreach (Feature item in features)
			{
				Items.Add (new FeatureHistoryInfo (item));
			}
		}

		public ObservableCollection <FeatureHistoryInfo> Items { get; private set; }
	}

	public class FeatureHistoryInfo : AbsModel
	{
		public FeatureHistoryInfo ()
		{
		}

		public FeatureHistoryInfo (Feature feature)
		{
			_feature = feature;
		}

		public Feature Feature
		{
			get { return _feature; }
			set
			{
				_feature = value;
				DoPropertyChanged ("SeqNumber");
				DoPropertyChanged ("CorNumber");
				DoPropertyChanged ("ValidTimeBegin");
				DoPropertyChanged ("ValidTimeEnd");
			}
		}
		
		public int SeqNumber
		{
			get { return _feature.TimeSlice.SequenceNumber; }
		}

		public int CorNumber
		{
			get { return _feature.TimeSlice.CorrectionNumber; }
		}

		public string ValidTimeBegin
		{
			get { return _feature.TimeSlice.ValidTime.BeginPosition.ToString ("yyyy - MM - dd"); }
		}

		public string ValidTimeEnd
		{
			get
			{
				if (_feature.TimeSlice.ValidTime.EndPosition == null)
					return "";

				return _feature.TimeSlice.ValidTime.EndPosition.Value.ToString ("yyyy - MM - dd");
			}
		}

		private Feature _feature;
	}


}
