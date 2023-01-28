using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchOfFunction
{
    class SinglePoisk
    {
        public double X0, H0;
        public int N = 0;
        double X1, Al, Bl, Eps, Delta;
        bool findMin;

        public SinglePoisk(double Eps, bool findMin)
        {
           this.Eps = Eps;
           this.findMin = findMin;
        }

        public bool Next()
        {
            N = N + 1;
            double F0 = SingleFunc.Calc(X0);
            H0 = F0;
            X1 = X0 + Delta;
            double F1 = SingleFunc.Calc(X1);

            if (findMin && F1 < F0 || !findMin && F1 > F0)
            {
                if (X1 >= Al && X1 <= Bl)
                {
                    X0 = X1;
                    H0 = F1;
                    return true;
                }
            }
            if (Math.Abs(Delta) < Eps)
                return false;
            if (!(X1 >= Al && X1 <= Bl))
                Delta = Delta / 4;
            else
            {
                X0 = X1;
                H0 = F1;
                Delta = -Delta / 4;
            }
            return true;
        }

        public bool SetFirst(double p, double b, double e )
        {
            double sz = (e-b)/20;
            this.Al = p-sz;
            if (Al < b)
                Al = b;
            this.Bl = Al + 2*sz;
            this.Delta = (Bl - Al) / 4;
            this.X0 = Al;
            H0 = SingleFunc.Calc(X0);
            return true;
        }

    }
}
