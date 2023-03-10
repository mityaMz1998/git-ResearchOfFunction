using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchOfFunction
{
    /// <summary>
    /// Class for calculating a singledimensional function
    /// </summary>
    public class SingleFunc
    {
        public static double b = 1; // coefficient
        public static double Calc(double Q)
        {
            return Math.Abs(b * Math.Sin(Q) * Math.Pow(Math.Cos(Q), 2) / (1 + b * Math.Pow(Math.Cos(Q), 2)));
        }
    }
}
