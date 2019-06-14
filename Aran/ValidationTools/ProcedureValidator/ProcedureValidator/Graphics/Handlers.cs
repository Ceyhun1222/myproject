using System;
using System.Collections.Generic;

namespace PVT.Graphics
{
    public class HandlerBase
    {
        
        protected HandlerBase(Guid id)
        {
            Identifier = id;
        }

        protected HandlerBase(DrawObject obj)
        {
            Identifier = obj.Id;
            Handler = obj;
        }

        public Guid Identifier { get; }
        public DrawObject Handler { get; protected set; }
    }

    class ProcedureHandler : HandlerBase
    {
        public List<TransitionHandler> Transitions { get; }
        public ProcedureHandler(Guid id) : base(id)
        {
            Transitions = new List<TransitionHandler>();
        }
    }

    class TransitionHandler : HandlerBase
    {
        public List<LegHandler> Legs { get; }
        public TransitionHandler(Guid id) : base(id)
        {
            Legs = new List<LegHandler>();
        }
    }

    class LegHandler : HandlerBase
    {
        public LegHandler(DrawObject obj) : base(obj)
        {
        }
    }

    class SignificantPointHandler : HandlerBase
    {
        public SignificantPointHandler(DrawObject obj) : base(obj)
        {
        }
    }

    class HoldingHanler : HandlerBase
    {
        public HoldingHanler(DrawObject obj) : base(obj)
        {
        }
    }

}