using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;

namespace MapEnv
{
    internal class AranDrawing : AranDrawingBase
    {
        public AranDrawing(AxPageLayoutControl axLayoutControl) : base()
        {
            _axLayoutControl = axLayoutControl;
        }

        protected override IActiveView ActiveView
        {
            get
            {
                return _axLayoutControl.ActiveView;
            }
        }

        protected override IGraphicsContainer GraphicContainer
        {
            get
            {
               return ActiveView.GraphicsContainer;
            }
        }

        private AxPageLayoutControl _axLayoutControl;
    }
}
