using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchOfFunction
{
    class MultiFunc
    {
        public static double Ow = 1;
        public static double R = 1;
        public static double E = 1;
        public static double Calc(double L, double C)
        {
            return E / Math.Sqrt(Math.Pow(R,2) + Math.Pow(Ow * L - 1 / Ow / C,2));
        }
        public static double CalcL(double I, double C)
        {
            return 1/Math.Pow(Ow,2)/C + Math.Sqrt(Math.Pow(E/I,2) - Math.Pow(R,2))/Ow;
        }
        // L = (Math.Pow(E/I,2) - Math.Pow(R,2) - 1 / Math.Pow(Ow*C,2)) / (Math.Pow(Ow,2) - 2/C);
    }
}
