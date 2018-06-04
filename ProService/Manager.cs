using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProService
{
    public static class Manager
    {
        public static List<Modelo> bancoModelos { get; set; }
        private static int numeroFilhos;
        private static int tempoSimulacao = 36000;
        private static IAlgoritimo iVisual;

        public static ProModel.CProModel pmObject { get; set; }
        public static ProModel.CProModelData pmDataObject { get; set; }
        public static RDBSrv.RDBInterface RDBObj { get; set; }

        public static void Inicializar(IAlgoritimo interfaceVisual)
        {
            Service.Error.Inicializar();

            pmObject = new ProModel.CProModel();
            pmDataObject = new ProModel.CProModelData();
            RDBObj = new RDBSrv.RDBInterface();

            dynamic pmEvents = pmObject.GetEventsObject();
            pmObject.SetMessageMode(0);

            if (Relatorios.resultados == null)
            {
                Relatorios.resultados = new List<Modelo>();
                bancoModelos = new List<Modelo>();

                iVisual = interfaceVisual;
            }

            pmObject.SetWindowPos(1, 10, 10, 700, 500, 0);
            pmObject.SetWindowPos(2, 10, 10, 700, 500, 0);
                        
            PassoAtual("Inicializado ProModel.");
        }

        public static void ErroFatal()
        {
            pmObject.Quit();
            pmDataObject = null;
            RDBObj.CloseFile();
            RDBObj = null;
            pmObject = null;

            Inicializar(null);
        }

        public static void PassoAtual(string passo)
        {
            using (StreamWriter escritor = new StreamWriter("Passo.txt", true))
                escritor.WriteLine(passo);

            iVisual.PassoAtual(passo);
        }

        public static void Simular(Modelo modelo)
        {
            pmObject.LoadModel(modelo.modelo);
            PassoAtual("Caregado " + modelo.modelo + ".");

            double SimulationTime = 0;
            pmObject.Simulate();

            while (SimulationTime < tempoSimulacao)
            {
                pmObject.GetSimTime(out SimulationTime);
                iVisual.TempoSimulacao(SimulationTime);
                //iVisual.Progresso(((SimulationTime * 100 / tempoSimulacao) / divisorProgresso) + valorInicial);

                System.Threading.Thread.Sleep(1);
            }
            pmObject.EndSimulation();
            PassoAtual("Terminada simulação.");
        }

        public static void thSegundaGeracao()
        {
            Task.Factory.StartNew(() => SegundaGeracao());
        }

        public static void SegundaGeracao()
        {
            try
            {                
                PassoAtual("SEGUNDA GERAÇÃO.");                

                Modelo[] modeloPrimeira = new Modelo[bancoModelos.Count];
                bancoModelos.CopyTo(modeloPrimeira);

                var top = from p in modeloPrimeira
                          orderby p.scoreSaidas descending
                          orderby p.scoreTotal descending
                          orderby p.scoreTempo ascending
                          select p;

                Relatorios.resultados.Add(top.First());

                foreach (Modelo item in top.Take(2))
                {
                    bancoModelos.Clear();
                    bancoModelos.RemoveRange(0, bancoModelos.Count);
                    bancoModelos = new List<Modelo>();
                    PassoAtual("Criando descendentes do filho " + item.modelo + ".");

                    AlgoritimoGenetico(item.modelo, false);
                    CriarDescendente(false);
                }

                var resultadoSegundaGeracao = from p in bancoModelos
                                              orderby p.scoreSaidas descending
                                              orderby p.scoreTotal descending
                                              orderby p.scoreTempo ascending
                                              select p;

                Relatorios.resultados.Add(resultadoSegundaGeracao.First());
                iVisual.ProximaEtapa();
            }
            catch (Exception ex)
            { Service.Error.GerarLog(ex); }
        }
        
        public static void thAlgoritimoGenetico(string file)
        {
            Task.Factory.StartNew(() => AlgoritimoGenetico(file, true));
        }

        public static void AlgoritimoGenetico(string file, bool modeloPai)
        {
            try
            {
                PassoAtual("Abrir " + file + ".");

                Modelo modelo = new Modelo();
                double[] result = null;

                do
                {
                    modelo.modelo = file;
                    modelo.modeloPai = file;
                    PassoAtual("Carregando " + file + ".");
                    pmObject.LoadModel(file);
                    System.Threading.Thread.Sleep(100);
                    modelo.banco = pmObject.GetOutputPath().Replace("RES", "") + "rdb";
                    PassoAtual("Criado modelo pai.");

                    Relatorios.CriarCabecalioPai(modelo.modelo);
                    PassoAtual("Criado cabecalio de arquivo.");

                    pmDataObject.Populate();
                    System.Threading.Thread.Sleep(100);
                    pmDataObject.GetRecordCount(1, out numeroFilhos);
                    PassoAtual("Carregada informações de locais.");

                    result = SimularExportarCalcularResultados(modelo);
                } while (result == null);

                modelo.scoreSaidas = result[0];
                modelo.scoreTotal = result[1];
                modelo.scoreTempo = result[2];

                bancoModelos.Add(modelo);
                System.Threading.Thread.Sleep(100);
                pmObject.Save();
                System.Threading.Thread.Sleep(100);
                RDBObj.CloseFile();
                       
                if(modeloPai)         
                iVisual.ProximaEtapa();
            }
            catch (Exception ex)
            { Service.Error.GerarLog(ex); }
        }

        public static void Resultado()
        {
            Relatorios.resultados.Add(bancoModelos[0]);
        }

        public static void Resultado(Modelo modelo)
        {
            Relatorios.resultados.Add(modelo);
        }

        public static void thCriarDescendente()
        {
            Task.Factory.StartNew(() => CriarDescendente(true));
        }

        public static void CriarDescendente(bool primeiraGeracao)
        {
            try
            {
                for (int contadorDescendentes = 1; contadorDescendentes <= numeroFilhos; contadorDescendentes++)
                {
                    double[] resposta = null;
                    Modelo modelo = new Modelo();

                        pmObject.SetWindowPos(1, 10, 10, 700, 500, 0);
                        pmObject.SetWindowPos(2, 10, 10, 700, 500, 0);
                        System.Threading.Thread.Sleep(100);

                        PassoAtual("Carregando modelo pai.");
                        pmObject.LoadModel(bancoModelos[0].modelo);
                        System.Threading.Thread.Sleep(100);
                        pmDataObject.Populate();
                        PassoAtual("Modificando modelo.");
                        System.Threading.Thread.Sleep(100);
                        if (!ModificaDescendente(contadorDescendentes))
                            continue;
                                                
                        modelo.modeloPai = bancoModelos[0].modelo;
                        modelo.modelo = Path.GetDirectoryName(bancoModelos[0].modelo) + "\\" + Path.GetFileNameWithoutExtension(bancoModelos[0].modelo) + "filho" + contadorDescendentes + ".mod";
                        pmObject.SaveAs(modelo.modelo);
                                                
                        System.Threading.Thread.Sleep(100);
                        pmObject.LoadModel(modelo.modelo);
                        System.Threading.Thread.Sleep(100);

                        string arquivo = pmObject.GetOutputPath();
                        modelo.banco = arquivo.Substring(0, arquivo.Length - 3) + "rdb";
                        PassoAtual("Salvando modelo filho " + modelo.modelo + ".");

                        resposta = SimularExportarCalcularResultados(modelo);

                        if (!File.Exists(modelo.modelo))
                        {
                            PassoAtual("Erro no ProModel, tentando corrigir.");
                            ErroFatal();
                        }

                    if (resposta == null)
                    {
                        PassoAtual("Erro no ProModel, tentando corrigir.");
                        System.Threading.Thread.Sleep(2000);
                        //ErroFatal();
                        RDBObj.CloseFile();
                        contadorDescendentes--;
                        continue;
                    }

                    modelo.scoreSaidas = resposta[0];
                    modelo.scoreTotal = resposta[1];
                    modelo.scoreTempo = resposta[2];

                    bancoModelos.Add(modelo);
                    System.Threading.Thread.Sleep(100);
                    pmObject.Save();
                    System.Threading.Thread.Sleep(100);
                    RDBObj.CloseFile();
                    System.Threading.Thread.Sleep(100);

                    iVisual.Progresso(contadorDescendentes * 100 / numeroFilhos);                    
                }
                if (primeiraGeracao)
                    SegundaGeracao();
            }
            catch (Exception ex)
            { Service.Error.GerarLog(ex); }
        }

        public static double[] SimularExportarCalcularResultados(Modelo modeloAtual)
        {
            Simular(modeloAtual);
            return Relatorios.ExportarResultados(modeloAtual);
        }

        public static void AbrirFinaleira(string local)
        {
            pmObject.LoadModel(local);
        }

        public static bool ModificaDescendente(int contadorDescente)
        {
            int valor;
            pmDataObject.SelectMainRecordByIndex(1, contadorDescente);

            string local;
            pmDataObject.GetStringFieldValue(1, 2, out local);
            if (local.Contains("."))
                return false;

            pmDataObject.GetIntFieldValue(1, 4, out valor);
            pmDataObject.SetIntFieldValue(1, 4, valor + 1);
            return true;
        }
    }

    public struct Modelo
    {
        public string modelo { get; set; }
        public string banco { get; set; }
        public string modeloPai { get; set; }
        public double scoreSaidas { get; set; }
        public double scoreTotal { get; set; }
        public double scoreTempo { get; set; }
    }
}
