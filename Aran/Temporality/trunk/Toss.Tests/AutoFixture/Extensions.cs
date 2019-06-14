using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace Toss.Tests
{
    public static class Extensions
    {
        public static object Create(this Fixture fixture, Type type)
        {
            var context = new SpecimenContext(fixture);
            return context.Resolve(type);
        }

        public static ISpecimenBuilder Build(this Fixture fixture, Type type)
        {
            var context = new SpecimenContext(fixture);
            return context.Builder;
        }
    }
}
