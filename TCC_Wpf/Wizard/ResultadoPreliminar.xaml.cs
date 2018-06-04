using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ProService;

namespace TCC_Wpf.Wizard
{
    /// <summary>
    /// Interaction logic for ResultadoPreliminar.xaml
    /// </summary>
    public partial class ResultadoPreliminar : Page, ProService.IAlgoritimo
    {
        public ResultadoPreliminar()
        {
            InitializeComponent();            
        }

        public void Resultado(ObservableCollection<MyData> t, string titulo)
        {
            BarChart1.Items = t;
            lblTitulo.Content = titulo;
        }

        void IAlgoritimo.Acao()
        {
         
        }

        void IAlgoritimo.PassoAtual(string passo)
        {
         
        }

        void IAlgoritimo.Progresso(double progresso)
        {
            
        }

        void IAlgoritimo.ProximaEtapa()
        {
            
        }

        void IAlgoritimo.TempoSimulacao(double tempo)
        {
            
        }
    }
}
