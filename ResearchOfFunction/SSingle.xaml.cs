using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using Excell = Microsoft.Office.Interop.Excel;

namespace ResearchOfFunction
{
    /// <summary>
    /// Логика взаимодействия для SSingle.xaml
    /// </summary>
    public partial class SSingle : Window
    {
        SSCanvas Can;
        public double Eps;
        public bool mx = true;
        bool hasResult;
        public List<StepInfo> Lst = new List<StepInfo>();
        string[] namesRow = { "угла поворота", "относительной погрешности" };
        string[] namesCol = { "нижней границы", "верхней границы", "шага" };
        Excell._Worksheet worksheet = null;

        public SSingle()
        {
            Eps = 0.001;

            InitializeComponent();

            Can = new SSCanvas(this);
            KeyDown += SSingle_KeyDown;
        }


        public void setStartPoint(double beg)
        {
            SinglePoisk mp = new SinglePoisk(Eps, !mx);
            mp.SetFirst(beg, Can.dim[0, 0], Can.dim[0, 1]);
            Lst.Clear();
            Lst.Add(new StepInfo(mp.N, beg, SingleFunc.Calc(beg)));
            while (mp.Next())
                Lst.Add(new StepInfo(mp.N, mp.X0, mp.H0));
            TBL.ItemsSource = null;
            TBL.Items.Clear();
            TBL.ItemsSource = Lst;
            StepInfo sm = Lst[Lst.Count - 1];
            T1.Text = sm.Number.ToString();
            T2.Text = sm.Ksi.ToString().Substring(0, 6);
            if (sm.DEc < 0.0001)
                T3.Text = "0";
            else
                T3.Text = sm.DEc.ToString().Substring(0, 6);
        }

        void SSingle_KeyDown(object sender, KeyEventArgs e)
        {
            double B;
            if (double.TryParse(EE.Text, out B))
            {
                if (e.Key == Key.A)
                {
                    B *= 2;
                    EE.Text = B.ToString();
                    SingleFunc.b *= B;
                }
                if (e.Key == Key.S)
                {
                    B /= 2;
                    EE.Text = B.ToString();
                    SingleFunc.b /= B;
                }
            }
        }

        private void RadioButton_Click1(object sender, RoutedEventArgs e)
        {
            mx = true;
        }
        private void RadioButton_Click2(object sender, RoutedEventArgs e)
        {
            mx = false;
        }

        private void Draw_Click(object sender, RoutedEventArgs e)
        {
            Can = new SSCanvas(this);
            if (testSetAll())
            {
                Can.init();
                Graf.Children.Clear();
                Graf.Children.Add(Can);
                hasResult = true;
            }
            else
                hasResult = false;
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".text";
            dlg.Filter = "Text documents (.txt)|*.txt";
            if (dlg.ShowDialog() == true)
            {
                FileStream fl = new FileStream(dlg.FileName, FileMode.Open);
                StreamReader reader = new StreamReader(fl);
                Lst.Clear();
                while (!reader.EndOfStream)
                {
                    string[] words = reader.ReadLine().Split(';');
                    Lst.Add(new StepInfo(int.Parse(words[0]), double.Parse(words[1]), double.Parse(words[2])));
                }
                reader.Close();
                TBL.ItemsSource = null;
                TBL.Items.Clear();
                TBL.ItemsSource = Lst;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (hasResult)
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.DefaultExt = ".text";
                dlg.Filter = "Text documents (.txt)|*.txt";
                if (dlg.ShowDialog() == true)
                {
                    FileStream fl = new FileStream(dlg.FileName, FileMode.OpenOrCreate);
                    StreamWriter writer = new StreamWriter(fl);
                    foreach (StepInfo sm in Lst)
                        writer.WriteLine(string.Concat(sm.Number, ";", sm.Ksi, ";", sm.DEc));
                    writer.Close();
                }
            }
        }


        private void Print_Click(object sender, RoutedEventArgs e)
        {
            if (hasResult)
            {
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(Can, "Распечатываем элемент Canvas");
                }
            }
        }

