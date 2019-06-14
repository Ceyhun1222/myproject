using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.AranEnvironment;
using ARAN.Contracts.Registry;
using ARAN.Contracts.Settings;
using ARAN.AIXMTypes;
using System.Runtime.InteropServices;
using ARAN.Common;
using ARAN.Contracts.GeometryOperators;

namespace ObjectDirectory
{
    public class ObjectDirectoryServicePlugin : IAranPlugin
    {
        private SRPackageReader _srReader;
        private SRPackageWriter _srWriter;

        public ObjectDirectoryServicePlugin ()
        {
            _entryPointMethod = new Registry_Contract.Method (EntryPoint);
            _entryPointHandle = GCHandle.Alloc (_entryPointMethod);
            _srReader = new SRPackageReader();
            _srWriter = new SRPackageWriter();
            _geomOperators = new GeometryOperators();
        }

        public void Startup (IAranEnvironment aranEnv)
        {
            Global.Env = aranEnv;
            Registry_Contract.RegisterClass ("ObjectDirectory", 0, _entryPointMethod);
        }

        public int EntryPoint (Int32 _this, Int32 command, Int32 inOut)
        {
            Int32 result  = Registry_Contract.rcOK;
            _srReader.Handle = inOut;
            _srWriter.Handle = inOut;
            try
            {
                switch (command)
                {
                    case Registry_Contract.svcGetInstance:
                        _service = new Service ();
                        break;
                    case Registry_Contract.svcFreeInstance:
                        _entryPointHandle.Free ();
                        break;

                    case (int) Commands.Connect:
                        {
                            _service.Connect ();
                        }
                        break;

                    case (int) Commands.GetAedromeList:
                        {
                            int count = Registry_Contract.GetInt32 (inOut);
                            List<string> strList = new List<string> ();
                            for (int i = 0; i < count; i++)
                            {
                                string s = Registry_Contract.GetString (inOut);
                                strList.Add (s);
                            }

                            ARAN.Common.PandaList<UnicalName> adhpList = _service.GetUnicalAeodomeList(strList);
                            adhpList.Pack (inOut);
                        }
                        break;
                    case (int)Commands.GetRWYList:
                        {
                            string adhpGuid = Registry_Contract.GetString(inOut);
                            ARAN.Common.PandaList<UnicalName> rwyList = _service.GetUnicalRwyList(adhpGuid);
                            rwyList.Pack(inOut);
                            break;
                        }
                    case (int)Commands.GetRWY:
                        {
                            string rwyGuid = Registry_Contract.GetString(inOut);
                            Rwy rwy = _service.GetRWY(rwyGuid);
                            rwy.Pack(inOut);
                            break;
                        }
                    case (int)Commands.GetRWYDirectionList:
                        {
                            string rwyGuid = Registry_Contract.GetString(inOut);
                            Ahp ahp = new Ahp();
                            //ahp.UnPack(inOut);
                            PandaList<RwyDirection> rwyDirectionList = _service.GetRWYDirectionList(rwyGuid);
                            ahp.UnPack(inOut);
                            rwyDirectionList.Pack(inOut);
                            break;
                        }
                    case (int) Commands.GetAedrome:
                        {
                            string adhpGuid = Registry_Contract.GetString (inOut);
                            Ahp ahp = _service.GetAedrome (adhpGuid);
                            if (ahp != null)
                                ahp.Pack (inOut);
                        }
                        break;

                    case (int) Commands.SetToSpatialReference:
                        {
                            _service.GeoSp.UnPack (inOut);
                        }
                        break;
                    case (int) Commands.GetToSpatialReference:
                        {
                            _service.GeoSp.Pack (inOut);
                        }
                        break;
                    case (int) Commands.DisConnect:
                        {
                            _service.Disconnect ();
                        }
                        break;
                    case (int) Commands.GetObstacles:
                        {
                            ARAN.GeometryClasses.Point ptCenter = new ARAN.GeometryClasses.Point ();
                            ptCenter.UnPack (inOut);
                            double range = Registry_Contract.GetDouble (inOut);
                            ARAN.Common.PandaList<Obstacle> obstacleList = _service.GetObstacleList (ptCenter, range);
                            obstacleList.Pack (inOut);
                        }
                        break;
                    case (int) Commands.GetSignificantPoints:
                        {
                            string aerodromeId = Registry_Contract.GetString (inOut);
                            Ahp ahp = new Ahp ();
                            ahp.UnPack (inOut);
                            Aran.Geometries.Point ptCentre = new Aran.Geometries.Point ();
                            SRPackageReader srReader = new SRPackageReader(inOut);
                            ptCentre.Unpack (srReader);
                            double range = Registry_Contract.GetDouble (inOut);
                            Aran.Geometries.Point tmpPtCentre = new Aran.Geometries.Point(ptCentre.X, ptCentre.Y, ptCentre.Z);
                            ARAN.Collection.PandaCollection sgfPoints =_service.GetSignificantPointsList(ahp.GetAIXMId(), tmpPtCentre, range);
                            sgfPoints.Pack(inOut);
                        }
                        break;
                }

                return result;
            }
            catch (Exception)
            {
                return Registry_Contract.rcException;
            }
        }

        private Registry_Contract.Method _entryPointMethod;
        private Service _service;
        private GCHandle _entryPointHandle;
        private GeometryOperators _geomOperators;



        public string Name
        {
            get { return "ObjectDirectory"; }
        }

        public void AddChildSubMenu(List<string> hierarcy)
        {
            throw new NotImplementedException();
        }
    }

    public enum Commands
    {
        GetAedromeList = 0,
        GetAedrome,
        GetRWYList,
        GetRWY,
        SetToSpatialReference,
        GetToSpatialReference,
        Connect,
        DisConnect,
        GetConnectionInfo,
        GetRWYDirectionList,
        GetSignificantPoints,
        GetObstacles,

        SetProcedure
    }
}
