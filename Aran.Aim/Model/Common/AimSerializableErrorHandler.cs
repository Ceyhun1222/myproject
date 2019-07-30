using System;

namespace Aran.Aim
{
    public delegate void SerializableEventHandler (object sender, Exception ex);

    public class AimSerializableErrorHandler
    {
        public static event SerializableEventHandler SerializableEvent;

        public static void DoSerializableEvent (object sender, Exception ex)
        {
            if (SerializableEvent != null)
                SerializableEvent (sender, ex);
        }
    }
}
