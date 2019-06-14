//#include <string>
//#include <memory.h>
#include <malloc.h>

#include <Panda/GeometryOperatorsContract/cpp/include/GeometryOperatorsContract.h>
#include <Panda/GeometryOperatorsContract/cpp/include/SpatialReference.h>
#include <Panda/GeometryOperatorsContract/cpp/include/SpatialReferenceEnums.h>

#ifdef MY_WIN64

#import "c:/Program files (x86)/ArcGIS/com/esriSystem.olb" raw_interfaces_only raw_native_types no_namespace named_guids exclude("OLE_COLOR", "OLE_HANDLE", "VARTYPE")
#import "c:/Program files (x86)/ArcGIS/com/esriGeometry.olb" raw_interfaces_only raw_native_types no_namespace named_guids

#else

#import "c:/Program files/ArcGIS/com/esriSystem.olb" raw_interfaces_only raw_native_types no_namespace named_guids exclude("OLE_COLOR", "OLE_HANDLE", "VARTYPE")
#import "c:/Program files/ArcGIS/com/esriGeometry.olb" raw_interfaces_only raw_native_types no_namespace named_guids

#endif

//#include <Esri/EsriClasses/cpp/include/ImportESRI.h>

#include <Panda/Geometry/cpp/include/GeometryType.h>
#include <Panda/Geometry/cpp/include/GeometryPacket.h>
#include <Panda/Geometry/cpp/include/Geometry.h>
#include <Panda/Geometry/cpp/include/NullGeometry.h>
#include <Panda/Geometry/cpp/include/Point.h>
#include <Panda/Geometry/cpp/include/MultiPoint.h>
#include <Panda/Geometry/cpp/include/Poly.h>
#include <Panda/Geometry/cpp/include/Part.h>
#include <Panda/Geometry/cpp/include/Ring.h>
#include <Panda/Geometry/cpp/include/Polyline.h>
#include <Panda/Geometry/cpp/include/Polygon.h>

/*
#include <Esri/EsriClasses/cpp/include/EsriGeometry.h>
#include <Esri/EsriClasses/cpp/include/EsriPacket.h>
#include <Esri/EsriClasses/cpp/include/EsriPoint.h>
#include <Esri/EsriClasses/cpp/include/EsriPointCollection.h>
#include <Esri/EsriClasses/cpp/include/EsriGeometryCollection.h>
*/
using namespace Panda;

namespace PandaToEsri
{
	ISpatialReference* SpatialReference(const Panda::SpatialReference&);
	int SpatialReferenceType(int);
	int SpatialReferenceParamType(int);
	int SpatialReferenceGeoType(int);
	int SpatialReferenceUnit(int);

	ISpatialReference* SpatialReference(const Panda::SpatialReference& pandaSR)
	{
		ISpatialReference* esriSR = 0;
		ISpatialReferenceFactory* srFact = 0;
		IProjection* projection = 0;
		IParameterPtr* params = 0;
		IGeographicCoordinateSystem* gcs = 0;
		int paramCount = 0;

		HRESULT res = CoCreateInstance(
						CLSID_SpatialReferenceEnvironment, 0, CLSCTX_INPROC,
						IID_ISpatialReferenceFactory, (void**)&srFact);
		if (res != S_OK)
			return 0;
		
		if( pandaSR.spatialReferenceType == Panda::SpatialReferenceType::Geographic)
		{
			srFact->CreateGeographicCoordinateSystem(
						PandaToEsri::SpatialReferenceGeoType(pandaSR.ellipsoid.srGeoType), &gcs);
			gcs->QueryInterface<ISpatialReference>(&esriSR);
			srFact->Release();
			gcs->Release();
			return esriSR;
		}
		else
		{
			res = srFact->CreateProjection(
							PandaToEsri::SpatialReferenceType( pandaSR.spatialReferenceType),
							&projection);
			if (res != S_OK)
			{
				srFact->Release();
				return 0;
			}

			paramCount = pandaSR.paramList.size();
			int esriParamCount = ( paramCount < 20 ? 20 : paramCount);
			params = new IParameterPtr[esriParamCount];

			for(int i = 0; i < paramCount; i++)
			{
				srFact->CreateParameter(
					PandaToEsri::SpatialReferenceParamType(pandaSR.paramList.at(i).srParamType),
					&(params[i]));
				params[i]->put_Value( pandaSR.paramList.at(i).value);
			}

			for(int i = paramCount; i < esriParamCount; i++)
				params[i] = 0;

			IUnit* projectedXYUnit;
			srFact->CreateUnit( SpatialReferenceUnit(pandaSR.spatialReferenceUnit), &projectedXYUnit);

			ILinearUnit* linearUnit;
			projectedXYUnit->QueryInterface<ILinearUnit>( &linearUnit);
			projectedXYUnit->Release();

			IProjectedCoordinateSystem* projCS;
			CoCreateInstance(CLSID_ProjectedCoordinateSystem, 0, CLSCTX_INPROC, IID_IProjectedCoordinateSystem, (void**)&projCS);

			IProjectedCoordinateSystemEdit* pcsEdit;
			projCS->QueryInterface<IProjectedCoordinateSystemEdit>( &pcsEdit);
			srFact->CreateGeographicCoordinateSystem(PandaToEsri::SpatialReferenceGeoType( pandaSR.ellipsoid.srGeoType), &gcs);
			srFact->Release();

			pcsEdit->DefineEx(
						_bstr_t (pandaSR.name.c_str ()),
						_bstr_t (pandaSR.name.c_str ()),
						L"",
						L"",
						L"",
						gcs,
						linearUnit,
						projection,
						(IParameter**)params);

			pcsEdit->Release();
			gcs->Release();
			linearUnit->Release();
			projection->Release();

			projCS->QueryInterface<ISpatialReference>(&esriSR);
			projCS->Release();

			for(int i = 0; i < paramCount; i++)
				if(params[i])
					params[i]->Release();

			params->Detach();
		}
		return esriSR;
	}

