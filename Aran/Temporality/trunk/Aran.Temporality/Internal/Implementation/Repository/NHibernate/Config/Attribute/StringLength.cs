using System;

namespace Aran.Temporality.Internal.Implementation.Repository.NHibernate.Config.Attribute
{
    [AttributeUsage(AttributeTargets.Property)]
    public class StringLength : System.Attribute
    {
        public int Length;
        public StringLength(int taggedStrLength)
        {
            Length = taggedStrLength;
        }
    }
}