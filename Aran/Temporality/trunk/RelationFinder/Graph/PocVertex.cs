using System;
using System.Diagnostics;
using ADM.Converter;
using Aran.Aim;
using Aran.Aim.Features;
using MvvmCore;

namespace ADM.Graph
{
    /// <summary>
    /// A simple identifiable vertex.
    /// </summary>
    [DebuggerDisplay("{ID}")]
    public class PocVertex : ViewModelBase
    {
        public int Border { get; set; }

        public string ID { get; set; }

        public Guid Identifier { get; set; }

        public Action<PocVertex> RelationsChanged { get; set; }

        public FeatureType FeatureType { get; set; }

        private Feature _feature;
        public Feature Feature
        {
            get { return _feature; }
            set
            {
                _feature = value;
                if (_feature == null) return;
                FeatureType = _feature.FeatureType;
                Identifier = _feature.Identifier;
                ID = _feature.FeatureType + " " + HumanReadableConverter.ShortAimDescription(_feature);
            }
        }

        public void SetDirect(bool direct)
        {
            _isDirect = direct;
            OnPropertyChanged("IsDirect");
        }

        public void SetReverse(bool reverse)
        {
            _isReverse = reverse;
            OnPropertyChanged("IsReverse");
        }

        private bool? _isDirect;
        public bool? IsDirect
        {
            get { return _isDirect; }
            set
            {
                if (_isDirect==null)
                {
                    _isDirect = true;
                }
                else
                {
                    _isDirect = value;
                }
                
                if (RelationsChanged!=null)
                {
                    RelationsChanged(this);
                }

                OnPropertyChanged("IsDirect");
            }
        }

        private bool? _isReverse;
        public bool? IsReverse
        {
            get { return _isReverse; }
            set
            {
                if (_isReverse==null)
                {
                    _isReverse = true;
                }
                else
                {
                    _isReverse = value;
                }
                
                if (RelationsChanged != null)
                {
                    RelationsChanged(this);
                }
                OnPropertyChanged("IsReverse");
            }
        }

        public Action<PocVertex> OnCommand { get; set; }

        private RelayCommand _command;
        public RelayCommand Command
        {
            get
            {
                return _command??(_command=new RelayCommand(
                    t=>
                        {
                            if (OnCommand!=null)
                            {
                                OnCommand(this);
                            }
                        }));
            }
            set { _command = value; }
        }


        public override string ToString()
        {
            return string.Format("{0}", ID);
        }
    }
}