	int SpatialReferenceType(int pandaSrType)
	{
		switch(pandaSrType)
		{
			case Panda::SpatialReferenceType::Transverse_Mercator:
				return esriSRProjection_TransverseMercator;
			case Panda::SpatialReferenceType::Mercator:
				return esriSRProjection_Mercator;
			case Panda::SpatialReferenceType::Gauss_Krueger:
				return esriSRProjection_GaussKruger;
		}
		return 0;
	}

	int SpatialReferenceParamType( int pandaSrParamType)
	{
		switch(pandaSrParamType)
		{
				case Panda::SpatialReferenceParamType::Azimuth:
					return esriSRParameter_Azimuth;
				case Panda::SpatialReferenceParamType::CentralMeridian:
					return esriSRParameter_CentralMeridian;
				case Panda::SpatialReferenceParamType::FalseEasting:
					return esriSRParameter_FalseEasting;
				case Panda::SpatialReferenceParamType::FalseNorthing:
					return esriSRParameter_FalseNorthing;
				case Panda::SpatialReferenceParamType::LatitudeOfOrigin:
					return esriSRParameter_LatitudeOfOrigin;
				case Panda::SpatialReferenceParamType::LongitudeOfCenter:
					return esriSRParameter_LongitudeOfCenter;
				case Panda::SpatialReferenceParamType::ScaleFactor:
					return esriSRParameter_ScaleFactor;
		}
		return 0;
	}

	int SpatialReferenceGeoType(int pandaSrGeoType)
	{
		switch(pandaSrGeoType)
		{
			case Panda::SpatialReferenceGeoType::WGS1984:
				return esriSRGeoCS_WGS1984;
			case Panda::SpatialReferenceGeoType::Krasovsky1940:
				return esriSRGeoCS_Krasovsky1940;
			case Panda::SpatialReferenceGeoType::NAD1983:
				return esriSRGeoCS_NAD1983;
		}
		return 0;
	}

	int SpatialReferenceUnit(int pandaSrUnit)
	{
		switch(pandaSrUnit)
		{
			case Panda::SpatialReferenceUnit::Meter:
				return esriSRUnit_Meter;
			case Panda::SpatialReferenceUnit::Kilometer:
				return esriSRUnit_Kilometer;
			case Panda::SpatialReferenceUnit::Foot:
				return esriSRUnit_Foot;
			case Panda::SpatialReferenceUnit::NauticalMile:
				return esriSRUnit_NauticalMile;
		}
		return 0;
	}

