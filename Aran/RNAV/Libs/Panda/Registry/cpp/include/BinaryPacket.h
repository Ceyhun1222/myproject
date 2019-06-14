#ifndef BINARY_PACKET_H
#define BINARY_PACKET_H

#include "LocalPacket.h"

class BinaryPacket: public LocalPacket
{
	public:
		virtual void putString (const Int32*);
		virtual void putArray (const char*, UInt32 size);

		virtual bool getString (const Int32*&);
		virtual bool getArray (const char*&, UInt32& size);
};

#endif
