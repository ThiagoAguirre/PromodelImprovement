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
using ProService;

namespace TCC_Wpf.Wizard
{
    /// <summary>
    /// Interaction logic for PrimeiraSimul.xaml
    /// </summary>
    public partial class SimulacaoPreliminar : Page, ProService.IAlgoritimo
    {
        public SimulacaoPreliminar()
        {
            InitializeComponent();
        }

        void IAlgoritimo.Acao()
        {
        }

        void IAlgoritimo.PassoAtual(string passo)
        {
            txtMensagem.Text = passo;
        }

        void IAlgoritimo.Progresso(double progresso)
        {
        }

        void IAlgoritimo.ProximaEtapa()
        {
            lblAguarde.Content = "Etapa concluída";
            pgbProgresso.IsIndeterminate = false;
            pgbProgresso.Value = 100;
        }

        void IAlgoritimo.TempoSimulacao(double tempo)
        {
            txtMensagem.Text = "Tempo de simulção: "+tempo;
        }
    }
}
