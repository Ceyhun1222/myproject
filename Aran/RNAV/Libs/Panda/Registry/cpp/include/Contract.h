#ifndef CONTRACT_H_
#define CONTRACT_H_

#pragma warning( disable : 4290 )

#include <cwchar>
#include <string>
#include <stdexcept>

#ifdef LINUX

#define EXPORT
#define STDCALL

#endif

#ifdef WINDOWS

#define EXPORT __declspec(dllexport)
#define STDCALL __stdcall

#endif

typedef __int32 Int32;
typedef __int64 Int64;
typedef unsigned __int32 UInt32;
typedef unsigned __int64 UInt64;

typedef Int32 Handle;
typedef Int32 HModule;

#define INVALID_HANDLE 0

enum
{
	svcGetInstance = 0x10000,
	svcFreeInstance,
	svcAttachClass,
	svcDetachClass
};

enum
{
	rcClassNotFound = -0x10000,
	rcClassInUse,
	rcErrorLoadingDll,
	rcEntryPointNotFound,
	rcObjectNotFound,
	rcException,
	rcClassAlreadyRegistered,
	rcInvalid,
	rcInvalidParameter,
	rcNotImplemented,

	rcOk = 0
};

extern "C"
{
	// Entry point for all service classes and event listeners
	// Arguments:
	//     privateData - any integer data returned when svcGetInstance command arrived
	//     command - the command (function) identifier
	//     inout - input and output message buffer handle. You read additional arguments
	//             by Registry_getXXX functions from this handle and write processing results
	//             by Registry_putXXX functions to this handle.
	// Returns:
	//     Should return one of the rcXXX messages above. Only exception is svcGetInstance
	//     command, where you can return your private data (any value)
	typedef Int32 (STDCALL *Method) (Handle privateData, Int32 command, Handle inout);
	
	// In order to use service you must get an instance of the service.
	// arguments:
	//     className - the name of the service class
	//     handle - the reference of a variable that will hold the handle of the service object
	// returns:
	//     handle - on success: holds the handle of the service; on error: 0
	//     rcOk - on success, errorCode - on error
	EXPORT Int32 STDCALL Registry_getInstance (const char* className, Handle& handle);

	// You must free the instance of the service
	// arguments:
	//     handle - the handle of the service object
	// returns:
	//     rcOk - on success, error code - on error
	EXPORT Int32 STDCALL Registry_freeInstance (Handle handle);

	// Creates an empty output message and adds command to it.
	// Additional parameters are sent by Registry_putXXX functions.
	// arguments:
	//     handle - the handle of the service object
	//     command - the message identifier
	// returns:
	//     rcOk - on success, error code - on error
	EXPORT Int32 STDCALL Registry_beginMessage (Handle handle, Int32 command);
	EXPORT Int32 STDCALL Registry_beginEvent (Handle handle, Int32 command);

	// Sends output message to the service object (indirectly calling entry point - method of the
	// service object), waits processing of this message and returns the result of the processing
	// in the service object's input message buffer.
	// arguments:
	//     handle - the handle of the service object
	// returns:
	//     rcOk - on success, error code on error
	//     result of the processing in the service object's input message buffer. You can read them
	//     by Registry_getXXX functions.
	EXPORT Int32 STDCALL Registry_endMessage (Handle handle);
	EXPORT Int32 STDCALL Registry_endEvent (Handle handle);
		
	// Registers you service class in order others can find, create instance and use it.
	// arguments:
	//     className - the name of your service class. Clients can find you service by this name calling
	//                 Registry_getInstance () function.
	//     priority - the priority of your service. When more than one service with the same name exits, registry
	//                can choose the service with high priority. Pass 0 by default.
	//     handle - the reference to the variable that will receive the handle of registered service class
	// returns:
	//     rcOk - on success, error code on error
	//     handle - the value of the handle or 0 when error occurs.
	EXPORT Int32 STDCALL Registry_registerClass (const char* className, Int32 priority, Method method, HModule hModule, Handle& handle);

	// Unregisters the service and frees resources allocated for it.
	// arguments:
	//     handle - the handle of the registered service class got by Registry_registerClass () function.
	// returns:
	//     rcOk - on success, error code on error
	EXPORT Int32 STDCALL Registry_unregisterClass (const char* className);    
	EXPORT Int32 STDCALL Registry_setHandler (Handle handle, Int32 handlerData, Method method);
	EXPORT Int32 STDCALL Registry_unsetHandler (Handle handle);
	EXPORT Int32 STDCALL Registry_loadService (const char* className, const char* dllName, const char* path, const char* entryName, Int32 priority, Handle& handle);

	EXPORT Int32 STDCALL Registry_putByte (Handle, Int32);
	EXPORT Int32 STDCALL Registry_putShort (Handle, Int32);
	EXPORT Int32 STDCALL Registry_putInt32 (Handle, Int32);
	EXPORT Int32 STDCALL Registry_putInt64 (Handle, Int64);

	EXPORT Int32 STDCALL Registry_putUShort (Handle, UInt32);
	EXPORT Int32 STDCALL Registry_putUInt32 (Handle, UInt32);
	EXPORT Int32 STDCALL Registry_putUInt64 (Handle, UInt64);

	EXPORT Int32 STDCALL Registry_putFloat (Handle, float);
	EXPORT Int32 STDCALL Registry_putDouble (Handle, double);
	EXPORT Int32 STDCALL Registry_putString (Handle, const Int32*);
	EXPORT Int32 STDCALL Registry_putArray (Handle, const char*, UInt32 size);
	EXPORT Int32 STDCALL Registry_putHandle (Handle, Handle);

	EXPORT Int32 STDCALL Registry_getByte (Handle, Int32&);
	EXPORT Int32 STDCALL Registry_getShort (Handle, Int32&);
	EXPORT Int32 STDCALL Registry_getInt32 (Handle, Int32&);
	EXPORT Int32 STDCALL Registry_getInt64 (Handle, Int64&);

	EXPORT Int32 STDCALL Registry_getUShort (Handle, UInt32&);
	EXPORT Int32 STDCALL Registry_getUInt32 (Handle, UInt32&);
	EXPORT Int32 STDCALL Registry_getUInt64 (Handle, UInt64&);

	EXPORT Int32 STDCALL Registry_getFloat (Handle, float&);
	EXPORT Int32 STDCALL Registry_getDouble (Handle, double&);
	EXPORT Int32 STDCALL Registry_getString (Handle, const Int32*&);
	EXPORT Int32 STDCALL Registry_getArray (Handle, const char*&, UInt32& size);
	EXPORT Int32 STDCALL Registry_getHandle (Handle, Handle& handle);

	EXPORT Int32 STDCALL Registry_isInProcess (Handle, Int32&);

	inline Int32* StringToIntPtr (const wchar_t* str)
	{
		static Int32 len = 256;
		static Int32* buf = new Int32 [len];

		Int32 size = wcslen (str);
		if (len < size + 1)
		{
			delete [] buf;
			len = size + 1;
			buf = new Int32 [len];
		}

		buf [0] = size;
		for (Int32 i=1; i <= size; ++i)
		{
			buf [i] = str [i];
		}
		return buf;
	}

	inline Int32 Registry_putWideString (Handle handle, const wchar_t* str)
	{
		return Registry_putString (handle, StringToIntPtr (str));
	}

	inline Int32 Registry_getWideString (Handle handle, const wchar_t*& str)
	{
		static Int32 len = 256;
		static wchar_t* buf = new wchar_t [len];
		
		const Int32* s;
		if (rcOk != Registry_getString (handle, s)) 
			return rcInvalid;

		Int32 size = *s++;
		
		if (len < size + 1)
		{
			delete [] buf;
			len = size + 1;
			buf = new wchar_t [len];
		}

		for (int i=0; i < size; ++i)
		{
			buf [i] = *s++;
		}

		str = buf;
		return rcOk;
	}

	inline Int32 Registry_putStdString (Handle handle, const std::wstring& str)
	{
		return Registry_putWideString (handle, str.c_str ());
	}

	inline Int32 Registry_getStdString (Handle handle, std::wstring& str)
	{
		const wchar_t* s;
		if (rcOk != Registry_getWideString (handle, s)) 
			return rcInvalid;

		str = s;
		return rcOk;
	}

	inline Int32 Registry_putBool (Handle handle, bool value)
	{
		return Registry_putByte (handle, value ? 1 : 0);
	}

	inline Int32 Registry_getBool (Handle handle, bool& value)
	{
		Int32 t;
		Int32 result = Registry_getByte (handle, t);
		value = (t == 1);
		return result;
	}
}

