using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace Toss.Tests.AutoFixture
{
    class CustomDateTimeSequenceGenerator : ISpecimenBuilder
    {
        private readonly ISpecimenBuilder _builder;

        public CustomDateTimeSequenceGenerator(ISpecimenBuilder builder)
        {
            _builder = builder;
        }

        public object Create(object request, ISpecimenContext context)
        {
            var t = request as Type;
            if (t == null)
                return new NoSpecimen();

            var specimen = _builder.Create(request, context);
            if (!(specimen is DateTime))
                return new NoSpecimen();
            var res = (DateTime)specimen;
            var newDate = new DateTime(res.Year, res.Month, res.Day, res.Hour, res.Minute, res.Second);
            return newDate;
        }
    }
}
