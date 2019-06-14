using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PDM;
using Aran.Aim.Features;

namespace ARENA.DataLoaders
{
    public class PDM_AIM_DataConverter : IARENA_DATA_Converter
    {
        private Environment.Environment _CurEnvironment;

        public Environment.Environment CurEnvironment
        {
            get { return _CurEnvironment; }
            set { _CurEnvironment = value; }
        }

        public PDM_AIM_DataConverter()
        {
        }

        public PDM_AIM_DataConverter(Environment.Environment env)
        {
            this.CurEnvironment = env;
        }

        public bool Convert_Data(ESRI.ArcGIS.Geodatabase.IFeatureClass _FeatureClass)
        {
           
            //CurEnvironment.Data.PdmObjectList

            var objList = (from element in this.CurEnvironment.Data.PdmObjectList
                            where (element != null) &&
                                (element.CreatedAutomatically)
                            select element).ToList();

            List<Aran.Aim.Features.Feature> listOfAimFeatures = new List<Feature>();

            foreach (var item in objList)
            {

                if (item is PDM.Procedure) continue;
                else
                {
                    List<Feature> _lst = PDM_AIM_Converter.PDM_Object_Convert(item);
                    if (_lst != null)
                    {
                        listOfAimFeatures.AddRange(PDM_AIM_Converter.PDM_Object_Convert(item));
                    }


                }
                
            }

            System.Windows.Forms.MessageBox.Show(listOfAimFeatures.Count.ToString());

            return true;
        }
    }
}
