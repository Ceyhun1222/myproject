#include <windows.h>
#include <iostream>

#include "../include/Contract.h"
#include "../include/ClassDirectory.h"
#include "../include/LocalClass.h"
#include "../include/Object.h"
#include "../include/Packet.h"
#include "../include/Config.h"
#include "../include/ObjectDirectory.h"

namespace
{
	Config* _config;
	ClassDirectory _classDirectory;
	ObjectDirectory _objectDirectory;

	void trim (std::string& str)
	{
		std::string::size_type pos = str.find_last_not_of (' ');
		if(pos != std::string::npos) 
		{
			str.erase (pos + 1);
			pos = str.find_first_not_of (' ');
			if (pos != std::string::npos) 
				str.erase (0, pos);
		}

		else str.erase(str.begin(), str.end());
	}

	Int32 loadService (std::string name, std::string dllName, std::string path, std::string entryPointName, Int32 priority, Handle& handle)
	{
		trim (path);
		path = path + "/" + dllName;

#ifdef WINDOWS

		HINSTANCE hInst = LoadLibraryA (path.c_str ());
		if (hInst == 0)
	    {
#if 0
#include <Winuser.h>

			DWORD	dwErrcode = GetLastError();
			TCHAR	szBuf[1024];
			TCHAR	szModuleFileName[256];
			LPVOID	lpMsgBuf;

			GetModuleFileName(hInst, szModuleFileName, 256);

			FormatMessage(FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM,
						NULL, dwErrcode, MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
						(LPTSTR) &lpMsgBuf, 0, NULL );
			wsprintf(szBuf, L"GetProcAddress failed with error %d: %s in module %s", dwErrcode, lpMsgBuf, szModuleFileName);
			LocalFree(lpMsgBuf);

			MessageBox(NULL, szBuf, L"Error", MB_OK);
#endif
	        return rcErrorLoadingDll;
	    }

	    Method method = (Method) GetProcAddress (hInst, entryPointName.c_str ());
	    if (method == 0)
	    {

#if 0
#include <Winuser.h>

			DWORD	dwErrcode = GetLastError();
			TCHAR	szBuf[1024];
			TCHAR	szModuleFileName[256];
			LPVOID	lpMsgBuf;

			GetModuleFileName(hInst, szModuleFileName, 256);

			FormatMessage(FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM,
						NULL, dwErrcode, MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
						(LPTSTR) &lpMsgBuf, 0, NULL );
			wsprintf(szBuf, L"GetProcAddress failed with error %d: %s in module %s", dwErrcode, lpMsgBuf, szModuleFileName);
			LocalFree(lpMsgBuf);

			MessageBox(NULL, szBuf, L"Error", MB_OK);
#endif
			FreeLibrary(hInst);
	        return rcEntryPointNotFound;
	    }
#endif

#ifdef LINUX
		return rcErrorLoadingDll;
#endif
		return Registry_registerClass (name.c_str (), priority, method, (HModule) hInst, handle);
	}

	Int32 loadService (std::string className)
	{
		const Config::ServiceInfo* serviceInfo = _config->findServiceInfo (className);
		if (serviceInfo == 0) return rcClassNotFound;			

		Handle handle;

        return loadService(
        		serviceInfo->name.c_str (),
				serviceInfo->dllName.c_str (),
        		serviceInfo->path.c_str (),
				serviceInfo->entry.c_str (),
        		serviceInfo->priority,
				handle);
	}
}

#ifdef WINDOWS

EXPORT BOOL STDCALL DllMain (HANDLE hModule, DWORD  ulReasonForCall, LPVOID)
{
	switch (ulReasonForCall)
	{
		case DLL_PROCESS_ATTACH:
		{
			char fullPath [MAX_PATH];
			GetModuleFileNameA ((HMODULE) hModule, fullPath, MAX_PATH);

			char* szFileName = strrchr(fullPath, '\\');
			*szFileName = 0;
			_config = new Config (fullPath);
		}

		case DLL_THREAD_ATTACH:
		case DLL_THREAD_DETACH:
			break;

		case DLL_PROCESS_DETACH:
			delete _config;
			break;
	}

    return TRUE;
}