namespace Panda
{
	class Registry
	{
		public:
			class Exception: public std::runtime_error
			{
				public:
					Exception (Int32 code, const char* msg) throw ():
						std::runtime_error (msg),
						errorCode (code)
					{
					}

					Int32 errorCode;
			};

			static void getInstance (const char* serviceName, Handle& handle) throw (Exception)
			{
				Int32 result = Registry_getInstance (serviceName, handle);
				if (result != rcOk)
					throwError (result);
			}

			static Handle getInstance (const char* serviceName) throw (Exception)
			{
				Handle handle;
				Int32 result = Registry_getInstance (serviceName, handle);
				if (result != rcOk)
					throwError (result);

				return handle;
			}

			static void freeInstance (Handle handle) throw (Exception)
			{
				Int32 result = Registry_freeInstance (handle);
				if (result != rcOk)
					throwError (result);
			}

			static void beginMessage (Handle handle, Int32 command) throw (Exception)
			{
				Int32 result = Registry_beginMessage (handle, command);
				if (result != rcOk)
					throwError (result);
			}

			static void endMessage (Handle handle) throw (Exception)
			{
				Int32 result = Registry_endMessage (handle);
				if (result != rcOk)
					throwError (result);
			}

			static void putByte (Handle handle, Int32 value) throw (Exception)
			{
				Int32 result = Registry_putByte (handle, value);
				if (result != rcOk)
					throwError (result);
			}

