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

namespace ResearchOfFunction
{
    public class SCanvas : Canvas
    {
        Single sng;
        
        int N, HNum, VNum;
        double Left, Right, Step, DiffX, rg, wd, BegX, BegY, VDiff, VZoom, VFirst, Vrn, wdy, Eps;
        Grid tNote = new Grid();
        Grid tLeg = new Grid();

        public SCanvas(Single sng, double lft, double rht, double ep, double rg, double b)
        {
            this.sng = sng;

            BegY = 30;
            Func.b = b;
            Left = lft;
            Right = rht;
            N = 100;
            Step = (Right - Left) / N;
            this.rg = rg;
            HNum = (int)((Right - Left) / rg);
            VDiff = 0.1;
            VNum = 10;
            Eps = ep;
            /*
            mn = 1000000000;
            mx = -1000000000;
            for (double j = Left; j <= Right; j += Step)
            {
                double fn = CalcFunc(j);
                if (mx < fn)
                    mx = fn;
                if (mn > fn)
                    mn = fn;
            }
            */
            DrawNote();
            DrawLeg();
            MouseLeftButtonDown += MouseClick;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            BegX = 30;// ActualWidth/(1 + HNum)/2;
            double prtX = (ActualWidth - BegX) / ((HNum + 1) * 2 - 1);
            wd = prtX * ((HNum + 1) * 2 - 2);
            DiffX = wd / N;
            double prtY = (ActualHeight - BegY) / ((VNum + 1) * 2 - 1);
            wdy = prtY * ((VNum + 1) * 2 - 2);
            VZoom = (VDiff * VNum) / wdy;
            Vrn = wdy / VNum;
            /*
            Line ln1 = new Line();
            ln1.X1 = 0;
            ln1.X2 = ActualWidth;
            ln1.Y1 = ActualHeight - 10;
            ln1.Y2 = ln1.Y1;
            ln1.Stroke = Brushes.Black;
            Children.Add(ln1);

            Line ln2 = new Line();
            ln2.X1 = 10;
            ln2.X2 = ln2.X1;
            ln2.Y1 = 0;
            ln2.Y2 = ActualHeight;
            ln2.Stroke = Brushes.Black;
            Children.Add(ln2);
            */
            //   drawingContext.DrawRectangle(Brushes.Gray, new Pen(Brushes.Black, 2),new Rect (10, 10, ActualWidth-10, ActualHeight-10));
            drawingContext.DrawLine(new Pen(Brushes.Black, 1), new Point(BegX, ActualHeight - BegY), new Point(BegX + wd, ActualHeight - BegY));
            drawingContext.DrawLine(new Pen(Brushes.Black, 1), new Point(BegX, ActualHeight - BegY - wdy), new Point(BegX, ActualHeight - BegY));
            for (double k = Left; k <= Right; k += rg)
            {
                double X = BegX + (k - Left) / (Right - Left) * wd;
                drawingContext.DrawLine(new Pen(Brushes.Black, 1), new Point(X, ActualHeight - BegY - 5), new Point(X, ActualHeight - BegY + 5));
            }
            for (int l = 0; l <= VNum; l++)
                drawingContext.DrawLine(new Pen(Brushes.Black, 1), new Point(BegX - 5, ActualHeight - BegY - l * Vrn), new Point(BegX + 5, ActualHeight - BegY - l * Vrn));
            for (int i = 0; i < N; i++)
                drawingContext.DrawLine(new Pen(Brushes.Red, 1), new Point(BegX + i * DiffX, CalcY(Left + i * Step)), new Point(BegX + (i + 1) * DiffX, CalcY(Left + (i + 1) * Step)));
            SetLeft(tNote, BegX - prtX);
            SetTop(tNote, ActualHeight - BegY + 10);
            tNote.Height = 20;
            tNote.Width = prtX * ((HNum + 1) * 2);
            SetLeft(tLeg, 0);
            SetTop(tLeg, 0);
            tLeg.Height = prtY * ((VNum + 1) * 2);
            tLeg.Width = BegX - 5;
        }

        public double CalcY(double Q)
        {
            return wdy - (Func.Calc(Q)) / VZoom;
        }


        private void DrawNote()
        {
            tNote.RowDefinitions.Add(new RowDefinition());

            for (int i = 0; i <= HNum; i++)
            {

                TextBlock txt = new TextBlock();
                txt.Text = (Left + i * rg).ToString();
                txt.TextAlignment = TextAlignment.Center;
                txt.VerticalAlignment = VerticalAlignment.Center;
                txt.Foreground = System.Windows.Media.Brushes.Black;
                tNote.ColumnDefinitions.Add(new ColumnDefinition());
                Grid.SetColumn(txt, i);
                Grid.SetRow(txt, 0);
                tNote.Children.Add(txt);
            }
            Children.Add(tNote);
        }

        private void DrawLeg()
        {
            tLeg.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i <= VNum; i++)
            {

                TextBlock txt = new TextBlock();
                txt.Text = ((VNum - i) * VDiff).ToString();
                txt.TextAlignment = TextAlignment.Left;
                txt.VerticalAlignment = VerticalAlignment.Center;
                txt.Foreground = System.Windows.Media.Brushes.Black;
                tLeg.RowDefinitions.Add(new RowDefinition());
                Grid.SetColumn(txt, 0);
                Grid.SetRow(txt, i);
                tLeg.Children.Add(txt);
            }
            Children.Add(tLeg);
        }
        private void MouseClick(object sender, MouseButtonEventArgs e)
        {
                Point p = e.GetPosition(this);
                if (p.X >= BegX && p.X < BegX + wd)
                { 
                    double cn = (p.X -  BegX)/ wd * (Right - Left) + Left;
                    MethodPPoisk mp = new MethodPPoisk(cn - (Right - Left) / 50, cn + (Right - Left) / 50, Eps, !sng.mx);
                    sng.Lst.Clear();
                    while (!mp.Next())
                        sng.Lst.Add(new StepInfo (mp.N, mp.X0, mp.H0));
                    if (Math.Abs(mp.H0) >= 0.0000001)
                        sng.Tb3.Text = mp.H0.ToString();
                    else
                        sng.Tb3.Text = "0";
                    sng.Tb2.Text = mp.X0.ToString();
                    sng.Tb1.Text = mp.N.ToString();
                    sng.Table.ItemsSource = null;
                    sng.Table.Items.Clear();
                    sng.Table.ItemsSource = sng.Lst;
            }
        }


    }
}
