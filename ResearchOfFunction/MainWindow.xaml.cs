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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ResearchOfFunction
{
    /// <summary>
    /// Interaction logic for для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Operation with a single optimisation
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SSingle single = new SSingle();
            single.Show();
        }

        /// <summary>
        /// Operation with a multi optimisation
        /// </summary>
        private void Multi_Click(object sender, RoutedEventArgs e)
        {
            Multi ml = new Multi();
            ml.Show();
        }
    }
}
