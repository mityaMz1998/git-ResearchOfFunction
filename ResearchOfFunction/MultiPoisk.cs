using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchOfFunction
{
    class MultiPoisk
    {
        public int N = 0;
        public double C, L, F0, Eps;
        double C1, L1, F1;
        bool findMin;
        bool LCFlag;
        int sign;
        double[,] dim;

        public MultiPoisk(bool findMin, double Eps, double[,] dim)
        {
            this.findMin = findMin;
            this.Eps = Eps;
            this.dim = dim;
        }

        public bool Next()
        {
            N = N + 1;
            bool Fl = true;
            while (Fl)
            {
                double F1 = CalcNext(1);

                if (findMin && F1 < F0 || !findMin && F1 > F0)
                {
                    if (C1 >= dim[0, 0] && C1 <= dim[0, 1] && L1 >= dim[1, 0] && L1 <= dim[1, 1])
                    {
                        C = C1;
                        L = L1;
                        F0 = F1;
                        return true;
                    }
                }
                if (!NextVar())
                    return false;

            }
            return false;
        }


        public bool SetFirst(double C, double L)
        {
            this.C = C;
            this.L = L;
            LCFlag = false;
            F0 = MultiFunc.Calc(L, C);

            if (NextVar())
                return true;
            return NextVar();
        }

        bool NextVar()
        {

            LCFlag = !LCFlag;
            sign = 1;
            double F1 = CalcNext(10);
            if (findMin && F1 < F0 || !findMin && F1 > F0)
                return true;
            sign = -1;
            F1 = CalcNext(10);
            if (findMin && F1 < F0 || !findMin && F1 > F0)
                return true;
            return false;
        }

        double CalcNext(int Step)
        {
            L1 = L;
            C1 = C;
            if (LCFlag)
                C1 += sign * Eps * Step;
            else
                L1 += sign * Eps * Step;
            return MultiFunc.Calc(L1, C1);
        }
    }
}