	Geometry* toARANGeometry(IGeometry *igeom)
	{
		long					i, j, n, m;
		double					x, y, z, fm;
		HRESULT					res;

		Panda::Point*			point;
		Panda::MultiPoint*		multiPoint;
		Panda::Part*			part;
		Panda::Ring*			ring;
		Panda::Polyline*		polyline;
		Panda::Polygon*			polygon;

		_WKSPoint*				WKSPoints = NULL;
		void*					newMem;

		IPoint*					ipoint;

		IGeometry*				geometry;
		IPath*					path;
		IRing*					iring;
		IPointCollection*		pointCollection;

		IGeometryCollection*	geometryCollection;
		esriGeometryType		esrigeometrytype;

		igeom->get_GeometryType(&esrigeometrytype);

		switch(esrigeometrytype)
		{
		case esriGeometryPoint:
			igeom->QueryInterface(__uuidof(IPoint), (void**)&ipoint);

			ipoint->get_X(&x);
			ipoint->get_Y(&y);
			ipoint->get_Z(&z);
			ipoint->get_M(&fm);
			ipoint->Release();

			point = new Panda::Point(x, y);
			point->setZ(z);
			point->setM(fm);
			point->setT(0);
			return point;
		case esriGeometryMultipoint:
		case esriGeometryPath:
		case esriGeometryRing:
			igeom->QueryInterface(__uuidof(IPointCollection), (void**)&pointCollection);

			res = pointCollection->get_PointCount(&n);
			if(res != S_OK)
			{
				pointCollection->Release();
				return NULL;
			}

			//WKSPoints = new _WKSPoint[n]();

			WKSPoints = (_WKSPoint*)malloc(n * sizeof(_WKSPoint));
			if(!WKSPoints)
			{
				pointCollection->Release();
				return NULL;
			}
			pointCollection->QueryWKSPoints(0, n, WKSPoints);
			pointCollection->Release();

			switch(esrigeometrytype)
			{
				case esriGeometryMultipoint:
					multiPoint = new MultiPoint();
					break;
				case esriGeometryPath:
					multiPoint = new Part();
					break;
				case esriGeometryRing:
					multiPoint = new Panda::Ring();
					break;
			}

			point = new Panda::Point();

			for(i = 0; i < n; i++)
			{
				point->setCoords(WKSPoints[i].X, WKSPoints[i].Y);
				multiPoint->addPoint(*point);
			}
			free(WKSPoints);

			delete point;
			return multiPoint;
		case esriGeometryPolyline:
			igeom->QueryInterface(__uuidof(IGeometryCollection), (void**)&geometryCollection);

			res = geometryCollection->get_GeometryCount(&m);
			if(res != S_OK)
			{
				geometryCollection->Release();
				return NULL;
			}

			polyline = new Panda::Polyline();
			part = new Panda::Part();
			point = new Panda::Point();

			for(j = 0; j < m; j++)
			{
				res = geometryCollection->get_Geometry(j, &geometry);
				if(res != S_OK)
				{
					geometryCollection->Release();
					free(WKSPoints);
					delete point;
					delete part;
					return polyline;
				}

				geometry->QueryInterface(__uuidof(IPath), (void**)&path);
				geometry->Release();
				if(path == NULL)
				{
					geometryCollection->Release();
					path->Release();
					free(WKSPoints);
					delete point;
					delete part;
					return polyline;
				}

				path->QueryInterface(__uuidof(IPointCollection), (void**)&pointCollection);
				path->Release();

				res = pointCollection->get_PointCount(&n);
				if(res != S_OK)
				{
					geometryCollection->Release();
					pointCollection->Release();
					free(WKSPoints);
					delete point;
					delete part;
					return polyline;
				}

				newMem = realloc(WKSPoints, n * sizeof(_WKSPoint));
				if(newMem == NULL)
				{
					geometryCollection->Release();
					pointCollection->Release();
					free(WKSPoints);
					delete point;
					delete part;
					return polyline;
				}

				WKSPoints = (_WKSPoint*)newMem;

				pointCollection->QueryWKSPoints(0, n, WKSPoints);
				pointCollection->Release();

				part->clear();

				for(i = 0; i < n; i++)
				{
					point->setCoords(WKSPoints[i].X, WKSPoints[i].Y);
					part->addPoint(*point);
				}
				polyline->add(*part);
			}

			geometryCollection->Release();

			free(WKSPoints);
			delete point;
			delete part;
			return polyline;
		case esriGeometryPolygon:
			igeom->QueryInterface(__uuidof(IGeometryCollection), (void**)&geometryCollection);

			res = geometryCollection->get_GeometryCount(&m);
			if(res != S_OK)
			{
				geometryCollection->Release();
				return NULL;
			}

			polygon = new Panda::Polygon();
			ring = new Panda::Ring();
			point = new Panda::Point();

			for(j = 0; j < m; j++)
			{
				res = geometryCollection->get_Geometry(j, &geometry);
				if(res != S_OK)
				{
					free(WKSPoints);
					geometryCollection->Release();
					delete point;
					delete ring;
					return polygon;
				}

				geometry->QueryInterface(__uuidof(IRing), (void**)&iring);
				geometry->Release();
				if(iring == NULL)
				{
					geometryCollection->Release();
					free(WKSPoints);
					iring->Release();
					delete point;
					delete ring;
					return polygon;
				}

				iring->QueryInterface(__uuidof(IPointCollection), (void**)&pointCollection);
				iring->Release();

				res = pointCollection->get_PointCount(&n);
				if(res != S_OK)
				{
					geometryCollection->Release();
					pointCollection->Release();
					free(WKSPoints);
					delete point;
					delete ring;
					return polygon;
				}

				newMem = realloc(WKSPoints, n * sizeof(_WKSPoint));
				if(newMem == NULL)
				{
					geometryCollection->Release();
					pointCollection->Release();
					free(WKSPoints);
					delete point;
					delete ring;
					return polygon;
				}

				WKSPoints = (_WKSPoint*)newMem;

				pointCollection->QueryWKSPoints(0, n, WKSPoints);
				pointCollection->Release();

				ring->clear();

				for(i = 0; i < n; i++)
				{
					point->setCoords(WKSPoints[i].X, WKSPoints[i].Y);
					ring->addPoint(*point);
				}
				polygon->add(*ring);
			}

			geometryCollection->Release();
			free(WKSPoints);
			delete point;
			delete ring;
			return polygon;
		}
		return NULL;
	}