#endif


EXPORT Int32 STDCALL Registry_loadService (const char* className, const char* dllName, const char* path, const char* entryName, Int32 priority, Handle& handle)
{
	return loadService (className, dllName, path, entryName, priority, handle);
}

EXPORT Int32 STDCALL Registry_getInstance (const char* className, Handle& handle)
{	
	Class* cl = _classDirectory.findClass (className);
	if (cl == 0)
	{
 		Int32 result = loadService (className);
		if (rcOk != result)
		{
			handle = 0;
			return result;
		}

		cl = _classDirectory.findClass (className);
		if (cl == 0)
		{
			handle = 0;
			return rcClassNotFound;
		}
	}
	
	Object* object = new Object (*cl);
	
	// We should add the object pointer to the special object directory
	// and return generated key for security. For simplicity we use new
	// simple the pointer itself.
	handle = _objectDirectory.addObject (object);
	object->setHandle (handle);

	/*
	std::string name = cl->getName ();
	std::wstring wname (name.length(), L'');
	std::copy (name.begin(), name.end(), wname.begin());
	
	object->getInputPacket ().clear ();
	object->getInputPacket ().putString (StringToIntPtr (wname.c_str ()));
	*/

	object->setPrivateData (cl->getInstance (handle));
	return rcOk;
}

EXPORT Int32 STDCALL Registry_freeInstance (Handle handle)
{
	Object* object = _objectDirectory.findObject (handle);
	if (object == 0)
		return rcObjectNotFound;

	object->free ();
	_objectDirectory.removeObject (handle);
	return rcOk;
}

EXPORT Int32 STDCALL Registry_setHandler (Handle handle, Int32 handlerData, Method method)
{
	Object* object = _objectDirectory.findObject (handle);
	if (object == 0) return rcObjectNotFound;

	object->setHandler (handlerData, method);
	return rcOk;
}

EXPORT Int32 STDCALL Registry_unsetHandler (Handle handle)
{
	Object* object = _objectDirectory.findObject (handle);
	if (object == 0) return rcObjectNotFound;

	object->unsetHandler ();
	return rcOk;
}


EXPORT Int32 STDCALL Registry_beginMessage (Handle handle, Int32 command)
{
	Object* object = _objectDirectory.findObject (handle);
	if (object == 0) return rcObjectNotFound;

	return object->beginMessage (command);
}

EXPORT Int32 STDCALL Registry_beginEvent (Handle handle, Int32 command)
{
	Object* object = _objectDirectory.findObject (handle);
	if (object == 0) return rcObjectNotFound;

	return object->beginEvent (command);
}

EXPORT Int32 STDCALL Registry_endMessage (Handle handle)
{
	Object* object = _objectDirectory.findObject (handle);
	if (object == 0) return rcObjectNotFound;

	Int32 retVal;
	try
	{
		retVal = object->endMessage ();
	}
	catch (...)
	{
		retVal = rcException;
	}

	return retVal;
}

EXPORT Int32 STDCALL Registry_endEvent (Handle handle)
{
	Object* object = _objectDirectory.findObject (handle);
	if (object == 0) return rcObjectNotFound;

	Int32 retVal;
	try
	{
		retVal = object->endEvent ();
	}
	catch (...)
	{
		retVal = rcException;
	}

	return retVal;
}

EXPORT Int32 STDCALL Registry_registerClass (const char* className, Int32 priority, Method method, HModule hInst, Handle& handle)
{
	Class* cl = _classDirectory.findClass (className, method);
	if (cl != 0) return rcClassAlreadyRegistered;

	cl = new LocalClass (className, priority, method, (HModule) hInst);
	handle = _classDirectory.addClass (cl);
	return rcOk;
}

