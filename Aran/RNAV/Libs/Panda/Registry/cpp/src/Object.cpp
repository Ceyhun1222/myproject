#include "../include/Object.h"
#include "../include/Class.h"
#include "../include/Packet.h"

Object::Object (Class& cl):
	_class (cl),
	_privateData (0),
	_outputPacket (cl.createPacket ()),
	_inputPacket (cl.createPacket ()),
	_handle (0),
	_handlerData (0),
	_handler (0)
{
}

Object::~Object ()
{
	delete _outputPacket;
	delete _inputPacket;
}

void Object::free ()
{
	_class.freeInstance (_privateData, _handle);
	delete this;
}

void Object::setHandle (Handle handle)
{
	_handle = handle;
}

Handle Object::getHandle () const
{
	return _handle;
}

void Object::setHandler (Int32 handlerData, Method method)
{
	_handlerData = handlerData;
	_handler = method;
}

void Object::unsetHandler ()
{
	_handlerData = 0;
	_handler = 0;
}

void Object::setPrivateData (Handle privateData)
{
	_privateData = privateData;
}

Handle Object::getPrivateData () const
{
	return _privateData;
}

Int32 Object::beginMessage (Int32 command)
{
	_outputPacket->clear ();
	_command = command;
	return rcOk;
}

Int32 Object::endMessage ()
{
	_inputPacket->clear ();

	Packet* t = _outputPacket;
	_outputPacket = _inputPacket;
	_inputPacket = t;

	Int32 result = _class.method (_privateData, _command, _handle);

	t = _outputPacket;
	_outputPacket = _inputPacket;
	_inputPacket = t;

	return result;
}

Int32 Object::beginEvent (Int32 command)
{
	if (_handler == 0)
		return rcOk;

	return beginMessage (command);
}

Int32 Object::endEvent ()
{
	if (_handler == 0)
		return rcOk;

	_inputPacket->clear ();

	Packet* t = _outputPacket;
	_outputPacket = _inputPacket;
	_inputPacket = t;

	Int32 result = _handler (_handlerData, _command, _handle);

	t = _outputPacket;
	_outputPacket = _inputPacket;
	_inputPacket = t;

	return result;
}

Packet& Object::getOutputPacket () const
{
	return *_outputPacket;
}

Packet& Object::getInputPacket () const
{
	return *_inputPacket;
}

bool Object::isLocal () const
{
	return _class.isLocal ();
}
