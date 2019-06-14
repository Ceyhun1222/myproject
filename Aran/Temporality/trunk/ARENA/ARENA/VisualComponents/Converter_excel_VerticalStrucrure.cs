using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PDM.PropertyExtension;
using PDM;

namespace ARENA
{
    public class Intermediate_VerticalStrucrure 
    {

        private string _lat;
        [Mandatory(true)]
        [PropertyOrder(580)]
        public string Lat
        {
            get { return _lat; }
            set { _lat = value; }
        }

        private string _lon;
        [Mandatory(true)]
        [PropertyOrder(590)]
        public string Lon
        {
            get { return _lon; }
            set { _lon = value; }
        }

        private string _name;
        [Mandatory(true)]
        [PropertyOrder(600)]
        //[Description("Type of the navaid service.")]
        public string VertcalStructureName
        {
            get { return _name; }
            set { _name = value; }
        }

        private VerticalStructureType _verticalStructureType;
        //[Mandatory(true)]
        [PropertyOrder(610)]
        //[Description("Type of the navaid service.")]
        public VerticalStructureType VerticalStructureType
        {
            get { return _verticalStructureType; }
            set { _verticalStructureType = value; }
        }

        private string _group;
        //[Mandatory(true)]
        [PropertyOrder(620)]
        //[Description("Type of the navaid service.")]
        public string Group
        {
            get { return _group; }
            set { _group = value; }
        }

        private double _length;
        //[Mandatory(true)]
        [PropertyOrder(630)]
        //[Description("Type of the navaid service.")]
        public double Length
        {
            get { return _length; }
            set { _length = value; }
        }

        private UOM_DIST_HORZ _length_UOM;
        //[Mandatory(true)]
        [PropertyOrder(640)]
        //[Description("Type of the navaid service.")]
        public UOM_DIST_HORZ Length_UOM
        {
            get { return _length_UOM; }
            set { _length_UOM = value; }
        }

        private double _width;
        //[Mandatory(true)]
        [PropertyOrder(650)]
        //[Description("Type of the navaid service.")]
        public double Width
        {
            get { return _width; }
            set { _width = value; }
        }

        private UOM_DIST_HORZ _width_UOM;
        //[Mandatory(true)]
        [PropertyOrder(660)]
        //[Description("Type of the navaid service.")]
        public UOM_DIST_HORZ Width_UOM
        {
            get { return _width_UOM; }
            set { _width_UOM = value; }
        }

        private double _radius;
        //[Mandatory(true)]
        [PropertyOrder(670)]
        //[Description("Type of the navaid service.")]
        public double Radius
        {
            get { return _radius; }
            set { _radius = value; }
        }

        private UOM_DIST_HORZ _radius_UOM;
        //[Mandatory(true)]
        [PropertyOrder(680)]
        //[Description("Type of the navaid service.")]
        public UOM_DIST_HORZ Radius_UOM
        {
            get { return _radius_UOM; }
            set { _radius_UOM = value; }
        }


        private bool _markingICAOStandard;
        //[Mandatory(true)]
        [PropertyOrder(690)]
        //[Description("Type of the navaid service.")]
        public bool MarkingICAOStandard
        {
            get { return _markingICAOStandard; }
            set { _markingICAOStandard = value; }
        }

        private bool _lighted;
        //[Mandatory(true)]
        [PropertyOrder(700)]
        //[Description("Type of the navaid service.")]
        public bool Lighted
        {
            get { return _lighted; }
            set { _lighted = value; }
        }

        private bool _lightingICAOStandard;
        //[Mandatory(true)]
        [PropertyOrder(710)]
        //[Description("Type of the navaid service.")]
        public bool LightingICAOStandard
        {
            get { return _lightingICAOStandard; }
            set { _lightingICAOStandard = value; }
        }

        private bool _synchronisedLighting;
        //[Mandatory(true)]
        [PropertyOrder(720)]
        //[Description("Type of the navaid service.")]
        public bool SynchronisedLighting
        {
            get { return _synchronisedLighting; }
            set { _synchronisedLighting = value; }
        }

