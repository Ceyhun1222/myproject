using Aerodrome.Enums;
using PDM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aerodrome.Converter
{
    class EnumConverterFactory : IConverterFactory
    {
        public EnumConverterFactory()
        {           
            //Enums
            RegisterConverter<UOM_DIST_VERT, UomDistance>(() => new UomDistVertConverter());
            RegisterConverter<UOM_DIST_HORZ, UomDistance>(() => new UomDistHorzConverter());
            RegisterConverter<CodeSurfaceCompositionType, SurfaceComposition>(() => new SurfaceCompositionConverter());
            RegisterConverter<dataInterpretation, AM_InterpretationType>(() => new InterpretationConverter());
            RegisterConverter<CodeDirectionType, AM_Direction>(() => new DirectionTypeConverter());
            RegisterConverter<ColourType, AM_Color>(() => new ColourTypeConverter());
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
