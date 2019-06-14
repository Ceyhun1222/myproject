#include "../include/Class.h"

Class::Class (std::string name, Int32 priority, Method method, HModule hModule):
	_method (method),
	_name (name),
	_priority (priority),
	_hModule (hModule),
	_refCount (0)
{
}

void Class::free ()
{
	delete this;
}

void Class::setPriority (Int32 priority)
{
	_priority = priority;
}

Int32 Class::getPriority () const
{
	return _priority;
}

std::string Class::getName () const
{
	return _name;
}

void Class::addRef ()
{
	++_refCount;
}

void Class::removeRef ()
{
	--_refCount;
}

Int32 Class::getRef () const
{
	return _refCount;
}

void Class::setHandle (Handle handle)
{
	_handle = handle;
}

Handle Class::getHandle () const
{
	return _handle;
}

HModule Class::getHModule () const
{
	return _hModule;
}

Method Class::getMethod () const
{
	return _method;
}
