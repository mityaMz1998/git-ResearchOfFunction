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

namespace ResearchOfFunction
{
    /// <summary>
    /// Логика взаимодействия для Single.xaml
    /// </summary>
    public partial class Single : Window
    {
        public double A, B, T, Step, Vb;
        public bool mx = true;
        SCanvas Can;
        public List<StepInfo> Lst = new List<StepInfo>();
        public Single()
        {

            InitializeComponent();

 //           Can = new SCanvas();
 //           Grd.Children.Add(Can);

        }

        private void vl_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!(double.TryParse(vl.Text, out A)))
                MessageBox.Show("Ошибка ввода");
        }

        private void vr_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!(double.TryParse(vr.Text, out B)))
                MessageBox.Show("Ошибка ввода");
            else if (B <= A)
                MessageBox.Show("Неверное значение: правая граница должна быть больше левой!");
        }

        private void oc_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!(double.TryParse(oc.Text, out Step)))
                MessageBox.Show("Ошибка ввода");
            else if ((B - A) / 20 > Step)
                MessageBox.Show("Слишком маленький шаг!");
            else if ((B - A) / 5 < Step)
                MessageBox.Show("Слишком большой шаг!");
        }

        private void th_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!(double.TryParse(th.Text, out T)))
                MessageBox.Show("Ошибка ввода");
            else if (T > 0.01)
                MessageBox.Show("Слишком большое значение!");
        }

        private void vb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!(double.TryParse(vb.Text, out Vb)))
                MessageBox.Show("Ошибка ввода");
        }

        private void RadioButton_Click1(object sender, RoutedEventArgs e)
        {
            mx = true;
        }
        private void RadioButton_Click2(object sender, RoutedEventArgs e)
        {
            mx = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Grd.Children.Clear();
            Can = new SCanvas(this, A, B, T, Step, Vb);
            Grd.Children.Add(Can);
            Table.ItemsSource = null;
            Table.Items.Clear();
            Lst.Clear();
        }
    }
}