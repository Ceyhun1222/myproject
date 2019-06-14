using PVT.Engine.Common.Utils;
using System;
using System.Collections.Generic;
using PVT.Model;
using Aran.Geometries.Operators;
using System.IO;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.DataSourcesGDB;
using Aran.Converters;
using ESRI.ArcGIS.esriSystem;
using Navaid = PVT.Model.Navaid;
using ObstacleAssessmentArea = PVT.Model.ObstacleAssessmentArea;
using RunwayDirection = PVT.Model.RunwayDirection;
using SegmentLeg = PVT.Model.SegmentLeg;
using TerminalSegmentPoint = PVT.Model.TerminalSegmentPoint;
using VerticalStructure = PVT.Model.VerticalStructure;
using VerticalStructurePart = PVT.Model.VerticalStructurePart;

namespace PVT.Engine.IAIM.Utils
{
    internal class Utils : GDBExportBase, IUtils
    {
        private readonly ProcedureGDBExport _procedureGbExport = new ProcedureGDBExport();
        private readonly HoldingGDBExport _holdingGdbExport= new HoldingGDBExport();

        public void ExportToGDB(string folder, Model.Feature feature)
        {
            ProcedureBase procedure = feature as ProcedureBase;
            if(procedure != null)
                 _procedureGbExport.ExportToGDB(folder, procedure);
            HoldingPattern pattern = feature as HoldingPattern;
                _holdingGdbExport.ExportToGDB(folder, pattern);
        }
    }
}
