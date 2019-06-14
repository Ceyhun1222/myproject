using System;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace ChartManager.Tests
{
    internal class PropertyTypeOmitter : ISpecimenBuilder
    {
        internal PropertyTypeOmitter(Type type)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }

        internal Type Type { get; }

        public object Create(object request, ISpecimenContext context)
        {
            if (request is PropertyInfo propInfo && propInfo.PropertyType == Type)
                return new OmitSpecimen();
            return new NoSpecimen();
        }
    }
}