#ifndef LIST_H
#define LIST_H

#include <vector>
#include <Panda/Registry/cpp/include/Contract.h>

namespace Panda
{
	template <class T> class List
	{
		public:
			~List ()
			{
				clear ();
			}

			void remove (int index)
			{
				_vector.erase (_vector.begin() + index);
			}
	        
			int size () const
			{
				return _vector.size ();
			}
	        
			void clear ()
			{
				int count = List <T>::size ();
				for (int i = 0; i < count; ++i)
				{
					delete &(List <T>::at (i));
				}

				_vector.clear ();
			}

			void add (const T& item)
			{
				_vector.push_back (dynamic_cast <T*> (item.clone ()));
			}

			void insert (int index, const T& item)
			{
				_vector.insert (_vector.begin () + index, dynamic_cast <T*> (item.clone ()));
			}
	        
			const T& at (int index) const
			{
				return *(_vector [index]);
			}

			void assign (const List <T>& src)
			{
				clear ();
				for (int i=0; i < src.size (); ++i)
				{
					add (src.at (i));
				}
			}

			List <T>* clone () const
			{
				List <T>* list = new List <T>;
				list->assign (this);
				return list;
			}

			void pack (Handle handle) const
            {
                int sz = List <T>::size ();
				Registry::putInt32 (handle, sz);
                
                for (int i=0; i < sz; ++i)
                {
                    List <T>::at (i).pack (handle);
                }
            }
            
            void unpack (Handle handle)
            {
                List <T>::clear ();
                int sz;
				Registry::getInt32 (handle, sz);

                for (int i=0; i<sz; ++i)
                {
                    T object;
                    object.unpack (handle);
                    List <T>::add (object);                    
                }
            }
	        
		private:
			std::vector <T*> _vector;
	};
}    

#endif
