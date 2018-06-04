using MahApps.Metro.Controls;
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
using System.Windows.Threading;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace TCC_Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow, ProService.IAlgoritimo
    {
        Object[] wizard = new object[10];
        int index = 0;

        public MainWindow()
        {
            InitializeComponent();

            wizard[0] = new Wizard.Apresentacao();
            wizard[1] = new Wizard.Arquivo();
            wizard[2] = new Wizard.SimulacaoPreliminar();
            wizard[3] = new Wizard.ResultadoPreliminar();
            wizard[4] = new Wizard.Metodo();
            wizard[5] = wizard[3];
            wizard[6] = new Wizard.Concluido();

            grdFrame.NavigationService.Navigate(wizard[index]);
            grdFrame.VerticalContentAlignment = VerticalAlignment.Top;

            grdPassos.Children.Add(new Passo("Bem vindo"));
            grdPassos.Children.Add(new Passo("Arquivos"));
            grdPassos.Children.Add(new Passo("Simlação preliminar"));
            grdPassos.Children.Add(new Passo("Resultados preliminar"));
            grdPassos.Children.Add(new Passo("Otimizações"));
            grdPassos.Children.Add(new Passo("Resultados"));
            grdPassos.Children.Add(new Passo("Finalizado"));

            ((Passo)grdPassos.Children[index]).Ok(true);
            grdLog.Children.Insert(0, new Log("Otimizador iniciado."));

            //Resultados();
            ProService.Manager.Inicializar(this);    
        }

        private void Resultados()
        {
            this.DataContext = this;

            Data = new ObservableCollection<MyData>();

            for (int i = 0; i < ProService.Relatorios.resultados.Count; i++)
            {
                Data.Add(new MyData() { Year = i, Value = Math.Round(ProService.Relatorios.resultados[i].scoreSaidas,1), WorkType = WorkTypes.Saídas });
                Data.Add(new MyData() { Year = i, Value = Math.Round(ProService.Relatorios.resultados[i].scoreTotal,1), WorkType = WorkTypes.Total });
                Data.Add(new MyData() { Year = i, Value = Math.Round(ProService.Relatorios.resultados[i].scoreTempo,1), WorkType = WorkTypes.Tempo });
            }

            if (ProService.Relatorios.resultados.Count == 1)
                ((Wizard.ResultadoPreliminar)wizard[index]).Resultado(Data, "Resuldados preliminares");
            else
                ((Wizard.ResultadoPreliminar)wizard[index]).Resultado(Data, "Resuldados");
        }

        private void btnProximo_Click(object sender, RoutedEventArgs e)
        {
            index++;
            grdFrame.NavigationService.Navigate(wizard[index]);
            ((Passo)grdPassos.Children[index]).Ok(true);

            btnAnterior.IsEnabled = true;

            if (index == 2)
            {
                btnAnterior.IsEnabled = false;
                btnProximo.IsEnabled = false;
                ((ProService.IAlgoritimo)wizard[index - 1]).Acao();
            }
            if (index == 3)
            {
                ProService.Relatorios.resultados.Add(ProService.Manager.bancoModelos[0]);
                Resultados();
            }
            if(index == 4)
            {
                ProService.Manager.thCriarDescendente();
            }
            if(index == 5)
            {
                Resultados();
            }
            if(index==6)
            {
                btnProximo.IsEnabled = false;
            }
        }

        private void btnAnterior_Click(object sender, RoutedEventArgs e)
        {
            if (index == 3)
            {
                ((Passo)grdPassos.Children[index]).Ok(false);
                index--;
            }
            ((Passo)grdPassos.Children[index]).Ok(false);
            index--;
            grdFrame.NavigationService.Navigate(wizard[index]);

            if (index == 0)
                btnAnterior.IsEnabled = false;
        }

        void IAlgoritimo.PassoAtual(string passo)
        {
            ExecutarCurrentThread(() =>
            {
                grdLog.Children.Insert(0, new Log(passo));
                ((ProService.IAlgoritimo)wizard[index]).PassoAtual(passo);
            });
        }

        void IAlgoritimo.TempoSimulacao(double tempo)
        {
            ExecutarCurrentThread(() =>
            ((ProService.IAlgoritimo)wizard[index]).TempoSimulacao(tempo));
        }

        void IAlgoritimo.Progresso(double progresso)
        {
            ExecutarCurrentThread(() =>
            ((ProService.IAlgoritimo)wizard[index]).Progresso(progresso));
        }

        void IAlgoritimo.ProximaEtapa()
        {
            ExecutarCurrentThread(() =>
            {
                btnAnterior.IsEnabled = true;
                btnProximo.IsEnabled = true;
                ((ProService.IAlgoritimo)wizard[index]).ProximaEtapa();
            });
        }

        void IAlgoritimo.Acao()
        {

        }

        private static void ExecutarCurrentThread(Action acao)
        {
            try
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, acao);
            }
            catch
            { }
        }

        private ObservableCollection<MyData> _data = null;
        public ObservableCollection<MyData> Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
                Notify("Data");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void Notify(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }

    public enum WorkTypes
    {
        Saídas,
        Total,
        Tempo,
    }

    public class MyData
    {
        public MyData()
        {
        }

        public int Year { get; set; }

        public double Value { get; set; }

        public WorkTypes WorkType { get; set; }
    }
}