			static void putShort (Handle handle, Int32 value) throw (Exception)
			{
				Int32 result = Registry_putShort (handle, value);
				if (result != rcOk)
					throwError (result);
			}

			static void putInt32 (Handle handle, Int32 value) throw (Exception)
			{
				Int32 result = Registry_putInt32 (handle, value);
				if (result != rcOk)
					throwError (result);
			}

			static void putInt64 (Handle handle, Int64 value) throw (Exception)
			{
				Int32 result = Registry_putInt64 (handle, value);
				if (result != rcOk)
					throwError (result);
			}

			static void putUShort (Handle handle, UInt32 value) throw (Exception)
			{
				Int32 result = Registry_putUShort (handle, value);
				if (result != rcOk)
					throwError (result);
			}

			static void putUInt32 (Handle handle, UInt32 value) throw (Exception)
			{
				Int32 result = Registry_putUInt32 (handle, value);
				if (result != rcOk)
					throwError (result);
			}

			static void putUInt64 (Handle handle, UInt64 value) throw (Exception)
			{
				Int32 result = Registry_putUInt64 (handle, value);
				if (result != rcOk)
					throwError (result);
			}

			static void putFloat (Handle handle, float value) throw (Exception)
			{
				Int32 result = Registry_putFloat (handle, value);
				if (result != rcOk)
					throwError (result);
			}

			static void putDouble (Handle handle, double value) throw (Exception)
			{
				Int32 result = Registry_putDouble (handle, value);
				if (result != rcOk)
					throwError (result);
			}

			static void putWideString (Handle handle, const wchar_t* str) throw (Exception)
			{
				Int32 result = Registry_putWideString (handle, str);
				if (result != rcOk)
					throwError (result);
			}

			static void putStdString (Handle handle, const std::wstring& value) throw (Exception)
			{
				Int32 result = Registry_putWideString (handle, value.c_str ());
				if (result != rcOk)
					throwError (result);
			}

			static void putArray (Handle handle, const char* buffer, UInt32 size) throw (Exception)
			{
				Int32 result = Registry_putArray (handle, buffer, size);
				if (result != rcOk)
					throwError (result);
			}

			static void getByte (Handle handle, Int32& value) throw (Exception)
			{
				Int32 result = Registry_getByte (handle, value);
				if (result != rcOk)
					throwError (result);
			}
            
            static Int32 getByte (Handle handle) throw (Exception)
            {
                Int32 value;
				Int32 result = Registry_getByte (handle, value);
				if (result != rcOk)
					throwError (result);
                
                return value;
            }

			static void getShort (Handle handle, Int32& value) throw (Exception)
			{
				Int32 result = Registry_getShort (handle, value);
				if (result != rcOk)
					throwError (result);
			}

			static Int32 getShort (Handle handle) throw (Exception)
            {
                Int32 value;
				Int32 result = Registry_getShort (handle, value);
				if (result != rcOk)
					throwError (result);
                
                return value;
            }

			static void getInt32 (Handle handle, Int32& value) throw (Exception)
			{
				Int32 result = Registry_getInt32 (handle, value);
				if (result != rcOk)
					throwError (result);
			}
            
            static Int32 getInt32 (Handle handle) throw (Exception)
            {
                Int32 value;
				Int32 result = Registry_getInt32 (handle, value);
				if (result != rcOk)
					throwError (result);
                
                return value;
            }

			static void getInt64 (Handle handle, Int64& value) throw (Exception)
			{
				Int32 result = Registry_getInt64 (handle, value);
				if (result != rcOk)
					throwError (result);
			}
            
            static Int64 getInt64 (Handle handle) throw (Exception)
            {
                Int64 value;
				Int32 result = Registry_getInt64 (handle, value);
				if (result != rcOk)
					throwError (result);
                
                return value;
            }


			static void getUShort (Handle handle, UInt32& value) throw (Exception)
			{
				Int32 result = Registry_getUShort (handle, value);
				if (result != rcOk)
					throwError (result);
			}

