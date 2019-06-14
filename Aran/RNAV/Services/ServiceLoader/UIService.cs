using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARAN.Contracts.Registry;
using ARAN.Contracts.UI;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ServiceLoader
{
    public class UIService
    {
        public UIService ()
        {
            _uiServiceEntryPoint = new Registry_Contract.Method (EntryPoint);
            _entryPointHandle = GCHandle.Alloc (_uiServiceEntryPoint);

            _srReader = new SRPackageReader ();
            _srWriter = new SRPackageWriter ();
            
        }

        public Registry_Contract.Method UIServiceEntyPoint
        {
            get { return _uiServiceEntryPoint; }
        }

        private int EntryPoint (Int32 handle, Int32 command, Int32 inout)
        {
            _srReader.Handle = inout;
            _srWriter.Handle = inout;
            _graphics = Globals.Env.Graphics;

            switch (command)
            {
                case Registry_Contract.svcAttachClass:
                    break;

                case Registry_Contract.svcDetachClass:
                    break;

                case Registry_Contract.svcGetInstance:
                    break;
                
                case Registry_Contract.svcFreeInstance:
                    _entryPointHandle.Free ();
                    break;
                
                default:
                    {
                        try
                        {
                            eUICommand uiCommand = (eUICommand) command;
                            switch (uiCommand)
                            {
                                case eUICommand.uiGetEnvInfo:
                                    break;
                                case eUICommand.uiGetRelatedFileName:
                                    break;
                                case eUICommand.uiGetViewProjection:
                                    GetViewProjection();
                                    break;
                                case eUICommand.uiSetViewProjection:
                                    SetViewProjection();
                                    break;
                                case eUICommand.uiGetExtent:
                                    GetExtent(inout);
                                    break;
                                case eUICommand.uiSetExtent:
                                    SetExtent(inout);
                                    break;
                                case eUICommand.uiGetDocumentLayerList:
                                    break;
                                case eUICommand.uiGetDocumentMap:
                                    break;
                                case eUICommand.uiDisplayMessage:
                                    break;
                                case eUICommand.uiDrawPoint:
                                    DrawPoint(inout);
                                    break;
                                case eUICommand.uiDrawPointWithText:
                                    DrawPointWithText(inout);
                                    break;
                                case eUICommand.uiDrawPolyline:
                                    DrawPolyline(inout);
                                    break;
                                case eUICommand.uiDrawPolygon:
                                    DrawPolygon(inout);
                                    break;
                                case eUICommand.uiSetVisible:
                                    break;
                                case eUICommand.uiDeleteGraphic:
                                    DeleteElement(inout);
                                    break;
                                case eUICommand.uiControlSetChecked:
                                    break;
                                case eUICommand.uiControlSetEnabled:
                                    int id =Registry_Contract.GetInt32(inout);
                                    bool value = Registry_Contract.GetBool(inout);
                                    break;
                                case eUICommand.uiCreateCommandButton:
                                    string name = Registry_Contract.GetString(inout);
                                    Registry_Contract.PutInt32(inout, 0);
                                    break;
                                default:
                                    
                                    break;
                            }
                           
                        }
                        catch (Exception)
                        {
                            return Registry_Contract.rcException;
                        }
                    }
                    break;
            }
            return Registry_Contract.rcOK;
        }

        private void GetViewProjection ()
        {
            Aran.Geometries.SpatialReferences.SpatialReference sr =
                _graphics.ViewProjection;

            bool notNull = (sr != null);
            _srWriter.PutBool (notNull);

            if (notNull)
                sr.Pack (_srWriter);
        }

        private void SetViewProjection()
        {
            try
            {
                Aran.Geometries.SpatialReferences.SpatialReference sr = new Aran.Geometries.SpatialReferences.SpatialReference();
                sr.Unpack(_srReader);
                _graphics.ViewProjection = sr;
            }
            catch (Exception)
            {

                throw new Exception("Error setting ViewProjection!");
            }
        
        }

        private void DrawPoint (int inOut)
        {
            try
            {
                ARAN.GeometryClasses.Point point = new ARAN.GeometryClasses.Point();
                point.UnPack(inOut);

                Aran.AranEnvironment.Symbols.PointSymbol pointSymbol =
                    new Aran.AranEnvironment.Symbols.PointSymbol();
                pointSymbol.Unpack(_srReader);

                int elemHandle = _graphics.DrawPoint(
                    GeometryConvert.ToPoint(point), pointSymbol);

                Registry_Contract.PutInt32(inOut, elemHandle);
            }
            catch (Exception)
            {
                throw new Exception("Error drawing point!");
            }
        }

        private void DrawPointWithText(int inOut) 
        {
            try
            {
               ARAN.GeometryClasses.Point point = new ARAN.GeometryClasses.Point();
               point.UnPack(inOut);
               Aran.AranEnvironment.Symbols.PointSymbol ptSymbol = new Aran.AranEnvironment.Symbols.PointSymbol();
               ptSymbol.Unpack(_srReader);
               string text = Registry_Contract.GetString(inOut);
               int elemHandle = _graphics.DrawPointWithText(GeometryConvert.ToPoint(point), ptSymbol, text);
               Registry_Contract.PutInt32(inOut,elemHandle);
            }
            catch (Exception)
            {
                throw new Exception("Error drawing point with text!");
            }
        
        }

        private void DrawPolyline(int inOut)
        {
            try
            {
                ARAN.GeometryClasses.PolyLine srPolyLine = new ARAN.GeometryClasses.PolyLine();
                srPolyLine.UnPack(inOut);
                Aran.AranEnvironment.Symbols.LineSymbol lineSymbol = new Aran.AranEnvironment.Symbols.LineSymbol();
                lineSymbol.Unpack(_srReader);
                Aran.Geometries.MultiLineString multiLineString = GeometryConvert.ToMulitLineString(srPolyLine);
                int elemHandle = _graphics.DrawMultiLineString(multiLineString, lineSymbol);
                Registry_Contract.PutInt32(inOut, elemHandle);
            }
            catch (Exception)
            {
                throw new Exception("Error drawing polyline!");
            }
        
        }

        private void DrawPolygon(int inOut)
        {
            try
            {
                ARAN.GeometryClasses.Polygon srPolygon= new ARAN.GeometryClasses.Polygon();
                srPolygon.UnPack(inOut);
                Aran.AranEnvironment.Symbols.FillSymbol fillSymbol = new Aran.AranEnvironment.Symbols.FillSymbol();
                fillSymbol.Unpack(_srReader);
                Aran.Geometries.MultiPolygon aimMPolgon = GeometryConvert.ToPolygon(srPolygon);
                int elemHandle = _graphics.DrawMultiPolygon (aimMPolgon, fillSymbol);
                Registry_Contract.PutInt32(inOut, elemHandle);
            }
            catch (Exception)
            {

                throw new Exception("Error drawing polygon!");
            }
        
        }

        private void GetExtent(int inOut)
        {
            try
            {
                Aran.Geometries.Box extent = _graphics.Extent;
                Registry_Contract.PutDouble(inOut, extent[0].X);
                Registry_Contract.PutDouble(inOut, extent[0].Y);
                Registry_Contract.PutDouble(inOut, extent[1].X);
                Registry_Contract.PutDouble(inOut, extent[1].Y);
            }
            catch (Exception)
            {
                throw new Exception("Error getting the extent!");
            }
          
        }

        private void SetExtent(int inOut)
        {
            try
            {
                double xMin = Registry_Contract.GetDouble(inOut);
                double yMin = Registry_Contract.GetDouble(inOut);
                double xMax = Registry_Contract.GetDouble(inOut);
                double yMax = Registry_Contract.GetDouble(inOut);
                _graphics.SetExtent(xMin, yMin, xMax, yMax);
            }
            catch (Exception)
            {
                throw new Exception("Error setting extent");
            }
            
        }

        private void DeleteElement(int inOut)
        {
            try
            {
                int id = Registry_Contract.GetInt32(inOut);
                _graphics.DeleteGraphic(id);
            }
            catch (Exception)
            {
                throw new Exception("Error deleting element");
            }
        }
        
        private SRPackageWriter _srWriter;
        private SRPackageReader _srReader;

        private Registry_Contract.Method _uiServiceEntryPoint;
        private GCHandle _entryPointHandle;
        private Aran.AranEnvironment.IAranGraphics _graphics;
    }
}