EXPORT Int32 STDCALL Registry_unregisterClass (const char* className)
{
	Class* cl = _classDirectory.findClass (className);
	if (cl == 0)
		return rcClassNotFound;
		
	if (cl->getRef () > 0)
		return rcClassInUse;
		
	HModule hModule = cl->getHModule ();
	_classDirectory.removeClass (cl);
	cl->free ();	

	if (hModule != 0)
		FreeLibrary ((HMODULE) hModule);

	return rcOk;
}

EXPORT Int32 STDCALL Registry_isInProcess (Handle handle, Int32& result)
{
	Object* object = _objectDirectory.findObject (handle);
	if (object == 0) return rcObjectNotFound;
	
	result = object->isLocal ();
	return rcOk;
}

EXPORT Int32 STDCALL Registry_putByte (Handle handle, Int32 value)
{
	Object* object = _objectDirectory.findObject (handle);
	if (object == 0) return rcObjectNotFound;
	object->getOutputPacket ().putByte (value);
	return rcOk;
}

EXPORT Int32 STDCALL Registry_putShort (Handle handle, Int32 value)
{
	Object* object = _objectDirectory.findObject (handle);
	if (object == 0) return rcObjectNotFound;
	object->getOutputPacket ().putShort (value);	
	return rcOk;
}

EXPORT Int32 STDCALL Registry_putInt32 (Handle handle, Int32 value)
{
	Object* object = _objectDirectory.findObject (handle);
	if (object == 0) return rcObjectNotFound;
	object->getOutputPacket ().putInt32 (value);	
	return rcOk;
}

EXPORT Int32 STDCALL Registry_putInt64 (Handle handle, Int64 value)
{
	Object* object = _objectDirectory.findObject (handle);
	if (object == 0) return rcObjectNotFound;
	object->getOutputPacket ().putInt64 (value);	
	return rcOk;
}

EXPORT Int32 STDCALL Registry_putUShort (Handle handle, UInt32 value)
{
	Object* object = _objectDirectory.findObject (handle);
	if (object == 0) return rcObjectNotFound;
	object->getOutputPacket ().putUShort (value);
	return rcOk;
}

EXPORT Int32 STDCALL Registry_putUInt32 (Handle handle, UInt32 value)
{
	Object* object = _objectDirectory.findObject (handle);
	if (object == 0) return rcObjectNotFound;
	object->getOutputPacket ().putUInt32 (value);
	return rcOk;
}

EXPORT Int32 STDCALL Registry_putUInt64 (Handle handle, UInt64 value)
{
	Object* object = _objectDirectory.findObject (handle);
	if (object == 0) return rcObjectNotFound;
	object->getOutputPacket ().putUInt64 (value);
	return rcOk;
}

EXPORT Int32 STDCALL Registry_putFloat (Handle handle, float value)
{
	Object* object = _objectDirectory.findObject (handle);
	if (object == 0) return rcObjectNotFound;
	object->getOutputPacket ().putFloat (value);	
	return rcOk;
}

EXPORT Int32 STDCALL Registry_putDouble (Handle handle, double value)
{
	Object* object = _objectDirectory.findObject (handle);
	if (object == 0) return rcObjectNotFound;
	object->getOutputPacket ().putDouble (value);		
	return rcOk;
}

EXPORT Int32 STDCALL Registry_putString (Handle handle, const Int32* value)
{
	Object* object = _objectDirectory.findObject (handle);
	if (object == 0) return rcObjectNotFound;
	object->getOutputPacket ().putString (value);			
	return rcOk;
}

EXPORT Int32 STDCALL Registry_putArray (Handle handle, const char* data, UInt32 size)
{
	Object* object = _objectDirectory.findObject (handle);
	if (object == 0) return rcObjectNotFound;
	object->getOutputPacket ().putArray (data, size);			
	return rcOk;
}

