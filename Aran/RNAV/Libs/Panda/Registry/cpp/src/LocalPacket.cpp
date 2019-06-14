#include <cstring>
#include <cmath>
#include "../include/LocalPacket.h"

LocalPacket::LocalPacket ()
{
	_size = 256;
    _buffer = new unsigned char [_size];
    _readPos = 0;
    _writePos = 0;
}

LocalPacket::~LocalPacket ()
{
	delete [] _buffer;
}

void LocalPacket::putByte (Int32 value)
{
	if (_writePos == _size)
	{
		Int32 newSize = _size * 2;
		unsigned char* newBuffer = new unsigned char [newSize];
		memcpy (newBuffer, _buffer, _size);
		delete [] _buffer;
		_buffer = newBuffer;
		_size = newSize;
	}
	_buffer [_writePos++] = (unsigned char)(value & 255);
}

void LocalPacket::putByte (UInt32 value)
{
	if (_writePos == _size)
	{
		Int32 newSize = _size * 2;
		unsigned char* newBuffer = new unsigned char [newSize];
		memcpy (newBuffer, _buffer, _size);
		delete [] _buffer;
		_buffer = newBuffer;
		_size = newSize;
	}
	_buffer [_writePos++] = (unsigned char)(value & 255);
}

void LocalPacket::putShort (Int32 value)
{
	putByte (value);
	putByte (value >> 8);
}

void LocalPacket::putInt32 (Int32 value)
{
	putShort (value);
	putShort (value >> 16);
}

void LocalPacket::putInt64 (Int64 value)
{
	putInt32 (value);
	putInt32 (value >> 32);
}

void LocalPacket::putUShort (UInt32 value)
{
	putByte (value);
	putByte (value >> 8);
}

void LocalPacket::putUInt32 (UInt32 value)
{
	putUShort (value);
	putUShort (value >> 16);
}

void LocalPacket::putUInt64 (UInt64 value)
{
	putUInt32 (value);
	putUInt32 (value >> 32);
}

void LocalPacket::putFloat (float value)
{
	UInt32 *tt = (UInt32*)(&value);
	putUInt32(*tt);

/*    Int32 exp;
    Int32 mantissa = (Int32) ldexp (frexp (value, &exp), 24);

    putInt32 (mantissa);
    putByte (exp - 24);
*/
}

void LocalPacket::putDouble (double value)
{
	UInt64 *tt = (UInt64*)(&value);
	putUInt64(*tt);

/*	Int32 exp;
	long long mantissa = (long long) ldexp (frexp (value, &exp), 53);

    putInt32 (static_cast <Int32> (mantissa & 0xffffffff));
    putInt32 (static_cast <Int32> (mantissa >> 32));
    putShort (exp - 53);*/
}

void LocalPacket::putString (const Int32* value)
{
	Int32 len = *value++;
	putInt32 (len);
	for (Int32 i=0; i<len; ++i)
		putInt32 (*value++);
}

void LocalPacket::putArray (const char* data, UInt32 size)
{
	putInt32 (size);
	for (UInt32 i=0; i < size; ++i)
		putByte (*data++);
}

void LocalPacket::putHandle (Handle handle)
{
	putInt32 (handle);
}

bool LocalPacket::getByte (Int32& value)
{
    if (_readPos < _writePos)
    {
        value = _buffer [_readPos++];
        return true;
    }

    value = 0;
    return false;
}

bool LocalPacket::getByte (UInt32& value)
{
    if (_readPos < _writePos)
    {
        value = _buffer [_readPos++];
        return true;
    }

    value = 0;
    return false;
}

bool LocalPacket::getShort (Int32& value)
{
	Int32 low;
	if (! getByte (low))
		return false;

	low &= 0xff;

	Int32 high;
	if (! getByte (high))
		return false;
	
	value = static_cast <unsigned char> (low) | (high << 8);
    return true;	
}

bool LocalPacket::getInt32 (Int32& value)
{
	Int32 low;
	if (! getShort (low))
		return false;
	
	low &= 0xffff;

	Int32 high;
	if (! getShort (high))
		return false;
	
	value = static_cast <unsigned short> (low) | (high << 16);
    return true;
}

bool LocalPacket::getInt64 (Int64& value)
{
	Int32	low;
	if (! getInt32 (low))
		return false;

	Int32	high;
	if (! getInt32 (high))
		return false;

	value = high;
	value <<= 32;
	value |= low;

    return true;
}

bool LocalPacket::getUShort (UInt32& value)
{
	UInt32 low;
	if (! getByte (low))
		return false;

	UInt32 high;
	if (! getByte (high))
		return false;

	value = (high << 8) | low;
    return true;	
}

bool LocalPacket::getUInt32 (UInt32& value)
{
	UInt32 low;
	if (! getUShort (low))
		return false;
	
	UInt32 high;
	if (! getUShort (high))
		return false;

	value = (high << 16) | low;
    return true;
}

bool LocalPacket::getUInt64 (UInt64& value)
{
	UInt32	low;
	if (! getUInt32 (low))
		return false;

	UInt32	high;
	if (! getUInt32 (high))
		return false;

	value = high;

	value = (value << 32) | low;

    return true;
}

bool LocalPacket::getFloat (float& value)
{
	UInt32 *tt = (UInt32*)(&value);
	getUInt32(*tt);
	return true;

/*	Int32 mantissa;
	if (! getInt32 (mantissa))
		return false;
	
	Int32 exp;
    if (! getByte (exp))
        return false;

    value = static_cast <float> (ldexp (static_cast <double> (mantissa), exp));
	return true;*/
}

bool LocalPacket::getDouble (double& value)
{
	UInt64 *tt = (UInt64*)(&value);
	getUInt64(*tt);
	return true;

/*	Int32 low;
	if (! getInt32 (low))
		return false;
	
	Int32 high;
	if (! getInt32 (high))
		return false;
	
	Int32 exp;
	if (! getShort (exp))
		return false;
	
	long long mantissa =  (static_cast <long long> (high) << 32) | static_cast <UInt32> (low);
	value = ldexp (static_cast <double> (mantissa), exp);
	return true;*/
}

bool LocalPacket::getString (const Int32*& value)
{
	value = reinterpret_cast <const Int32*> (&_buffer [_readPos]);
	Int32 len;
	if (! getInt32 (len))
		return false;

	_readPos += len * sizeof (Int32);
	return true;	
}

bool LocalPacket::getArray (const char*& value, UInt32& size)
{
	if (! getInt32 (reinterpret_cast <Int32&> (size)))
		return false;

	value = (char *)(&_buffer [_readPos]);
	_readPos += size;
	return true;
}

bool LocalPacket::getHandle (Handle& handle)
{
	return getInt32 (handle);
}

void LocalPacket::clear ()
{
	_readPos = 0;
	_writePos = 0;
}
