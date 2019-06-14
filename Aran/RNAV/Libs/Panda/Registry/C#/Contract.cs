using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ARAN.Contracts.Registry
{
	public class Registry_Contract
	{
		#region Putting Simple Data Types into Packet

		public static void PutDouble(int handle, double value)
		{
			int res = Registry_PutDouble(handle, value);
			if (res != rcOK)
				throw new AranException(res, "Exception in putting double");
		}

		public static void PutInt32(int handle, int value)
		{
			int res = Registry_PutInt32(handle, value);
			if (res != rcOK)
				throw new AranException(res, "Exception in putting int");
		}

		public static void PutInt64(int handle, int value)
		{
			int res = Registry_PutInt64(handle, value);
			if (res != rcOK)
				throw new AranException(res, "Exception in putting int");
		}

		public static void PutString(int handle, string value)
		{
			int[] ip = new int[value.Length + 1];
			ip[0] = value.Length;
			for (int i = 0; i < value.Length; i++)
				ip[i + 1] = value[i];

			int res = Registry_PutString(handle, ip);
			if (res != rcOK)
				throw new AranException(res, "Exception in putting string");
		}

		public static void PutBool(int handle, bool value)
		{
			int res = Registry_PutByte(handle, Convert.ToByte(value));
			if (res != rcOK)
				throw new AranException(res, "Exception in putting bool");
		}

		public static void PutByte(int handle, int value)
		{
			int res = Registry_PutByte(handle, value);
			if (res != rcOK)
				throw new AranException(res, "Exception in putting byte");
		}

		public static void PutShort(int handle, int value)
		{
			int res = Registry_PutShort(handle, value);
			if (res != rcOK)
				throw new AranException(res, "Exception in putting short");
		}

		public static void PutFloat(int handle, float value)
		{
			int res = Registry_PutFloat(handle, value);
			if (res != rcOK)
				throw new AranException(res, "Exception in putting float");
		}

		unsafe public static void PutArray(int handle, void* dataPtr, uint size)
		{
			int res = Registry_PutArray(handle, dataPtr, size);
			if (res != rcOK)
				throw new AranException(res, "Exception in putting array");
		}

		public static void PutHandle(int handle, int h)
		{
			int res = Registry_PutHandle(handle, h);
			if (res != rcOK)
				throw new AranException(res, "Exception in putting handle");
		}

		#endregion

		#region Getting Simple Data Types from Packet

		public static double GetDouble(int handle)
		{
			double val = 0;
			int res = Registry_GetDouble(handle, ref val);
			if (res != rcOK)
				throw new AranException(res, "Exception in getting double");
			return val;
		}

		public static int GetInt32(int handle)
		{
			int val = 0;
			int res = Registry_GetInt32(handle, ref val);
			if (res != rcOK)
				throw new AranException(res, "Exception in getting int");
			return val;
		}

		public static int GetInt64(int handle)
		{
			int val = 0;
			int res = Registry_GetInt64(handle, ref val);
			if (res != rcOK)
				throw new AranException(res, "Exception in getting int");
			return val;
		}

		public static string GetString(int handle)
		{
			unsafe
			{
				string val = "";
				int temp = 0;
				int* value = &temp;
				int res = Registry_GetString(handle, ref value);
				if (res != rcOK)
					throw new AranException(res, "Exception in getting string");
				int length = *value;
				while (length > 0)
				{
					value++;
					val += (char)*value;
					length--;
				}
				return val;
			}
		}

		public static List<String> GetStringArray(int handle)
		{
			Int32 strLen = GetInt32(handle);
			List<String> result = new List<String>();

			for (int i = 0; i < strLen; i++)
				result.Add(GetString(handle));

			return result;
		}

		public static bool GetBool(int handle)
		{
			int val = 0;
			int res = Registry_GetByte(handle, ref val);
			if (res != rcOK)
				throw new AranException(res, "Exception in getting bool");
			return (val != 0);
		}

		public static byte GetByte(int handle)
		{
			int val = 0;
			int res = Registry_GetByte(handle, ref val);
			if (res != rcOK)
				throw new AranException(res, "Exception in getting byte");
			return (byte)val;
		}

		public static short GetShort(int handle)
		{
			int val = 0;
			int res = Registry_GetShort(handle, ref val);
			if (res != rcOK)
				throw new AranException(res, "Exception in getting short");
			return (short)val;
		}

		public static float GetFloat(int handle)
		{
			float val = 0;
			int res = Registry_GetFloat(handle, ref val);
			if (res != rcOK)
				throw new AranException(res, "Exception in getting float");
			return val;
		}

		unsafe public static void GetArray(int handle, ref void* dataPtr, ref uint size)
		{
			int res = Registry_GetArray(handle, ref dataPtr, ref size);
			if (res != rcOK)
				throw new AranException(res, "Exception in getting Array");
		}

		public static int GetHandle(int handle)
		{
			int val = 0;
			int res = Registry_GetHandle(handle, ref val);
			if (res != rcOK)
				throw new AranException(res, "Exception in getting handle");
			return val;
		}

		#endregion

		public static int GetInstance(string className)
		{
			int result = 0;
			int error = Registry_GetInstance(className, ref result);
			if (error != rcOK)
				throw new AranException(error, "Exception in getting Instance");
			return result;
		}

		public static void FreeInstance(int handle)
		{
			int result = Registry_FreeInstance(handle);
			if (result != rcOK)
				throw new AranException(result, "Exception in process of free Instance");
		}

		public static void BeginMessage(int handle, int command)
		{
			int error = Registry_BeginMessage(handle, command);
			if (error != rcOK)
				throw new AranException(error, "Exception in Begining Message");
		}

		public static void EndMessage(int handle)
		{
			int error = Registry_EndMessage(handle);
			if (error != rcOK)
				throw new AranException(error, "Exception in Ending Message");
		}

		public static GCHandle SetHandler(int handle, Object _this, Method method)
		{
			GCHandle gcHandle = GCHandle.Alloc(_this, GCHandleType.Normal);
			IntPtr pManagedObject = GCHandle.ToIntPtr(gcHandle);

			int res = Registry_SetHandler(handle, pManagedObject.ToInt32(), method);
			if (res != rcOK)
				throw new AranException(res, "Exception in Setting handler");
			return gcHandle;
		}

		public static void UnsetHandler(int handle)
		{
			int res = Registry_UnsetHandler(handle);
			if (res != rcOK)
				throw new AranException(res, "Exception in unsetting handler");
		}

		public static int RegisterClass(string className, int priority, Method method)
		{
			int handle = 0;
			int res = Registry_RegisterClass(className, priority, method, 0, ref handle);
			if (res != rcOK)
				throw new AranException(res, "Exception in registering class");
			return handle;
		}

        public static int RegisterClass2 (string className, int priority, int method)
        {
            int handle = 0;
            int res = Registry_RegisterClass2 (className, priority, method, 0, ref handle);
            if (res != rcOK)
                throw new AranException (res, "Exception in registering class");
            return handle;
        }

		public static void UnregisterClass(int handle)
		{
			int res = Registry_UnregisterClass(handle);
			if (res != rcOK)
				throw new AranException(res, "Exception in unregistering class");
		}

		public static int LoadService(string className, string path, string entyrName, int priority)
		{
			int val = 0;
			int res = Registry_LoadService(className, path, entyrName, priority, ref val);
			if (res != rcOK)
				throw new AranException(res, "Exception in loading service");
			return val;
		}

		public static bool IsInProcess(int handle)
		{
			int val = 0;
			int res = Registry_IsInProcess(handle, ref val);
			if (res != rcOK)
				throw new AranException(res, "Exception in trying to learn service is in process or not");
			return (val != 0);
		}

		public static void BeginEvent(int handle, int command)
		{
			int res = Registry_BeginEvent(handle, command);
			if (res != rcOK)
				throw new AranException(res, "Exception in beginning event");
		}

		public static void EndEvent(int handle)
		{
			int res = Registry_EndEvent(handle);
			if (res != rcOK)
				throw new AranException(res, "Exception in ending event");
		}

		#region Private Access Modifier Part

		public const Int32 svcGetInstance = 0x10000;
		public const Int32 svcFreeInstance = 0x10001;
		public const Int32 svcAttachClass = 0x10002;
		public const Int32 svcDetachClass = 0x10003;

		public const Int32 rcClassNotFound = -0x10000;
		public const Int32 rcClassInUse = rcClassNotFound + 01;
		public const Int32 rcErrorLoadingDll = rcClassNotFound + 02;
		public const Int32 rcEntryPointNotFound = rcClassNotFound + 03;
		public const Int32 rcObjectNotFound = rcClassNotFound + 04;
		public const Int32 rcException = rcClassNotFound + 05;
		public const Int32 rcClassAlreadyRegistered = rcClassNotFound + 06;
		public const Int32 rcInvalid = rcClassNotFound + 07;
		public const Int32 rcInvalidParameter = rcClassNotFound + 08;
		public const Int32 rcNotImplemented = rcClassNotFound + 09;
		public const Int32 rcOK = 0;

		#endregion

		[UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
		public delegate int Method(Int32 _this, Int32 command, Int32 inout);

		#region Imported Methods from Registry.DLL which has written in C++

		[DllImport("Registry.dll", EntryPoint = "Registry_getInstance")]
		private static extern int Registry_GetInstance(string className, ref int handle);

		[DllImport("Registry.dll", EntryPoint = "Registry_freeInstance")]
		private static extern int Registry_FreeInstance(int handle);

		[DllImport("Registry.dll", EntryPoint = "Registry_beginMessage")]
		private static extern int Registry_BeginMessage(int handle, int command);

		[DllImport("Registry.dll", EntryPoint = "Registry_endMessage")]
		private static extern int Registry_EndMessage(int handle);

		[DllImport("Registry.dll", EntryPoint = "Registry_putString")]
		private static extern int Registry_PutString(int handle,
					[MarshalAs(UnmanagedType.LPArray)] Int32[] value);

		[DllImport("Registry.dll", EntryPoint = "Registry_getString")]
		unsafe private static extern int Registry_GetString(int handle, ref int* value);
		//[MarshalAs (UnmanagedType.LPArray)] ref Int32[] value);

		[DllImport("Registry.dll", EntryPoint = "Registry_putDouble")]
		private static extern int Registry_PutDouble(int handle, double value);

		[DllImport("Registry.dll", EntryPoint = "Registry_getDouble")]
		private static extern int Registry_GetDouble(int handle, ref double value);

		[DllImport("Registry.dll", EntryPoint = "Registry_putInt32")]
		private static extern int Registry_PutInt32(int handle, int value);

		[DllImport("Registry.dll", EntryPoint = "Registry_putInt64")]
		private static extern int Registry_PutInt64(int handle, int value);

		[DllImport("Registry.dll", EntryPoint = "Registry_getInt32")]
		private static extern int Registry_GetInt32(int handle, ref int value);

		[DllImport("Registry.dll", EntryPoint = "Registry_getInt64")]
		private static extern int Registry_GetInt64(int handle, ref int value);

		[DllImport("Registry.dll", EntryPoint = "Registry_putByte")]
		private static extern int Registry_PutByte(int handle, int value);

		[DllImport("Registry.dll", EntryPoint = "Registry_getByte")]
		private static extern int Registry_GetByte(int handle, ref int value);

		[DllImport("Registry.dll", EntryPoint = "Registry_putShort")]
		private static extern int Registry_PutShort(int handle, int value);

		[DllImport("Registry.dll", EntryPoint = "Registry_getShort")]
		private static extern int Registry_GetShort(int handle, ref int value);

		[DllImport("Registry.dll", EntryPoint = "Registry_putFloat")]
		private static extern int Registry_PutFloat(int handle, float value);

		[DllImport("Registry.dll", EntryPoint = "Registry_getFloat")]
		private static extern int Registry_GetFloat(int handle, ref float value);

		[DllImport("Registry.dll", EntryPoint = "Registry_putArray")]
		unsafe private static extern int Registry_PutArray(int handle, void* dataPtr, uint size);

		[DllImport("Registry.dll", EntryPoint = "Registry_getArray")]
		unsafe private static extern int Registry_GetArray(int handle, ref void* dataPtr, ref uint size);

		[DllImport("Registry.dll", EntryPoint = "Registry_putHandle")]
		private static extern int Registry_PutHandle(int handle, int h);

		[DllImport("Registry.dll", EntryPoint = "Registry_getHandle")]
		private static extern int Registry_GetHandle(int handle, ref int value);

		[DllImport("Registry.dll", EntryPoint = "Registry_setHandler")]
		private static extern int Registry_SetHandler(Int32 handle, Int32 _this, Method method);

		[DllImport("Registry.dll", EntryPoint = "Registry_unsetHandler")]
		private static extern int Registry_UnsetHandler(int handle);

		[DllImport("Registry.dll", EntryPoint = "Registry_registerClass")]
		private static extern int Registry_RegisterClass(string className, int priority, Method method, int hInst, ref int handle);

        [DllImport ("Registry.dll", EntryPoint = "Registry_registerClass")]
        private static extern int Registry_RegisterClass2 (string className, int priority, int method, int hInst, ref int handle);

		[DllImport("Registry.dll", EntryPoint = "Registry_unregisterClass")]
		private static extern int Registry_UnregisterClass(int handle);

		[DllImport("Registry.dll", EntryPoint = "Registry_loadService")]
		private static extern int Registry_LoadService(string className, string path, string entryName, int priority, ref int handle);

		[DllImport("Registry.dll", EntryPoint = "Registry_isInProcess")]
		private static extern int Registry_IsInProcess(int handle, ref int result);

		[DllImport("Registry.dll", EntryPoint = "Registry_beginEvent")]
		private static extern int Registry_BeginEvent(int handle, int command);

		[DllImport("Registry.dll", EntryPoint = "Registry_endEvent")]
		private static extern int Registry_EndEvent(int handle);

		# endregion
	}

	public class AranException : Exception
	{
		public AranException(int errorCode, string message)
			: base(message)
		{
			_errorCode = errorCode;
			_errorMessage = message;
		}

		public int ErrorCode
		{
			get
			{
				return _errorCode;
			}
		}

		private string ErrorMessage
		{
			get
			{
				return _errorMessage;
			}
		}
		private int _errorCode;
		private string _errorMessage;

		public static void ThrowException(Int32 errorCode)
		{
			string msg;
			switch (errorCode)
			{
				case Registry_Contract.rcClassNotFound:
					msg = "Class not found.";
					break;
				case Registry_Contract.rcClassInUse:
					msg = "Class in use.";
					break;
				case Registry_Contract.rcErrorLoadingDll:
					msg = "Error loading dll.";
					break;
				case Registry_Contract.rcEntryPointNotFound:
					msg = "Entry point not found.";
					break;
				case Registry_Contract.rcClassAlreadyRegistered:
					msg = "Class is already registered.";
					break;
				case Registry_Contract.rcObjectNotFound:
					msg = "Object not found.";
					break;
				case Registry_Contract.rcException:
					msg = "Exception occured during execution.";
					break;
				case Registry_Contract.rcInvalid:
					msg = "Invalid call.";
					break;
				case Registry_Contract.rcInvalidParameter:
					msg = "Invalid parameter.";
					break;
				case Registry_Contract.rcNotImplemented:
					msg = "Not implemented.";
					break;
				default:
					msg = "Unknown error.";
					break;
			}

			//throw new Exception(msg);
			throw new AranException(errorCode, msg);
		}
	}
}