EXPORT Int32 STDCALL Registry_putHandle (Handle handle, Handle h)
{
	Object* object = _objectDirectory.findObject (handle);
	if (object == 0) return rcObjectNotFound;
	object->getOutputPacket ().putHandle (h);			
	return rcOk;
}

EXPORT Int32 STDCALL Registry_getByte (Handle handle, Int32& value)
{
	Object* object = _objectDirectory.findObject (handle);
	if (object == 0) return rcObjectNotFound;
	return object->getInputPacket ().getByte (value) ? rcOk : rcInvalid;
}

EXPORT Int32 STDCALL Registry_getShort (Handle handle, Int32& value)
{
	Object* object = _objectDirectory.findObject (handle);
	if (object == 0) return rcObjectNotFound;
	return object->getInputPacket ().getShort (value) ? rcOk : rcInvalid;
}

EXPORT Int32 STDCALL Registry_getInt32 (Handle handle, Int32& value)
{
	Object* object = _objectDirectory.findObject (handle);
	if (object == 0) return rcObjectNotFound;
	return object->getInputPacket ().getInt32 (value) ? rcOk : rcInvalid;
}

EXPORT Int32 STDCALL Registry_getInt64 (Handle handle, Int64& value)
{
	Object* object = _objectDirectory.findObject (handle);
	if (object == 0) return rcObjectNotFound;
	return object->getInputPacket ().getInt64 (value) ? rcOk : rcInvalid;
}

EXPORT Int32 STDCALL Registry_getUShort (Handle handle, UInt32& value)
{
	Object* object = _objectDirectory.findObject (handle);
	if (object == 0) return rcObjectNotFound;
	return object->getInputPacket ().getUShort (value) ? rcOk : rcInvalid;
}

EXPORT Int32 STDCALL Registry_getUInt32 (Handle handle, UInt32& value)
{
	Object* object = _objectDirectory.findObject (handle);
	if (object == 0) return rcObjectNotFound;
	return object->getInputPacket ().getUInt32 (value) ? rcOk : rcInvalid;
}

EXPORT Int32 STDCALL Registry_getUInt64 (Handle handle, UInt64& value)
{
	Object* object = _objectDirectory.findObject (handle);
	if (object == 0) return rcObjectNotFound;
	return object->getInputPacket ().getUInt64 (value) ? rcOk : rcInvalid;
}

EXPORT Int32 STDCALL Registry_getFloat (Handle handle, float& value)
{
	Object* object = _objectDirectory.findObject (handle);
	if (object == 0) return rcObjectNotFound;
	return object->getInputPacket ().getFloat (value) ? rcOk : rcInvalid;
}

EXPORT Int32 STDCALL Registry_getDouble (Handle handle, double& value)
{
	Object* object = _objectDirectory.findObject (handle);
	if (object == 0) return rcObjectNotFound;
	return object->getInputPacket ().getDouble (value) ? rcOk : rcInvalid;
}

EXPORT Int32 STDCALL Registry_getString (Handle handle, const Int32*& value)
{
	Object* object = _objectDirectory.findObject (handle);
	if (object == 0) return rcObjectNotFound;
	return object->getInputPacket ().getString (value) ? rcOk : rcInvalid;	
}

EXPORT Int32 STDCALL Registry_getArray (Handle handle, const char*& data, UInt32& size)
{
	Object* object = _objectDirectory.findObject (handle);
	if (object == 0) return rcObjectNotFound;
	return object->getInputPacket ().getArray (data, size) ? rcOk : rcInvalid;	
}

EXPORT Int32 STDCALL Registry_getHandle (Handle handle, Handle& h)
{
	Object* object = _objectDirectory.findObject (handle);
	if (object == 0) return rcObjectNotFound;
	return object->getInputPacket ().getHandle (h) ? rcOk : rcInvalid;	
}
