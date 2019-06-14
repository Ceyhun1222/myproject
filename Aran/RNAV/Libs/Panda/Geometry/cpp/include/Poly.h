#ifndef POLY_H
#define POLY_H

#include "Geometry.h"
#include <Panda/Common/cpp/include/List.h>

namespace Panda
{
	class MultiPoint;

	template <class T>
    class Poly:  public Geometry
    {
		public:
			Poly () throw ()
			{
			}

			virtual ~Poly () throw ()
			{
			}

			virtual void assign (const Geometry& geometry) throw (std::bad_cast)
			{
				const Poly <T>& src = dynamic_cast <const Poly <T>&> (geometry);
				_multiPointList.assign (src._multiPointList);
			}

			virtual void pack (Handle handle) const throw (Registry::Exception)
			{
				_multiPointList.pack (handle);
			}

			virtual void unpack (Handle handle) throw (Registry::Exception)
			{
				_multiPointList.unpack (handle);
			}

			void add (const T& multiPoint)
			{
				_multiPointList.add (multiPoint);
			}

			void insert (int index, const T& multiPoint)
			{
				_multiPointList.insert (index, multiPoint);
			}

			const T& get (int index) const
			{
				return _multiPointList.at (index);
			}

			void removeAt (int index)
			{
				_multiPointList.remove (index);
			}

			int count() const
			{
				return _multiPointList.size ();
			}

			void clear()
			{
				_multiPointList.clear ();
			}

		private:
			List <T> _multiPointList;
    };
}

#endif /*POLY_H*/
