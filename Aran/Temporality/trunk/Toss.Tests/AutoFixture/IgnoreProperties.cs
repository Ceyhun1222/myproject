using System;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Toss.Tests.AutoFixture
{
    public class IgnoreProperties : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var propInfo = request as PropertyInfo;
            if (propInfo != null && !IsValid(propInfo.PropertyType))
                return new OmitSpecimen();

            return new NoSpecimen();
        }

        private bool IsValid(Type type)
        {
            return type.IsPrimitive
                   || type == typeof(string) || type == typeof(Guid) || (Nullable.GetUnderlyingType(type) != null);
        }
    }
}
