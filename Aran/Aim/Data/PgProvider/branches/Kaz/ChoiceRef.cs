
namespace Aran.Aim.Data
{
    internal class ChoiceRef
    {
        public ChoiceRef ()
        {
        }

        public long Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        public int PropType
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        public bool IsFeature
        {
            get
            {
                return _isFeature;
            }
            set
            {
                _isFeature = value;
            }
        }

        public AimObject AimObj
        {
            get
            {
                return _aimObj;
            }
            set
            {
                _aimObj = value;
            }
        }

        public int ValueType
        {
            get
            {
                return _ValueType;
            }
            set
            {
                _ValueType = value;
            }
        }

        private int _ValueType;
        private AimObject _aimObj;
        public bool _isFeature;
        private long _id;
        private int _type;
    }
}