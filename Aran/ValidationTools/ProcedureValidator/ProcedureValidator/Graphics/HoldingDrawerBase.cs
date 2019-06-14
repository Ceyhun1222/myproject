using System;
using System.Collections.Generic;
using PVT.Model;
using PVT.Model.Drawing;

namespace PVT.Graphics
{
    public abstract class HoldingDrawerBase : DrawerBase, IHoldingDrawer
    {
        private  Dictionary<Guid, HandlerBase> HoldingPatterns => Handlers[typeof(HoldingHanler)];
     


        protected HoldingDrawerBase()
        {
            Handlers = new Dictionary<Type, Dictionary<Guid, HandlerBase>>
            {
                {typeof(HoldingHanler), new Dictionary<Guid, HandlerBase>()},
            };
            Style = new Styles();
        }


        public void Draw(HoldingPattern pattern)
        {
            if (!IsEnabled()) return;
            if (HoldingPatterns.ContainsKey(pattern.Identifier))
                return;
            var handler = new HoldingHanler(DrawHoldingPattern(pattern));
            HoldingPatterns.Add(handler.Identifier, handler);
        }

        public void Clean(HoldingPattern obj)
        {
            if (HoldingPatterns.ContainsKey(obj.Identifier))
            {
                Delete(HoldingPatterns[obj.Identifier].Handler);
                HoldingPatterns.Remove(obj.Identifier);
            }
        }

        protected abstract DrawObject DrawHoldingPattern(HoldingPattern pattern);

        public virtual void Draw(HoldingPattern pattern, int index)
        {
            
        }

        public virtual void Clean(HoldingPattern pattern, int index)
        {
            
        }
    }
}