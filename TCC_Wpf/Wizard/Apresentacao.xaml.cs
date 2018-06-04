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
    /// Interaction logic for Apresentacao.xaml
    /// </summary>
    public partial class Apresentacao : Page, ProService.IAlgoritimo
    {
        public Apresentacao()
        {
            InitializeComponent();
        }

        void IAlgoritimo.Acao()
        {
            throw new NotImplementedException();
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
