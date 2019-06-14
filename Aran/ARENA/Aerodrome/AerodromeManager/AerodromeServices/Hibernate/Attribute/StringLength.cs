using System;

namespace AerodromeServices.Hibernate.Attribute
{
    [AttributeUsage(AttributeTargets.Property)]
    public class StringLength : System.Attribute
    {
        public readonly int Length;
        public StringLength(int taggedStrLength)
        {
            Length = taggedStrLength;
        }
    }
}