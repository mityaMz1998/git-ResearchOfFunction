using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchOfFunction
{
    public class StepInfo
    {
        public int Number { get; set; }
        public double Ksi { get; set; }
        public double DEc { get; set; }

        public StepInfo(int Number, double Ksi, double DEc)
        {
            this.Number = Number;
            this.Ksi = Ksi;
            if (Math.Abs(DEc) < 0.00000000000001)
                this.DEc = 0;
            else
                this.DEc = DEc;
        }
    }

    public class StepMInfo
    {
        public int Number { get; set; }
        public double C { get; set; }
        public double L { get; set; }
        public double I { get; set; }

        public StepMInfo(int Number, double C, double L, double I)
        {
            this.Number = Number;
            this.C = C;
            this.L = L;
            if (Math.Abs(I) < 0.00000000000001)
                this.I = 0;
            else
                this.I = I;
        }
    }

}
