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

namespace TCC_Wpf.Wizard
{
    /// <summary>
    /// Interaction logic for Concluido.xaml
    /// </summary>
    public partial class Concluido : Page
    {
        public Concluido()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            ProService.Manager.AbrirFinaleira(ProService.Relatorios.resultados[2].modelo);
            Environment.Exit(0);
        }
    }
}
