using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchOfFunction
{
    /// <summary>
    /// Class for calculating a multidimensional function
    /// </summary>
    class MultiFunc
    {
        public static double Ow = 1; // AC frequency
        public static double R = 1; // Resistance
        public static double E = 1; // EMF

        public static double Calc(double L, double C)
        {
            return E / Math.Sqrt(Math.Pow(R,2) + Math.Pow(Ow * L - 1 / Ow / C,2));
        }

        public static double CalcL(double I, double C)
        {
            return 1 / Math.Pow(Ow,2)/C + Math.Sqrt(Math.Pow(E/I,2) - Math.Pow(R,2))/Ow;
        }
    }
}
