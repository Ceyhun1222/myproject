#ifndef CLASS_H_
#define CLASS_H_

#include <string>
#include "Contract.h"

class Packet;

class Class
{
	public:
		Class (std::string name, Int32 priority, Method method = 0, HModule hModule = 0);
		virtual ~Class () { }
		
		void free ();
		virtual void setHandle (Handle handle);
		virtual Handle getHandle () const;
		virtual HModule getHModule () const;
		
		virtual Handle getInstance (Handle serviceObject) = 0;
		virtual void freeInstance (Handle privateData, Handle serviceObject) = 0;
		virtual Packet* createPacket () = 0;
		virtual Int32 method (Handle privateData, Int32 command, Handle handle) = 0;

		virtual void setPriority (Int32 priority);
		virtual Int32 getPriority () const;
		virtual std::string getName () const;
		virtual Method getMethod () const;
		
		virtual void addRef ();
		virtual void removeRef ();
		virtual Int32 getRef () const;

		virtual bool isLocal () const = 0;
	
	protected:
		Method _method;
		std::string _name;
		Int32 _priority;
		Int32 _refCount;
		Handle _handle;
		HModule _hModule;
};

#endif /*CLASS_H_*/
