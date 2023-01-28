using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Drawing;
using System.Globalization;

namespace ResearchOfFunction
{
    public class SSCanvas : Canvas
    {

        public SSingle sng;

        public string[] dimNames = new string[2];
        public double[,] dim = new double[2, 5];
        public bool drawP = false;

        double N, NF;
        double Beg;

        public SSCanvas(SSingle sng)
        {
            this.sng = sng;
            MouseDown += MouseClickN;
        }

        public void init()
        {
            N = 100;

            dim[0, 2] = (dim[0, 1] - dim[0, 0]) / N;
        }


        protected override void OnRender(DrawingContext drawingContext)
        {
            double wd = ActualWidth;
            if (wd > ActualHeight)
                wd = ActualHeight;
            Beg = 30;
            wd -= 50;

            for (int i = 0; i < 2; i++)
                dim[i, 3] = wd / (dim[i, 1] - dim[i, 0]);

            drawingContext.DrawRectangle(Brushes.Transparent, null, new Rect(new Point(0, 0), new Point(ActualWidth, ActualHeight)));

            Pen pen1 = new Pen(Brushes.Black, 1);
            double BegY = ActualHeight - Beg;
            drawingContext.DrawLine(pen1, new Point(Beg, BegY), new Point(Beg + wd, BegY));
            drawingContext.DrawLine(pen1, new Point(Beg, BegY - wd), new Point(Beg, BegY));

            double nn;
            nn = dim[0, 0];
            for (double l = Beg; l <= Beg + wd; l += dim[0, 4] * dim[0, 3])
            {
                drawingContext.DrawLine(pen1, new Point(l, BegY - 5), new Point(l, BegY + 5));
                drawingContext.DrawText(getFormattedText(strFormat(nn)), new Point(l - 10, BegY + 5));
                nn += dim[0, 4];
            }
            nn = dim[1, 0];
            for (double l = BegY; l >= BegY - wd; l -= dim[1, 4] * dim[1, 3])
            {
                drawingContext.DrawLine(pen1, new Point(Beg - 5, l), new Point(Beg + 5, l));
                drawingContext.DrawText(getFormattedText(strFormat(nn)), new Point(Beg - 20, l - 5));
                nn += dim[1, 4];
            }

            drawingContext.DrawText(getFormattedText(dimNames[0]), new Point(Beg + wd + 5, BegY - 10));
            drawingContext.DrawText(getFormattedText(dimNames[1]), new Point(Beg + 5, BegY - wd - 10));


            for (int i = 0; i < N; i++)
                drawingContext.DrawLine(new Pen(Brushes.Blue, 1),
                    new Point(Beg + (i * dim[0, 2]) * dim[0, 3], CalcY(dim[0, 0] + i * dim[0, 2])),
                    new Point(Beg + ((i + 1) * dim[0, 2]) * dim[0, 3], CalcY(dim[0, 0] + (i + 1) * dim[0, 2])));
            if (drawP)
                drawPoisk(drawingContext);
        }

        public double CalcY(double Q)
        {
            return ActualHeight - Beg - SingleFunc.Calc(Q) * dim[1, 3];
        }


        void drawPoisk(DrawingContext drawingContext)
        {
            for (int i = 0; i < sng.Lst.Count - 1; i++)
            {
                StepInfo sto = sng.Lst[i];
                StepInfo stn = sng.Lst[i + 1];
                drawingContext.DrawLine(new Pen(Brushes.Red, 2), new Point(Beg + (sto.Ksi-dim[0,0]) * dim[0, 3], CalcY(sto.Ksi)),
                    new Point(Beg + (stn.Ksi - dim[0, 0]) * dim[0, 3], CalcY(stn.Ksi)));
            }
        }

        string strFormat(double nn)
        {
            string str = nn.ToString();
            if (str.Length > 4)
                str = str.Substring(0, 4);
            return str;
        }

        FormattedText getFormattedText(string str)
        {
            return new FormattedText(str, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 8, Brushes.Black);
        }


        public void MouseClickN(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(this);
            if (p.X > Beg)
            {
                double nX = dim[0, 0] + (p.X-Beg) / dim[0, 3];
                sng.setStartPoint(nX);
                drawP = true;
                InvalidateVisual();
            }
        }
    }
}
