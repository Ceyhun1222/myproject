using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Panda.VisualManoeuvring
{
    class VM_Track
    {
        public Guid associatedProcedureId
        {
            get;
            set;
        }
        public List<VM_TrackSegment> trackSteps;

        public VM_Track(Guid id)
        {
            this.associatedProcedureId = id;
            trackSteps = new List<VM_TrackSegment>();
        }
    }
}
