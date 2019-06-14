using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim;
using Aran.Aim.Features;

namespace Aran45ToAixm
{
    public abstract class ConverterToAixm51
    {
        public abstract void OpenFile (string fileName);

        public abstract List<Aran.Aim.Features.Feature> ConvertFeature<TypeField> (List<string> errorList);

        public abstract List<List<Aran.Aim.Features.Feature>> PostConvert<TypeField> (List<Aran.Aim.Features.Feature> featList, List<string> errorList);
        
        public List<Aran.Aim.Features.Feature> ConvertFeatureType (Type fieldType, List<string> errorList)
        {
            var methodInfo = this.GetType ().GetMethod ("ConvertFeature");
            methodInfo = methodInfo.MakeGenericMethod (fieldType);
            var result = methodInfo.Invoke (this, new object [] { errorList });
            return result as List<Aran.Aim.Features.Feature>;
        }

        public List<List<Aran.Aim.Features.Feature>> PostConvertType (Type fieldType, List<Aran.Aim.Features.Feature> featList, List<string> errorList)
        {
            var methodInfo = this.GetType ().GetMethod ("PostConvert");
            methodInfo = methodInfo.MakeGenericMethod (fieldType);
            var result = methodInfo.Invoke (this, new object [] { featList, errorList });
            return result as List<List<Aran.Aim.Features.Feature>>;
        }

        public abstract List<Type> GetFeaturesList ();
    }
}
