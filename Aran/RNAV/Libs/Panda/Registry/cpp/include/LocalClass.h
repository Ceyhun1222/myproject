#ifndef LOCALCLASS_H_
#define LOCALCLASS_H_

#include "Class.h"
#include "Contract.h"
#include "Object.h"

class LocalClass: public Class
{
	public:
		LocalClass (std::string name, Int32 priority, Method method, HModule hModule);
		virtual ~LocalClass ();
		
		virtual Handle getInstance (Handle serviceObject);
		virtual void freeInstance (Handle privateData, Handle serviceObject);
		virtual Packet* createPacket ();
		
		virtual Int32 method (Handle privateData, Int32 command, Handle handle);
		virtual bool isLocal () const;
		
	private:
		Int32 attach ();
		Int32 detach ();
};


#endif /*LOCALCLASS_H_*/
