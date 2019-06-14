#include "../include/LocalClass.h"
#include "../include/LocalPacket.h"

LocalClass::LocalClass (std::string name, Int32 priority, Method method, HModule hModule):
	Class (name, priority, method, hModule)
{
	attach ();
}

LocalClass::~LocalClass ()
{
	detach ();
}

Int32 LocalClass::attach ()
{
	return _method (0, svcAttachClass, 0);
}

Int32 LocalClass::detach ()
{
	return _method (0, svcDetachClass, 0);
}

Handle LocalClass::getInstance (Handle serviceObject)
{
	addRef ();
	return _method (0, svcGetInstance, serviceObject);
}

void LocalClass::freeInstance (Handle privateData, Handle serviceObject)
{
	removeRef ();
	method (privateData, svcFreeInstance, serviceObject);
}

Packet* LocalClass::createPacket ()
{
	return new LocalPacket ();
}

Int32 LocalClass::method (Handle privateData, Int32 command, Handle handle)
{
	return _method (privateData, command, handle);
}

bool LocalClass::isLocal () const
{
	return true;
}
