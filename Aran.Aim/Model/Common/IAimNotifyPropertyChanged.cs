using System;

namespace Aran.Aim
{
    public delegate void AimPropertyChangedEventHandler ( object sender, AimPropertyChangedEventArgs e);

    public interface IAimNotifyPropertyChanged
    {
        event AimPropertyChangedEventHandler AimPropertyChanged;
		event AimPropertyChangedEventHandler Assigned;
    }

    public class AimPropertyChangedEventArgs : EventArgs
    {
        public AimPropertyChangedEventArgs (int propertyIndex)
        {
            PropertyIndex = propertyIndex;
        }

        public int PropertyIndex { get; private set; }
    }
}
