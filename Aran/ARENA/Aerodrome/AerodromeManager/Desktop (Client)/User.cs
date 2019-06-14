using System;

namespace AerodromeManager.AmdbService
{
    public partial class User : IComparable
    {
        public int CompareTo(object obj)
        {
            return CompareTo(obj as User);
        }

        public int CompareTo(User employee)
        {
            if (Equals(employee))
                return 0;
            int num = String.Compare(FirstName, employee.FirstName, StringComparison.Ordinal);
            if (num == 0)
                return String.Compare(LastName, employee.LastName, StringComparison.Ordinal);
            return num;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is User))
                return false;
            User employee = (User) obj;
            return employee.FirstName == FirstName && employee.LastName == LastName;
        }

        public override int GetHashCode()
        {
            return (FirstName + LastName).GetHashCode();
        }

        public override string ToString()
        {
            return FirstName + " " + LastName;
        }
    }
}