	IGeometry *toESRIGeometry(Geometry *geom)
	{
		GUID guid;
		GUID iid;
		IGeometry *iGeom;

		switch(geom->geometryType())
		{
		case GeometryType::Point:
			guid = CLSID_Point;
			iid = IID_IPoint;
			break;
		case GeometryType::MultiPoint:
			guid = CLSID_Multipoint;
			iid = IID_IMultipoint;
			break;
		case GeometryType::Part:
			guid = CLSID_Path;
			iid = IID_IPath;
			break;
		case GeometryType::Ring:
			guid = CLSID_Ring;
			iid = IID_IRing;
			break;
		case GeometryType::Polyline:
			guid = CLSID_Polyline;
			iid = IID_IPolyline;
			break;
		case GeometryType::Polygon:
			guid = CLSID_Polygon;
			iid = IID_IPolygon;
			break;
		}

		HRESULT res = CoCreateInstance(guid, 0, CLSCTX_INPROC, iid, (void**)&iGeom);
		if(res != S_OK)
			return NULL;

		Panda::Point*			point;
		Panda::MultiPoint*		multiPoint;
		Panda::Part*			part;
		Panda::Ring*			ring;
		Panda::Polyline*		polyline;
		Panda::Polygon*			polygon;

		_WKSPoint*				WKSPoints = NULL;
		void*					newMem;

		IGeometry*				geometry;
		IPoint*					ipoint;
		IPointCollection*		pointCollection;
		IGeometryCollection*	geometryCollection;

		IPath*					path;
		IRing*					iring;
		ITopologicalOperator2* topoOper;

		int						i, j, n, m;

		switch(geom->geometryType())
		{
		case GeometryType::Point:
			point = (Panda::Point*)geom;
			ipoint = (IPoint*)iGeom;

			ipoint->PutCoords(point->getX(), point->getY());
			ipoint->put_Z(point->getZ());
			ipoint->put_M(point->getM());
			return iGeom;
		case GeometryType::MultiPoint:
		case GeometryType::Part:
		case GeometryType::Ring:
			multiPoint = (Panda::MultiPoint*)geom;

			n = multiPoint->count();
			WKSPoints = (_WKSPoint*)malloc(n * sizeof(_WKSPoint));
			if(!WKSPoints)
				return NULL;

			for(i = 0; i < n; i++)
			{
				point = (Panda::Point*)(&multiPoint->getPoint(i));
				WKSPoints[i].X = point->getX();
				WKSPoints[i].Y = point->getY();
			}

			iGeom->QueryInterface(__uuidof(IPointCollection), (void**)&pointCollection);
			pointCollection->AddWKSPoints (n, WKSPoints);
			free(WKSPoints);

			iGeom->QueryInterface(__uuidof(ITopologicalOperator2), (void**)&topoOper);
			topoOper->put_IsKnownSimple(VARIANT_FALSE);
			topoOper->Simplify();
			topoOper->Release();

			return iGeom;
		case GeometryType::Polyline:
			polyline = (Panda::Polyline*)geom;
			iGeom->QueryInterface(__uuidof(IGeometryCollection), (void**)&geometryCollection);

			m = polyline->count();
			for(j = 0; j < m; j++)
			{
				part = (Panda::Part*)(&polyline->get(j));
				n = part->count();

				newMem = realloc(WKSPoints, n * sizeof(_WKSPoint));
				if(!newMem)
				{
					geometryCollection->Release();
					free(WKSPoints);
					return iGeom;
				}

				WKSPoints = (_WKSPoint*)newMem;

				res = CoCreateInstance(CLSID_Path, 0, CLSCTX_INPROC, IID_IPath, (void**)&path);
				if(res != S_OK)
				{
					geometryCollection->Release();
					free(WKSPoints);
					return iGeom;
				}

				for(i = 0; i < n; i++)
				{
					point = (Panda::Point*)(&part->getPoint(i));
					WKSPoints[i].X = point->getX();
					WKSPoints[i].Y = point->getY();
				}

				path->QueryInterface(__uuidof(IPointCollection), (void**)&pointCollection);
				pointCollection->AddWKSPoints (n, WKSPoints);
				pointCollection->Release();

				path->QueryInterface(__uuidof(IGeometry), (void**)&geometry);
				geometryCollection->AddGeometry(geometry);
				geometry->Release();
				path->Release();
			}

			geometryCollection->Release();
			free(WKSPoints);
			return iGeom;
		case GeometryType::Polygon:
			polygon = (Panda::Polygon*)geom;
			iGeom->QueryInterface(__uuidof(IGeometryCollection), (void**)&geometryCollection);

			m = polygon->count();
			for(j = 0; j < m; j++)
			{
				ring = (Panda::Ring*)(&polygon->get(j));
				n = ring->count();

				newMem = realloc(WKSPoints, (n ) * sizeof(_WKSPoint));
				if(!newMem)
				{
					geometryCollection->Release();
					free(WKSPoints);
					return iGeom;
				}

				WKSPoints = (_WKSPoint*)newMem;

				res = CoCreateInstance(CLSID_Ring, 0, CLSCTX_INPROC, IID_IPath, (void**)&iring);
				if(res != S_OK)
				{
					geometryCollection->Release();
					free(WKSPoints);
					return iGeom;
				}

				for(i = 0; i < n; i++)
				{
					point = (Panda::Point*)(&ring->getPoint(i));
					WKSPoints[i].X = point->getX();
					WKSPoints[i].Y = point->getY();
				}

				//WKSPoints[n].X = WKSPoints[0].X;
				//WKSPoints[n].Y = WKSPoints[0].Y;

				iring->QueryInterface(__uuidof(IPointCollection), (void**)&pointCollection);
				pointCollection->AddWKSPoints (n, WKSPoints);
				pointCollection->Release();

				iring->QueryInterface(__uuidof(IGeometry), (void**)&geometry);
				geometryCollection->AddGeometry(geometry);
				geometry->Release();
				iring->Release();
			}

			geometryCollection->Release();
			free(WKSPoints);

			iGeom->QueryInterface(__uuidof(ITopologicalOperator2), (void**)&topoOper);
			topoOper->put_IsKnownSimple(VARIANT_FALSE);
			topoOper->Simplify();
			topoOper->Release();
			return iGeom;
		}
		return NULL;
	}
}

using namespace PandaToEsri;

static int UnionGeom(Handle inout)
{
	int result = rcOk;

	Geometry	*geom1 = (Geometry*)(&(unpackGeometry(inout)));
	Geometry	*geom2 = (Geometry*)(&(unpackGeometry(inout)));

	IGeometry	*iGeom1 = toESRIGeometry(geom1);
	delete geom1;

	IGeometry	*iGeom2 = toESRIGeometry(geom2);
	delete geom2;

	IGeometry	*iGeomResult;

	ITopologicalOperator2* topoOper;
	iGeom1->QueryInterface(__uuidof(ITopologicalOperator2), (void**)&topoOper);
	iGeom1->Release();

	HRESULT	res = topoOper->Union(iGeom2, &iGeomResult);
	topoOper->Release();
	iGeom2->Release();

	//if(res != S_OK)
	//{
	//	iGeomResult->Release();
	//	return rcException;
	//}
	Geometry	*geomResult;
	if(res != S_OK)
	{
		geomResult = new Panda::NullGeometry ();
	}
	else
	{
		iGeomResult->QueryInterface(__uuidof(ITopologicalOperator2), (void**)&topoOper);
		topoOper->put_IsKnownSimple(FALSE);
		topoOper->Simplify();

		geomResult = toARANGeometry(iGeomResult);
		iGeomResult->Release();
	}

	packGeometry(inout, *geomResult);
	delete geomResult;

	return result;
}

/*			{
				Esri::Geometry* geom = 0;
				Esri::Geometry* geomResult = 0;

				result = Esri::unpackGeometry(inout, geom); if( result != rcOk) goto finishConvex;
				
				geom->Simplify();

				geomResult = geom->ConvexHull();
				if( geomResult != 0)
					geomResult->Simplify();

				result = Esri::packGeometry(inout, geomResult);
		
			finishConvex:
				if( geom != 0) geom->Release();
				if( geomResult != 0) geomResult->Release();
				
				return result;
			}*/
