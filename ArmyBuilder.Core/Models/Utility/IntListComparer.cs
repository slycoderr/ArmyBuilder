using System.Collections.Generic;
using System.Linq;

namespace ArmyBuilder.Core.Utility
{
    public class IntListComparer : IEqualityComparer<List<int>>
    {
        public bool Equals(List<int> x, List<int> y)
        {
            if (x == y)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            if (x.Count != y.Count)
            {
                return false;
            }

            using (var xenum = x.GetEnumerator())
            {
                foreach (var yval in y)
                {
                    xenum.MoveNext();
                    if (yval != xenum.Current)
                        return false;
                }
            }
            return true;
        }

        // You also have to implement the GetHashCode which
        // must have the property that
        // if Equals(x, y) => GetHashCode(x) == GetHashCode(y)
        public int GetHashCode(List<int> x)
        {
            const int primeMultiplier = 17;
            return x.Aggregate(1, (current, xval) => current*primeMultiplier*xval);
        }
    }
}