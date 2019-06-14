using Aran.Temporality.CommonUtil.Util;
using QuickGraph;
using System.Diagnostics;
using System.ComponentModel;

namespace TOSSM.Graph
{
    /// <summary>
    /// A simple identifiable edge.
    /// </summary>
    [DebuggerDisplay("{Source.ID} -> {Target.ID}")]
    public class PocEdge : Edge<PocVertex>, INotifyPropertyChanged
    {
        private string id;

        public string ID
        {
            get => id;
            set
            {
                id = value;
                NotifyPropertyChanged("ID");
            }
        }

        private MtObservableCollection<string> _connectionList;
        public MtObservableCollection<string> ConnectionList
        {
            get => _connectionList??(_connectionList=new MtObservableCollection<string>());
            set => _connectionList = value;
        }

        public PocEdge(string id, PocVertex source, PocVertex target)
            : base(source, target)
        {
            ID = id;
        }


        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion
    }
}