static int convexHull(Handle inout)
{
	int result = rcOk;

	Geometry				*geom = (Geometry*)(&(unpackGeometry(inout)));
	IGeometry				*iGeom = toESRIGeometry(geom);
	delete geom;


	ITopologicalOperator2	*topoOper;
	iGeom->QueryInterface(__uuidof(ITopologicalOperator2), (void**)&topoOper);
	iGeom->Release();

	IGeometry				*iGeomResult;
	HRESULT	res = topoOper->ConvexHull(&iGeomResult);
	topoOper->Release();

	Geometry	*geomResult;
	if(res != S_OK)
	{
		//iGeomResult->Release();
		//return rcException;
		geomResult = new Panda::NullGeometry ();
	}
	else
	{

		iGeomResult->QueryInterface(__uuidof(ITopologicalOperator2), (void**)&topoOper);
		topoOper->put_IsKnownSimple(FALSE);
		topoOper->Simplify();

		geomResult = toARANGeometry(iGeomResult);
		iGeomResult->Release();
	}

	packGeometry(inout, *geomResult);
	delete geomResult;

	return result;
}

			/*{
				Esri::Geometry* geom1 = 0;
				Esri::Geometry* geom2 = 0;
				Esri::Geometry* geomLeft = 0;
				Esri::Geometry* geomRight = 0;

				result = Esri::unpackGeometry(inout, geom1); if( result != rcOk) goto finishCut;
				result = Esri::unpackGeometry(inout, geom2); if( result != rcOk) goto finishCut;

				geom1->Simplify();
				geom2->Simplify();			

				geom1->Cut(geom2->asPolyline(), geomLeft, geomRight);

				if( geomLeft != 0)
					geomLeft->Simplify();

				result = Esri::packGeometry(inout, geomLeft); if( result != rcOk) goto finishCut;

				if(geomRight != 0)
					geomRight->Simplify();

				result = Esri::packGeometry(inout, geomRight);
			
			finishCut:
				if( geom1 != 0) geom1->Release();
				if( geom2 != 0) geom2->Release();
				if( geomLeft != 0) geomLeft->Release();
				if( geomRight != 0) geomRight->Release();

				return result;
			}*/
static int cut(Handle inout)
{
	int result = rcOk;

	Geometry				*geom1 = (Geometry*)(&(unpackGeometry(inout)));
	Geometry				*geom2 = (Geometry*)(&(unpackGeometry(inout)));

	IGeometry				*iGeom1 = toESRIGeometry(geom1);
	delete geom1;

	ITopologicalOperator2	*topoOper;
	iGeom1->QueryInterface(__uuidof(ITopologicalOperator2), (void**)&topoOper);
	iGeom1->Release();


	IGeometry				*iGeom2 = toESRIGeometry(geom2);
	delete geom2;

	IPolyline				*cutter;
	iGeom2->QueryInterface(__uuidof(IPolyline), (void**)&cutter);
	iGeom2->Release();


	topoOper->put_IsKnownSimple (FALSE);
	topoOper->Simplify();

	IGeometry	*iGeomLeft, *iGeomRight;
	
	HRESULT	res = topoOper->Cut(cutter, &iGeomLeft, &iGeomRight);
	topoOper->Release();
	cutter->Release();

	if(res != S_OK)
	{
	//	return rcException;
		Panda::Geometry* geometry = new Panda::NullGeometry ();
		packGeometry(inout, *geometry);
		packGeometry(inout, *geometry);
		delete geometry;
	}
	else
	{
		iGeomLeft->QueryInterface(__uuidof(ITopologicalOperator2), (void**)&topoOper);
		topoOper->put_IsKnownSimple(FALSE);
		topoOper->Simplify();
		topoOper->Release();

		Panda::Geometry* geometry = toARANGeometry(iGeomLeft);
		iGeomLeft->Release();

		packGeometry(inout, *geometry);
		delete geometry;

		iGeomRight->QueryInterface(__uuidof(ITopologicalOperator2), (void**)&topoOper);
		topoOper->put_IsKnownSimple(FALSE);
		topoOper->Simplify();
		topoOper->Release();

		geometry = toARANGeometry(iGeomRight);
		iGeomRight->Release();
		packGeometry(inout, *geometry);
		delete geometry;
	}
	return result;
}

