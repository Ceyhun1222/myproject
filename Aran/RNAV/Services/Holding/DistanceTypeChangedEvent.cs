using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Holding
{
    public delegate void DistanceTypeChangedEventHandler(object sender, DistanceTypeChangedEventArgs e);

      public class DistanceTypeChangedEventArgs : EventArgs
      {
          public bool CanChanged { get; set; }
          Aran.Geometries.Point pt;
          
      }
   
}
