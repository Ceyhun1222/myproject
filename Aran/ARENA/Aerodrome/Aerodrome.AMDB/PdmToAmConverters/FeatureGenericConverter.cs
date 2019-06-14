using Aerodrome.Features;
using PDM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aerodrome.Converter
{
    public class FeatureGenericConverter : IGenericConverter
    {
        private readonly IConverterFactory _factory;

        public FeatureGenericConverter(IConverterFactory factory)
        {
            _factory = factory;
        }

        public TTarget Convert<TSource, TTarget>(TSource source)
        {
            var converter = _factory.GetConverter<TSource, TTarget>();
            var amObj = converter.Convert(source);
            CommonProperties(source, amObj);
            //Здесь можно конвертировать свойства приходящие из базового класса(общие для всех конвертируемых классов)
            return amObj;
        }

        public void CommonProperties<TSource, TTarget>(TSource source, TTarget target)
        {
            //LifeTime, ValidTime, Interpretation
            var pdmObj = source as PDMObject;
            if (pdmObj == null) return;

            var amAbstractFeat = target as AM_AbstractFeature;
            if (amAbstractFeat == null) return;

            if (pdmObj.FeatureLifeTime != null)
            {
                amAbstractFeat.stfeat = pdmObj.FeatureLifeTime.BeginPosition;
                amAbstractFeat.endfeat = pdmObj.FeatureLifeTime.EndPosition.Value;
            }
            
            if (pdmObj.ValidTime != null)
            {
                amAbstractFeat.stvalid = pdmObj.ValidTime.BeginPosition;
                amAbstractFeat.endvalid = pdmObj.ValidTime.EndPosition.Value;
            }

            amAbstractFeat.interp = Global.EnumGenericConverter.Convert<dataInterpretation, Enums.AM_InterpretationType>(pdmObj.Interpritation);

            //HorResolution and HorAccuracy
            var amFeatBase = target as AM_FeatureBase;
            if (amFeatBase == null) return;

            if(pdmObj.GeoProperties?.HorizontalAccuracy!=null)
            {
                amFeatBase.hacc = new DataType.DataType<Enums.UomDistance>()
                {
                    Uom = Global.EnumGenericConverter.Convert<UOM_DIST_HORZ, Enums.UomDistance>(pdmObj.GeoProperties.HorizontalAccuracy_UOM),
                    Value = pdmObj.GeoProperties.HorizontalAccuracy.Value
                };
            }

            if (pdmObj.Metadata?.HorizontalResolution != null)
            {
                amFeatBase.hres =new DataType.DataType<Enums.UomHorResolution>()
                {
                    Uom = Enums.UomHorResolution.Decimal_Degrees,
                    Value = pdmObj.Metadata.HorizontalResolution.Value
                };
            }

            //VerResolution and VerAccuracy
            var amVertQuality = target as AM_FeatureVerticalQuality;
            if (amVertQuality == null) return;

            if (pdmObj.GeoProperties?.VerticalAccuracy != null)
            {
                amVertQuality.vacc = new DataType.DataType<Enums.UomDistance>()
                {
                    Uom = Global.EnumGenericConverter.Convert<UOM_DIST_HORZ, Enums.UomDistance>(pdmObj.GeoProperties.VerticalAccuracy_UOM),
                    Value = pdmObj.GeoProperties.VerticalAccuracy.Value
                };
            }

            if (pdmObj.Metadata?.VerticalResolution != null)
            {
                amVertQuality.vres = new DataType.DataType<Enums.UomVerResolution>
                {
                    Uom = Enums.UomVerResolution.meters,
                    Value = pdmObj.Metadata.VerticalResolution.Value
                };
            }
        }
    }
}