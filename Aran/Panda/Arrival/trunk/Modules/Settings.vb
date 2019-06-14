Imports Aran.Package

Public Class Settings
	Implements IPackable
	'Implements IVersion

	Private Version As Byte

	Public Sub New()

	End Sub

	Sub Unpack(reader As PackageReader)
		'Dim I As Integer
		'Dim count As Integer
		Version = reader.GetByte()

		'If Version = 1 Then
		'	_userInterface.Unpack(reader)
		'	_db.Unpack(reader)

		'	count = reader.GetInt32()
		'	For I = 0 To count - 1
		'		Layers.Add(reader.GetEnum(Of LayerInfo)())
		'	Next
		'End If
	End Sub

	Sub Pack(writer As PackageWriter)
		'Dim I As Integer
		'Dim count As Integer

		'writer.PutByte(Version)
		'_userInterface.Pack(writer)
		'_db.Pack(writer)
		'count = Layers.Count

		'writer.PutInt32(count)
		'For I = 0 To count - 1
		'	writer.PutEnum(Of Layer)(lyr)
		'Next
	End Sub
End Class

Public Class UserInterface
	Implements IPackable
	'Implements IVersion

	Public Sub New()
            _speed = new Speed ();
            _elevation = new Elevation ();
            _distance = new Distance ();
            _gradientAccuracy = 0.01;
            _angleAccuracy = 0.01;
            _dirAngleAccuracy = 0.01;
	End Sub

        public Speed Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = value;
            }
        }

        public Elevation Elevation
        {
            get
            {
                return _elevation;
            }
            set
            {
                _elevation = value;
            }
        }

        public Distance Distance
        {
            get
            {
                return _distance;
            }
            set
            {
                _distance = value;
            }
        }

        public Languages Language
        {
            get
            {
                return _language;
            }
            set
            {
                _language = value;
            }
        }

        public double GradientAccuracy
        {
            get
            {
                return _gradientAccuracy;
            }
            set
            {
                _gradientAccuracy = value;
            }
        }

        public double AngleAccuracy
        {
            get
            {
                return _angleAccuracy;
            }
            set
            {
                _angleAccuracy = value;
            }
        }

        public double DirectionalAngleAccuracy
        {
            get
            {
                return _dirAngleAccuracy;
            }
            set
            {
                _dirAngleAccuracy = value;
            }
        }

        public void Pack ( PackageWriter writer )
        {
            _speed.Pack ( writer );
            _elevation.Pack ( writer );
            _distance.Pack ( writer );
            writer.PutEnum<Languages> ( _language );
            writer.PutDouble ( _gradientAccuracy );
            writer.PutDouble ( _angleAccuracy );
            writer.PutDouble ( _dirAngleAccuracy );
        }

        public void Unpack( PackageReader reader )
        {
            if ( Version == 1 )
            {
                _speed.Unpack ( reader );
                _elevation.Unpack ( reader );
                _distance.Unpack ( reader );
                _language = reader.GetEnum<Languages> ();
                _gradientAccuracy = reader.GetDouble ();
                _angleAccuracy = reader.GetDouble ();
                _dirAngleAccuracy = reader.GetDouble ();
            }
        }

        public byte Version
        {
            get
            {
                return _version;
            }
            set
            {
                _version = value;
                _speed.Version = _version;
                _elevation.Version = _version;
                _distance.Version = _version;
            }
        }

        private Speed _speed;
        private Elevation _elevation;
        private Distance _distance;
        private Languages _language;
        private double _gradientAccuracy;
        private double _angleAccuracy;
        private double _dirAngleAccuracy;
        private byte _version;
    }

End Class
