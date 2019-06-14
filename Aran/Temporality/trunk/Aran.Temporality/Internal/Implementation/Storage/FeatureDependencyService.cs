using System.Collections.Generic;
using System.Linq;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Entity.Util;
using Aran.Temporality.Common.Util;
using Aran.Temporality.Internal.Abstract;
using Aran.Temporality.Internal.WorkFlow.Routines;

namespace Aran.Temporality.Internal.Implementation.Storage
{
    internal class FeatureDependencyService<T> where T : class
    {
        public ValidationResult Validate(LightFeature lightFeature, CompressedFeatureDependencyConfiguration dependency)
        {
            //check for error
            if (dependency.MandatoryProperties!=null)
            {
                //check field completeness
                if (dependency.MandatoryProperties.Any(prop => lightFeature.Fields.All(t => t.Name != prop)))
                {
                    return ValidationResult.Error;
                }
            }

            if (dependency.Children!=null)
            {
                if (dependency.Children.Any(subDependency => subDependency.MandatoryLinks.Any(linkPath => lightFeature.Links.All(t => t.Name != linkPath))))
                {
                    return ValidationResult.Error;
                }
            }
            

            if (dependency.MandatoryLinks != null)
            {
                //check link completeness
                if (dependency.MandatoryLinks.Any(link => lightFeature.Links.All(t => t.Name != link)))
                {
                    return ValidationResult.Error;
                }


                foreach(var link in lightFeature.Links.Where(t=>dependency.MandatoryLinks.Contains(t.Name)))
                {

                }
            }


            //here error was not detected----------------------------------------
            //check for warning
            if (dependency.OptionalProperties != null)
            {
                if (dependency.OptionalProperties.Any(prop => lightFeature.Fields.All(t => t.Name != prop)))
                {
                    return ValidationResult.Warning;
                }
            }

            if (dependency.OptionalLinks != null)
            {
                if (dependency.OptionalLinks.Any(prop => lightFeature.Links.All(t => t.Name != prop)))
                {
                    return ValidationResult.Warning;
                }
            }

            //here even warning was not detected---------------------------------
            return ValidationResult.Ok;
        }

        //public LightFeature GetLightFeature(Feature feature, CompressedFeatureDependencyConfiguration dependency)
        //{
        //    dependency.
        //}

        public void GetConfigurationData(int id, AbstractTemporalityService<T> service)
        {
            FeatureDependencyConfiguration dependency = StorageService.GetFeatureDependencyById(id);
            var result=new List<LightFeature>();
        }
    }
}