static int intersect(Handle inout)
{
	int	result = rcOk;

	Geometry				*geom1 = (Geometry*)(&(unpackGeometry(inout)));
	Geometry				*geom2 = (Geometry*)(&(unpackGeometry(inout)));

	IGeometry				*iGeom1 = toESRIGeometry(geom1);
	delete geom1;

	IGeometry				*iGeom2 = toESRIGeometry(geom2);
	delete geom2;

	esriGeometryType		esrigeometrytype1;
	iGeom1->get_GeometryType(&esrigeometrytype1);

	ITopologicalOperator2	*topoOper;
	iGeom1->QueryInterface(__uuidof(ITopologicalOperator2), (void**)&topoOper);
	iGeom1->Release();

	esriGeometryType		esrigeometrytype2;
	iGeom2->get_GeometryType(&esrigeometrytype2);

	IGeometry				*iGeomResult;
	HRESULT					res = S_FALSE;

	switch (esrigeometrytype1)
	{
		case esriGeometryPolyline:								//--- Panda::GeometryType::Polyline
			if (esrigeometrytype2 == esriGeometryPolygon)
				res = topoOper->Intersect(iGeom2, esriGeometry1Dimension, &iGeomResult);
			else
				res = topoOper->Intersect(iGeom2, esriGeometry0Dimension, &iGeomResult);
			break;
		case esriGeometryPolygon:								//--- Panda::GeometryType::Polygon
			if (esrigeometrytype2 == esriGeometryPolyline)		// Panda::GeometryType::Polyline
				res = topoOper->Intersect(iGeom2, esriGeometry1Dimension, &iGeomResult);
			else if (esrigeometrytype2 == esriGeometryPolygon)	// Panda::GeometryType::Polygon
				res = topoOper->Intersect(iGeom2, esriGeometry2Dimension, &iGeomResult);
			break;
		default:												// Panda::GeometryType::{ Point, MultiPoint, etc }
			res = topoOper->Intersect(iGeom2, esriGeometry0Dimension, &iGeomResult);
	}

	topoOper->Release();
	iGeom2->Release();

	//if(res != S_OK)
	//	return rcException;
	Panda::Geometry* geometry;

	if(res != S_OK)
		geometry = new Panda::NullGeometry ();
	else
	{
		iGeomResult->QueryInterface(__uuidof(ITopologicalOperator2), (void**)&topoOper);
		topoOper->put_IsKnownSimple(FALSE);
		topoOper->Simplify();

		geometry = toARANGeometry(iGeomResult);
		iGeomResult->Release();
	}

	packGeometry(inout, *geometry);
	delete geometry;
	return result;
}

			/*{
				Esri::Geometry* geom = 0;
				Esri::Geometry* geomResult = 0;

				result = Esri::unpackGeometry(inout, geom); if( result != rcOk) goto finishBoundary;

				geom->Simplify();
				geomResult = geom->Boundary();
				if(geomResult != 0)
					geomResult->Simplify();

				result = Esri::packGeometry(inout, geomResult);

			finishBoundary:
				if( geom != 0) geom->Release();
				if( geomResult != 0) geomResult->Release();
				return result;
			}*/
static int boundary(Handle inout)
{
	int result = rcOk;

	Geometry		*geom = (Geometry*)(&(unpackGeometry(inout)));
	IGeometry		*iGeom = toESRIGeometry(geom);
	delete geom;

	ITopologicalOperator2* topoOper = 0;
	iGeom->QueryInterface(__uuidof(ITopologicalOperator2), (void**)&topoOper);
	iGeom->Release();

	IGeometry	*iGeomResult;

	HRESULT res = topoOper->get_Boundary(&iGeomResult);
	topoOper->Release();

//	if(res != S_OK)
//		return rcException;
	Panda::Geometry* geometry;

	if(res != S_OK)
		geometry = new Panda::NullGeometry ();
	else
	{
		iGeomResult->QueryInterface(__uuidof(ITopologicalOperator2), (void**)&topoOper);
		topoOper->put_IsKnownSimple(FALSE);
		topoOper->Simplify();

		geometry = toARANGeometry(iGeomResult);
		iGeomResult->Release();
	}

	packGeometry(inout, *geometry);
	delete geometry;

	return result;
}

static int difference(Handle inout)
{
	int result = rcOk;

	Geometry	*geom1 = (Geometry*)(&(unpackGeometry(inout)));
	Geometry	*geom2 = (Geometry*)(&(unpackGeometry(inout)));

	IGeometry	*iGeom1 = toESRIGeometry(geom1);
	delete geom1;

	IGeometry	*iGeom2 = toESRIGeometry(geom2);
	delete geom2;

	ITopologicalOperator2* topoOper = 0;
	iGeom1->QueryInterface(__uuidof(ITopologicalOperator2), (void**)&topoOper);
	iGeom1->Release();

	IGeometry	*iGeomResult;
	HRESULT	res = topoOper->Difference(iGeom2, &iGeomResult);
	topoOper->Release();
	iGeom2->Release();

//	if(res != S_OK)
//		return rcException;

	Geometry	*geomResult;

	if(res != S_OK)
		geomResult = new Panda::NullGeometry ();
	else
	{
		iGeomResult->QueryInterface(__uuidof(ITopologicalOperator2), (void**)&topoOper);
		topoOper->put_IsKnownSimple(FALSE);
		topoOper->Simplify();

		geomResult = toARANGeometry(iGeomResult);
		iGeomResult->Release();
	}

	packGeometry(inout, *geomResult);
	delete geomResult;

	return result;
}

static int buffer(Handle inout)
{
	int			result = rcOk;
	double		width;

	Geometry	*geom = (Geometry*)(&(unpackGeometry(inout)));
	result = Registry_getDouble(inout, width);
	if(result != rcOk)
	{
		delete geom;
		return result;
	}

	IGeometry	*iGeom = toESRIGeometry(geom);
	delete geom;

	ITopologicalOperator2* topoOper = 0;
	iGeom->QueryInterface(__uuidof(ITopologicalOperator2), (void**)&topoOper);
	iGeom->Release();

	IGeometry	*iGeomResult;
	HRESULT	res = topoOper->Buffer(width, &iGeomResult);
	topoOper->Release();

	//if(res != S_OK)
	//	return rcException;
	Geometry	*geomResult;

	if(res != S_OK)
		geomResult = new Panda::NullGeometry ();
	else
	{
		iGeomResult->QueryInterface(__uuidof(ITopologicalOperator2), (void**)&topoOper);
		topoOper->put_IsKnownSimple(FALSE);
		topoOper->Simplify();

		geomResult = toARANGeometry(iGeomResult);
		iGeomResult->Release();
	}

	packGeometry(inout, *geomResult);
	delete geomResult;

	return result;
}

			/*{
				Esri::Geometry* geom = 0;
				Esri::Geometry* geomPoint = 0;
				Esri::Point* nearestPoint = 0;

				result = Esri::unpackGeometry(inout, geom); if( result != rcOk) goto finishNearestPoint;
				result = Esri::unpackGeometry(inout, geomPoint); if( result != rcOk) goto finishNearestPoint;

				geom->Simplify();
				nearestPoint = geom->getNearestPoint(geomPoint->asPoint(), esriNoExtension);
				result = Esri::packGeometry(inout, (nearestPoint != 0 ? nearestPoint->asGeometry() : 0));

			finishNearestPoint:
				if( geom != 0) geom->Release();
				if(geomPoint != 0) geomPoint->Release();
				if(nearestPoint != 0) nearestPoint->Release();
				return result;
			}*/
