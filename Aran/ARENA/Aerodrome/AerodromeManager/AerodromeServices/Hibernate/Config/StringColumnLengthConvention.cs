using AerodromeServices.Hibernate.Attribute;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace AerodromeServices.Hibernate.Config
{
    public class StringColumnLengthConvention : IPropertyConvention, IPropertyConventionAcceptance
    {
        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
        {
            criteria.Expect(x => x.Type == typeof(string)).Expect(x => x.Length == 0);
        }

        public void Apply(IPropertyInstance instance)
        {
            var leng = 255;

            var myMemberInfos = ((PropertyInstance)instance).EntityType.GetMember(instance.Name);
            if (myMemberInfos.Length > 0)
            {
                object[] myCustomAttrs = myMemberInfos[0].GetCustomAttributes(false);
                if (myCustomAttrs.Length > 0)
                {
                    if (myCustomAttrs[0] is StringLength length)
                    {
                        leng = length.Length;
                    }
                }
            }
            instance.Length(leng);
        }
    }
}