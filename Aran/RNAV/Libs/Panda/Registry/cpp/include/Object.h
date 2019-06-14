#ifndef OBJECT_H_
#define OBJECT_H_

#include "Contract.h"
class Class;
class Packet;

class Object
{
	public:
		Object (Class& cl);
		void free ();
		
		Int32 beginMessage (Int32 command);
		Int32 endMessage ();

		Int32 beginEvent (Int32 command);
		Int32 endEvent ();

		Packet& getOutputPacket () const;
		Packet& getInputPacket () const;

		void setHandle (Handle handle);
		Handle getHandle () const;

		void setPrivateData (Handle privateData);
		Handle getPrivateData () const;

		void setHandler (Int32 handlerData, Method method);
		void unsetHandler ();

		bool isLocal () const;

	protected:
		~Object ();

	private:
		Class& _class;
		Handle _privateData;
		Int32 _command;

		Packet* _outputPacket;
		Packet* _inputPacket;

		Handle _handle;

		Int32 _handlerData;
		Method _handler;
};

#endif /*OBJECT_H_*/
