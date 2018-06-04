using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProService
{
    public static class Relatorios
    {
        public static List<Modelo> resultados { get; set; }

        public static double[] ExportarResultados(Modelo modelo)
        {
            double[] modeloLocal = new double[3];

            System.Threading.Thread.Sleep(2000);
            Manager.PassoAtual("Abrindo banco " + modelo.banco + ".");
            if (!File.Exists(modelo.banco))
                return null;

            Manager.RDBObj.OpenFile(modelo.banco);            

            int locaisModelo;
            int entidadesModelo;
            Manager.pmDataObject.GetRecordCount(1, out locaisModelo);
            Manager.pmDataObject.GetRecordCount(2, out entidadesModelo);

            double[,] InformLocais = new double[10, locaisModelo];
            double[,] InformEntidades = new double[9, entidadesModelo];

            #region LeituraInformações
            for (int contadorLocais = 1; contadorLocais <= locaisModelo; contadorLocais++)
            {
                Manager.RDBObj.SelectData(1, 1, 1, 1, 5, contadorLocais);
                InformLocais[0, contadorLocais - 1] = Manager.RDBObj.GetValue();

                Manager.RDBObj.SelectData(1, 1, 1, 1, 6, contadorLocais);
                InformLocais[1, contadorLocais - 1] = Manager.RDBObj.GetValue();

                Manager.RDBObj.SelectData(1, 1, 1, 1, 8, contadorLocais);
                InformLocais[2, contadorLocais - 1] = Manager.RDBObj.GetValue();

                Manager.RDBObj.SelectData(1, 1, 1, 3, 2, contadorLocais);
                InformLocais[3, contadorLocais - 1] = Manager.RDBObj.GetValue();

                Manager.RDBObj.SelectData(1, 1, 1, 3, 5, contadorLocais);
                InformLocais[4, contadorLocais - 1] = Manager.RDBObj.GetValue();

                Manager.RDBObj.SelectData(1, 1, 1, 3, 6, contadorLocais);
                InformLocais[5, contadorLocais - 1] = Manager.RDBObj.GetValue();

                Manager.RDBObj.SelectData(1, 1, 1, 3, 7, contadorLocais);
                InformLocais[6, contadorLocais - 1] = Manager.RDBObj.GetValue();

                Manager.RDBObj.SelectData(1, 1, 1, 7, 1, contadorLocais);
                InformLocais[7, contadorLocais - 1] = Manager.RDBObj.GetValue();

                Manager.RDBObj.SelectData(1, 1, 1, 7, 3, contadorLocais);
                InformLocais[8, contadorLocais - 1] = Manager.RDBObj.GetValue();

                Manager.RDBObj.SelectData(1, 1, 1, 7, 5, contadorLocais);
                InformLocais[9, contadorLocais - 1] = Manager.RDBObj.GetValue();
            }

            for (int contadorEntidades = 1; contadorEntidades <= entidadesModelo; contadorEntidades++)
            {
                Manager.RDBObj.SelectData(1, 1, 1, 5, 1, contadorEntidades);
                InformEntidades[0, contadorEntidades - 1] = Manager.RDBObj.GetValue();

                Manager.RDBObj.SelectData(1, 1, 1, 5, 3, contadorEntidades);
                InformEntidades[1, contadorEntidades - 1] = Manager.RDBObj.GetValue();

                Manager.RDBObj.SelectData(1, 1, 1, 5, 4, contadorEntidades);
                InformEntidades[2, contadorEntidades - 1] = Manager.RDBObj.GetValue();

                Manager.RDBObj.SelectData(1, 1, 1, 7, 2, contadorEntidades);
                InformEntidades[3, contadorEntidades - 1] = Manager.RDBObj.GetValue();

                Manager.RDBObj.SelectData(1, 1, 1, 6, 1, contadorEntidades);
                InformEntidades[4, contadorEntidades - 1] = Manager.RDBObj.GetValue();

                Manager.RDBObj.SelectData(1, 1, 1, 6, 2, contadorEntidades);
                InformEntidades[5, contadorEntidades - 1] = Manager.RDBObj.GetValue();

                Manager.RDBObj.SelectData(1, 1, 1, 6, 4, contadorEntidades);
                InformEntidades[6, contadorEntidades - 1] = Manager.RDBObj.GetValue();

                Manager.RDBObj.SelectData(1, 1, 1, 8, 1, contadorEntidades);
                InformEntidades[7, contadorEntidades - 1] = Manager.RDBObj.GetValue();

                Manager.RDBObj.SelectData(1, 1, 1, 8, 2, contadorEntidades);
                InformEntidades[8, contadorEntidades - 1] = Manager.RDBObj.GetValue();
            }
            #endregion
            Manager.PassoAtual("Lida informações inicias.");

            #region RelatorioParcial
            using (StreamWriter relatorio = new StreamWriter(Path.GetFileNameWithoutExtension(modelo.modelo) + ".psr"))
            {
                relatorio.WriteLine("Relatorio parcial de simulacao v1.0");
                relatorio.WriteLine("Thiago Aguirre Lorscheiter");
                relatorio.WriteLine("");
                relatorio.WriteLine("A alteração de qualquer parametro pode afetar ou mesmo inviabilizar o funcionamento do software");
                relatorio.WriteLine("");
                relatorio.WriteLine("Parametros");
                relatorio.WriteLine(locaisModelo + ";" + entidadesModelo);
                relatorio.WriteLine(modelo.modeloPai);
                relatorio.WriteLine("");

                relatorio.WriteLine("RELATORIO LOCAIS");
                relatorio.WriteLine("ConteudoMedio;ConteudoMaximo;Utilização;Operacao;Aguardando;Bloqueado;ParadaNaoPlanejada;CustosdeOperacao;CustoRecurso;Total");
                for (int contadorLocais = 0; contadorLocais < locaisModelo; contadorLocais++)
                {
                    for (int i = 0; i < 10; i++)
                        relatorio.Write(InformLocais[i, contadorLocais] + ";");
                    relatorio.WriteLine("");
                }

                relatorio.WriteLine("RELATORIO ENTIDADES");
                relatorio.WriteLine("TotalSaidas;TempoMedioSistema;TempoMedioEmMovimentacao;TempoMedioEmBloqueado;EmLogicaMovimento;Aguardando;EmBloqueado;SaidasExplicitas;CustoTotal");
                for (int contadorEntidades = 0; contadorEntidades < entidadesModelo; contadorEntidades++)
                {
                    for (int i = 0; i < 9; i++)
                        relatorio.Write(InformEntidades[i, contadorEntidades] + ";");
                    relatorio.WriteLine("");
                }
            }
            #endregion
            Manager.PassoAtual("Criado relatorio parcial.");

            #region RelatorioTotal
            using (StreamWriter relatorio = new StreamWriter(Path.GetFileNameWithoutExtension(modelo.modeloPai) + ".csr", true))
            {
                relatorio.Write(modelo.modelo + ";");
                relatorio.Write(locaisModelo + ";" + entidadesModelo + ";");

                for (int i = 0; i < 10; i++)
                {
                    double media = 0;
                    for (int contadorLocais = 0; contadorLocais < locaisModelo; contadorLocais++)
                        media += InformLocais[i, contadorLocais];

                    relatorio.Write(media + ";");
                }

                for (int i = 0; i < 9; i++)
                {
                    double media = 0;
                    for (int contadorEntidades = 0; contadorEntidades < entidadesModelo; contadorEntidades++)
                        media += InformEntidades[i, contadorEntidades];

                    if (i == 7)
                        modeloLocal[0] = media;
                    if (i == 0)
                        modeloLocal[1] = media;
                    if (i == 1)
                        modeloLocal[2] = media;

                    relatorio.Write(media + ";");
                }

                relatorio.WriteLine("");
            }
            #endregion
            Manager.PassoAtual("Criado relatorio.");
            return modeloLocal;
        }

        public static void CriarCabecalioPai(string local)
        {
            //Directory.Delete();

            using (StreamWriter relatorio = new StreamWriter(Path.GetFileNameWithoutExtension(local) + ".csr", false))
            {
                relatorio.WriteLine("Relatorio completo de simulacao v1.0");
                relatorio.WriteLine("Thiago Aguirre Lorscheiter");
                relatorio.WriteLine("");
                relatorio.WriteLine("A alteração de qualquer parametro pode afetar ou mesmo inviabilizar o funcionamento do software");
                relatorio.WriteLine("");

                relatorio.Write("Modelo;Locais;Entidades;ConteudoMedio;ConteudoMaximo;Utilização;Operacao;Aguardando;Bloqueado;ParadaNaoPlanejada;CustosdeOperacao;CustoRecurso;Total;");
                relatorio.Write("TotalSaidas;TempoMedioSistema;TempoMedioEmMovimentacao;TempoMedioEmBloqueado;EmLogicaMovimento;Aguardando;EmBloqueado;SaidasExplicitas;CustoTotal");
                relatorio.Write("ScoreLocais;ScoreEntidades");
                relatorio.WriteLine("");
            }
        }
    }
}