			static UInt32 getUShort (Handle handle) throw (Exception)
            {
                UInt32 value;
				Int32 result = Registry_getUShort (handle, value);
				if (result != rcOk)
					throwError (result);
                
                return value;
            }

			static void getUInt32 (Handle handle, UInt32& value) throw (Exception)
			{
				Int32 result = Registry_getUInt32 (handle, value);
				if (result != rcOk)
					throwError (result);
			}
            
            static UInt32 getUInt32 (Handle handle) throw (Exception)
            {
                UInt32 value;
				Int32 result = Registry_getUInt32 (handle, value);
				if (result != rcOk)
					throwError (result);
                
                return value;
            }

			static void getUInt64 (Handle handle, UInt64& value) throw (Exception)
			{
				Int32 result = Registry_getUInt64 (handle, value);
				if (result != rcOk)
					throwError (result);
			}
            
            static UInt64 getUInt64 (Handle handle) throw (Exception)
            {
                UInt64 value;
				Int32 result = Registry_getUInt64 (handle, value);
				if (result != rcOk)
					throwError (result);
                
                return value;
            }

			static void getFloat (Handle handle, float& value) throw (Exception)
			{
				Int32 result = Registry_getFloat (handle, value);
				if (result != rcOk)
					throwError (result);
			}

			static float getFloat (Handle handle) throw (Exception)
            {
                float value;
				Int32 result = Registry_getFloat (handle, value);
				if (result != rcOk)
					throwError (result);
                
                return value;                
            }
            
			static void getDouble (Handle handle, double& value) throw (Exception)
			{
				Int32 result = Registry_getDouble (handle, value);
				if (result != rcOk)
					throwError (result);
			}

			static double getDouble (Handle handle) throw (Exception)
			{
                double value;
				Int32 result = Registry_getDouble (handle, value);
				if (result != rcOk)
					throwError (result);
                
                return value;
			}

			static void getWideString (Handle handle, const wchar_t*& str) throw (Exception)
			{
				Int32 result =  Registry_getWideString (handle, str);
				if (result != rcOk)
					throwError (result);
			}

			static const wchar_t* getWideString (Handle handle) throw (Exception)
			{
				const wchar_t* str;
				Int32 result =  Registry_getWideString (handle, str);
				if (result != rcOk)
					throwError (result);

				return str;
			}

			static void getStdString (Handle handle, std::wstring& value) throw (Exception)
			{
				const wchar_t* str;
				getWideString (handle, str);
				value = str;
			}

			static std::wstring getStdString (Handle handle) throw (Exception)
            {
				const wchar_t* str;
				getWideString (handle, str);
				return str;
            }
            
			static void getArray (Handle handle, const char*& buffer, UInt32& size) throw (Exception)
			{
				Int32 result = Registry_getArray (handle, buffer, size);
				if (result != rcOk)
					throwError (result);
			}

			static void putBool (Handle handle, bool value) throw (Exception)
			{
				Int32 result = Registry_putByte (handle, value ? 1 : 0);
				if (result != rcOk)
					throwError (result);
			}

			static void getBool (Handle handle, bool& value) throw (Exception)
			{
				Int32 t;
				Int32 result = Registry_getByte (handle, t);
				if (result != rcOk)
					throwError (result);

				value = (t == 1);
			}

			static bool getBool (Handle handle) throw (Exception)
            {
				Int32 t;
				Int32 result = Registry_getByte (handle, t);
				if (result != rcOk)
					throwError (result);

				return (t == 1);
            }

			static bool isInProcess (Handle handle) throw (Exception)
			{
				Int32 t;
				Int32 result = Registry_isInProcess (handle, t);
				if (result != rcOk)
					throwError (result);

				return (t == 1);
			}
            
			static void throwError (Int32 errorCode) throw (Exception)
			{
				const char* msg;
				switch (errorCode)
				{
					case rcClassNotFound:
						msg = "Class not found";
						break;

					case rcClassInUse:
						msg = "Class in use";
						break;

					case rcErrorLoadingDll:
						msg = "Error loading dll";
						break;

					case rcEntryPointNotFound:
						msg = "Entry point not found";
						break;

					case rcClassAlreadyRegistered:
						msg = "Class is already registered";
						break;

					case rcObjectNotFound:
						msg = "Object not found";
						break;

					case rcException:
						msg = "Exception occured during execution";
						break;

					case rcInvalid:
					case rcInvalidParameter:
						msg = "Invalid parameter";
						break;

					case rcNotImplemented:
						msg = "Not implemented";
						break;

					default:
						msg = "Unknown error";
				}

				throw Exception (errorCode, msg);
			}
	};
}

#endif /*CONTRACT_H_*/
