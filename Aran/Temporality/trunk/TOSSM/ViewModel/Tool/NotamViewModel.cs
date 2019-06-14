using System;
using System.Windows;
using Aran.Temporality.Common.Entity;
using FluentNHibernate.Conventions;
using MvvmCore;
using TOSSM.Util.Notams;

namespace TOSSM.ViewModel.Tool
{
    public class NotamViewModel: ViewModelBase
    {
        private readonly string _dateFormat = "u";

        public NotamViewModel()
        {
        }

        public void Load(Notam notam)
        {
            Notam = notam;
            CreatedOn = notam.CreatedOn.ToString(_dateFormat);
            Header = NotamConverter.GetHeader(notam);
            NotamType = ((NotamType)notam.Type).ToString();
            RefNotam = notam.RefNotam;
            Fir = Notam.FIR;
            Category = NotamConverter.CategoryToHumanReadable(notam.Code23.Substring(0, 1));
            Subject = NotamConverter.SubjectToHumanReadable(notam.Code23); 
            Status = NotamConverter.StatusToHumanReadable(notam.Code45);
            Purpose = Notam.Purpose;
            Scope = NotamConverter.ScopeToHumanReadable(Notam.Scope);
            Icao = Notam.ICAO;
            Traffic = NotamConverter.TrafficToHumanReadable(Notam.Traffic);
            Limits = $"From {Notam.Lower}FL To {Notam.Upper}FL";
            BeginDate = Notam.StartValidity.ToString(_dateFormat);
            if (notam.EndValidity != default(DateTime))
            {
                var est = Notam.EndValidityEst ? " EST" : "";
                EndDate = $"{Notam.EndValidity.ToString(_dateFormat)}{est}";
            }

            Schedule = notam.Schedule;
            Position = Notam.Coordinates;
            Radius = Notam.Radius;
            Text = Notam.ItemE;
            FullText = Notam.Text;
        }

        public void Clean()
        {
            Notam = null;
            Header = null;
            Fir = null;
            Category = null;
            Subject = null;
            Status = null;
            Purpose = null;
            Scope = null;
            Icao = null;
            Traffic = null;
            Limits = null;
            BeginDate = null;
            EndDate = null;
            Position = null;
            Radius = null;
            Text = null;
            FullText = null;
            Schedule = null;
        }
        public Notam Notam { get; private set; }


        private string _createdOn;
        public string CreatedOn
        {
            get => _createdOn;
            set
            {
                _createdOn = value;
                OnPropertyChanged(nameof(CreatedOn));
            }
        }


        private string _header;
        public string Header
        {
            get => _header;
            set
            {
                _header = value;
                OnPropertyChanged(nameof(Header));
            }
        }

        private string _notamType;
        public string NotamType
        {
            get => _notamType;
            set
            {
                _notamType = value;
                OnPropertyChanged(nameof(NotamType));
            }
        }

        private string _refNotam;
        public string RefNotam
        {
            get => _refNotam;
            set
            {
                _refNotam = value;
                OnPropertyChanged(nameof(RefNotam));
            }
        }

        private string _fir;
        public string Fir
        {
            get => _fir;
            set
            {
                _fir = value;
                OnPropertyChanged(nameof(Fir));
            }
        }

        private string _category;
        public string Category
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged(nameof(Category));
            }
        }



        private string _subject;
        public string Subject
        {
            get => _subject;
            set
            {
                _subject = value;
                OnPropertyChanged(nameof(Subject));
            }
        }


        private string _status;
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }


        private string _traffic;
        public string Traffic
        {
            get => _traffic;
            set
            {
                _traffic = value;
                OnPropertyChanged(nameof(Traffic));
            }
        }


        private string _purpose;
        public string Purpose
        {
            get => _purpose;
            set
            {
                _purpose = value;
                OnPropertyChanged(nameof(Purpose));
            }
        }


        private string _scope;
        public string Scope
        {
            get => _scope;
            set
            {
                _scope = value;
                OnPropertyChanged(nameof(Scope));
            }
        }


        private string _limits;
        public string Limits
        {
            get => _limits;
            set
            {
                _limits = value;
                OnPropertyChanged(nameof(Limits));
            }
        }


        private string _icao;
        public string Icao
        {
            get => _icao;
            set
            {
                _icao = value;
                OnPropertyChanged(nameof(Icao));
            }
        }


        private string _beginDate;
        public string BeginDate
        {
            get => _beginDate;
            set
            {
                _beginDate = value;
                OnPropertyChanged(nameof(BeginDate));
            }
        }


        private string _endDate;
        public string EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }


        private string _schedule;
        public string Schedule
        {
            get => _schedule;
            set
            {
                _schedule = value;
                OnPropertyChanged(nameof(Schedule));
                ScheduleVisibility = Schedule != null && Schedule.IsNotEmpty()
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
        }

        private Visibility _scheduleVisibility = Visibility.Collapsed;
        public Visibility ScheduleVisibility
        {
            get => _scheduleVisibility;
            set
            {
                _scheduleVisibility = value;
                OnPropertyChanged(nameof(ScheduleVisibility));
            }
        }
        private string _text;
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged(nameof(Text));
            }
        }


        private string _fullText;
        public string FullText
        {
            get => _fullText;
            set
            {
                _fullText = value;
                OnPropertyChanged(nameof(FullText));
            }
        }



        private string _position;
        public string Position
        {
            get => _position;
            set
            {
                _position = value;
                OnPropertyChanged(nameof(Position));
            }
        }

        private string _radius;



        public string Radius
        {
            get => _radius;
            set
            {
                _radius = value;
                OnPropertyChanged(nameof(Radius));
            }
        }
    }
}
