using System.Collections.Generic;
using System.ServiceModel;

namespace AerodromeServices.Helpers
{
    public class WcfOperationContext : IExtension<OperationContext>
    {
        private WcfOperationContext()
        {
            Items = new Dictionary<string, object>();
        }

        public IDictionary<string, object> Items { get; }

        public static WcfOperationContext Current
        {
            get
            {
                if (OperationContext.Current == null) return null;

                var context = OperationContext.Current.Extensions.Find<WcfOperationContext>();
                if (context == null)
                {
                    context = new WcfOperationContext();
                    OperationContext.Current.Extensions.Add(context);
                }
                return context;
            }
        }

        public void Attach(OperationContext owner) { }
        public void Detach(OperationContext owner) { }
    }
}
