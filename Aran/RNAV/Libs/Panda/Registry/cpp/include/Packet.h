#ifndef PACKET_H_
#define PACKET_H_

#include "Contract.h"

class Packet
{
	public:
		virtual ~Packet () { }
		virtual void putByte (Int32) = 0;
		virtual void putShort (Int32) = 0;
		virtual void putInt32 (Int32) = 0;
		virtual void putInt64 (Int64) = 0;
		//virtual void putByte (UInt32) = 0;
		virtual void putUShort (UInt32) = 0;
		virtual void putUInt32 (UInt32) = 0;
		virtual void putUInt64 (UInt64) = 0;

		virtual void putFloat (float) = 0;;
		virtual void putDouble (double) = 0;;
		virtual void putString (const Int32*) = 0;;
		virtual void putArray (const char*, UInt32 size) = 0;
		virtual void putHandle (Handle handle) = 0;

		virtual bool getByte (Int32&) = 0;
		virtual bool getShort (Int32&) = 0;
		virtual bool getInt32 (Int32&) = 0;
		virtual bool getInt64 (Int64&) = 0;
		//virtual bool getByte (UInt32&) = 0;
		virtual bool getUShort (UInt32&) = 0;
		virtual bool getUInt32 (UInt32&) = 0;
		virtual bool getUInt64 (UInt64&) = 0;

		virtual bool getFloat (float&) = 0;
		virtual bool getDouble (double&) = 0;
		virtual bool getString (const Int32*&) = 0;
		virtual bool getArray (const char*&, UInt32& size) = 0;
		virtual bool getHandle (Handle& handle) = 0;
		
		virtual void clear () = 0;
};

#endif
