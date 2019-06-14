using Aran.Aim.Features;
using TOSSM.Converter;
using TOSSM.ViewModel.Document.Single.ReadOnly;

namespace TOSSM.ViewModel.Document.Single
{
    public class HistorySingleViewModel : SingleModelBase
    {
        //TODO:reimplement it
        private Feature _timeSlice;
        public Feature TimeSlice
        {
            get => _timeSlice;
            set
            {
                _timeSlice = value;
                InitValues(TimeSlice);
            }
        }

        private void InitValues(Feature timeSlice)
        {
            PropertyName = timeSlice==null ? "" : HumanReadableConverter.ToHuman(timeSlice.TimeSlice);
            StringValue = "TimeSlice";
            //if (timeSlice.)
            GroupName = "Main Database";
        }


    }
}
