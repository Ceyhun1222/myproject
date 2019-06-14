#ifndef OBJECT_DIRECOTRY_H_
#define OBJECT_DIRECTORY_H_

#include <map>
#include "Contract.h"

class Object;

class ObjectDirectory
{
	public:
		ObjectDirectory ();

		Int32 addObject (Object* object);
		void removeObject (Int32 handle);
		Object* findObject (Int32 handle);

	private:
		std::map <Int32, Object*> _idObjectMap;
		Object* _objectCache;
		Int32 _idCache;

	private:
		Int32 generateId ();
};


#endif
