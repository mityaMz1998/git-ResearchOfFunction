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
    /// Логика взаимодействия для Multidimensional.xaml
    /// </summary>
    public partial class Multidimensional : Window
    {
        MCanvas Can;
        public Multidimensional()
        {
            InitializeComponent();

            Can = new MCanvas(this);
            Graf.Children.Add(Can);

        }
    }
}
