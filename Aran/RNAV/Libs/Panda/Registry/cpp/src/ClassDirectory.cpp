#include <cstdlib>
#include "../include/ClassDirectory.h"
#include "../include/Class.h"

ClassDirectory::ClassDirectory ()
{
}

Int32 ClassDirectory::addClass (Class* cl)
{
	Int32 id = generateId ();
	cl->setHandle (id);
	_idClassMap.insert (std::pair <Int32, Class*> (id, cl));
	_nameClassMultiMap.insert (std::pair <std::string, Class*> (cl->getName (), cl));
	return id;
}

Int32 ClassDirectory::generateId ()
{
	Int32 id;
	do
	{
		id = rand ();
		if (id == 0)
			continue;
	}
	while (_idClassMap.find (id) != _idClassMap.end ());
	
	return id;
}

Class* ClassDirectory::findClass (Int32 id)
{
	Class* cl = 0;
	std::map <Int32, Class*>::iterator i = _idClassMap.find (id);
	if (i != _idClassMap.end ())
	{
		cl = i->second;
	}
	return cl;
}

Class* ClassDirectory::findClass (std::string name)
{
	std::multimap <std::string, Class*>::iterator i = _nameClassMultiMap.find (name);
	
	Class* cl = 0;
	// Here will be released some choosing facility
	// Now we just return first occurence
	if (i != _nameClassMultiMap.end ())
		cl = i->second;
	
	return cl;
}

Class* ClassDirectory::findClass (std::string name, Method method)
{
	std::multimap <std::string, Class*>::iterator i = _nameClassMultiMap.find (name);
	
	// Here will be released some choosing facility
	// Now we just return first occurence
	for (; i != _nameClassMultiMap.end (); ++i)
	{
		Class* cl = i->second;
		if (cl->getMethod () == method)
			return cl;
	}

	return 0;
}


void ClassDirectory::removeClass (Class* cl)
{
	_idClassMap.erase (cl->getHandle ());

	std::multimap <std::string, Class*>::iterator i = _nameClassMultiMap.find (cl->getName ());
	for (; i != _nameClassMultiMap.end (); ++i)
	{
		if (i->second == cl)
		{
			_nameClassMultiMap.erase (i);
			return;
		}
	}
}