        private void Excel_Click(object sender, RoutedEventArgs e)
        {
            if (hasResult)
            {
                Excell._Application app = new Excell.Application();
                Excell._Workbook workbook = app.Workbooks.Add(Type.Missing);
                worksheet = workbook.Sheets["Лист1"];
                worksheet = workbook.ActiveSheet;

                worksheet.Cells[1, 1] = "NN";
                worksheet.Cells[1, 2] = "Xi";
                worksheet.Cells[1, 3] = "Dec";
                int i = 2;
                foreach (StepInfo sm in Lst)
                {
                    worksheet.Cells[i, 1] = sm.Number;
                    worksheet.Cells[i, 2] = sm.Ksi;
                    worksheet.Cells[i++, 3] = sm.DEc;
                }

                app.Visible = true;

            }
        }

        private bool testSetAll()
        {
            if (!testBoxA(EE, ref SingleFunc.b))
                return false;
            if (!testSetDim(S0, E00, E01, E02, 0))
                return false;
            if (!testSetDim(S1, E10, E11, E12, 1))
                return false;
            return true;
        }

        private bool testSetDim(TextBox S, TextBox E0, TextBox E1, TextBox E2, int num)
        {
            if (!testSBox(S))
                return false;

            Can.dimNames[num] = S.Text;

            if (!testBox(E0, ref Can.dim[num, 0]))
                return false;

            if (!testBox(E1, ref Can.dim[num, 1]))
                return false;

            if (Can.dim[num, 1] <= Can.dim[num, 0])
            {
                MessageBox.Show(string.Concat("Верхнее значение ", namesRow[num], " должно быть больше нижнего"));
                return false;
            }

            if (!testBox(E2, ref Can.dim[num, 4]))
                return false;

            double diff = Can.dim[num, 1] - Can.dim[num, 0];
            double B = Can.dim[num, 4];
            if (diff / 10 > B || diff / 3 < B)
            {
                MessageBox.Show(string.Concat("Выберите другое значение шага ", namesRow[num]));
                return false;
            }

            return true;
        }

        private bool testBox(TextBox bx, ref double val)
        {
            double B;
            if (!(double.TryParse(bx.Text, out B)))
            {
                MessageBox.Show(string.Concat("Неверное значение ", namesCol[Grid.GetColumn(bx) - 2], " ", namesRow[Grid.GetRow(bx) - 1]));
                return false;
            }
            val = B;
            return true;
        }

        private bool testBoxA(TextBox bx, ref double val)
        {
            double B;
            if (!(double.TryParse(bx.Text, out B)))
            {
                MessageBox.Show("Неверное значение B");
                return false;
            }
            val = B;
            return true;
        }


        private bool testSBox(TextBox bx)
        {
            if (bx.Text.Length > 3)
            {
                MessageBox.Show(string.Concat("Название ", namesRow[Grid.GetRow(bx) - 1], " не может быть больше 3-х символов"));
                return false;
            }
            return true;
        }

        private void EN_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox bx = (TextBox)sender;
            if (bx.Text.Length > 3)
            {
                MessageBox.Show(string.Concat("Название ", namesRow[Grid.GetRow(bx) - 1], " не может быть больше 3-х символов"));
            }
        }

        private void SS_LostFocus(object sender, RoutedEventArgs e)
        {
            double B;
            TextBox bx = (TextBox)sender;
            if (!(double.TryParse(bx.Text, out B)))
            {
                MessageBox.Show(string.Concat("Неверное значение ", namesCol[Grid.GetColumn(bx) - 2], " ", namesRow[Grid.GetRow(bx) - 1]));
            }
        }

        private void S_LostFocus(object sender, RoutedEventArgs e)
        {
            double B;
            TextBox bx = (TextBox)sender;
            if (!(double.TryParse(bx.Text, out B)))
            {
                MessageBox.Show("Неверное значение B");
            }
        }

    }
}
