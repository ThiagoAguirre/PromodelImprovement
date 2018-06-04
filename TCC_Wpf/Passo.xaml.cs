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

namespace TCC_Wpf
{
    /// <summary>
    /// Interaction logic for Passo.xaml
    /// </summary>
    public partial class Passo : UserControl
    {
        public Passo(string texto)
        {
            InitializeComponent();
            txtTexto.Text = texto;
        }

        public void Ok(bool passo)
        {
            if (passo)
            {
                imgCheck.Visibility = Visibility.Visible;
                imgRight.Visibility = Visibility.Hidden;

                txtTexto.Foreground = new SolidColorBrush(Color.FromRgb(160, 160, 160));
            }
            else
            {
                imgCheck.Visibility = Visibility.Hidden;
                imgRight.Visibility = Visibility.Visible;

                txtTexto.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            }
        }
    }
}
