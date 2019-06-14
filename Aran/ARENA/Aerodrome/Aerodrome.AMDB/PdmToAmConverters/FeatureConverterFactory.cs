using Aerodrome.Features;
using PDM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aerodrome.Converter
{
    public class FeatureConverterFactory : IConverterFactory
    {
        public FeatureConverterFactory()
        {
            //Features
            RegisterConverter<AirportHeliport, AM_AerodromeReferencePoint>(() => new AirportConverter());
            RegisterConverter<Runway, AM_Runway>(() => new RunwayConverter());
            RegisterConverter<Taxiway, AM_Taxiway>(() => new TaxiwayConverter());
            RegisterConverter<RunwayDirection, AM_RunwayDirection>(() => new RwyDirectionConverter());
            RegisterConverter<RunwayElement, AM_RunwayElement>(() => new RwyElementConverter());
            RegisterConverter<RunwayCenterLinePoint, AM_RunwayCenterlinePoint>(() => new RwyCenterLinePntConverter());
            RegisterConverter<GuidanceLine, AM_TaxiwayGuidanceLine>(() => new TwyGuidanceLineConverter());
            
        }
        public Dictionary<Tuple<Type, Type>, Func<object>> _converters { get; set; } = new Dictionary<Tuple<Type, Type>, Func<object>>();

        public void RegisterConverter<TSource, TTarget>(Func<object> constructor)
        {
            _converters.Add(new Tuple<Type, Type>(typeof(TSource), typeof(TTarget)), constructor);
        }

        public IConverter<TSource, TTarget> GetConverter<TSource, TTarget>()
        {
            var constructor = _converters[new Tuple<Type, Type>(typeof(TSource), typeof(TTarget))];
            return (IConverter<TSource, TTarget>)constructor();
        }
    }
}
