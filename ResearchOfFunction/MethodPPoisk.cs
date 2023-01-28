using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchOfFunction
{
    class MethodPPoisk
    {
        public double X0, H0;
        public int N = 0;
        double X1, Al, Bl, Eps, Delta;
        bool findMin;

        public MethodPPoisk(double Al, double Bl, double Eps, bool findMin)
        {
            this.Al = Al;
            this.Bl = Bl;
            this.Eps = Eps;
            this.findMin = findMin;
            this.Delta = (Bl - Al) / 4;
            this.X0 = Al;
        }

        public bool Next()
        {
            N = N + 1;
            double F0 = Func.Calc(X0);
            H0 = F0;
            X1 = X0 + Delta;
            double F1 = Func.Calc(X1);

            if (findMin && F1 < F0 || !findMin && F1 > F0)
            {
                if (X1 >= Al && X1 <= Bl)
                {
                    X0 = X1;
                    H0 = F1;
                    return false;
                }
            }
            if (Math.Abs(Delta) < Eps)
                return true;
            if (!(X1 >= Al && X1 <= Bl))
                Delta = Delta / 4;
            else
            {
                X0 = X1;
                H0 = F1;
                Delta = -Delta / 4;
            }
            return false;
        }
    }
}
