using Aerodrome.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Stasy.Context
{
    
    public class CollectionsDictionary<TValue> : Dictionary<Type, TValue> 
    {
        public TValue Get<T>()
        {
            return this[typeof(T)];
        }

        public void Add<T>(TValue value)
        {
            Add(typeof(T), value);
        }


        public bool TryGetValue<T>(out TValue value)
        {
            return TryGetValue(typeof(T), out value);
        }

        public bool ContainsKey<T>()
        {
            return ContainsKey(typeof(T));
        }
    }

    public class DictionaryByType
    {
        private readonly IDictionary<Type, object> dictionary = new Dictionary<Type, object>();
        /// <summary>
        /// Maps the specified type argument to the given value. If
        /// the type argument already has a value within the dictionary,
        /// ArgumentException is thrown.
        /// </summary>
        public void Add<T>(CompositeCollection<T> value) where T:class
        {
            dictionary.Add(typeof(T), value);
        }

        /// <summary>
        /// Maps the specified type argument to the given value. If
        /// the type argument already has a value within the dictionary, it
        /// is overwritten.
        /// </summary>
        public void Put<T>(T value)
        {
            dictionary[typeof(T)] = value;
        }

        /// <summary>
        /// Attempts to fetch a value from the dictionary, throwing a
        /// KeyNotFoundException if the specified type argument has no
        /// entry in the dictionary.
        /// </summary>
        public T Get<T>()
        {
            return (T)dictionary[typeof(T)];
        }

        /// <summary>
        /// Attempts to fetch a value from the dictionary, returning false and
        /// setting the output parameter to the default value for T if it
        /// fails, or returning true and setting the output parameter to the
        /// fetched value if it succeeds.
        /// </summary>
        public bool TryGet<T>(out T value)
        {
            object tmp;
            if (dictionary.TryGetValue(typeof(T), out tmp))
            {
                value = (T)tmp;
                return true;
            }
            value = default(T);
            return false;
        }
    }
}
