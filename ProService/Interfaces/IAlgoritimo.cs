using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProService
{
    public interface IAlgoritimo
    {
        void PassoAtual(string passo);
        void TempoSimulacao(double tempo);
        void Progresso(double progresso);
        void ProximaEtapa();
        void Acao(); 
    }
}
