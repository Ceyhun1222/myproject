using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using AIMSLServiceClient.Services.Encoder;

namespace AIMSLServiceClient.Services.Extensions
{
    public class UnsecureMessageEncodingElement : BindingElementExtensionElement
    {
        public override void ApplyConfiguration(BindingElement bindingElement)
        {
            base.ApplyConfiguration(bindingElement);
            UnsecureMessageEncodingBindingElement binding = (UnsecureMessageEncodingBindingElement)bindingElement;
        }

      

        public override Type BindingElementType { get; } = typeof(UnsecureMessageEncodingElement);

        protected override BindingElement CreateBindingElement()
        {
            UnsecureMessageEncodingBindingElement binding = new UnsecureMessageEncodingBindingElement();
            this.ApplyConfiguration(binding);
            return binding;
        }
    }
}
