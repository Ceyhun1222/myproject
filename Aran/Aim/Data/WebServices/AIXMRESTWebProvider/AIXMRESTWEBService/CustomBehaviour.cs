using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Web;

namespace Aran.Aim.Data.WebServices.AIXMRESTService
{
    public class CustomBehavior : WebHttpBehavior
    {
        protected override QueryStringConverter GetQueryStringConverter(OperationDescription operationDescription)
        {
            return new TypeConverter(base.GetQueryStringConverter(operationDescription));
        }
    }

    public class FuatureTypeBehaviorExtension : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get { return typeof(CustomBehavior); }
        }

        protected override object CreateBehavior()
        {
            return new CustomBehavior();
        }
    }
}