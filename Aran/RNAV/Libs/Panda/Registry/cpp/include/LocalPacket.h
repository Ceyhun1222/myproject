#ifndef LOCAL_PACKET_H_
#define LOCAL_PACKET_H_

#include "Packet.h"

class LocalPacket: public Packet
{
	public:
		LocalPacket ();
		virtual ~LocalPacket ();
		
		virtual void putByte (Int32);
		virtual void putShort (Int32);
		virtual void putInt32 (Int32);
		virtual void putInt64 (Int64);

		virtual void putByte (UInt32);
		virtual void putUShort (UInt32);
		virtual void putUInt32 (UInt32);
		virtual void putUInt64 (UInt64);

		virtual void putFloat (float);
		virtual void putDouble (double);
		virtual void putString (const Int32*);
		virtual void putArray (const char*, UInt32 size);
		virtual void putHandle (Handle handle);

		virtual bool getByte (Int32&);
		virtual bool getByte (UInt32&);
		virtual bool getShort (Int32&);
		virtual bool getInt32 (Int32&);
		virtual bool getInt64 (Int64&);
		virtual bool getUShort (UInt32&);
		virtual bool getUInt32 (UInt32&);
		virtual bool getUInt64 (UInt64&);

		virtual bool getFloat (float&);
		virtual bool getDouble (double&);
		virtual bool getString (const Int32*&);
		virtual bool getArray (const char*&, UInt32& size);
		virtual bool getHandle (Handle& handle);
		
		virtual void clear ();

	private:
	    unsigned char* _buffer;
	    Int32 _size;
	    Int32 _readPos;
	    Int32 _writePos;
};

#endif /*LOCALMESSAGE_H_*/
