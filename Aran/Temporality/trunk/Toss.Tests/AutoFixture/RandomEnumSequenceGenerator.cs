using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ploeh.AutoFixture.Kernel;

namespace Toss.Tests.AutoFixture
{
    public class RandomEnumSequenceGenerator : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            var propInfo = request as PropertyInfo;
            if (propInfo != null)
            {
                var nullable = Nullable.GetUnderlyingType(propInfo.PropertyType);
                if (propInfo.PropertyType.IsEnum || (nullable != null && nullable.IsEnum))
                {
                    List<object> enumValues;
                    if (propInfo.PropertyType.IsEnum)
                        enumValues = propInfo.PropertyType.GetEnumValues().Cast<object>().ToList();
                    else
                        enumValues = nullable.GetEnumValues().Cast<object>()
                            .ToList();
                    Random random = new Random();
                    var randomIndex = random.Next(0, enumValues.Count);
                    var result = enumValues[randomIndex];
                    return result;
                }
            }
            return new NoSpecimen();
        }
    }

}