static int getNearestPoint(Handle inout)
{
	int result = rcOk;

	Geometry		*geom = (Geometry*)(&(unpackGeometry(inout)));
	Panda::Point	*point = (Panda::Point*)(&(unpackGeometry(inout)));

	IGeometry		*iGeom = toESRIGeometry(geom);
	delete geom;

	IPoint	*ipoint = (IPoint*)toESRIGeometry(point);
	delete point;

	IProximityOperator* proxOper;
	iGeom->QueryInterface(__uuidof(IProximityOperator), (void**)&proxOper);
	iGeom->Release();

	IPoint*		nearestPoint;
	HRESULT	res = proxOper->ReturnNearestPoint(ipoint, esriNoExtension, (IPoint**)&nearestPoint);
	proxOper->Release();
	ipoint->Release();

//	if(res != S_OK)
//		return rcException;

	Panda::Geometry* geomResult;

	if(res != S_OK)
		geomResult = new Panda::NullGeometry ();
	else
	{
		geomResult = toARANGeometry(nearestPoint);
		nearestPoint->Release();
	}

	packGeometry(inout, *geomResult);
	delete geomResult;

	return result;
}

static int getDistance(Handle inout)
{
	Geometry	*geom1 = (Geometry*)(&(unpackGeometry(inout)));
	Geometry	*geom2 = (Geometry*)(&(unpackGeometry(inout)));

	IGeometry	*iGeom1 = toESRIGeometry(geom1);
	delete geom1;

	IGeometry	*iGeom2 = toESRIGeometry(geom2);
	delete geom2;

	IProximityOperator* proxOper;
	iGeom1->QueryInterface(__uuidof(IProximityOperator), (void**)&proxOper);
	iGeom1->Release();

	double	distance;
	HRESULT	res = proxOper->ReturnDistance(iGeom2, &distance);
	proxOper->Release();
	iGeom2->Release();

	if(res != S_OK)
		return rcException;
	return  Registry_putDouble(inout, distance);
}

			/*{
				Esri::Geometry* geom = 0;
				Esri::Geometry* geomOther = 0;

				result = Esri::unpackGeometry(inout, geom); if( result != rcOk) goto finishContains;
				result = Esri::unpackGeometry(inout, geomOther); if( result != rcOk) goto finishContains;

				geom->Simplify();
				geomOther->Simplify();
				bool retVal = geom->Contains(geomOther);
				result = Registry_putBool(inout, retVal);
			finishContains:
				if( geom != 0) geom->Release();
				if( geomOther != 0) geomOther->Release();
				return result;
			}*/
static int contains(Handle inout)
{
	Geometry		*geom1 = (Geometry*)(&(unpackGeometry(inout)));
	Geometry		*geom2 = (Geometry*)(&(unpackGeometry(inout)));

	IGeometry		*iGeom1 = toESRIGeometry(geom1);
	delete geom1;

	IGeometry		*iGeom2 = toESRIGeometry(geom2);
	delete geom2;

	IRelationalOperator	*relOper;
	iGeom1->QueryInterface(__uuidof(IRelationalOperator), (void**)&relOper);
	iGeom1->Release();

	VARIANT_BOOL resultVal;

	HRESULT res = relOper->Contains(iGeom2, &resultVal);
	relOper->Release();
	iGeom2->Release();

//	if(res != S_OK)
//		return rcException;

	if(res != S_OK)
		resultVal = VARIANT_FALSE;

	return  Registry_putBool(inout, resultVal == VARIANT_TRUE);
}

			/*{
				Esri::Geometry* geom = 0;
				Esri::Geometry* geomOther = 0;

				result = Esri::unpackGeometry(inout, geom); if( result != rcOk) goto finishCrosses;
				result = Esri::unpackGeometry(inout, geomOther); if( result != rcOk) goto finishCrosses;

				geom->Simplify();
				geomOther->Simplify();
				bool retVal = geom->Crosses(geomOther);
				result = Registry_putBool(inout, retVal);
			finishCrosses:
				if( geom != 0) geom->Release();
				if( geomOther != 0) geomOther->Release();
				return result;
			}*/
static int crosses(Handle inout)
{
	Geometry		*geom1 = (Geometry*)(&(unpackGeometry(inout)));
	Geometry		*geom2 = (Geometry*)(&(unpackGeometry(inout)));


	IGeometry		*iGeom1 = toESRIGeometry(geom1);
	delete geom1;

	IGeometry		*iGeom2 = toESRIGeometry(geom2);
	delete geom2;

	IRelationalOperator	*relOper;
	iGeom1->QueryInterface(__uuidof(IRelationalOperator), (void**)&relOper);
	iGeom1->Release();

	VARIANT_BOOL resultVal;

	HRESULT res = relOper->Crosses(iGeom2, &resultVal);
	relOper->Release();
	iGeom2->Release();

//	if(res != S_OK)
//		return rcException;

	if(res != S_OK)
		resultVal = VARIANT_FALSE;

	return  Registry_putBool(inout, resultVal == VARIANT_TRUE);
}

			/*{
				Esri::Geometry* geom = 0;
				Esri::Geometry* geomOther = 0;

				result = Esri::unpackGeometry(inout, geom); if( result != rcOk) goto finishDisjoint;
				result = Esri::unpackGeometry(inout, geomOther); if( result != rcOk) goto finishDisjoint;

				geom->Simplify();
				geomOther->Simplify();
				bool retVal = geom->Disjoint(geomOther);
				result = Registry_putBool(inout, retVal);
			finishDisjoint:
				if( geom != 0) geom->Release();
				if( geomOther != 0) geomOther->Release();
				return result;
			}*/
