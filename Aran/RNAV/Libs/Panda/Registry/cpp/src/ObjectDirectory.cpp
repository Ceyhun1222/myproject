#include <cstdlib>
#include "../include/ObjectDirectory.h"

ObjectDirectory::ObjectDirectory ()
{
}

Int32 ObjectDirectory::addObject (Object* object)
{
	Int32 id = generateId ();
	_idObjectMap.insert (std::pair <Int32, Object*> (id, object));
	return id;
}

Int32 ObjectDirectory::generateId ()
{
	Int32 id;

	do
	{
		id = rand ();
	}
	while ((!id) || (_idObjectMap.find (id) != _idObjectMap.end ()));

	return id;
}

Object* ObjectDirectory::findObject (Int32 id)
{
	if (id && id == _idCache)
		return _objectCache;

	Object* object = 0;
	std::map <Int32, Object*>::iterator i = _idObjectMap.find (id);
	if (i != _idObjectMap.end ())
	{
		_objectCache = object = i->second;
		_idCache = id;
	}

	return object;
}

void ObjectDirectory::removeObject (Int32 id)
{
	_idObjectMap.erase (id);
	_idCache = 0;
}
