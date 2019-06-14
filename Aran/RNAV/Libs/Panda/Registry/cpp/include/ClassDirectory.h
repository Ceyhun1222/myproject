#ifndef CLASSDIRECTORY_H_
#define CLASSDIRECTORY_H_

#include <map>
#include <string>
#include "../include/Contract.h"

class Class;

class ClassDirectory
{
	public:
		ClassDirectory ();
		Int32 addClass (Class* cl);
		Class* findClass (std::string className);
		Class* findClass (std::string className, Method method);
		Class* findClass (Int32 id);
		void removeClass (Class* cl);
		
	private:
		Int32 generateId ();

	private:
		std::multimap <std::string, Class*> _nameClassMultiMap;
		std::map <Int32, Class*> _idClassMap;
};

#endif /*CLASSDIRECTORY_H_*/
