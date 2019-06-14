#ifndef ESRI_IMPORT_H
#define ESRI_IMPORT_H

//#import <Esri/TLB/esrisystem.olb>
//#import <Esri/TLB/esrigeometry.olb>

#ifdef MY_WIN64

#import "c:/Program files (x86)/ArcGIS/com/esriSystem.olb" raw_interfaces_only raw_native_types no_namespace named_guids exclude("OLE_COLOR", "OLE_HANDLE", "VARTYPE")
#import "c:/Program files (x86)/ArcGIS/com/esriGeometry.olb" raw_interfaces_only raw_native_types no_namespace named_guids

#else

#import "c:/Program files/ArcGIS/com/esriSystem.olb" raw_interfaces_only raw_native_types no_namespace named_guids exclude("OLE_COLOR", "OLE_HANDLE", "VARTYPE")
#import "c:/Program files/ArcGIS/com/esriGeometry.olb" raw_interfaces_only raw_native_types no_namespace named_guids

#endif


inline void* _CoCreateInstance(const IID &rclsid, const IID &riid)
{
    void* obj;
    HRESULT res = CoCreateInstance(rclsid, 0, CLSCTX_INPROC, riid, (void**) &obj);
    return (res == S_OK) ? obj : 0;
}

#endif /*ESRI_IMPORT_H*/