        private string _designator;
        [Mandatory(true)]
        [PropertyOrder(730)]
        //[Description("Type of the navaid service.")]
        public string PartDesignator
        {
            get { return _designator; }
            set { _designator = value; }
        }

        private double _height;
        //[Mandatory(true)]
        [PropertyOrder(740)]
        //[Description("Type of the navaid service.")]
        public double PartHeight
        {
            get { return _height; }
            set { _height = value; }
        }

        private UOM_DIST_VERT _height_UOM;
        //[Mandatory(true)]
        [PropertyOrder(750)]
        //[Description("Type of the navaid service.")]
        public UOM_DIST_VERT PartHeight_UOM
        {
            get { return _height_UOM; }
            set { _height_UOM = value; }
        }

        private StatusConstructionType _constructionStatus;
        //[Mandatory(true)]
        [PropertyOrder(760)]
        //[Description("Type of the navaid service.")]jr
        public StatusConstructionType PartConstructionStatus
        {
            get { return _constructionStatus; }
            set { _constructionStatus = value; }
        }

        private bool _frangible;
        //[Mandatory(true)]
        [PropertyOrder(770)]
        //[Description("Type of the navaid service.")]
        public bool PartFrangible
        {
            get { return _frangible; }
            set { _frangible = value; }
        }

        private ColourType _markingFirstColour;
        //[Mandatory(true)]
        [PropertyOrder(780)]
        //[Description("Type of the navaid service.")]
        public ColourType PartMarkingFirstColour
        {
            get { return _markingFirstColour; }
            set { _markingFirstColour = value; }
        }

        private VerticalStructureMarkingType _markingPattern;
        //[Mandatory(true)]
        [PropertyOrder(790)]
        //[Description("Type of the navaid service.")]
        public VerticalStructureMarkingType PartMarkingPattern
        {
            get { return _markingPattern; }
            set { _markingPattern = value; }
        }

        private ColourType _markingSecondColour;
        //[Mandatory(true)]
        [PropertyOrder(800)]
        //[Description("Type of the navaid service.")]
        public ColourType PartMarkingSecondColour
        {
            get { return _markingSecondColour; }
            set { _markingSecondColour = value; }
        }

        private VerticalStructureType _type;
        //[Mandatory(true)]
        [PropertyOrder(820)]
        //[Description("Type of the navaid service.")]
        public VerticalStructureType PartType
        {
            get { return _type; }
            set { _type = value; }
        }

        private double _verticalExtent;
        //[Mandatory(true)]
        [PropertyOrder(830)]
        //[Description("Type of the navaid service.")]
        public double PartVerticalExtent
        {
            get { return _verticalExtent; }
            set { _verticalExtent = value; }
        }

        private UOM_DIST_VERT _verticalExtent_UOM;
        //[Mandatory(true)]
        [PropertyOrder(840)]
        //[Description("Type of the navaid service.")]
        public UOM_DIST_VERT PartVerticalExtent_UOM
        {
            get { return _verticalExtent_UOM; }
            set { _verticalExtent_UOM = value; }
        }

        private double _verticalExtentAccuracy;
        //[Mandatory(true)]
        [PropertyOrder(850)]
        //[Description("Type of the navaid service.")]
        public double PartVerticalExtentAccuracy
        {
            get { return _verticalExtentAccuracy; }
            set { _verticalExtentAccuracy = value; }
        }

        private UOM_DIST_VERT _verticalExtentAccuracy_UOM;
        //[Mandatory(true)]
        [PropertyOrder(860)]
        //[Description("Type of the navaid service.")]
        public UOM_DIST_VERT PartVerticalExtentAccuracy_UOM
        {
            get { return _verticalExtentAccuracy_UOM; }
            set { _verticalExtentAccuracy_UOM = value; }
        }

        private VerticalStructureMaterialType _visibleMaterial;
        //[Mandatory(true)]
        [PropertyOrder(870)]
        //[Description("Type of the navaid service.")]
        public VerticalStructureMaterialType PartVisibleMaterial
        {
            get { return _visibleMaterial; }
            set { _visibleMaterial = value; }
        }


    }
}
