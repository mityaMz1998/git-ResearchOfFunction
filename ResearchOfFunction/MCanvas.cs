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
    /// <summary>
    /// Class for visual display of the graph of multidimensional optimization
    /// </summary>
    public class MCanvas : Canvas
    {
        public Multi mlt;

        public string[] dimNames = new string[3];
        public double[,] dim = new double[3, 5];
        public bool drawP = false;

        double N, NF;
        double Beg, sq;

        public MCanvas(Multi mlt)
        {
            this.mlt = mlt;
            MouseDown += MouseClickN;
        }

        public void init()
        {
            sq = 1 / Math.Sqrt(2);
            N = 100;
            NF = 18;

            for (int i = 0; i < 2; i++)
                dim[i, 2] = (dim[i, 1] - dim[i, 0]) / N;
            dim[2, 2] = (dim[2, 1] - dim[2, 0]) / NF;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            double mm = ActualWidth;
            if (mm > ActualHeight)
                mm = ActualHeight;
            double wd = mm / (1 + sq);
            Beg = wd * sq;
            wd -= 20;

            for (int i = 0; i < 3; i++ )
                dim[i, 3] = wd / (dim[i, 1] - dim[i, 0]);

            drawingContext.DrawRectangle(Brushes.Transparent, null, new Rect(new Point(0, ActualHeight - Beg), new Point(ActualWidth, ActualHeight)));

            Pen pen1 = new Pen(Brushes.Black, 1);
            double BegY = ActualHeight - Beg;
            drawingContext.DrawLine(pen1, new Point(Beg, BegY), new Point(Beg + wd, BegY));
            drawingContext.DrawLine(pen1, new Point(Beg, BegY - wd), new Point(Beg, BegY));
            drawingContext.DrawLine(pen1, new Point(Beg, BegY), new Point(Beg - wd*sq, BegY + wd*sq));

            double nn;
            nn = dim[0, 0];
            for (double l = Beg; l <= Beg + wd; l += dim[0, 4] * dim[0, 3])
            {
                drawingContext.DrawLine(pen1, new Point(l, BegY - 5), new Point(l, BegY + 5));
                drawingContext.DrawText(getFormattedText(strFormat(nn)), new Point(l - 10, BegY + 5));
                nn += dim[0, 4];
            }
            nn = dim[1, 0];
            double lx = Beg;
            for (double l = BegY; l <= BegY + wd*sq; l += dim[1, 4] * dim[1, 3]*sq)
            {
                drawingContext.DrawLine(pen1, new Point(lx - 3, l-3), new Point(lx + 3, l+3));
                drawingContext.DrawText(getFormattedText(strFormat(nn)), new Point(lx - 20, l - 5));
                nn += dim[1, 4];
                lx -= dim[1, 4] * dim[1, 3] * sq;
            }
            nn = dim[2, 0];
            for (double l = BegY; l >= BegY - wd; l -= dim[2, 4] * dim[2, 3])
            {
                drawingContext.DrawLine(pen1, new Point(Beg-5, l), new Point(Beg+5, l));
                drawingContext.DrawText(getFormattedText(strFormat(nn)), new Point(Beg - 20, l - 5));
                nn += dim[2, 4];
            }

            drawingContext.DrawText(getFormattedText(dimNames[0]), new Point(Beg + wd + 5, BegY - 10));
            drawingContext.DrawText(getFormattedText(dimNames[1]), new Point(Beg - wd * sq + 5, BegY + wd * sq + 5 ));
            drawingContext.DrawText(getFormattedText(dimNames[2]), new Point(Beg + 5, BegY - wd - 10));

            Pen pen2 = new Pen(Brushes.Blue, 1);
            pen2.DashStyle = DashStyles.Dash;
            for (double k = dim[2, 0]; k <= dim[2, 1] - 0.01; k += dim[2, 2])
                for (int i = 0; i < N; i++)
                {
                    double C1 = dim[0, 0] + dim[0, 2] * i;
                    double L11 = MultiFunc.CalcL(k, C1);
                    double C2 = dim[0, 0] + dim[0, 2] * (i + 1);
                    double L21 = MultiFunc.CalcL(k, C2);
                    if (!(L21 < dim[1, 0] || L21 > dim[1, 1] || L11 < dim[1, 0] || L11 > dim[1, 1]))
                        drawingContext.DrawLine(pen2, new Point(coordX(C1, L11), coordY(C1, L11, k)), 
                            new Point(coordX(C2, L21), coordY(C2, L21, k)));
                }

            if (drawP)
                drawPoisk(drawingContext);
        }

        void drawPoisk(DrawingContext drawingContext)
        {
            for (int i = 0; i < mlt.Lst.Count-1;i++)
            {
                StepMInfo sto = mlt.Lst[i];
                StepMInfo stn = mlt.Lst[i+1];
                drawingContext.DrawLine(new Pen(Brushes.Red, 1), new Point(coordX(sto.C, sto.L), coordY(sto.C, sto.L, sto.I)),
                    new Point(coordX(stn.C, stn.L), coordY(stn.C, stn.L, stn.I)));
            } 
        }

        double coordX(double C, double L)
        {
            return Beg + (C - dim[0, 0]) * dim[0, 3] - (L - dim[1, 0]) * dim[1, 3] * sq;
        }

        double coordY(double C, double L, double F)
        {
            return ActualHeight - Beg - MultiFunc.Calc(L, C) * dim[2, 3] + (L - dim[1, 0]) * dim[1, 3] * sq;
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
            if (p.Y > ActualHeight - Beg)
            {
                double Y = (p.Y - (ActualHeight - Beg)) / sq;
                double X = p.X - Beg + Y * sq;
                double nX = dim[0, 0] + X / dim[0, 3];
                double nY = dim[1, 0] + Y / dim[1, 3];
                if (nX >= dim[0, 0] && nX <= dim[0, 1] && nY >= dim[1, 0] && nY <= dim[1, 1])
                {
                    mlt.setStartPoint(nX, nY);
                    drawP = true;
                    InvalidateVisual();
                }
            }
        }
    }
}
