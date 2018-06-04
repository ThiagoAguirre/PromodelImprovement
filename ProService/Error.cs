using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Service
{
    public class Error
    {
        private static List<Exception> mensagemList = new List<Exception>();
        private static Timer timerGravacao = new Timer(50);

        public static void Inicializar()
        {
            timerGravacao.Elapsed += timerGravacao_Elapsed;
        }

        static void timerGravacao_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                timerGravacao.Stop();

                using (StreamWriter escritor = new StreamWriter(new FileStream(string.Concat("Exception\\", DateTime.Now.ToShortDateString().Replace('/', '-'), ".txt"), FileMode.OpenOrCreate)))
                    for (int nContador = 0; nContador < mensagemList.Count; nContador++)
                    {
                        escritor.WriteLine(DateTime.Now.ToLongTimeString());
                        escritor.WriteLine("Mensagem\n" + mensagemList[nContador].Message + "\n");
                        escritor.WriteLine("Data\n" + mensagemList[nContador].Data + "\n");
                        escritor.WriteLine("Inner\n" + mensagemList[nContador].InnerException + "\n");
                        escritor.WriteLine("Sorce\n" + mensagemList[nContador].Source + "\n");
                        escritor.WriteLine("Stack\n" + mensagemList[nContador].StackTrace + "\n");
                        escritor.WriteLine("Target\n" + mensagemList[nContador].TargetSite + "\n");
                        escritor.WriteLine("-----------------------------------------------------------------------------\n");
                    }

                mensagemList.Clear();
            }

            catch
            {

            }
        }

        public static void GerarLog(Exception e)
        {
            mensagemList.Add(e);
            timerGravacao.Start();
        }
    }
}