static int disjoint(Handle inout)
{
	Geometry		*geom1 = (Geometry*)(&(unpackGeometry(inout)));
	Geometry		*geom2 = (Geometry*)(&(unpackGeometry(inout)));

	IGeometry		*iGeom1 = toESRIGeometry(geom1);
	delete geom1;

	IGeometry		*iGeom2 = toESRIGeometry(geom2);
	delete geom2;

	IRelationalOperator	*relOper;
	iGeom1->QueryInterface(__uuidof(IRelationalOperator), (void**)&relOper);
	iGeom1->Release();

	VARIANT_BOOL resultVal;
	HRESULT res = relOper->Disjoint(iGeom2, &resultVal);
	relOper->Release();
	iGeom2->Release();

//	if(res != S_OK)
//		return rcException;

	if(res != S_OK)
		resultVal = VARIANT_FALSE;

	return  Registry_putBool(inout, resultVal == VARIANT_TRUE);
}

			/*{
				Esri::Geometry* geom = 0;
				Esri::Geometry* geomOther = 0;

				result = Esri::unpackGeometry(inout, geom); if( result != rcOk) goto finishEquals;
				result = Esri::unpackGeometry(inout, geomOther); if( result != rcOk) goto finishEquals;

				geom->Simplify();
				geomOther->Simplify();
				bool retVal = geom->Equals(geomOther);
				result = Registry_putBool(inout, retVal);
			finishEquals:
				if( geom != 0) geom->Release();
				if( geomOther != 0) geomOther->Release();
				return result;
			}*/
static int equals(Handle inout)
{
	Geometry		*geom1 = (Geometry*)(&(unpackGeometry(inout)));
	Geometry		*geom2 = (Geometry*)(&(unpackGeometry(inout)));

	IGeometry		*iGeom1 = toESRIGeometry(geom1);
	delete geom1;

	IGeometry		*iGeom2 = toESRIGeometry(geom2);
	delete geom2;

	IRelationalOperator	*relOper;
	iGeom1->QueryInterface(__uuidof(IRelationalOperator), (void**)&relOper);
	iGeom1->Release();

	VARIANT_BOOL resultVal;

	HRESULT res = relOper->Equals(iGeom2, &resultVal);
	relOper->Release();
	iGeom2->Release();

//	if(res != S_OK)
//		return rcException;

	if(res != S_OK)
		resultVal = VARIANT_FALSE;

	return  Registry_putBool(inout, resultVal == VARIANT_TRUE);
}

static int geoTransformations(Handle inout)
{
	int result = rcOk;
	Panda::Geometry				*geom;
	Panda::SpatialReference		pandaFromSR;
	Panda::SpatialReference		pandaToSR;

	IGeometry					*iGeom;
	ISpatialReference			*esriFromSR;
	ISpatialReference			*esriToSR;


	geom = (Geometry*)(&(unpackGeometry(inout)));
	pandaFromSR.unpack(inout);
	pandaToSR.unpack(inout);

	iGeom = toESRIGeometry(geom);
	delete geom;

	esriFromSR = PandaToEsri::SpatialReference(pandaFromSR);
	iGeom->putref_SpatialReference(esriFromSR);
	esriFromSR->Release();

	esriToSR = PandaToEsri::SpatialReference(pandaToSR);
	iGeom->Project(esriToSR);
	esriToSR->Release();

	Panda::Geometry* GeomResult = toARANGeometry(iGeom);
	iGeom->Release();

	packGeometry(inout, *GeomResult);
	delete GeomResult;

	return result;
}

extern "C"
{
	EXPORT int STDCALL GeometryOperators_entryPoint (Handle /*privateData*/, int command, Handle inout)
	{
		int result = rcOk;

		switch (command)
		{
			case svcGetInstance:
				CoInitialize (NULL);
				break;

			case svcFreeInstance:
				CoUninitialize ();
				break;

			case Panda::GeometryOperatorsContract::Commands::union_:
				return UnionGeom(inout);

			case Panda::GeometryOperatorsContract::Commands::convexHull:
				return convexHull(inout);

			case Panda::GeometryOperatorsContract::Commands::cut:
				return cut(inout);

			case Panda::GeometryOperatorsContract::Commands::intersect:
				return intersect(inout);

			case Panda::GeometryOperatorsContract::Commands::boundary:
				return boundary(inout);

			case Panda::GeometryOperatorsContract::Commands::difference:
				return difference(inout);

			case Panda::GeometryOperatorsContract::Commands::buffer:
				return buffer(inout);

			case Panda::GeometryOperatorsContract::Commands::getNearestPoint:
				return getNearestPoint(inout);

			case Panda::GeometryOperatorsContract::Commands::getDistance:
				return getDistance(inout);

			case Panda::GeometryOperatorsContract::Commands::contains:
				return contains(inout);

			case Panda::GeometryOperatorsContract::Commands::crosses:
				return crosses(inout);
			
			case Panda::GeometryOperatorsContract::Commands::disjoint:
				return disjoint(inout);

			case Panda::GeometryOperatorsContract::Commands::equals:
				return equals(inout);

			case Panda::GeometryOperatorsContract::Commands::geoTransformations:
				return geoTransformations(inout);
		}

		return result;
	}
}
